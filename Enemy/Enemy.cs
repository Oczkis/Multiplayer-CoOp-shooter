using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Enemy : Character, IDamageable
{
    [SyncVar(hook = nameof(ClientHandleEnemyTargetUpdated))]
    public int targetID;

    [SyncVar(hook = nameof(ClientHandleEnemyMovingUpdated))]
    public bool movingLegs;

    EnemyBrain brain;
    EnemyMotor motor; 
    EnemyTargeting targeting;
    public AnimatorHandler animatorHandler;

    // 0 friendly, 1 angry, 2 error
    public Material[] botFaces = new Material[0];
    public Material defaultBotMaterial;
    public MeshRenderer botMeshRenderer;
    public GameObject guidingOrbPrefab;
    public GameObject[] explodeFX = new GameObject[0];

    private int ticksAlive;
    public bool canShoot { private set; get; }

    [Header("Settings")]
    public int brainCalculationsTickInterval;
    public int repositionTickInterval;
    public int shootingTickInterval;
    public int shootDistance;
    public float moveSpeed;
    public float turnSpeed;
    public float chaseDistance;

    [Header("Shooting Settings")]
    public Transform shootingPoint;
    public float bulletSpeed; 
    public int bulletDurationInTicks;
    public int bulletDamage;


    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        brain = GetComponent<EnemyBrain>();
        motor = GetComponent<EnemyMotor>();
        targeting = GetComponent<EnemyTargeting>();

        brain.guideOrb = Instantiate(guidingOrbPrefab, transform.position, Quaternion.identity).GetComponent<EnemyGuideOrb>();
    }

    public override void OnStartServer()
    {
        GlobalTimer.clockTicks += EnemyHandleClockTick;
        health = maxHealth;
    }

    public override void OnStopServer()
    {
        GlobalTimer.clockTicks -= EnemyHandleClockTick;
    }

    public override void OnStartLocalPlayer()
    {
        this.enabled = false;
    }

    private void ClientHandleEnemyTargetUpdated(int oldValue, int newValue)
    {
        botMeshRenderer.materials = new Material[] { defaultBotMaterial, botFaces[targetID == PlayerManager.Instance.myPlayer.playerID ? 1 : 0] };
    }

    private void ClientHandleEnemyMovingUpdated(bool oldValue, bool newValue)
    {
        //animatorHandler.UpdateAnimatorValues(newValue ? 1 : 0,0);
    }

    protected override void Die(int playerID)
    {
        CharacterManager.Instance.EnemyGotKilled();

        base.Die(playerID);
    }

    protected override void DeathEffect()
    {
        Instantiate(explodeFX[Random.Range(0, explodeFX.Length)], transform.position, transform.rotation);
        base.DeathEffect();
    }

    private void EnemyHandleClockTick()
    {
        if(!isAlive) { return; }

        ticksAlive++;

        if (ticksAlive % brainCalculationsTickInterval == 0)
            brain.Tick();

        if (ticksAlive % repositionTickInterval == 0)
            motor.RePosition();

        if (ticksAlive % shootingTickInterval == 0)
            canShoot = true;
    }

    public void ShotFired()
    {
        canShoot = false;
    }

    public EnemyTargeting GetEnemyTargeting()
    {
        return targeting;
    }

    public EnemyBrain GetEnemyBrain()
    {
        return brain;
    }

    public EnemyMotor GetEnemyMotor()
    {
        return motor;
    }
}
