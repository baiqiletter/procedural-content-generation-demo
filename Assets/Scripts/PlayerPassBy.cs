using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassBy : MonoBehaviour
{
    public GameObject room;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.posX = room.GetComponent<Room>().x;
            GameManager.Instance.posY = room.GetComponent<Room>().y;
        }
    }
}
