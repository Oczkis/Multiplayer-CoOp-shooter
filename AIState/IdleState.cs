using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public State chaseState;

    public override State Tick(Enemy enemy)
    {
        //enemy.movingLegs = false;
        float distance = 0;
        PlayerCharacter possibleTarget = null;

        foreach (PlayerCharacter player in CharacterManager.Instance.playerCharacters)
        {
            if (!player.isAlive)
                continue;

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

            if (distanceToPlayer > distance)
            {
                distance = distanceToPlayer;
                possibleTarget = player;
            }               
        }

        if (possibleTarget == null) { return this; }

        enemy.GetEnemyTargeting().SetTarget(possibleTarget);

        return chaseState;
    }
}
