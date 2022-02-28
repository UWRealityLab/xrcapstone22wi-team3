using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Remove hard coding of instantiation, destroy past tiles, clean up overall
public class TerrainManager : MonoBehaviour
{
    // Used for winds
    public GliderInfo gliderInfo;  
    
    // Used to keep track of glider position
    public Transform penguinXROTransform;
    
    // how many tiles to generate before flying begins
    public int numInstantiateInAdvance;
    
    // how many tiles to generate per scene before picking new scene
    public int tilesPerScene;
    
    // used to track glider position
    private float lastPositionZ;
    private float totalDistance;
    
    // tile metrics
    public const float tileSize = 240;
    public const float subtileSize = 120;

    // used to keep track of tile generation
    private int numInstantiated;

    // used to keep track of current scene
    private String sceneWorld;
    
    [NonSerialized] public List<String> sceneWorlds = new List<String>
    {
        "Snow World",
        "Desert World",
        "Garden World"
    };
    
    // [angle, xPosOffset, zPosOffset]
    private List<int[]> subtileRotations = new List<int[]>
    {
        new int[] {0, 0, 0},
        new int[] {90, 0, 90},
        new int[] {180, 90, 90},
        new int[] {270, 90, 0},
    };
    
    // rnd picker for scene, subtile type and subtile angle
    private System.Random rnd = new System.Random();
    
    private static TerrainManager _instance;

    public static TerrainManager Instance
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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        lastPositionZ = penguinXROTransform.position.z;
        
        for (int i = 0; i < numInstantiateInAdvance; i++)
        {
            generateTile(sceneWorlds[0], i);
            LevelManager.Instance.GenerateInitialElements( tileSize * i, sceneWorlds[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update position marker
        float newPositionZ = penguinXROTransform.position.z;
        totalDistance += (newPositionZ - lastPositionZ);
        lastPositionZ = newPositionZ;
        
        // generate tile if reaching end of current tile
        if ((int) (totalDistance / tileSize) == numInstantiated)
        {
            if (numInstantiated % tilesPerScene == 0)
            {
                sceneWorld = sceneWorlds[rnd.Next(0, 3)];
            }

            generateTile(sceneWorld, numInstantiateInAdvance);
            LevelManager.Instance.GenerateObstacles(totalDistance + tileSize * numInstantiateInAdvance, sceneWorld);
            
            numInstantiated++;
            Debug.Log("SAVE:Tile Instantiated:" + numInstantiated);
        }
    }

    // Subtile positioning
    // 3, 4
    // 1, 2
    void generateTile(String sceneName, int inAdvance)
    {
        // Tile 1:
        int[] subtileRotation1 = subtileRotations[rnd.Next(0, 4)];
        int tilePicker1 = rnd.Next(1, 4);
        Instantiate(Resources.Load( sceneName + "/Tile" + tilePicker1),
            new Vector3(-105 + subtileRotation1[1], 0, totalDistance + tileSize * inAdvance + subtileRotation1[2]),
            Quaternion.Euler(0, subtileRotation1[0], 0));
        
        // Tile 2:
        int[] subtileRotation2 = subtileRotations[rnd.Next(0, 4)];
        int tilePicker2 = rnd.Next(1, 4);
        Instantiate(Resources.Load( sceneName + "/Tile" + tilePicker2),
            new Vector3(-105 + subtileSize + subtileRotation2[1], 0, totalDistance + tileSize * inAdvance + subtileRotation2[2]),
            Quaternion.Euler(0, subtileRotation2[0], 0));
        
        // Tile 3:
        int[] subtileRotation3 = subtileRotations[rnd.Next(0, 4)];
        int tilePicker3 = rnd.Next(1, 4);
        Instantiate(Resources.Load( sceneName + "/Tile" + tilePicker3),
            new Vector3(-105 + subtileRotation3[1], 0, totalDistance + tileSize * inAdvance + subtileSize + subtileRotation3[2]),
            Quaternion.Euler(0, subtileRotation3[0], 0));
        
        // Tile 4:
        int[] subtileRotation4 = subtileRotations[rnd.Next(0, 4)];
        int tilePicker4 = rnd.Next(1, 4);
        Instantiate(Resources.Load( sceneName + "/Tile" + tilePicker4),
            new Vector3(-105 + subtileSize + subtileRotation4[1], 0, totalDistance + tileSize * inAdvance + subtileSize + subtileRotation4[2]),
            Quaternion.Euler(0, subtileRotation4[0], 0));
    }
}