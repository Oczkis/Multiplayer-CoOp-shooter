using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : State
{
    public State chaseState;
    public State idleState;

    public override State Tick(Enemy enemy)
    {
        if(!enemy.GetEnemyTargeting().target.isAlive)
        {
            enemy.GetEnemyTargeting().target = null;
            return idleState;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.GetEnemyTargeting().target.transform.position);

        if (distanceToPlayer >= enemy.shootDistance)
        {
            return chaseState;
        }
        
        if(enemy.canShoot)
        {
            enemy.SpawnBulletRpc(enemy.shootingPoint.position, enemy.GetEnemyTargeting().head.transform.rotation, enemy.bulletSpeed, enemy.bulletDurationInTicks, enemy.bulletDamage, -1, true);
            enemy.ShotFired();
        }

        return this;
    }
}
