using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public GameObject checkpointPrefab;
    public Transform penguinXROTransform;

    private int _numObstacleIntervalsGenerated = 0;
    private int _numCheckpointsInstantiated = 0;
    private float _distanceOfLastCheckpoint;
    
    private const float ObstacleCheckRadius = 10f;
    private int _maxSpawnAttemptsPerObstacle = 10;  // to prevent infinite loop

    private const float MaxObstacleHeight = 500f;
    
    // All our obstacles
    public GameObject cloudBigObstacle;
    public GameObject cloudFluffyObstacle;
    public GameObject snowflakeObstacle;
    public GameObject balloonObstacle;
    public GameObject flowerRoseObstacle;
    public GameObject flowerCarnationObstacle;
    public GameObject treeOakObstacle;

    // Initialized in Awake()
    private Dictionary<string, GameObject[]> terrainToObstacleTypes;

    // TODO: remove, this is for debugging purposes
    public static int NumObstaclesActiveInGame = 0;
    
    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        terrainToObstacleTypes = new Dictionary<string, GameObject[]>
        {
            {"Snow World", new[]{snowflakeObstacle, snowflakeObstacle, snowflakeObstacle, balloonObstacle, 
                cloudBigObstacle, cloudFluffyObstacle}},
            {"Desert World", new[]{cloudBigObstacle, cloudFluffyObstacle}},
            {"Garden World", new[]{flowerRoseObstacle, flowerCarnationObstacle, 
                treeOakObstacle, cloudBigObstacle, cloudFluffyObstacle}}
        };
    }


    // Start is called before the first frame update
    void Start()
    {
        // Sanity check that we have all the required terrain types
        // Have to do this in Start() instead of Awake() because we need to guarantee that TerrainManager.Instance is set up
        foreach (string terrainType in TerrainManager.Instance.sceneWorlds)
        {
            if (!terrainToObstacleTypes.ContainsKey(terrainType))
            {
                Debug.Log("ERROR: LevelManager missing " + terrainType + "!");
            }
        }
        foreach (string terrainType in terrainToObstacleTypes.Keys)
        {
            if (!TerrainManager.Instance.sceneWorlds.Contains(terrainType))
            {
                Debug.Log("ERROR: TerrainManager missing " + terrainType + "!");
            }
        }
        
        // Hardcode spawn first checkpoint
        float checkpointX = Random.Range(-75f, 75f);
        float checkpointZ = 200f;
        Instantiate(Resources.Load("Checkpoint"),
            new Vector3(checkpointX, 20, checkpointZ),
            Quaternion.identity);
        _distanceOfLastCheckpoint = checkpointZ;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SAVE:numObstaclesActive:" + NumObstaclesActiveInGame);
    }
    
    public void PlayerMissedCheckpoint()
    {
        // The player missed a checkpoint, so spawn one that's extra far away
        float spawnZOffset = _distanceOfLastCheckpoint * 2;
        SpawnCheckpoint(spawnZOffset);
        _numCheckpointsInstantiated++;
        
        Debug.Log("Missed checkpoint! Spawn a far one");
    }
    
    public void PlayerPassedCheckpoint()
    {
        StartCoroutine(IncreaseSpeed(GetSpeedIncrease()));
        
        GenerateNextCheckpoint();
        _numCheckpointsInstantiated++;
        Debug.Log("num checkpoints passed:" + _numCheckpointsInstantiated);
    }

    private void GenerateNextCheckpoint()
    {
        GliderInfo gliderInfo = GameController.Instance.gliderInfo;

        float predictedPositionY = gliderInfo.penguinXROTransform.position.y;
        // TODO: physics says this should be sqrt, but removing it gives a much better approximation
        float targetHeightToHitCheckpoint = 40;
        float timeUntilWeHitGround = -2 * (predictedPositionY - targetHeightToHitCheckpoint)/ (Physics.gravity.y + gliderInfo.drag); // sqrt

        float projectedSpeed = gliderInfo.ActualSpeed + GetSpeedIncrease();
        float distanceWhereWeWillLand = projectedSpeed * timeUntilWeHitGround;
            
        Debug.Log("time until land:" + timeUntilWeHitGround);
        Debug.Log("distance until land:" + distanceWhereWeWillLand);

        float minSpawnOffset = 200f;
        float spawnZOffset = Math.Max(minSpawnOffset, distanceWhereWeWillLand);

        SpawnCheckpoint(spawnZOffset);
    }

    private void SpawnCheckpoint(float spawnZOffset)
    {
        _distanceOfLastCheckpoint = spawnZOffset;
        float spawnZ = GameController.Instance.gliderInfo.penguinXROTransform.position.z + spawnZOffset;
        
        bool validPosition = false;
        Vector3 position = Vector3.zero;
        int spawnAttempts = 0;

        // these are checkpoints, but just reuse this number of max attempts
        while(!validPosition && spawnAttempts < _maxSpawnAttemptsPerObstacle)
        {
            spawnAttempts++;
            
            float spawnX = Random.Range(-50f, 50f);
            position = new Vector3(spawnX, 20, spawnZ);
            
            Collider[] colliders = Physics.OverlapBox(position, checkpointPrefab.transform.localScale / 2,
                Quaternion.identity);

            validPosition = colliders.Length == 0;
        }

        // Even if we never found a valid position, we need to spawn a checkpoint anyway
        Instantiate(checkpointPrefab, position, Quaternion.identity);
        Debug.Log("num attempts to spawn checkpoint: " + spawnAttempts);
    }

    // StartOfInterval - where we should start spawning obstacles
    // EndOfInterval = StartOfInterval + TerrainManager.TileSize
    public void GenerateObstacles(float startOfInterval, string terrainType)
    {
        // Generate obstacles in the danger zone
        int numObstaclesPerInterval = 2 + _numCheckpointsInstantiated / 3;
        
        for (int i = 0; i < numObstaclesPerInterval; i++)
        {
            // Generate obstacles for first subtile interval
            SpawnRandomObstacle(startOfInterval, GetPositionForObstacleInDangerZone, terrainType);
            
            // Generate obstacles for second subtile interval
            SpawnRandomObstacle(startOfInterval + TerrainManager.subtileSize, GetPositionForObstacleInDangerZone, terrainType);
        }
        

        // Generate cosmetic obstacles
        // int numCosmeticLower = Math.Max(2, (int) penguinXROTransform.position.y / 100);
        // for (int i = 0; i < numCosmeticLower; i++)
        // {
        //     SpawnRandomObstacle(startOfInterval, GetPositionForCosmeticObstacleLower);
        // }
        //
        // int numCosmeticHigher = Math.Max(2, (int)(MaxObstacleHeight - penguinXROTransform.position.y) / 100);
        // for (int i = 0; i < numCosmeticHigher; i++)
        // {
        //     SpawnRandomObstacle(startOfInterval, GetPositionForCosmeticObstacleHigher);
        // }
    }

    private void SpawnRandomObstacle(float startOfInterval, Func<Obstacle, float, Vector3> getPositionForObstacle, string terrainType)
    {
        Vector3 position = Vector3.zero;
        bool validPosition = false;
        int spawnAttempts = 0;
        int layerMask = LayerMask.GetMask("Obstacles");
            
        // Choose a random obstacle
        GameObject[] obstacleChoices = terrainToObstacleTypes[terrainType];
        GameObject obstacle = obstacleChoices[Random.Range(0, obstacleChoices.Length)];
        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        

        // While we don't have a valid position and we haven't tried spawning this obstacle too many times
        while(!validPosition && spawnAttempts < _maxSpawnAttemptsPerObstacle)
        {
            spawnAttempts++;

            // Pick a random position
            position = getPositionForObstacle(obstacleScript, startOfInterval);

            // Collect all colliders within our Obstacle Check Radius
            Collider[] colliders = Physics.OverlapSphere(position, ObstacleCheckRadius, layerMask);
            
            validPosition = colliders.Length == 0;
        }
            
        if(validPosition)
        {
            // Spawn the obstacle here
            Instantiate(obstacle, position, obstacleScript.GetSpawnRotation());
            NumObstaclesActiveInGame++;
        }
    }

    // Obstacles that the user will probably player
    private Vector3 GetPositionForObstacleInDangerZone(Obstacle obstacleScript, float startOfInterval)
    {
        float x = Random.Range(-120f, 120f);
        float y;
        if (obstacleScript.ShouldSpawnOnGround())
        {
            y = 0;
        }
        else
        {
            y = penguinXROTransform.position.y + 
                Random.Range(obstacleScript.GetSpawnOffsetLowerBound(), obstacleScript.GetSpawnOffsetUpperBound());
            y = Math.Max(20, y);  // min spawn height of 20
        }
        float z = startOfInterval + Random.Range(0, TerrainManager.subtileSize);
        return new Vector3(x, y, z);
    }
    
    // Obstacles that are lower than the player
    private Vector3 GetPositionForCosmeticObstacleLower(Obstacle obstacleScript, float startOfInterval)
    {
        // TODO: maybe this x range is too wide
        float x = Random.Range(-200, 200);
        float y = Random.Range(10, obstacleScript.GetSpawnOffsetLowerBound() + penguinXROTransform.position.y);
        float z = startOfInterval + Random.Range(0, TerrainManager.subtileSize);
        return new Vector3(x, y, z);
    }
    
    // Obstacles that are higher than the player
    private Vector3 GetPositionForCosmeticObstacleHigher(Obstacle obstacleScript, float startOfInterval)
    {
        float x = Random.Range(-200, 200);
        float y = Random.Range(penguinXROTransform.position.y + obstacleScript.GetSpawnOffsetUpperBound(), MaxObstacleHeight);
        float z = startOfInterval + Random.Range(0, TerrainManager.subtileSize);
        return new Vector3(x, y, z);
    }

    private float GetSpeedIncrease()
    {
        // Increase speed depending on number of checkpoints we've passed
        if (_numCheckpointsInstantiated == 0)
        {
            return 0f;
        }
        if (_numCheckpointsInstantiated < 5)
        {
            return 2f;
        }
        if (_numCheckpointsInstantiated < 10)
        {
            return 1f;
        }
        if (_numCheckpointsInstantiated < 20)
        {
            return 0.5f;
        }
        if (_numCheckpointsInstantiated < 30)
        {
            return 0.1f;
        }
        return 0;
    }
    
    IEnumerator IncreaseSpeed(float speedIncrease)
    {
        // Apply speed over 10 seconds
        for (int i = 0; i < 100; i++)
        {
            GameController.Instance.gliderInfo.speed += 0.01f * speedIncrease;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
