using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // player
    public int level;
    public float health;
    public float maxHealth;
    public float attack;
    public float defend;
    public int posX;
    public int posY;
    // optimize
    public float roomDetectDistance;
    // statistic
    public int score;
    public int killScore;
    public int itemScore;
    public int passScore;
    public int killNum;
    public int killNumTotal;
    public int itemNum;
    public int itemNumTotal;
    public int passRoomNum;
    public int passRoomNumTotal;
    public bool[,] reached;
    // dynamic difficulty
    public int minEnemyCount;
    public int maxEnemyCount;
    public float enemyHealthScale;
    public float enemyAttackScale;
    public int mapScale;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }

        InitVar();
        InitReached();

    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Min(health, maxHealth);

        if (posX >= 0 && posY >= 0 && !reached[posX, posY])
        {
            reached[posX, posY] = true;
            passRoomNum++;
            score += passScore;
            print("(" + posX + ", " + posY + ")");
        }
    }

    public void InitVar()
    {
        //player
        level = 0;
        maxHealth = 20f;
        health = maxHealth;
        attack = 4f;
        defend = 0f;
        posX = -1;
        posY = -1;
        //optimize
        roomDetectDistance = 10f;
        //statistic
        score = 0;
        killScore = 10;
        itemScore = 10;
        passScore = 5;
        killNumTotal = 0;
        itemNumTotal = 0;
        passRoomNumTotal = 0;
        //dynamic difficulty
        minEnemyCount = 1;
        maxEnemyCount = 3;
        enemyHealthScale = 1f;
        enemyAttackScale = 1f;
        mapScale = 4;
    }

    public void InitReached()
    {
        reached = new bool[mapScale, mapScale];
        for (int i = 0; i < mapScale; i++)
        {
            for (int j = 0; j < mapScale; j++)
            {
                reached[i, j] = false;
            }
        }
    }
}
