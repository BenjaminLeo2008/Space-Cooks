using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Config de la grilla")]
    public Vector2Int size = new Vector2Int(5, 5);
    public Vector2Int offset = Vector2Int.zero;
    public float cellSize = 1f;

    private Grid2D<int> grid;

    void Start()
    {
        // Le paso los valores públicos del inspector a tu clase
        grid = new Grid2D<int>(size, offset);

        // Ejemplo de uso
        grid[1, 2] = 10;
        Debug.Log("Valor en (1,2): " + grid[1, 2]);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Dibujo cada celda como un cuadrito
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 pos = new Vector3(x + offset.x, 0, y + offset.y) * cellSize;
                Gizmos.DrawWireCube(pos, Vector3.one * cellSize);
            }
        }
    }
    void update()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();

// Posición del objeto en el mundo
Vector3 objectPosition = transform.position;

// Calcula los índices de la celda de la grilla
int gridX = Mathf.FloorToInt((objectPosition.x - offset.x) / cellSize);
int gridY = Mathf.FloorToInt((objectPosition.z - offset.y) / cellSize); 
// Índices de la celda deseada
int targetX = 2;
int targetY = 3;

// Calcula la posición en el mundo para el centro de la celda
Vector3 targetPosition = new Vector3(
    targetX * cellSize + offset.x + (cellSize / 2f),
    0f, // Mantén la posición Y en 0, o en la altura deseada
    targetY * cellSize + offset.y + (cellSize / 2f)
);

// Mueve el objeto a la nueva posición
transform.position = targetPosition;
    }
}
