using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public State shootState;
    public State idleState;

    public override State Tick(Enemy enemy)
    {
        if (!enemy.GetEnemyTargeting().target.isAlive)
        {
            enemy.GetEnemyTargeting().target = null;
            return idleState;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.GetEnemyTargeting().target.transform.position);

        if(distanceToPlayer > enemy.chaseDistance)
        {
            enemy.GetEnemyBrain().guideOrb.MoveToTarget(enemy.GetEnemyTargeting().target.transform.position);
        }
        else
        {
            return shootState;
        }

        return this;
    }
}
