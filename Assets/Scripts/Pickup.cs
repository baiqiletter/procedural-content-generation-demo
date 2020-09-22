using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float healthPercentageChange = 0f;
    public float maxHealthChange = 0f;
    public float attackChange = 0f;
    public float defendChange = 0f;
    public float rare = 3f;

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
        if (collision.gameObject.tag == "Player")
        {
            //print("pick");
            // change GameManager variables
            GameManager.Instance.health += GameManager.Instance.maxHealth * healthPercentageChange;
            GameManager.Instance.maxHealth += maxHealthChange;
            GameManager.Instance.attack += attackChange;
            GameManager.Instance.defend += defendChange;

            // send message to game ui
            if (healthPercentageChange != 0)
            {
                GameObject.Find("UI Controller").GetComponent<DisplayMessage>().SendToMessage("HP + " + GameManager.Instance.maxHealth * healthPercentageChange);
            }
            if (maxHealthChange != 0)
            {
                GameObject.Find("UI Controller").GetComponent<DisplayMessage>().SendToMessage("Max HP + " + maxHealthChange);
            }
            if (attackChange != 0)
            {
                GameObject.Find("UI Controller").GetComponent<DisplayMessage>().SendToMessage("Attack + " + attackChange);
            }
            if (defendChange != 0)
            {
                GameObject.Find("UI Controller").GetComponent<DisplayMessage>().SendToMessage("Defend + " + defendChange);
            }

            GameManager.Instance.itemNum++;
            GameManager.Instance.score += GameManager.Instance.itemScore;

            // destroy this pickup item
            Destroy(gameObject);
        }
    }
}
