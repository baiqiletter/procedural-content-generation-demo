using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayer : MonoBehaviour
{
    private float detectDistance;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        detectDistance = GameManager.Instance.roomDetectDistance;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < detectDistance)
        {
            gameObject.GetComponent<EnemyGenerator>().enabled = true;
        }
    }
}
