using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;
    public Enemy enemySpawned;
    public Animator animator;

    public void SpawnAnEnemy(Enemy enemy)
    {
        enemySpawned = enemy;
        enemySpawned.transform.position = spawnPoint.position;
        enemySpawned.GetComponent<EnemyBrain>().GoStasisState();
        enemySpawned.Respawn();
        SpawningAnimation();
    }

    private void SpawningAnimation()
    {
        animator.Play("Spawn");
    }

    public void SpawningAnimationDone()
    {
        if(GameManager.Instance.IsServer())
        enemySpawned.GetComponent<EnemyBrain>().GoIdleState();
    }
}
