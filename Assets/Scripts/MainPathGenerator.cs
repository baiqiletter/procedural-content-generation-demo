using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

struct Map
{
    public int level;
    public GameObject Tile;
    public int select;

    public int type;//房间类型
    public int up;
    public int down;
    public int left;
    public int right;
    public bool placed;
};

struct Route
{
    public int x;
    public int y;
};



public class MainPathGenerator : MonoBehaviour
{
    public GameObject player;
    public Image tileImage;
    public GameObject mainDisplayPanel;
    public GameObject subDisplayPanel;
    public GameObject signPanel;
    public GameObject generatingCanvas;
    public Button mainPathButton;
    public Button subPathButton;
    public Button startButton;

    private RoomManager roomManager;
    private int generateTime = 0;
    private Image playerLocation;

    public int Size;//地图边长
    private Map[,] maps;//原始地图
    private int blankNum;
    private bool isEntered;
    private Image locationTile;

    List<Route> rou = new List<Route>();//存放已找到的通路
    List<Route> rouSub = new List<Route>();

    //List<int> ordera = new List<int>();//原始序列
    List<int> order = new List<int>();

    private int roulen;  //路径长度
    //private int rouSubLen;
    public int minlen; //最小路径长度
    public int maxlen; //最大路径长度

    void Start()
    {
        //player = UnitManager.Instance.player;
        Size = GameManager.Instance.mapScale;
        roomManager = GetComponent<RoomManager>();
        maps = new Map[Size, Size];
        blankNum = Size * Size;
        mainPathButton.interactable = true;
        subPathButton.interactable = false;
        startButton.interactable = false;
        isEntered = false;
        minlen = Size * 2;
        maxlen = Size * 3;

        //print(maps.Length);
        //locTile =
        //    Instantiate(tileImage, signPanel.transform);
        //locTile.sprite = roomManager.locSign;

        for (int i = 0; i < Size; i++) //初始化矩阵
        {
            for (int j = 0; j < Size; j++)
            {
                //print(i + " " + j + "size: " + Size);
                maps[i, j].select = 0;
                maps[i, j].up = 0;
                maps[i, j].down = 0;
                maps[i, j].left = 0;
                maps[i, j].right = 0;
                maps[i, j].placed = false;
            }
        }
        maxlen += 1; //Random.Range取不到最大值

        if (minlen < 1) minlen = 1;
        if (maxlen > 17) maxlen = 17;
        roulen = Random.Range(minlen, maxlen); //随机获取路径长度

        if (mainPathButton.interactable == true)
            subPathButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEntered)
        {
            // mini-map
            generatingCanvas.SetActive(true);
            int biasX = 400;
            int biasY = 300;
            generatingCanvas.transform.Find("MainDisplayPanel").transform.localPosition = new Vector3(biasX, biasY, 0);
            generatingCanvas.transform.Find("SubDisplayPanel").transform.localPosition = new Vector3(biasX, biasY, 0);
            generatingCanvas.transform.Find("SignPanel").transform.localPosition = new Vector3(biasX, biasY, 0);
            generatingCanvas.transform.Find("MainDisplayPanel").transform.localScale = new Vector3(.5f, .5f, 0);
            generatingCanvas.transform.Find("SubDisplayPanel").transform.localScale = new Vector3(.5f, .5f, 0);
            generatingCanvas.transform.Find("SignPanel").transform.localScale = new Vector3(.5f, .5f, 0);
            generatingCanvas.transform.Find("MainDisplayPanel").GetComponent<Image>().color = new Color(255, 255, 255, 0);
            generatingCanvas.transform.Find("SubDisplayPanel").GetComponent<Image>().color = new Color(255, 255, 255, 0);
            generatingCanvas.transform.Find("SignPanel").GetComponent<Image>().color = new Color(255, 255, 255, 0);

            for (int i = 3; i < generatingCanvas.transform.childCount; i++)
            {
                generatingCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }
            //for (int i = 3; i < 6; i++)
            //{
            //    generatingCanvas.transform.GetChild(i).gameObject.SetActive(false);
            //}
            //for (int i = 6; i < generatingCanvas.transform.childCount; i++)
            //{
            //    GameObject child = generatingCanvas.transform.GetChild(i).gameObject;
            //    child.transform.position =
            //        new Vector3(
            //            child.transform.position.x,
            //            -child.transform.position.y,
            //            0);
            //    child.GetComponent<Text>().color = new Color(255, 255, 255, 255);
            //}

            // locate player
            if (GameManager.Instance.posX == -1 && GameManager.Instance.posY == -1)
            {
                locationTile.transform.position =
                signPanel.transform.position +
                new Vector3(
                    (rou[0].x - (Size - 1) / 2f) * locationTile.rectTransform.rect.width * .5f,
                    (rou[0].y - (Size - 1) / 2f) * locationTile.rectTransform.rect.height * .5f,
                    0f);
            } else
            {
                locationTile.transform.position =
                signPanel.transform.position +
                new Vector3(
                    (GameManager.Instance.posX - (Size - 1) / 2f) * locationTile.rectTransform.rect.width * .5f,
                    (GameManager.Instance.posY - (Size - 1) / 2f) * locationTile.rectTransform.rect.height * .5f,
                    0f);
            }
            

        }

