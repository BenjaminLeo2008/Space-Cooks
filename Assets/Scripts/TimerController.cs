using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//1
public class TimerController : MonoBehaviour
{//2
    public Image timer_linear_image;
    //3
    public float tiempoInicialSeg;
    private float tiempoRestante;
    //4
    void Start()
    {//5
        tiempoRestante = tiempoInicialSeg;
        if (timer_linear_image == null)
        {//6
            Debug.Log("La imagen no esta asignada vro");
        }//7
        else
        {//8
            timer_linear_image.fillAmount = 1f;
        }//9
    }//10
    //11
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
//1//1