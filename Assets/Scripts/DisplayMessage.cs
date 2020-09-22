using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMessage : MonoBehaviour
{
    public GameObject messagePrefab;
    public float stayTime = 1.5f;
    public float distToTop = 200f;
    public float verticalGap = 50f;

    private bool isDisplaying;
    private Queue<GameObject> messageQueue;

    // Start is called before the first frame update
    void Start()
    {
        isDisplaying = false;
        messageQueue = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach(GameObject messageText in messageQueue)
        {
            //messageText.transform.position =
            //    new Vector3(
            //        messageText.transform.position.x,
            //        -distToTop - i * verticalGap,
            //        0);
            messageText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -distToTop - i * verticalGap);
            i++;
            //print(messageText.transform.position);
        }
    }

    public void SendToMessage(string message)
    {
        //print("display message");
        GameObject messageText = Instantiate(messagePrefab, transform.Find("Canvas"));
        messageText.GetComponent<Text>().text = message;
        messageText.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, 1f);
        messageText.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, 1f);
        messageQueue.Enqueue(messageText);
        StartCoroutine(RemoveTopMessage());
    }

    IEnumerator RemoveTopMessage()
    {
        yield return new WaitForSeconds(stayTime);
        Destroy(messageQueue.Dequeue());
    }
}
