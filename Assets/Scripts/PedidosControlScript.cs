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

    public float orderTime = 10f;
    private int waitingRecipesMax = 6;

    private List<GameObject> activeOrders = new List<GameObject>();


    // Start se llama antes de la primera actualización del frame
    void Start()
{
    StartCoroutine(SpawnOrders());
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

    Debug.Log("Escala del nuevo pedido: " + newOrderGO.transform.localScale);
    Debug.Log($"Pedido generado. Total activo: {activeOrders.Count}/{waitingRecipesMax}");
}
}
}
}
