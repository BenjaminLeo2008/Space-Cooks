using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosScript : MonoBehaviour
{
public List<Image> timeBars;
private float totalTime;

private float remainingTime;

void Start()
{
    totalTime = 20f;
    remainingTime = 20f;
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
    Destroy(gameObject);
}
}
}
