using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidosControlScript : MonoBehaviour
{
    public float spawnInterval = 10f;
    public List<GameObject> orderPrefabs;
    public Transform orderSpawnPoint;
    public float offsetBetweenOrders = 2f;
    private Vector3 nextSpawnPoint;

        void Start()
    {
        nextSpawnPoint = orderSpawnPoint.position;
        StartCoroutine(SpawnOrders());
    }
    IEnumerator SpawnOrders()
    {
        while (true) // bucle infinito para seguir generando pedidos
        {
            //GameObject newOrder = Instantiate(orderPrefabs[randomIndex], nextSpawnPoint, Quaternion.identity);
            //nextSpawnPoint.x += offsetBetweenOrders;
            yield return new WaitForSeconds(spawnInterval);
            if (orderPrefabs.Count > 0)
            {
                int randomIndex = Random.Range(0, orderPrefabs.Count);
                GameObject newOrder = Instantiate(orderPrefabs[randomIndex], nextSpawnPoint, Quaternion.identity);
                nextSpawnPoint = new Vector3(nextSpawnPoint.x + offsetBetweenOrders, nextSpawnPoint.y, nextSpawnPoint.z);
            }
        }
    }

    //void GenerateRandomOrder()
   // {
   //     int randomIndex = Random.Range(0, orderPrefabs.Count);
    //    Instantiate(orderPrefabs[randomIndex], orderSpawnPoint.position, Quaternion.identity);
   // }
}
