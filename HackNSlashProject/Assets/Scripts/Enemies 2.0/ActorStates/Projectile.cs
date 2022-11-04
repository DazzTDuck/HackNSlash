using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    float velocity;
    float angle;
    Vector3 startPos;

    public LayerMask playerLayer;
    public GameObject splashEffect;
    float radius;
    int damage;

    public void Throw(int damage_, float radius_)
    {
        damage = damage_;
        radius = radius_;
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        Vector3 targetPos = CombatManager.combatManager.player.position;
        Vector3 dir = targetPos - startPos;
        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);
        targetPos = new Vector3(groundDir.magnitude, dir.y, 0);
        float InitialVelocity;
        float time;
        float angle;
        float height = targetPos.y + targetPos.magnitude / 4f;
        height = Mathf.Max(0.001f, height);
        CalculatePathWithHeight(targetPos, height, out InitialVelocity, out angle, out time);
        StartCoroutine(InAirMoving(groundDir.normalized, InitialVelocity, angle, time + 5));
    }

    float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }
    void CalculatePathWithHeight(Vector3 targetPos, float h, out float initialVelocity, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float a = (-0.5f * g);
        float b = Mathf.Sqrt(2 * g * h);
        float c = -yt;

        float tPlus = QuadraticEquation(a, b, c, 1);
        float tMin = QuadraticEquation(a, b, c, -1);

        time = tPlus > tMin ? tPlus : tMin;
        angle = Mathf.Atan(b * time / xt);
        initialVelocity = b / Mathf.Sin(angle);
    }
    void CalculatePath(Vector3 targetPos, float angle, out float initialVelocity, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        initialVelocity = Mathf.Sqrt(v1 / (v2 - v3));
        time = xt / (initialVelocity * Mathf.Cos(angle));
    }
    IEnumerator InAirMoving(Vector3 dir, float initialVelocity, float angle, float time)
    {
        float t = 0;
        while (t < time)
        {
            float x = initialVelocity * t * Mathf.Cos(angle);
            float y = initialVelocity * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            rb.MovePosition(startPos + dir * x + Vector3.up * y);

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(splashEffect, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySound("GlassSmash");
        Collider[] players = Physics.OverlapSphere(transform.position, radius, playerLayer);
        foreach (Collider player in players)
        {
            player.GetComponent<CharacterHealth>()?.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
