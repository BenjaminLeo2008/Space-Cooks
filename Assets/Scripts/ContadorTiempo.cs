using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ContadorTiempo : MonoBehaviour
{
    public int min, seg;
    public TextMeshProUGUI TimerTxt;

    private float restante;
    private bool enMarcha;

    private void Awake()
    {
        restante = (min * 60) + seg;
        enMarcha = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enMarcha)
        {
            if (restante > 0)
            {
                restante -= Time.deltaTime;
            
            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            TimerTxt.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
            }
            else
            {
                restante = 0;
                enMarcha = false;
                TimerTxt.text = string.Format("{00:00}:{01:00}", 00 ,00);
            }
        }
    }
}
