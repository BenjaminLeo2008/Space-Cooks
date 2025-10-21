using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosControlScript : MonoBehaviour
{
    
public float spawnInterval = 5f;
public List<GameObject> orderPrefabs;
public RectTransform orderSpawnPoint;
public float offsetBetweenOrders = 50f;
public float orderTime = 10f;

private Vector2 nextSpawnPoint;

// Start se llama antes de la primera actualización del frame
void Start()
{
// Inicializa el punto de spawn con la posición anclada del RectTransform
nextSpawnPoint = orderSpawnPoint.anchoredPosition;

StartCoroutine(SpawnOrders());
}

IEnumerator SpawnOrders()
{
// Bucle infinito para seguir generando pedidos
while (true)
{
// Espera el tiempo definido antes de crear el próximo pedido
yield return new WaitForSeconds(spawnInterval);

if (orderPrefabs.Count > 0)
{
                // Elige un prefab de forma aleatoria
                int randomIndex = Random.Range(0, orderPrefabs.Count);
                GameObject newOrder = Instantiate(orderPrefabs[randomIndex]);
                RectTransform newOrderRect = newOrder.GetComponent<RectTransform>();
                Debug.Log("La posicion del proximo pedido sera: " + nextSpawnPoint);
                Debug.Log("Escala del nuevo pedido: " + newOrder.transform.localScale);

                newOrderRect.anchoredPosition = nextSpawnPoint;
                newOrder.transform.SetParent(orderSpawnPoint.parent, false);

                PedidosScript pedidoScript = newOrder.GetComponent<PedidosScript>();
                if (pedidoScript != null)
                {
                    pedidoScript.StartTimer(orderTime);
                }

                nextSpawnPoint.x += offsetBetweenOrders;


// Llama a StartTimer() en el pedido recién creado
if (pedidoScript != null)
{
    pedidoScript.StartTimer(orderTime);
}

// Actualiza el punto de spawn para que el próximo pedido se mueva a la derecha
nextSpawnPoint.x += offsetBetweenOrders;
}
}
}
}
