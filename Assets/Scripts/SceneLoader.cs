using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider loadBar;
    [SerializeField] private GameObject LoadPanel;

    public void SceneLoad(int sceneIndex)
    {
        LoadPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {
            Debug.Log(asyncOperation.progress);
            loadBar.value = asyncOperation.progress;
            yield return null;
        }

    }

    // link del video: https://youtu.be/cRavxipZQ3s?si=eBl_huWqM4YIhDxK
}



