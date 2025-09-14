using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosScript : MonoBehaviour
{
    public float tiempoPedido = 15f;
    private float tiempoRestantePedido;
    public Image pedidoImage;
    
    // Start is called before the first frame update
    void Start()
    {
        tiempoRestantePedido = tiempoPedido;
    }

    // Update is called once per frame
    void Update()
    {
                        if (tiempoRestantePedido > 0)
        {
            tiempoRestantePedido -= Time.deltaTime;
            pedidoImage.fillAmount = tiempoRestantePedido / tiempoPedido;
            }
    }
}
