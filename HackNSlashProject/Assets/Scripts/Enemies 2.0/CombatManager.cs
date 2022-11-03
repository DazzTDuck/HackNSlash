using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager combatManager;
    public Transform player;
    public int engagedMaxValue;
    public List<EnemyActor> engagedEnemies = new List<EnemyActor>();
    public int backupMaxValue;
    public List<EnemyActor> backupEnemies = new List<EnemyActor>();
    public List<EnemyActor> rangedEnemies = new List<EnemyActor>();
    public List<EnemyActor> livingEnemies = new List<EnemyActor>();
    public List<EnemyActor> deadEnemies = new List<EnemyActor>();

    private void Awake()
    {
        combatManager = this;
        EnemyActor[] actors = FindObjectsOfType<EnemyActor>();
        foreach (EnemyActor actor in actors)
            livingEnemies.Add(actor);
    }

    public void EnemyJoinsFight(EnemyActor actor)
    {
        if (actor.enemyType == EnemyType.Ranged)
        {
            rangedEnemies.Add(actor);
            actor.EngagePlayer();
        }
        else
        {
            int currentEngagedEnemies = 0;
            foreach (EnemyActor engagedActor in engagedEnemies)
                currentEngagedEnemies += engagedActor.occupySpace;
            int currentBackupEnemies = 0;
            foreach (EnemyActor backupActor in backupEnemies)
                currentBackupEnemies += backupActor.occupySpace;
            if (currentEngagedEnemies + actor.occupySpace <= engagedMaxValue)
            {
                engagedEnemies.Add(actor);
                actor.EngagePlayer();
            }
            else if (currentBackupEnemies + actor.occupySpace <= backupMaxValue)
            {
                backupEnemies.Add(actor);
                actor.GoBackup();
                if (actor.priority > 1)
                    OverTakePosition();
            }
        }
    }

    public bool CheckToFallBack(EnemyActor actor)
    {
        if (backupEnemies.Count != 0)
        {
            for (int i = 0; i < backupEnemies.Count; i++)
            {
                if (backupEnemies[i].occupySpace <= actor.occupySpace && ActualPriority(backupEnemies[i]) > ActualPriority(actor))
                {
                    FallingBack(actor, i);
                    return true;
                }
            }
        }
        return false;
    }
    void FallingBack(EnemyActor actor, int replacement)
    {
        EnemyActor replacementActor = backupEnemies[replacement];
        backupEnemies.RemoveAt(replacement);
        engagedEnemies.Remove(actor);
        engagedEnemies.Add(replacementActor);
        replacementActor.EngagePlayer();
        backupEnemies.Add(actor);
        actor.GoBackup();
        if (replacementActor.occupySpace < actor.occupySpace)
            FillFrontLine(actor.occupySpace - replacementActor.occupySpace);
    }

    void OverTakePosition()
    {
        EnemyActor VIP = null;
        foreach (EnemyActor actor in backupEnemies)
        {
            if (!VIP)
                VIP = actor;
            if (ActualPriority(VIP) < ActualPriority(actor))
                VIP = actor;
        }
        int fillSpace = -VIP.occupySpace;
        EnemyActor dmy1 = null;
        EnemyActor dmy2 = null;
        EnemyActor dmy3 = null;
        for (int i = 0; i < engagedEnemies.Count; i++)
        {
            if (!dmy1)
                dmy1 = engagedEnemies[i];
            else if (ActualPriority(dmy1) > ActualPriority(engagedEnemies[i]))
                dmy1 = engagedEnemies[i];
        }
        if (dmy1.occupySpace < VIP.occupySpace)
        {
            for (int i = 0; i < engagedEnemies.Count; i++)
            {
                if (!dmy2 && dmy1 != engagedEnemies[i])
                    dmy2 = engagedEnemies[i];
                else if (dmy2 &&ActualPriority(dmy2) > ActualPriority(engagedEnemies[i]) && dmy1 != engagedEnemies[i])
                    dmy2 = engagedEnemies[i];

            }
        }
        if (dmy2 && dmy1.occupySpace + dmy2.occupySpace < VIP.occupySpace)
        {
            for (int i = 0; i < engagedEnemies.Count; i++)
            {
                if (!dmy3 && dmy1 != engagedEnemies[i] && dmy2 != engagedEnemies[i])
                    dmy3 = engagedEnemies[i];
                else if (dmy3 && ActualPriority(dmy3) > ActualPriority(engagedEnemies[i]) && dmy1 != engagedEnemies[i] && dmy2 != engagedEnemies[i])
                    dmy3 = engagedEnemies[i];
            }
        }
        engagedEnemies.Remove(dmy1);
        backupEnemies.Add(dmy1);
        dmy1.GoBackup();
        fillSpace += dmy1.occupySpace;
        if (dmy2)
        {
            engagedEnemies.Remove(dmy2);
            backupEnemies.Add(dmy2);
            fillSpace += dmy2.occupySpace;
            dmy2.GoBackup();
        }
        if (dmy3)
        {
            engagedEnemies.Remove(dmy3);
            backupEnemies.Add(dmy3);
            dmy3.GoBackup();
            fillSpace += dmy3.occupySpace;
        }
        backupEnemies.Remove(VIP);
        VIP.EngagePlayer();
        engagedEnemies.Add(VIP);
        if (fillSpace >0)
            FillFrontLine(fillSpace);
    }

    float ActualPriority(EnemyActor actor)
    {
        CharacterHealth actorHP = null; ;
        if (actor?.GetComponent<CharacterHealth>())
            actorHP = actor.GetComponent<CharacterHealth>();
        float truePriority = 0;
        if (actorHP)
            truePriority = (actorHP.currentHP / actorHP.maxHP) * actor.priority;
        return truePriority;
    }

    void FillFrontLine(int fillSpace)
    {
        EnemyActor VIP = null;
        int spaceLeft = fillSpace;
        for (int i = 0; i < fillSpace; i++)
        {
            foreach (EnemyActor actor in backupEnemies)
            {
                if (!VIP && actor.occupySpace <= spaceLeft)
                    VIP = actor;
                if (ActualPriority(VIP) < ActualPriority(actor) && actor.occupySpace <= spaceLeft)
                    VIP = actor;
            }
            if (VIP)
            {
                spaceLeft -= VIP.occupySpace;
                VIP.EngagePlayer();
                engagedEnemies.Add(VIP);
                backupEnemies.Remove(VIP);
                VIP = null;
            }
        }
    }

    public void ResetEnemies()
    {
        while (engagedEnemies.Count > 0)
        {
            engagedEnemies[0].ReturnToPatrol();
            engagedEnemies[0].GetComponent<CharacterHealth>().ReturnToMaxHP();
            engagedEnemies.RemoveAt(0);
        }
        while (backupEnemies.Count > 0)
        {
            backupEnemies[0].ReturnToPatrol();
            backupEnemies[0].GetComponent<CharacterHealth>().ReturnToMaxHP();
            backupEnemies.RemoveAt(0);
        }
        while (rangedEnemies.Count > 0)
        {
            rangedEnemies[0].ReturnToPatrol();
            rangedEnemies[0].GetComponent<CharacterHealth>().ReturnToMaxHP();
            rangedEnemies.RemoveAt(0);
        }
        player.GetComponent<CharacterHealth>().ReturnToMaxHP();
    }

    public void RemoveFromCombat(EnemyActor actor)
    {
        if (actor.enemyType == EnemyType.Ranged)
            rangedEnemies.Remove(actor);
        else if (actor.state != Enemystates.Partoling)
        {
            engagedEnemies.Remove(actor);
            int space = engagedMaxValue;
            for (int i = 0; i < engagedEnemies.Count; i++)
                space -= engagedEnemies[i].occupySpace;
            FillFrontLine(space);
        }
        else if (actor.state == Enemystates.BackUp)
            backupEnemies.Remove(actor);
        if (actor.GetComponent<CharacterHealth>().currentHP != 0)
            actor.ReturnToPatrol();
    }

    public void EnemyDied(EnemyActor actor)
    {
        livingEnemies.Remove(actor);
        deadEnemies.Add(actor);
        RemoveFromCombat(actor);
    }

    public void RezAllEnemies()
    {
        for (int i = deadEnemies.Count; i > 0; i--)
        {
            EnemyActor actor = deadEnemies[i - 1];
            livingEnemies.Add(actor);
            deadEnemies.Remove(actor);
            actor.TurnActive();
        }
    }

    public void KillAllEnemies()
    {
        for (int i = livingEnemies.Count; i > 0; i--)
        {
            livingEnemies[i - 1].GetComponent<CharacterHealth>().TakeDamage(100000);
        }
    }
}

public enum Enemystates
{
    Partoling,
    BackUp,
    Engaged,
    Attacking,
    Dead
}
