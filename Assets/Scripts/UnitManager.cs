using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    public GameObject player;
    public GameObject[] enemy;
    public GameObject[] boss;
    public GameObject[] npc;

    void Awake()
    {
        player = GameObject.Find("Player");
        print("UnitManager awake");
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
