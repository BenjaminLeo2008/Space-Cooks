using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] public ObjectCatcherScript objectCatcher;

    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform PlayerInteractionZone;

    [SerializeField] private float detectionRadius;
    [SerializeField] private SphereCollider col;
    private GameObject _closestTile;
    [Tooltip("El objeto Tile más cercano en el radio de detección.")]
    private GameObject _previousClosestTile;
    [SerializeField] private Color highlightColor = Color.white;
    [Tooltip("La intensidad del resaltado, de 0.0 (tenue) a 1.0 (fuerte).")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float highlightIntensity = 0.3f;

    private Dictionary<GameObject, Color> _originalColors = new Dictionary<GameObject, Color>();
    private List<GameObject> _tileCloseList = new List<GameObject>();

    private Vector3 originalScale;

    // Nueva variable para guardar la referencia al script del objeto.
    private ObjectCatcherScript _caughtCatcherScript;
    private SphereCollider _caughtCol;


    void Start()
    {
        col = GetComponent<SphereCollider>();

        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }

        col.radius = detectionRadius;
        col.isTrigger = true;
    }


    private void IsPlateInFront()
    {
        
    }

    void Update()
    {
        if (ObjectToPickUp && ObjectToPickUp.GetComponent<PickableObject>().IsPickable && PickedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                pickUpObject();
                Debug.Log("toco la F");
            }
        }
        else if (PickedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                dropObject();
            }
        }

    

        

        // Si hay tiles detectados, encuentra el más cercano.
        if (_tileCloseList.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            GameObject tempWinnerTile = null;

            // Recorre la lista para encontrar el tile más cercano.
            foreach (GameObject tile in _tileCloseList)
            {
                if (tile != null)
                {
                    float distance = Vector3.Distance(transform.position, tile.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        tempWinnerTile = tile;
                    }
                }
            }
            _closestTile = tempWinnerTile;
        }
        else
        {
            // Si la lista está vacía, no hay un tile ganador.
            _closestTile = null;
        }
        // --- Lógica para el cambio de color ---
        // Comprueba si el tile ganador ha cambiado
        if (_closestTile != _previousClosestTile)
        {
            // Si el tile anterior no era nulo, restablece su color original
            if (_previousClosestTile != null)
            {
                ResetTileColor(_previousClosestTile);
            }

            // Si hay un nuevo tile ganador, resáltalo
            if (_closestTile != null)
            {
                Highlight_closestTile(_closestTile);
            }
        }

        // Actualiza el tile anterior para el siguiente fotograma
        _previousClosestTile = _closestTile;
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el objeto que entra tiene el tag "Tile" y no está ya en la lista, lo añade.
        if (other.CompareTag("Tile") && !_tileCloseList.Contains(other.gameObject))
        {
            _tileCloseList.Add(other.gameObject);
            // Almacena el color original del tile.
            Renderer tileRenderer = other.GetComponentInChildren<Renderer>();
            if (tileRenderer != null && tileRenderer.material != null)
            {
                // Solo guarda el color si aún no lo hemos hecho.
                if (!_originalColors.ContainsKey(other.gameObject))
                {
                    _originalColors.Add(other.gameObject, tileRenderer.material.color);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el objeto que sale tiene el tag "Tile" y está en la lista, lo elimina.
        if (other.CompareTag("Tile") && _tileCloseList.Contains(other.gameObject))
        {
            ResetTileColor(other.gameObject);
            _tileCloseList.Remove(other.gameObject);
            // Elimina el color del diccionario al salir.
            if (_originalColors.ContainsKey(other.gameObject))
            {
                _originalColors.Remove(other.gameObject);
            }
        }
    }

    private void pickUpObject()
    {
       PickedObject = ObjectToPickUp;
       PickedObject.GetComponent<PickableObject>().IsPickable = false;

      // Guarda la escala original del objeto antes de modificarla.
       originalScale = PickedObject.transform.localScale;

        Rigidbody PickedObjRb = PickedObject.GetComponent<Rigidbody>();

        if (PickedObjRb) {
            Debug.Log("Picking Object: " + PickedObject.gameObject.name);
        }

        //PickedObject.GetComponent<Rigidbody>().useGravity = false;
        //PickedObject.GetComponent<Rigidbody>().isKinematic = true;
        PickedObject.transform.position = PlayerInteractionZone.transform.position;
        PickedObject.transform.SetParent(PlayerInteractionZone);
        PickedObject.transform.localRotation = Quaternion.identity;
                
        PickedObject.transform.localScale = new Vector3 (
            originalScale.x / PlayerInteractionZone.lossyScale.x,
            originalScale.y / PlayerInteractionZone.lossyScale.y,
            originalScale.z / PlayerInteractionZone.lossyScale.z
        );
    }

    private void dropObject()
    {
        PickedObject.GetComponent<PickableObject>().IsPickable = true;
        PickedObject.transform.SetParent(null);
        PickedObject.GetComponent<Rigidbody>().useGravity = true;
        PickedObject.GetComponent<Rigidbody>().isKinematic = false;

        PickedObject.transform.localScale = originalScale;

        PickedObject = null;
    }
    
    /// Cambia el color del tile a un tono más claro.
    private void Highlight_closestTile(GameObject tile)
    {
        if (tile != null)
        {
            Renderer tileRenderer = tile.GetComponentInChildren<Renderer>();
            if (tileRenderer != null && tileRenderer.material != null)
            {
                if (_originalColors.ContainsKey(tile))
                {
                    Color originalColor = _originalColors[tile];
                    // Mezcla el color original con el color de resaltado.
                    tileRenderer.material.color = Color.Lerp(originalColor, highlightColor, highlightIntensity);
                }
            }
        }
    }

    /// <summary>
    /// Restablece el color del tile a su valor original (asumiendo que era blanco).
    /// </summary>
    /// <param name="tile">El GameObject del tile a restablecer.</param>
    private void ResetTileColor(GameObject tile)
    {
        if (tile != null)
        {
            Renderer tileRenderer = tile.GetComponentInChildren<Renderer>();
            if (tileRenderer != null && tileRenderer.material != null)
            {
                // Restablece el color usando el valor guardado en el diccionario.
                if (_originalColors.ContainsKey(tile))
                {
                    tileRenderer.material.color = _originalColors[tile];
                }
            }
        }
    }
}
