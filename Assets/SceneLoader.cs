using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{ // Hola codis
    public GameObject loaderUI;
    public Image BarraAzul;

    public void LoadScene(int index)
    {
        StartCoroutine(LoadScene_Coroutine(index));
    }
    public IEnumerator LoadScene_Coroutine(int index)
    {
        BarraAzul.fillAmount = 0f;
        loaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        float progress = 0f;

        while (asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            BarraAzul.fillAmount = progress;
            if (progress >= 0.9f)
            {
                BarraAzul.fillAmount = 1;
                asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
