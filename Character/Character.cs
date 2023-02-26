using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Character : NetworkBehaviour, IDamageable
{
    public Helpers.CharacterType characterType;

    [SyncVar]
    public int characterID;

    [SyncVar(hook = nameof(ClientHandleHealthChanged))]
    public int health;
    public int maxHealth;

    [SyncVar(hook = nameof(ClientHandleCharacterAlive))]
    public bool isAlive;

    [SerializeField] private GameObject[] hitFX = new GameObject[0];

    [SerializeField] private GameObject bulletPrefab;

    private void ClientHandleHealthChanged(int oldValue, int newValue)
    {
        if (hasAuthority)
            UIManager.Instance.myHealth.text = health.ToString();

        if (!NetworkServer.active) { return; }

        if (health <= 0)
            Die();
    }

    private void ClientHandleCharacterAlive(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            SpawnEffect();
        }
        else
        {
            DeathEffect();
        }

        gameObject.SetActive(newValue);
    }

    protected virtual void DeathEffect()
    {
        
    }

    protected virtual void SpawnEffect()
    {
        
    }
    public void ReceiveDamage(int damage, int playerID)
    {
        health -= damage;

        if (health <= 0)
            Die(playerID);
    }

    protected virtual void Die(int playerID)
    {
        foreach (Player player in CustomNetworkManager.Instance.players)
        {
            if (player.playerID == playerID)
                player.score++;
        }

        isAlive = false;
    }

    private void Die()
    {
        isAlive = false;
    }

    public virtual void Respawn()
    {
        health = maxHealth;
        isAlive = true;
    }

    [ClientRpc]
    public void SpawnBulletRpc(Vector3 pos, Quaternion rot, float speed, int durationInTicks, int damage, int playerID, bool canHitPlayers)
    {
        Bullet bullet = Instantiate(bulletPrefab, pos, rot).GetComponent<Bullet>();
        bullet.InitializeBullet(speed, durationInTicks, damage, playerID, canHitPlayers);
    }

    public GameObject GetHitFX()
    {
        return hitFX[Random.Range(0, hitFX.Length)];
    }

    public void SetChracterID(int characterId)
    {
        characterID = characterId;
    }

    public Helpers.CharacterType GetCharacterType()
    {
        return characterType;
    }
}
