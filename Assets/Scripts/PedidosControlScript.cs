using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosControlScript : MonoBehaviour
{
    public float spawnInterval = 5f;
    public List<GameObject> orderPrefabs;
    //public RectTransform orderSpawnPoint;
    public RectTransform orderQueueContainer;
    public float offsetBetweenOrders = 50f;
    public float orderTime = 10f;

    //private Vector2 nextSpawnPoint;
    private int waitingRecipesMax = 6;
    private List<GameObject> activeOrders = new List<GameObject>();


    // Start se llama antes de la primera actualización del frame
    void Start()
{
// Inicializa el punto de spawn con la posición anclada del RectTransform
//nextSpawnPoint = orderSpawnPoint.anchoredPosition;

StartCoroutine(SpawnOrders());
}

IEnumerator SpawnOrders()
{
// Bucle infinito para seguir generando pedidos
while (true)
{
// Espera el tiempo definido antes de crear el próximo pedido
yield return new WaitForSeconds(spawnInterval);
            if (activeOrders.Count >= waitingRecipesMax)
                {
                Debug.Log("Limite de pedidos alcanzado. No se instancio un nuevo pedido.");
                continue;
                }

            if (orderPrefabs.Count > 0)
{
                // Elige un prefab de forma aleatoria
                int randomIndex = Random.Range(0, orderPrefabs.Count);
                GameObject newOrderGO = Instantiate(orderPrefabs[randomIndex], orderQueueContainer);

                activeOrders.Add(newOrderGO);
                RectTransform newOrderRect = newOrderGO.GetComponent<RectTransform>();
                PedidosScript pedidosScript = newOrderGO.GetComponent<PedidosScript>();

                Debug.Log("Escala del nuevo pedido: " + newOrderGO.transform.localScale);

                //newOrderRect.anchoredPosition = nextSpawnPoint;
                //newOrderGO.transform.SetParent(orderSpawnPoint.parent, false);

                PedidosScript pedidoScript = newOrderGO.GetComponent<PedidosScript>();
                if (pedidoScript != null)
                {
                    pedidoScript.StartTimer(orderTime);
                }

                Debug.Log($"Pedido generado. Total activo: {activeOrders.Count}/{waitingRecipesMax}");
                //nextSpawnPoint.x += offsetBetweenOrders;

// Actualiza el punto de spawn para que el próximo pedido se mueva a la derecha
}
}
}
}
