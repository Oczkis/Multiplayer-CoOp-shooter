using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterManager : NetworkBehaviour
{
    private static CharacterManager _instance;

    public static CharacterManager Instance { get { return _instance; } }

    public GameObject playableCharacter;
    public List<GameObject> enemyCharacterPefabs = new List<GameObject>();

    public List<Transform> playerSpawnPoints = new List<Transform>();
    public List<EnemySpawnPoint> enemySpawnPoints = new List<EnemySpawnPoint>();

    public List<PlayerCharacter> playerCharacters = new List<PlayerCharacter>();
    public List<Enemy> enemyCharacters = new List<Enemy>();

    private int spawnPointsFetched;
    private int enemyRespawnTimersInTicks;
    private int clockTicks;
    private int enemiesOnMap;

    [Header("Settings")]
    [SerializeField] private int enemyRespawnTimersInSeconds;
    [SerializeField] private int maxEnemySpawnedOnMap;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        enemyRespawnTimersInTicks = enemyRespawnTimersInSeconds * GlobalTimer.Instance.ticksPerSecond;
    }

    public override void OnStartServer()
    {
        GameManager.ServerOnGameStarted += SpawnPlayerCharacter;
        GameManager.ServerOnGameStoped += DeSpawnCharacter;
        GlobalTimer.clockTicks += CharacterManagerClockHandle;
    }

    public override void OnStopServer()
    {
        GameManager.ServerOnGameStarted -= SpawnPlayerCharacter;
        GameManager.ServerOnGameStoped -= DeSpawnCharacter;
        GlobalTimer.clockTicks -= CharacterManagerClockHandle;
    }

    private void CharacterManagerClockHandle()
    {
        if(!GameManager.Instance.isGameInProgress) { return; }

        clockTicks++;
        if (clockTicks % enemyRespawnTimersInTicks == 0)
            SpawnEnemy();
    }

    private void SpawnPlayerCharacter()
    {
        foreach (Player player in CustomNetworkManager.Instance.players)
        {
            PlayerCharacter playerCharacter = GetPlayerCharacterByID(player.playerID);
            Vector3 startPos = playerSpawnPoints[player.playerID].position;

            if (playerCharacter == null)
            {
                GameObject character = Instantiate(playableCharacter, Vector3.zero, Quaternion.identity);
                NetworkServer.Spawn(character, player.gameObject);
                playerCharacter = character.GetComponent<PlayerCharacter>();
            }

            playerCharacter.Initialize(player.colourID, player.playerID, startPos);

            playerCharacters.Add(playerCharacter);
        }
    }

    private void SpawnEnemy()
    {
        if(enemiesOnMap >= maxEnemySpawnedOnMap) { return; }

        Enemy enemy = GetFirstNonActiveEnemy();
        EnemySpawnPoint enemySpawnPoint = GetNextEnemySpawnPoint();

        if (enemy == null)
        {
            GameObject enemyGO = Instantiate(enemyCharacterPefabs[0]);
            NetworkServer.Spawn(enemyGO);
            enemy = enemyGO.GetComponent<Enemy>();
            enemyCharacters.Add(enemy);
        }        

        enemySpawnPoint.SpawnAnEnemy(enemy);
        enemiesOnMap++;
    }

    public void EnemyGotKilled()
    {
        enemiesOnMap--;
    }

    private EnemySpawnPoint GetNextEnemySpawnPoint()
    {
        EnemySpawnPoint enemySpawnPoint = enemySpawnPoints[spawnPointsFetched];

        spawnPointsFetched++;

        if (spawnPointsFetched == enemySpawnPoints.Count)
        {
            spawnPointsFetched = 0;
            Helpers.Shuffle(enemySpawnPoints);
        }
            

        return enemySpawnPoint;
    }

    private PlayerCharacter GetPlayerCharacterByID(int playerID)
    {
        foreach (PlayerCharacter playerCharacter in playerCharacters)
        {
            if (playerCharacter.characterID == playerID)
                return playerCharacter;
        }

        return null;
    }

    private Enemy GetFirstNonActiveEnemy()
    {
        foreach (Enemy enemy in enemyCharacters)
        {
            if (!enemy.gameObject.activeSelf)
                return enemy;
        }

       return null;
    }

    private void DeSpawnCharacter()
    {
        DeSpawnPlayerCharacter();
        DeSpawnEnemyCharacter();
    }

    private void DeSpawnPlayerCharacter()
    {
        foreach (PlayerCharacter playerCharacter in playerCharacters)
        {
            playerCharacter.isAlive = false;
        }
    }

    private void DeSpawnEnemyCharacter()
    {
        foreach (Enemy enemy in enemyCharacters)
        {
            enemy.isAlive = false;
        }
    }
}
