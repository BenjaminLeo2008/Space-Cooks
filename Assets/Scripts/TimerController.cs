using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public Image timer_linear_image;

    public float tiempoInicial = 60f;
    private float tiempoRestante;

    void Start()
    {
        tiempoRestante = tiempoInicial;
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
            }
        }

    }
}
