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
}
