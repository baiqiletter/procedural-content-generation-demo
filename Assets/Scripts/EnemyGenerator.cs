using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyCount;
    public int minEnemyCount = 1;
    public int maxEnemyCount = 3;
    public bool isSpecialRoom;
    public bool isBossRoom;
    public List<GameObject> enemies;

    private GameObject enemySpawn;
    public Tilemap roomTilemap;

    // room borders
    private int x_min = -18;
    private int x_max = 17;
    private int y_min = -12;
    private int y_max = 11;

    // Start is called before the first frame update
    void Start()
    {
        if (isSpecialRoom)
        {
            enemyCount = 3;
            GenerateDrop();
        }
        else if (isBossRoom)
        {
            enemyCount = 1;
            GenerateBoss();
        }
        else
        {
            enemyCount = Random.Range(minEnemyCount, maxEnemyCount);
            GenerateEnemy();
        }

        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateBoss()
    {
        Vector3Int blankTilePos = GetBlankTile();
        //print("GotPos : " + blankTilePos);

        enemySpawn = UnitManager.Instance.boss[Random.Range(0, UnitManager.Instance.boss.Length)];

        Instantiate(enemySpawn, gameObject.transform);
        enemySpawn.transform.position =
            new Vector3(
                blankTilePos.x * .99f * .5f,
                blankTilePos.y * .99f * .5f,
                0);
        //print("ActualPos : " + enemySpawn.transform.position);
        enemies.Add(enemySpawn);
    }

    void GenerateEnemy()
    {
        int generatedEnemyCount = 0;
        while (generatedEnemyCount < enemyCount)
        {
            // generate an enemy
            Vector3Int blankTilePos = GetBlankTile();
            //print("GotPos : " + blankTilePos);

            enemySpawn = UnitManager.Instance.enemy[Random.Range(0, UnitManager.Instance.enemy.Length)];

            GameObject enemyObject = Instantiate(enemySpawn, gameObject.transform);
            enemyObject.transform.localPosition =
                new Vector3(
                    blankTilePos.x * .99f * .5f,
                    blankTilePos.y * .99f * .5f,
                    0);
            //print(enemySpawn.name + " : " + enemySpawn.transform.position);
            
            generatedEnemyCount++;
            enemies.Add(enemyObject);
        }
    }

    void GenerateDrop()
    {
        int generatedEnemyCount = 0;
        while (generatedEnemyCount < enemyCount)
        {
            // generate an enemy
            Vector3Int blankTilePos = GetBlankTile();

            enemySpawn = UnitManager.Instance.drop;

            Instantiate(enemySpawn, gameObject.transform);
            enemySpawn.transform.localPosition =
                new Vector3(
                    blankTilePos.x * .99f * .5f,
                    blankTilePos.y * .99f * .5f,
                    0);
            print(enemySpawn.name + " : " + enemySpawn.transform.position);
            
            generatedEnemyCount++;
            enemies.Add(enemySpawn);
        }
    }

    Vector3Int GetBlankTile()
    {
        Vector3Int blankTilePos;

        do
        {
            blankTilePos = new Vector3Int(
                Random.Range(x_min + 2, x_max - 1),
                Random.Range(y_min + 2, y_max - 1),
                0);
        } while (!SpareAround(blankTilePos));

        //print(blankTilePos);

        return blankTilePos;
    }

    bool SpareAround(Vector3Int pos)
    {
        for (int i = pos.x - 1; i <= pos.x + 1; i++)
        {
            for (int j = pos.y - 2; j <= pos.y + 2; j++)
            {
                if (roomTilemap.HasTile(new Vector3Int(i, j, 0)))
                    return false;
            }
        }
        return true;
    }
    
    public void DestroyRoom()
    {
        foreach (GameObject enemy in enemies){
            Destroy(enemy);
        }
        //print("enemies: " + enemies.Count + " / " + enemyCount);
        Destroy(gameObject);
    }

    void testTilemapAPI()
    {
        print("tilemap size : " + roomTilemap.size);
        print("tilemap cellSize : " + roomTilemap.cellSize);
        print("tilemap cellBounds : " + roomTilemap.cellBounds);
        print("tilemap localBounds : " + roomTilemap.localBounds);

        // test tilemap scale
        int tileCount = 0;
        int xMin = roomTilemap.size.x;
        int xMax = -roomTilemap.size.x;
        int yMin = roomTilemap.size.y;
        int yMax = -roomTilemap.size.y;
        for (int i = -roomTilemap.size.x; i < roomTilemap.size.x; i++)
        {
            for (int j = -roomTilemap.size.y; j < roomTilemap.size.y; j++)
            {
                if (roomTilemap.HasTile(new Vector3Int(i, j, 0)))
                {
                    tileCount++;
                    print("(" + i + ", " + j + ")");
                    xMin = Mathf.Min(xMin, i);
                    xMax = Mathf.Max(xMax, i);
                    yMin = Mathf.Min(yMin, j);
                    yMax = Mathf.Max(yMax, j);
                }
            }
        }
        print("tileCount : " + tileCount);
        print("xMin : " + xMin);
        print("xMax : " + xMax);
        print("yMin : " + yMin);
        print("yMax : " + yMax);
    }
}
