using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public GameObject TiempoUI;
    public Image timer_linear_image;

    public float tiempoInicial = 60f;
    private float tiempoRestante;
    public PlayerController player;
    public string escenaPuntaje = "EscenaPuntaje";

    void Start()
    {
        tiempoRestante = tiempoInicial;
        TiempoUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            timer_linear_image.fillAmount = tiempoRestante / tiempoInicial;
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                StartCoroutine(FinDelTiempo());
            }
        }

    }
    IEnumerator FinDelTiempo()
    {
        TiempoUI.SetActive(true);
        if (player != null)
        {
            player.enabled = false;
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(escenaPuntaje);
    }
}
