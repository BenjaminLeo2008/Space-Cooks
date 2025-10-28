using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosControlScript : MonoBehaviour
{
    public float spawnIntervalMin = 5f;
    public float spawnIntervalMax = 8f;  
    public List<GameObject> orderPrefabs;
    public RectTransform orderQueueContainer;

    private int waitingRecipesMax = 6;

    private List<GameObject> activeOrders = new List<GameObject>();


    // Start se llama antes de la primera actualización del frame
    void Start()
{
    StartCoroutine(SpawnOrders());
}
    public void QuitarPedidoActivo(GameObject pedidoExpirado)
    {
        if (activeOrders.Contains(pedidoExpirado))
        {
            //sacarlo de la lista al pedido
            activeOrders.Remove(pedidoExpirado);
            Debug.Log("Pedido expirado quitado de la lista. Pedidos activos restantes: " + activeOrders.Count);
            //spawn orders crea otro pedido
        }
    }
IEnumerator SpawnOrders()
{
    while (true)
    {
        float randomWaitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
        yield return new WaitForSeconds(randomWaitTime);

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
                PedidosScript scriptPedido = newOrderGO.GetComponent<PedidosScript>();

                if (scriptPedido != null)
                {
                    scriptPedido.pedidosControlScript = this;
                }

    Debug.Log("Escala del nuevo pedido: " + newOrderGO.transform.localScale);
    Debug.Log($"Pedido generado. Total activo: {activeOrders.Count}/{waitingRecipesMax}");
}
}
}
}
