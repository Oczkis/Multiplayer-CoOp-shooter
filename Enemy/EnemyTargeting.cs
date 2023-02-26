using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    Enemy enemy;

    public PlayerCharacter target;
    public GameObject head;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (target != null)
        {
            HandleRotation();
        }
    }

    public void HandleRotation()
    {
        Vector3 relativePos = target.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        rotation.Normalize();
        rotation.x = 0;
        rotation.z = 0;

        head.transform.rotation = Quaternion.RotateTowards(head.transform.rotation, rotation, enemy.turnSpeed * Time.deltaTime);
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, enemy.moveSpeed * Time.deltaTime);
    }

    public void SetTarget(PlayerCharacter player)
    {
        target = player;
        enemy.targetID = player.characterID;
    }
}
