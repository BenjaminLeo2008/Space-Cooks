using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerController : MonoBehaviour
{
    public GameObject Tiempo_image;
    public Image timer_linear_image;
    float time_remaining;
    public int max_time = 60;

    void Start()
    {
        time_remaining = max_time;
    }

    // Update is called once per frame
    void Update()
    {
        if (time_remaining > 0)
        {
            time_remaining -= Time.deltaTime;
            timer_linear_image.fillAmount = time_remaining / max_time;
        }
        else
        {
            Tiempo_image.SetActive(true);
        }
    }
}
