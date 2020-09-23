using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject aboutPanel;
    public GameObject rankingPanel;

    public void ShowRank()
    {
        rankingPanel.SetActive(true);
        rankingPanel.transform.Find("Text1").GetComponent<Text>().text = "1st		" + PlayerPrefs.GetInt("rank1");
        rankingPanel.transform.Find("Text2").GetComponent<Text>().text = "2nd		" + PlayerPrefs.GetInt("rank2");
        rankingPanel.transform.Find("Text3").GetComponent<Text>().text = "3rd		" + PlayerPrefs.GetInt("rank3");
        rankingPanel.transform.Find("Text4").GetComponent<Text>().text = "4th		" + PlayerPrefs.GetInt("rank4");
        rankingPanel.transform.Find("Text5").GetComponent<Text>().text = "5th		" + PlayerPrefs.GetInt("rank5");
    }

    public void ShowAbout()
    {
        aboutPanel.SetActive(true);
    }

    public void ExitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void ReturnMenu()
    {
        aboutPanel.SetActive(false);
        rankingPanel.SetActive(false); 
    }
}
