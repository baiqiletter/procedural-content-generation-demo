using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void LoadLevel(int sceneIndex)
    {
        //LevelFinish.Instance.transform.Find("GameOver Canvas").gameObject.SetActive(false);

        // load scene
        StartCoroutine(loadAsynchronously(sceneIndex));
        // change global value

        GameManager.Instance.level++;
        if (GameManager.Instance.level % 5 == 0)
        {
            GameManager.Instance.mapScale++;
        }
    }

    IEnumerator loadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;  // wait until next frame
        }

        //if (!UIController.Instance.transform.Find("Canvas").gameObject.activeSelf)
        //    UIController.Instance.transform.Find("Canvas").gameObject.SetActive(true);
        if (LevelFinish.Instance.transform.Find("Canvas").gameObject.activeSelf)
            LevelFinish.Instance.transform.Find("Canvas").gameObject.SetActive(false);

        GameObject.Find("Player").GetComponent<CharacterController2D>().invincible = true;
    }
}
