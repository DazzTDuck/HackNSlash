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

    public void FallBack(EnemyActor actor)
    {
        if (backupEnemies.Count != 0)
        {

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
