using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public Image timer_linear_image;

    public float tiempoInicialSeg;
    private float tiempoRestante;

    void Start()
    {
        tiempoRestante = tiempoInicialSeg;
        if (timer_linear_image == null)
        {
            Debug.Log("La imagen no esta asignada vro");
        }
        else
        {
            timer_linear_image.fillAmount = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;
            timer_linear_image.fillAmount = tiempoRestante / tiempoInicialSeg;


            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                timer_linear_image.fillAmount = 0;
                Debug.Log("El temporizador ha terminado.");
            }
        }

    }
}
