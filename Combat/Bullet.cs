using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private int duration;
    private int ownerID;
    private int timeAlive;
    private int damage;
    private bool isServer;
    private bool canHitPlayers;

    [SerializeField] private GameObject[] shootFX = new GameObject[0];

    public void InitializeBullet(float speed, int duration, int damage, int ownerID, bool canHitPlayers)
    {
        this.speed = speed;
        this.duration = duration;
        this.damage = damage;
        this.ownerID = ownerID;
        this.canHitPlayers = canHitPlayers;
    }

    private void Awake()
    {
        isServer = GameManager.Instance.isServer;
        GlobalTimer.clockTicks += HandleClockTick;
        Instantiate(shootFX[Random.Range(0, shootFX.Length)], transform.position, transform.rotation);
    }

    private void OnDisable()
    {
        GlobalTimer.clockTicks -= HandleClockTick;
    }

    private void HandleClockTick()
    {
        timeAlive++;

        if (timeAlive >= duration)
            BulletExpired();
    }

    private void DestroyBullet(GameObject hitFX)
    {
        Instantiate(hitFX, transform.position, transform.rotation);
        BulletExpired();
    }

    private void BulletExpired()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Hit Box"))
        {
           if(other.TryGetComponent(out IDamageable hit))
           {
                if(isServer && (canHitPlayers || hit.GetCharacterType() == Helpers.CharacterType.Foe))
                    hit.ReceiveDamage(damage, ownerID);

                DestroyBullet(hit.GetHitFX());
           }
        }
    }
}
