using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : MonoBehaviour
{
    Enemy enemy;
    Transform guideOrbTransform;
    public AnimatorHandler animatorHandler;
    
    NavMeshAgent agent;

    public GameObject legs;
    private bool isMoving;
    Vector3 repositionSpot;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();       
    }

    private void Start()
    {
        guideOrbTransform = enemy.GetEnemyBrain().guideOrb.transform;
    }

    private void Update()
    {
        if(isMoving)
        {
            //Move();
            HandleRotation();
            //enemy.movingLegs = true;
            animatorHandler.UpdateAnimatorValues(1, 0);
            if (Vector3.Distance(transform.position, repositionSpot) <= 0.1f)
            {
                animatorHandler.UpdateAnimatorValues(0, 1);
                isMoving = false;
                agent.speed = 0;
            }
                         
        }
        else
        {
            //enemy.movingLegs = false;           
        }
    }

    public void RePosition()
    {
        if (!enemy.isAlive) { return; }
       
        repositionSpot = guideOrbTransform.position;
        agent.destination = repositionSpot;
        isMoving = true;        
        agent.speed = enemy.moveSpeed;
    }

    public void HandleRotation()
    {
        Vector3 relativePos = guideOrbTransform.position - transform.position;

        if(relativePos == Vector3.zero) { return; }

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        rotation.Normalize();
        rotation.x = 0;
        rotation.z = 0;

        legs.transform.rotation = Quaternion.RotateTowards(legs.transform.rotation, rotation, enemy.turnSpeed * Time.deltaTime);
    }

    public void Move()
    {
        
        //transform.position = Vector3.MoveTowards(transform.position, repositionSpot, enemy.moveSpeed * Time.deltaTime);
    }
}
