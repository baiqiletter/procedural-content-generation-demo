using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextStage : MonoBehaviour
{
    private bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered && collision.gameObject.tag == "Player")
        {
            isTriggered = true;

            GameManager.Instance.killNumTotal += GameManager.Instance.killNum;
            GameManager.Instance.passRoomNumTotal += GameManager.Instance.passRoomNum;
            LevelFinish.Instance.levelText.text = "--------------------\nLevel - " + GameManager.Instance.level + "\nFinished\n--------------------";
            LevelFinish.Instance.text1.text = "You just killed " + GameManager.Instance.killNum + " enemies, " + GameManager.Instance.killNumTotal + " in total";
            LevelFinish.Instance.text3.text = "You just passed " + GameManager.Instance.passRoomNum + " rooms, " + GameManager.Instance.passRoomNumTotal + " in total";
            GameManager.Instance.killNum = 0;
            GameManager.Instance.passRoomNum = 0;
            GameManager.Instance.InitReached();

            UIController.Instance.transform.Find("Canvas").gameObject.SetActive(false);
            LevelFinish.Instance.transform.Find("Canvas").gameObject.SetActive(true);

            CharacterController2D player = GameObject.Find("Player").GetComponent<CharacterController2D>();
            player.FreezePlayer();
        }
    }
}
