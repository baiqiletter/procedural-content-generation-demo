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

        // check rank storage
        BuildRankStorage();
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

    void BuildRankStorage()
    {
        if (!PlayerPrefs.HasKey("rank1"))
        {
            PlayerPrefs.SetInt("rank1", 0);
        }
        if (!PlayerPrefs.HasKey("rank2"))
        {
            PlayerPrefs.SetInt("rank3", 0);
        }
        if (!PlayerPrefs.HasKey("rank3"))
        {
            PlayerPrefs.SetInt("rank3", 0);
        }
        if (!PlayerPrefs.HasKey("rank4"))
        {
            PlayerPrefs.SetInt("rank4", 0);
        }
        if (!PlayerPrefs.HasKey("rank5"))
        {
            PlayerPrefs.SetInt("rank5", 0);
        }
    }

    public void UpdateRank()
    {
        if (score > PlayerPrefs.GetInt("rank5"))
        {
            PlayerPrefs.SetInt("rank5", score);
        }
        if (score > PlayerPrefs.GetInt("rank4"))
        {
            int tempScore = PlayerPrefs.GetInt("rank4");
            PlayerPrefs.SetInt("rank4", score);
            PlayerPrefs.SetInt("rank5", tempScore);
        }
        if (score > PlayerPrefs.GetInt("rank3"))
        {
            int tempScore = PlayerPrefs.GetInt("rank3");
            PlayerPrefs.SetInt("rank3", score);
            PlayerPrefs.SetInt("rank4", tempScore);
        }
        if (score > PlayerPrefs.GetInt("rank2"))
        {
            int tempScore = PlayerPrefs.GetInt("rank2");
            PlayerPrefs.SetInt("rank2", score);
            PlayerPrefs.SetInt("rank3", tempScore);
        }
        if (score > PlayerPrefs.GetInt("rank1"))
        {
            int tempScore = PlayerPrefs.GetInt("rank1");
            PlayerPrefs.SetInt("rank1", score);
            PlayerPrefs.SetInt("rank2", tempScore);
        }
        print("update rank");
    }
}
