using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Image HPImage;
    public Text HPText;

    void Awake()
    {
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
        // set HP Bar
        HPImage.fillAmount = GameManager.Instance.health / GameManager.Instance.maxHealth;
        HPText.text = "HP " + GameManager.Instance.health + " / " + GameManager.Instance.maxHealth;
    }
}
