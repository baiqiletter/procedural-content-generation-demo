using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // map size ( room account )
    public int mapWidth = 1;
    public int mapHeight = 1;

    // room arrays
    public GameObject[] rooms0001;
    public GameObject[] rooms0010;
    public GameObject[] rooms0011;
    public GameObject[] rooms0100;
    public GameObject[] rooms0101;
    public GameObject[] rooms0110;
    public GameObject[] rooms0111;
    public GameObject[] rooms1000;
    public GameObject[] rooms1001;
    public GameObject[] rooms1010;
    public GameObject[] rooms1011;
    public GameObject[] rooms1100;
    public GameObject[] rooms1101;
    public GameObject[] rooms1110;
    public GameObject[] rooms1111;

    // room templates
    public Sprite[] roomTempletes;
    public Sprite startSign;
    public Sprite endSign;
    public Sprite locSign;
}