        transform.Find("GeneratingCanvas").Find("LevelText").GetComponent<Text>().text = 
            "[ Level ]				" + GameManager.Instance.level;
        transform.Find("GeneratingCanvas").Find("EnemyCountText").GetComponent<Text>().text =
            "[ Enemy Count ]		" + GameManager.Instance.minEnemyCount + " - " + GameManager.Instance.maxEnemyCount;
        transform.Find("GeneratingCanvas").Find("EnemyHealthText").GetComponent<Text>().text =
            "[ Enemy Health ]		x" + GameManager.Instance.enemyHealthScale;
        transform.Find("GeneratingCanvas").Find("EnemyAttackText").GetComponent<Text>().text =
            "[ Enemy Attack ]		x" + GameManager.Instance.enemyAttackScale;
        transform.Find("GeneratingCanvas").Find("MapScaleText").GetComponent<Text>().text =
            "[ Map Scale ]			" + GameManager.Instance.mapScale;

        
    }

    public void onClickGenerateMainPathButton()
    {
        generateMainPath(Size);
        displayMainPath();
    }

    public void onClickGenerateSubPathButton()
    {
        generateSubPath(Size);
        displaySubPath();
    }

    void generateMainPath(int S) //生成通路
    {
        rou.Clear();

        blankNum--;

        //第一行选取起始位置
        int entranceX = Random.Range(0, Size);  //随机起点
        int entranceY = Random.Range(0, Size);

        Route nextNode;
        nextNode.x = entranceX;
        nextNode.y = entranceY;
        maps[entranceX, entranceY].select = 1;//添加进节点

        // 设置玩家初始位置
        player.transform.position = new Vector3(entranceX * 18 * 0.99f, entranceY * 12 * 0.99f,-.0001f);

        rou.Add(nextNode);
        //print("entrance: (" + entranceX + "," + entranceY + ")");
        //print("roulen=" + roulen);
        int k = 0;
        while (true)
        {

            getRand(); //0上，1下，2左，3右
            bool flag = false;

            for (int i = 0; i < 4; i++)//四个方向进行随机搜索
            {
                //print("b="+b.Count);
                if (canSelect(order[i], rou[k].x, rou[k].y, S, 0) == true) //可以扩展
                {
                    if (order[i] == 0)//向上扩展
                    {
                        nextNode.x = rou[k].x;
                        nextNode.y = rou[k].y + 1;

                        //原本位置向上开口
                        maps[rou[k].x, rou[k].y].up = 1;
                        //扩展位置向下开口
                        maps[nextNode.x, nextNode.y].down = 1;
                    }

                    if (order[i] == 1)//向下扩展
                    {
                        nextNode.x = rou[k].x;
                        nextNode.y = rou[k].y - 1;

                        //原本位置向下开口
                        maps[rou[k].x, rou[k].y].down = 1;
                        //扩展位置向上开口
                        maps[nextNode.x, nextNode.y].up = 1;
                    }

                    if (order[i] == 2)//向左扩展
                    {
                        nextNode.x = rou[k].x - 1;
                        nextNode.y = rou[k].y;

                        //原本位置向左开口
                        maps[rou[k].x, rou[k].y].left = 1;
                        //扩展位置向右开口
                        maps[nextNode.x, nextNode.y].right = 1;
                    }

                    if (order[i] == 3)//向右扩展
                    {
                        nextNode.x = rou[k].x + 1;
                        nextNode.y = rou[k].y;

                        //原本位置向右开口
                        maps[rou[k].x, rou[k].y].right = 1;
                        //扩展位置向左开口
                        maps[nextNode.x, nextNode.y].left = 1;
                    }

                    maps[nextNode.x, nextNode.y].select = 1;
                    rou.Add(nextNode);
                    flag = true;
                    break;
                }
            }

            if (rou.Count == roulen || flag == false) //达到最大路径数或者无法扩展
            {
                break;
            }

            k++;
        }

        //blankNum -= rou.Count;

        getType();
    }

    void generateSubPath(int S)
    {
        rouSub.Clear();

        //第一行选取起始位置
        if (blankNum == 0)
            return;

        blankNum--;

        int entranceX = Random.Range(0, Size);  //随机起点
        int entranceY = Random.Range(0, Size);

        if (blankNum > Size)
        {
            while (maps[entranceX, entranceY].select == 1)
            {
                entranceX = Random.Range(0, Size);
                entranceY = Random.Range(0, Size);
            }
        } else
        {
            int count = 0;
            List<int> indexX = new List<int>();
            List<int> indexY = new List<int>();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (maps[i, j].select == 0)
                    {
                        indexX.Add(i);
                        indexY.Add(j);
                        count++;
                    }
            int ind = Random.Range(0, count);
            entranceX = indexX[ind];
            entranceY = indexY[ind];
        }

        Route nextNode;
        nextNode.x = entranceX;
        nextNode.y = entranceY;
        maps[entranceX, entranceY].select = 1;//添加进节点

        // 设置玩家初始位置
        //player.transform.position = new Vector3(entranceX * 18 * 0.99f, entranceY * 12 * 0.99f, 0f);

        rouSub.Add(nextNode);
        print("entrance: (" + entranceX + "," + entranceY + ")");
        //print("rouSubLen=" + rouSubLen);
        int k = 0;
        bool toLink = false;
        int lastDir = -1;
        while (true)
        {

            getRand(); //0上，1下，2左，3右
            bool flag = false;
            for (int i = 0; i < 4; i++)//四个方向进行随机搜索
            {
                //print("b="+b.Count);
                // 若为连接至已有路径操作，则置1；若为扩展操作，则置0
                int select = toLink ? 1 : 0;
                if (canSelect(order[i], rouSub[k].x, rouSub[k].y, S, select) == true) //可以扩展
                {
                    if (lastDir != -1 && toLink)
                    {
                        // 若为连接至已有路径操作，则需避免回头（连接到上一个节点）
                        if ((order[i] == 0 && lastDir == 1) ||
                            (order[i] == 1 && lastDir == 0) ||
                            (order[i] == 2 && lastDir == 3) ||
                            (order[i] == 3 && lastDir == 2))
                        {
                            continue;
                        }
                    }
                    Debug.Log("lastDir=" + lastDir + "\torder[i]=" + order[i]);

                    if (order[i] == 0)//向上扩展
                    {
                        nextNode.x = rouSub[k].x;
                        nextNode.y = rouSub[k].y + 1;

                        //原本位置向上开口
                        maps[rouSub[k].x, rouSub[k].y].up = 1;
                        //扩展位置向下开口
                        maps[nextNode.x, nextNode.y].down = 1;
                    }

                    if (order[i] == 1)//向下扩展
                    {
                        nextNode.x = rouSub[k].x;
                        nextNode.y = rouSub[k].y - 1;

                        //原本位置向下开口
                        maps[rouSub[k].x, rouSub[k].y].down = 1;
                        //扩展位置向上开口
                        maps[nextNode.x, nextNode.y].up = 1;
                    }

                    if (order[i] == 2)//向左扩展
                    {
                        nextNode.x = rouSub[k].x - 1;
                        nextNode.y = rouSub[k].y;

                        //原本位置向左开口
                        maps[rouSub[k].x, rouSub[k].y].left = 1;
                        //扩展位置向右开口
                        maps[nextNode.x, nextNode.y].right = 1;
                    }

                    if (order[i] == 3)//向右扩展
                    {
                        nextNode.x = rouSub[k].x + 1;
                        nextNode.y = rouSub[k].y;

                        //原本位置向右开口
                        maps[rouSub[k].x, rouSub[k].y].right = 1;
                        //扩展位置向左开口
                        maps[nextNode.x, nextNode.y].left = 1;
                    }

                    maps[nextNode.x, nextNode.y].select = 1;
                    rouSub.Add(nextNode);
                    flag = true;
                    lastDir = order[i];
                    break;
                }
            }

            //if (rouSub.Count == rouSubLen) //达到最大路径数
            //{
            //    break;
            //}
            if (toLink == true)// 已连接至已有路径，退出循环
            {
                break;
            }

            if (flag == false) // 无法扩展
            {
                if (toLink == false) // 连接已有路径（再循环一次）
                {
                    toLink = true;
                    continue;
                }
            }


            k++;
        }

        //blankNum -= rouSub.Count;

        getType();
    }


    void getRand() //随机排序
    {
        List<int> ordera = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            ordera.Add(i);
        }

        order.Clear();
        int countNum = ordera.Count;
        while (order.Count < countNum)
        {
            int index = Random.Range(0, ordera.Count); ;
            if (!order.Contains(ordera[index]))
            {
                order.Add(ordera[index]);
                ordera.Remove(ordera[index]);
            }
        }
    }

    bool canSelect(int dir, int x, int y, int S, int select)//能否扩展路径
    {
        if (select == 0) blankNum--;
        if (dir == 0)//上
        {
            if (y + 1 < S)
            {
                if (maps[x, y + 1].select == select) return true;
            }
        }
        if (dir == 1)//下
        {

            if (y - 1 >= 0)
            {
                if (maps[x, y - 1].select == select) return true;
            }
        }
        if (dir == 2)//左
        {
            if (x - 1 >= 0)
            {
                if (maps[x - 1, y].select == select) return true;
            }
        }
        if (dir == 3)//右
        {
            if (x + 1 < S)
            {
                if (maps[x + 1, y].select == select) return true;
            }
        }
        if (select == 0) blankNum++;
        return false;
    }

    void getType()//计算类型
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (maps[i, j].select == 1)
                {
                    //类型顺序：上下左右
                    maps[i, j].type = maps[i, j].up * 2 * 2 * 2 + maps[i, j].down * 2 * 2 + maps[i, j].left * 2 + maps[i, j].right;
                }
            }
        }
    }

    //通路保存在rou结构体中，矩阵在maps里
    void displayMainPath()
    {
        // disable all buttons temperally
        mainPathButton.interactable = false;
        subPathButton.interactable = false;
        startButton.interactable = false;

        generateTime = 0;
        InvokeRepeating("processMainPath", 0.3f, 0.3f);
        Invoke("stopProcessMainPath", (rou.Count + 1) * 0.3f);

        for (int i = 0; i < rou.Count; i++)
        {
            //print("(" + rou[i].x + "," + rou[i].y + ")" + "  " + "type=" + maps[rou[i].x, rou[i].y].type + ":" + maps[rou[i].x, rou[i].y].up + maps[rou[i].x, rou[i].y].down + maps[rou[i].x, rou[i].y].left + maps[rou[i].x, rou[i].y].right);

            int roomType = maps[rou[i].x, rou[i].y].type;
            GameObject room = roomManager.rooms1111[Random.Range(0, roomManager.rooms1111.Length)];
            switch (roomType)
            {
                case 1: room = roomManager.rooms0001[Random.Range(0, roomManager.rooms0001.Length)]; break;
                case 2: room = roomManager.rooms0010[Random.Range(0, roomManager.rooms0010.Length)]; break;
                case 3: room = roomManager.rooms0011[Random.Range(0, roomManager.rooms0011.Length)]; break;
                case 4: room = roomManager.rooms0100[Random.Range(0, roomManager.rooms0100.Length)]; break;
                case 5: room = roomManager.rooms0101[Random.Range(0, roomManager.rooms0101.Length)]; break;
                case 6: room = roomManager.rooms0110[Random.Range(0, roomManager.rooms0110.Length)]; break;
                case 7: room = roomManager.rooms0111[Random.Range(0, roomManager.rooms0111.Length)]; break;
                case 8: room = roomManager.rooms1000[Random.Range(0, roomManager.rooms1000.Length)]; break;
                case 9: room = roomManager.rooms1001[Random.Range(0, roomManager.rooms1001.Length)]; break;
                case 10: room = roomManager.rooms1010[Random.Range(0, roomManager.rooms1010.Length)]; break;
                case 11: room = roomManager.rooms1011[Random.Range(0, roomManager.rooms1011.Length)]; break;
                case 12: room = roomManager.rooms1100[Random.Range(0, roomManager.rooms1100.Length)]; break;
                case 13: room = roomManager.rooms1101[Random.Range(0, roomManager.rooms1101.Length)]; break;
                case 14: room = roomManager.rooms1110[Random.Range(0, roomManager.rooms1110.Length)]; break;
                case 15: room = roomManager.rooms1111[Random.Range(0, roomManager.rooms1111.Length)]; break;
            }

            int endRoomIndex = Random.Range(0, roomManager.roomsEnd.Length);
            if (i == rou.Count - 1) // 最终房间
            {
                room = roomManager.roomsEnd[endRoomIndex];
            }

            maps[rou[i].x, rou[i].y].Tile = 
                Instantiate(room, new Vector3(rou[i].x * 18 * .99f, rou[i].y * 12 * .99f, 0f), Quaternion.identity);
            maps[rou[i].x, rou[i].y].placed = true;
            maps[rou[i].x, rou[i].y].Tile.GetComponent<Room>().x = rou[i].x;
            maps[rou[i].x, rou[i].y].Tile.GetComponent<Room>().y = rou[i].y;
            maps[rou[i].x, rou[i].y].Tile.transform.Find("Grid").Find("Tilemap Ground").GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (i == rou.Count - 1)
            {
                if (maps[rou[i].x, rou[i].y].up == 1)
                {
                    maps[rou[i].x, rou[i].y].Tile.transform.Find("Grid").Find("Tilemap WallUp").gameObject.SetActive(false);
                }
                if (maps[rou[i].x, rou[i].y].down == 1)
                {
                    maps[rou[i].x, rou[i].y].Tile.transform.Find("Grid").Find("Tilemap WallDown").gameObject.SetActive(false);
                }
                if (maps[rou[i].x, rou[i].y].left == 1)
                {
                    maps[rou[i].x, rou[i].y].Tile.transform.Find("Grid").Find("Tilemap WallLeft").gameObject.SetActive(false);
                }
                if (maps[rou[i].x, rou[i].y].right == 1)
                {
                    maps[rou[i].x, rou[i].y].Tile.transform.Find("Grid").Find("Tilemap WallRight").gameObject.SetActive(false);
                }
            }
        }

        //Debug.Log("blankNum = " + blankNum);
    }

    void displaySubPath()
    {
        // disable all buttons temperally
        mainPathButton.interactable = false;
        subPathButton.interactable = false;
        startButton.interactable = false;

        generateTime = 0;
        InvokeRepeating("processSubPath", 0.3f, 0.3f);
        Invoke("stopProcessSubPath", (rouSub.Count + 1) * 0.3f);

        for (int i = 0; i < rouSub.Count; i++)
        {
            //print("(" + rouSub[i].x + "," + rouSub[i].y + ")" + "  " + "type=" + maps[rouSub[i].x, rouSub[i].y].type + ":" + maps[rouSub[i].x, rouSub[i].y].up + maps[rouSub[i].x, rouSub[i].y].down + maps[rouSub[i].x, rouSub[i].y].left + maps[rouSub[i].x, rouSub[i].y].right);

            int roomType = maps[rouSub[i].x, rouSub[i].y].type;
            GameObject room = roomManager.rooms1111[Random.Range(0, roomManager.rooms1111.Length)];
            switch (roomType)
            {
                case 1: room = roomManager.rooms0001[Random.Range(0, roomManager.rooms0001.Length)]; break;
                case 2: room = roomManager.rooms0010[Random.Range(0, roomManager.rooms0010.Length)]; break;
                case 3: room = roomManager.rooms0011[Random.Range(0, roomManager.rooms0011.Length)]; break;
                case 4: room = roomManager.rooms0100[Random.Range(0, roomManager.rooms0100.Length)]; break;
                case 5: room = roomManager.rooms0101[Random.Range(0, roomManager.rooms0101.Length)]; break;
                case 6: room = roomManager.rooms0110[Random.Range(0, roomManager.rooms0110.Length)]; break;
                case 7: room = roomManager.rooms0111[Random.Range(0, roomManager.rooms0111.Length)]; break;
                case 8: room = roomManager.rooms1000[Random.Range(0, roomManager.rooms1000.Length)]; break;
                case 9: room = roomManager.rooms1001[Random.Range(0, roomManager.rooms1001.Length)]; break;
                case 10: room = roomManager.rooms1010[Random.Range(0, roomManager.rooms1010.Length)]; break;
                case 11: room = roomManager.rooms1011[Random.Range(0, roomManager.rooms1011.Length)]; break;
                case 12: room = roomManager.rooms1100[Random.Range(0, roomManager.rooms1100.Length)]; break;
                case 13: room = roomManager.rooms1101[Random.Range(0, roomManager.rooms1101.Length)]; break;
                case 14: room = roomManager.rooms1110[Random.Range(0, roomManager.rooms1110.Length)]; break;
                case 15: room = roomManager.rooms1111[Random.Range(0, roomManager.rooms1111.Length)]; break;
            }

            if (i == 0) // 特殊房间
            {
                room = roomManager.roomsSpecial[Random.Range(0, roomManager.roomsSpecial.Length)];
            }

            if (maps[rouSub[i].x, rouSub[i].y].placed == false) // 该位置原为空（包含特殊房间和普通房间）
            {
                maps[rouSub[i].x, rouSub[i].y].Tile = 
                    Instantiate(room, new Vector3(rouSub[i].x * 18 * .99f, rouSub[i].y * 12 * .99f, 0f), Quaternion.identity);
                maps[rouSub[i].x, rouSub[i].y].placed = true; 
                if (i == 0) // 处理特殊房间的四个出口
                {
                    if (maps[rouSub[i].x, rouSub[i].y].up == 1)
                    {
                        maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallUp").gameObject.SetActive(false);
                    }
                    if (maps[rouSub[i].x, rouSub[i].y].down == 1)
                    {
                        maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallDown").gameObject.SetActive(false);
                    }
                    if (maps[rouSub[i].x, rouSub[i].y].left == 1)
                    {
                        maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallLeft").gameObject.SetActive(false);
                    }
                    if (maps[rouSub[i].x, rouSub[i].y].right == 1)
                    {
                        maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallRight").gameObject.SetActive(false);
                    }
                }
            } 
            else if (rouSub[i].x == rou[rou.Count-1].x && rouSub[i].y == rou[rou.Count-1].y) // 该位置已有房间，但该房间是终点房间，不重新加载
            {
                if (maps[rouSub[i].x, rouSub[i].y].up == 1)
                {
                    maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallUp").gameObject.SetActive(false);
                }
                if (maps[rouSub[i].x, rouSub[i].y].down == 1)
                {
                    maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallDown").gameObject.SetActive(false);
                }
                if (maps[rouSub[i].x, rouSub[i].y].left == 1)
                {
                    maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallLeft").gameObject.SetActive(false);
                }
                if (maps[rouSub[i].x, rouSub[i].y].right == 1)
                {
                    maps[rouSub[i].x, rouSub[i].y].Tile.transform.Find("Grid").Find("Tilemap WallRight").gameObject.SetActive(false);
                }
            } 
            else // 该位置已有房间，但现在类型做了更改，需要重新加载
            {
                //maps[rouSub[i].x, rouSub[i].y].Tile.SetActive(false);
                //Destroy(maps[rouSub[i].x, rouSub[i].y].Tile);
                maps[rouSub[i].x, rouSub[i].y].Tile.GetComponent<EnemyGenerator>().DestroyRoom();
                maps[rouSub[i].x, rouSub[i].y].Tile =
                    Instantiate(room, new Vector3(rouSub[i].x * 18 * .99f, rouSub[i].y * 12 * .99f, 0f), Quaternion.identity);
            }

            maps[rouSub[i].x, rouSub[i].y].Tile.GetComponent<Room>().x = rouSub[i].x;
            maps[rouSub[i].x, rouSub[i].y].Tile.GetComponent<Room>().y = rouSub[i].y;
        }

        Debug.Log("blankNum = " + blankNum);
    }

    void processMainPath()
    {
        if (generateTime >= rou.Count)
            return;
        int roomType = maps[rou[generateTime].x, rou[generateTime].y].type;
        //Debug.Log("gT/Cnt : " + generateTime + "/" + rou.Count);

        Image tile =
                Instantiate(tileImage, mainDisplayPanel.transform);
        tile.transform.position +=
            new Vector3(
                (rou[generateTime].x - (Size - 1) / 2f) * tile.rectTransform.rect.width,
                (rou[generateTime].y - (Size - 1) / 2f) * tile.rectTransform.rect.height,
                0f);
        tile.sprite = roomManager.roomTempletes[roomType];

        if (generateTime == 0)
        {
            Image startTile =
                Instantiate(tileImage, signPanel.transform);
            startTile.transform.position +=
            new Vector3(
                (rou[generateTime].x - (Size - 1) / 2f) * tile.rectTransform.rect.width,
                (rou[generateTime].y - (Size - 1) / 2f) * tile.rectTransform.rect.height,
                0f);
            startTile.sprite = roomManager.startSign;
        } else if (generateTime == rou.Count - 1)
        {
            Image endTile =
                Instantiate(tileImage, signPanel.transform);
            endTile.transform.position +=
            new Vector3(
                (rou[generateTime].x - (Size - 1) / 2f) * tile.rectTransform.rect.width,
                (rou[generateTime].y - (Size - 1) / 2f) * tile.rectTransform.rect.height,
                0f);
            endTile.sprite = roomManager.endSign;
        }

        generateTime++;
    }

    void processSubPath()
    {
        if (generateTime >= rouSub.Count)
            return;
        int roomType = maps[rouSub[generateTime].x, rouSub[generateTime].y].type;
        Image tile =
                Instantiate(tileImage, subDisplayPanel.transform);
        tile.transform.position +=
            new Vector3(
                (rouSub[generateTime].x - (Size - 1) / 2f) * tile.rectTransform.rect.width,
                (rouSub[generateTime].y - (Size - 1) / 2f) * tile.rectTransform.rect.height,
                0f);
        tile.sprite = roomManager.roomTempletes[roomType + 16];

        if (generateTime == 0)
        {
            Image specialTile =
                Instantiate(tileImage, signPanel.transform);
            specialTile.transform.position +=
            new Vector3(
                (rouSub[generateTime].x - (Size - 1) / 2f) * tile.rectTransform.rect.width,
                (rouSub[generateTime].y - (Size - 1) / 2f) * tile.rectTransform.rect.height,
                0f);
            specialTile.sprite = roomManager.specialSign;
        }

        generateTime++;
    }

    void stopProcessMainPath()
    {
        CancelInvoke("processMainPath");

        // disable button
        mainPathButton.interactable = false;
        subPathButton.interactable = true;
        startButton.interactable = true;
    }

    void stopProcessSubPath()
    {
        CancelInvoke("processSubPath");

        // disable button conditionally
        if (blankNum == 0)
            subPathButton.interactable = false;
        else
            subPathButton.interactable = true;
        startButton.interactable = true;
    }

    public void skipGenerating()
    {
        //player.SetActive(true);
        //player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<CharacterController2D>().UnfreezePlayer();
        player.GetComponent<CharacterController2D>().invincible = false;
        generatingCanvas.SetActive(false);
        UIController.Instance.transform.Find("Canvas").gameObject.SetActive(true);

        isEntered = true;

        locationTile = Instantiate(tileImage, signPanel.transform);
        locationTile.sprite = roomManager.locSign;

        
    }
}
