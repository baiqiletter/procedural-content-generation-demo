using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject aboutPanel;

    public void ShowRank()
    {

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
        
    }
}
