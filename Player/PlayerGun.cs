using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerGun : NetworkBehaviour
{
    public AnimatorHandler animatorHandler;
    public PlayerCharacter playerCharacter;
    public Transform gunPointTransform;

    private int tickBetweenShots;

    public bool isAiming;

    [SyncVar]
    private bool canShoot;
    
    [Header("GunSettings")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int ticksBetweenShots;
    [SerializeField] private int bulletDurationInTicks;
    [SerializeField] private int bulletDamage;



    public override void OnStartClient()
    {
        GlobalTimer.clockTicks += HandleTimeTick;
    }

    public override void OnStopClient()
    {
        GlobalTimer.clockTicks -= HandleTimeTick;
    }

    public void Aim(bool aiming)
    {
        isAiming = aiming;

        if(aiming)
        {
            animatorHandler.StartAimAnimation();
        }
        else
        {
            animatorHandler.StopAimAnimation();
        }
        
    }

    private void HandleTimeTick()
    {
        if(!playerCharacter.isAlive) { return; }

        //if(!canShoot)
            tickBetweenShots++;

        if (tickBetweenShots % ticksBetweenShots == 0)
            canShoot = true;

        if (isAiming && canShoot)
            CmdShoot();
    }

    [Command]
    private void CmdShoot()
    {
        if(!canShoot) { return; }
        canShoot = false;
        playerCharacter.SpawnBulletRpc(gunPointTransform.position, transform.rotation, bulletSpeed, bulletDurationInTicks, bulletDamage, playerCharacter.characterID, false);
    }
}
