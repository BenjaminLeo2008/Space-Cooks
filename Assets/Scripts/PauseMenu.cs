using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject Pausa;
    private bool gameIsPaused = false;
    public GameObject image3;
    public GameObject image2;
    public GameObject image1;
    public GameObject imageACocinar;

    public MonoBehaviour PlayerController;
    public MonoBehaviour ContadorTiempo;

    void Start()
    {
        Time.timeScale = 0f;
        PlayerController.enabled = false;
        ContadorTiempo.enabled = false;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        float intervalTime = 1.0f;

        image3.SetActive(true);
        yield return new WaitForSecondsRealtime(intervalTime);
        image3.SetActive(false);

        image2.SetActive(true);
        yield return new WaitForSecondsRealtime(intervalTime);
        image2.SetActive(false);

        image1.SetActive(true);
        yield return new WaitForSecondsRealtime(intervalTime);
        image1.SetActive(false);

        imageACocinar.SetActive(true);
        yield return new WaitForSecondsRealtime(2.0f);
        imageACocinar.SetActive(false);

        PlayerController.enabled = true;
        ContadorTiempo.enabled = true;
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Pausa.SetActive(true);
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Pausa.SetActive(false);
        gameIsPaused = false;
    }

}