using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Requests : MonoBehaviour
{
    public Image timer_linear_image;
    private float TiempoInicial = 40f;
    public float _tiempoRestante;
    // Start is called before the first frame update
    void Start()
    {
        _tiempoRestante = TiempoInicial;
    }

    // Update is called once per frame
    void Update()
    {
        if (_tiempoRestante > 0)
        {
            _tiempoRestante -= Time.deltaTime;
            timer_linear_image.fillAmount = _tiempoRestante / TiempoInicial;
        }
    }
}
