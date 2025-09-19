using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectCatcherScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private Vector3 offset;

    private SphereCollider _col;
    public Transform superficieTransform;
    private GameObject _pickedObject;
    private Rigidbody _caughtRigidbody;

    public GameObject ObjectToPickUp;
    public GameObject pickedObject;
    public Transform interactionZone;
    private Vector3 originalScale;

    // Propiedad pública para que otros scripts puedan leer el objeto atrapado.
    public GameObject PickedObject => _pickedObject;

    // Nuevo método público para que otros scripts puedan establecer el objeto atrapado.
    public void SetPickedObject(GameObject newObject)
    {
        _pickedObject = newObject;
        if (newObject != null)
        {
            _caughtRigidbody = newObject.GetComponent<Rigidbody>();
        }
        else
        {
            _caughtRigidbody = null;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Obtiene o añade el SphereCollider
        _col = GetComponent<SphereCollider>();
        if (_col == null)
        {
            _col = gameObject.AddComponent<SphereCollider>();
        }

        // Configura el collider
        _col.radius = detectionRadius;
        _col.isTrigger = true;

        // Busca el GameObject "Superficie"
        if (superficieTransform == null)
        {
            Debug.LogError("No se encontró el hijo 'Superficie'. Asegúrate de que existe.");
        }
    }

    // El Update se encargará de posicionar el objeto solo si ya ha sido detectado
    void Update()
    {
        if (gameObject.CompareTag("Tile") || gameObject.CompareTag("Floor"))
        {
            // Solo ejecuta la lógica si un objeto ha sido atrapado
            if (_pickedObject != null)
            {
                // Opcional: Asegúrate de que el objeto no tiene un padre (esto ayuda a evitar problemas)
                if (_pickedObject.transform.parent == null)
                {
                    // Verifica que la superficie exista y el objeto tenga el tag correcto
                    if (superficieTransform != null && _pickedObject.CompareTag("Object") || _pickedObject.CompareTag("Plate"))
                    {
                        // Obtiene la altura del objeto con su collider
                        float objectHeight = _pickedObject.GetComponent<Collider>().bounds.extents.y;
                        Vector3 superficiePosition = superficieTransform.position;

                        // Calcula la nueva posición del objeto
                        Vector3 newPosition = new Vector3(
                            superficiePosition.x + offset.x,
                            superficiePosition.y + objectHeight + offset.y,
                            superficiePosition.z + offset.z
                        );

                        // Establece la rotación y posición
                        _pickedObject.transform.rotation = superficieTransform.rotation;
                        _pickedObject.transform.position = newPosition;
                    }
                }
            }
        }
    }

    // Este método se activa cuando un Collider entra en el trigger
    void OnTriggerEnter(Collider other)
    {
        // Detecta el objeto solo si no estamos sosteniendo uno y si tiene el tag correcto
        if (_pickedObject == null && other.gameObject.CompareTag("Object") || other.gameObject.CompareTag("Plate"))
        {
            // Obtiene el Rigidbody del objeto que entró
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Si el objeto tiene un Rigidbody, lo "atrapamos"
                _pickedObject = other.gameObject;
                _caughtRigidbody = rb;
                // Lo hacemos cinemático para que no sea afectado por las físicas
                _caughtRigidbody.isKinematic = true;
            }
        }
    }

    // Este método se activa cuando un Collider sale del trigger
    void OnTriggerExit(Collider other)
    {
        // Si el objeto que sale es el que teníamos atrapado
        if (other.gameObject == _pickedObject)
        {
            // Si el Rigidbody existe, lo volvemos a un estado no cinemático
            if (_caughtRigidbody != null)
            {
                _caughtRigidbody.isKinematic = false;
            }
            // Limpiamos las referencias
            _pickedObject = null;
            _caughtRigidbody = null;
        }
    }
}
