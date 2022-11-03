using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleManager : MonoBehaviour
{
    public int amount;
    public VisualEffect example;
    List<VisualEffect> particles = new List<VisualEffect>();
    int particle;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            particles.Add(Instantiate(example, transform.position + Vector3.down * 10, Quaternion.identity, transform));
        }
    }

    public void GetParticle(Transform origin)
    {
        particles[particle].transform.position = origin.position;
        particles[particle].transform.rotation = origin.rotation;
        particles[particle].Play();
        particle++;
        if (particle == particles.Count)
            particle = 0;
    }
}
