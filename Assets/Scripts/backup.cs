//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//struct Map
//{
//    public int level;
//    public GameObject Tile;
//    public int select;
//    public int type;
//    public bool up;
//    public bool down;
//    public bool left;
//    public bool right;
//};

//struct Route
//{

//    public int x;
//    public int y;
//};



//public class MainPathGenerator : MonoBehaviour
//{
//    public MapGenerator mapGenerator;

//    // Start is called before the first frame update
//    private Map[,] arr = new Map[4, 4];

//    List<Route> rou = new List<Route>();

//    List<int> a = new List<int>();
//    List<int> b = new List<int>();

//    int len = 4;

//    void Start()
//    {
//        mapGenerator = GetComponent<MapGenerator>();

//        for (int i = 0; i < len; i++)
//        {
//            for (int j = 0; j < len; j++)
//            {
//                arr[i, j].select = 0;
//            }
//        }


//        init(len);
//        testPrint();

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    void init(int len)
//    {
//        //第一行选取起始位置
//        int entrance = Random.Range(0, 4);

//        Route nextNode;
//        nextNode.x = 0;
//        nextNode.y = entrance;
//        arr[0, entrance].select = 1;
//        rou.Add(nextNode);
//        print("start=" + entrance);
//        int k = 0;
//        while (true)
//        {

//            getRand(a); //0上，1下，2左，3右
//            bool flag = false;
//            //print("("+b[0]+","+b[1]+","+b[2]+","+b[3]+")");           
//            for (int i = 0; i < 4; i++)
//            {
//                //print("b="+b.Count);
//                if (canSelect(b[i], rou[k].x, rou[k].y, len) == true) //可以扩展
//                {
//                    if (b[i] == 0)
//                    {
//                        nextNode.x = rou[k].x - 1;
//                        nextNode.y = rou[k].y;
//                    }

//                    if (b[i] == 1)
//                    {
//                        nextNode.x = rou[k].x;
//                        nextNode.y = rou[k].y - 1;
//                    }

//                    if (b[i] == 2)
//                    {
//                        nextNode.x = rou[k].x + 1;
//                        nextNode.y = rou[k].y;
//                    }

//                    if (b[i] == 3)
//                    {
//                        nextNode.x = rou[k].x;
//                        nextNode.y = rou[k].y + 1;
//                    }

//                    arr[nextNode.x, nextNode.y].select = 1;
//                    rou.Add(nextNode);
//                    flag = true;
//                    break;
//                }
//            }

//            if (nextNode.x == len - 1 || flag == false) //寻路到最后一行或者无法扩展
//            {
//                break;
//            }

//            k++;
//        }
//    }

//    void getRand(List<int> a) //随机排序
//    {
//        for (int i = 0; i < len; i++)
//        {
//            a.Add(i);
//        }
//        b.Clear();
//        int countNum = a.Count;
//        while (b.Count < countNum)
//        {
//            int index = Random.Range(0, a.Count); ;
//            if (!b.Contains(a[index]))
//            {
//                b.Add(a[index]);
//                a.Remove(a[index]);
//            }
//        }
//    }

//    bool canSelect(int dir, int x, int y, int len)
//    {
//        if (dir == 0)//上
//        {
//            if (x - 1 >= 0)
//            {
//                if (arr[x - 1, y].select == 0) return true;
//            }
//        }
//        if (dir == 1)//左
//        {
//            if (y - 1 >= 0)
//            {
//                if (arr[x, y - 1].select == 0) return true;
//            }
//        }
//        if (dir == 2)//下
//        {

//            if (x + 1 < len)
//            {
//                if (arr[x + 1, y].select == 0) return true;
//            }
//        }
//        if (dir == 3)//右
//        {
//            if (y + 1 < len)
//            {
//                if (arr[x, y + 1].select == 0) return true;
//            }
//        }
//        return false;
//    }

//    //通路保存在rou结构体中，矩阵在arr里
//    void testPrint()
//    {
//        for (int i = 0; i < rou.Count; i++)
//        {
//            print("(" + rou[i].x + "," + rou[i].y + ")");

//        }
//    }

//    //
//}
