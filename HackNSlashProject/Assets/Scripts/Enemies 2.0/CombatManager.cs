using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Transform player;
    public int engagedMaxValue;
    public List<EnemyActor> engagedEnemies = new List<EnemyActor>();
    public int backupMaxValue;
    public List<EnemyActor> backupEnemies = new List<EnemyActor>();
    public List<EnemyActor> rangedEnemies = new List<EnemyActor>();

    public void EnemyJoinsFight(EnemyActor actor)
    {
        if (actor.enemyType == EnemyType.Ranged)
        {
            rangedEnemies.Add(actor);
            actor.state = Enemystates.Engaged;
        }
        else
        {
            int currentEngagedEnemies = 0;
            foreach (EnemyActor engagedActor in engagedEnemies)
            {
                currentEngagedEnemies += engagedActor.occupySpace;
            }
            int currentBackupEnemies = 0;
            foreach (EnemyActor backupActor in backupEnemies)
            {
                currentBackupEnemies += backupActor.occupySpace;
            }
            if (currentEngagedEnemies + actor.occupySpace <= engagedMaxValue)
            {

            }
            else if (currentBackupEnemies + actor.occupySpace <= backupMaxValue)
            {

            }
        }
    }

    public bool CheckToFallBack(EnemyActor actor)
    {
        if (backupEnemies.Count != 0)
        {
            int p = actor.priority;
            int s = actor.occupySpace;

            for (int i = 0; i < backupEnemies.Count; i++)
            {
                if (backupEnemies[i].occupySpace <= s && backupEnemies[i].priority <= p && !backupEnemies[i].backedOff)
                {
                    FallingBack(actor, i);
                    return true;
                }
            }
            for (int i = 0; i < backupEnemies.Count; i++)
            {
                if (backupEnemies[i].occupySpace <= s && backupEnemies[i].priority <= p)
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
        for (int i = 0; i < engagedEnemies.Count; i++)
        {
            if (engagedEnemies[i] == actor)
            {
                engagedEnemies.RemoveAt(i);
            }
        }
        engagedEnemies.Add(replacementActor);
        replacementActor.state = Enemystates.Engaged;
        backupEnemies.Add(actor);
        actor.state = Enemystates.BackUp;
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
        {
            for (int i = 0; i < rangedEnemies.Count; i++)
            {
                if (rangedEnemies[i] == actor)
                    rangedEnemies.RemoveAt(i);
            }
        }
        else if (actor.state == Enemystates.Attacking || actor.state == Enemystates.Engaged)
        {
            for (int i = 0; i < engagedEnemies.Count; i++)
            {
                if (engagedEnemies[i] == actor)
                    engagedEnemies.RemoveAt(i);
            }
        }
        else if (actor.state == Enemystates.BackUp)
        {
            for (int i = 0; i < backupEnemies.Count; i++)
            {
                if (backupEnemies[i] == actor)
                    backupEnemies.RemoveAt(i);
            }
        }
    }
}

public enum Enemystates
{
    Partoling,
    BackUp,
    Engaged,
    Attacking
}
