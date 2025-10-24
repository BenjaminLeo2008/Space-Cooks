using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosScript : MonoBehaviour
{
    public List<Image> timeBars;
    public PedidosControlScript controlDePedidos;
    public string pedidoID;
    private float totalTime;
    private float remainingTime;

    void Start()
    {
        controlDePedidos = FindObjectOfType<PedidosControlScript>();
        if (string.IsNullOrEmpty(pedidoID))
        {
            pedidoID = System.Guid.NewGuid().ToString();
        }

        if (!controlDePedidos.activeOrders.ContainsKey(pedidoID))
        {
            controlDePedidos.activeOrders.Add(pedidoID, gameObject);
        }

        if (totalTime == 0)
        {
            totalTime = 15f;
            remainingTime = 15f;
        }
    }
    // StartTimer se llama desde PedidosControlScript para inicializar el pedido
    public void StartTimer(float maxTime)
    {
        Debug.Log("El temporizador del pedido ha comenzado. Duracion total: " + maxTime);
        totalTime = maxTime;
        remainingTime = maxTime;

        // Reinicia la barra de tiempo
        foreach (Image bar in timeBars)
            {
                bar.fillAmount = 1f;
            }
        }

    // Update se llama una vez por frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            float fillAmount = remainingTime / totalTime;

            foreach (Image bar in timeBars)
            {
                bar.fillAmount = fillAmount;
            }
        }
        else
        {
            Debug.Log("El pedido se destruye!");
            controlDePedidos.activeOrders.Remove(pedidoID);
            Destroy(gameObject);
        }
    }
}
