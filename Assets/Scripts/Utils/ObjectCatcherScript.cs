using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectCatcherScript : MonoBehaviour
{

    //[Header("New settings")]
    //[SerializeField] private Transform objectPosTransform;

    [Header("Settings")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private Vector3 offset;

    private SphereCollider _col;
    private GameObject _pickedObject;
    private Rigidbody _caughtRigidbody;
    private IngredientData _ingredientData;
    private Vector3 originalScale;
    
    #region PUBLIC API
    public GameObject ObjectToPickUp;
    public GameObject pickedObject;
    public Transform interactionZone;
    public Transform superficieTransform;
    public Transform finalSuperficieTransform;
    public GameObject PickedObject => _pickedObject;
    public IngredientData IngredientData => _ingredientData;
    #endregion


    // Propiedad pública para que otros scripts puedan leer el objeto atrapado.

    private PickableObject _currentPickableObjectScript;

    // Nuevo método público para que otros scripts puedan establecer el objeto atrapado.
    public void SetPickedObject(GameObject newObject)
    {
        _pickedObject = newObject;
        if (newObject != null)
        {
            _caughtRigidbody = newObject.GetComponent<Rigidbody>();
            _currentPickableObjectScript = newObject.GetComponent<PickableObject>();
            _ingredientData = _currentPickableObjectScript.Data;
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
        PickObject();
    }

    // Este método se activa cuando un Collider entra en el trigger
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer != LayerMask.NameToLayer("Object"))
        {
            Debug.Log("no objetopp");
            return;
        }

        PickableObject pickable = other.gameObject.GetComponent<PickableObject>();
        Debug.Log("Sanguche de migaaaaaaaaaaa"); 

        if (pickable && !pickable.IsPicked)
        {
            Debug.Log("Objecttovich");
            _pickedObject = other.gameObject;
            _ingredientData = _currentPickableObjectScript.Data;
            _currentPickableObjectScript = other.GetComponent<PickableObject>();
            _caughtRigidbody = pickable.Rb;
            pickable.transform.position = superficieTransform.position;
            pickable.Rb.isKinematic = true;
           
        }
            // logica para enchufar el objeto a un transform en especifico.
    }
        
        /*

        // Detecta el objeto solo si no estamos sosteniendo uno y si tiene el tag correcto
        if (_pickedObject == null)
        {
            // Obtiene el Rigidbody del objeto que entró
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Si el objeto tiene un Rigidbody, lo "atrapamos"
                _pickedObject = other.gameObject;
                _caughtRigidbody = rb;
                _currentPickableObjectScript = other.GetComponent<PickableObject>();
                _ingredientData = _currentPickableObjectScript.Data;
                // Lo hacemos cinemático para que no sea afectado por las físicas
                _caughtRigidbody.isKinematic = true;
            }
        }
        */

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
            _ingredientData = null;
            _currentPickableObjectScript = null;
            _caughtRigidbody = null;
        }
    }

    void PickObject()
    {
        if (_pickedObject.layer != LayerMask.NameToLayer("Object"))
            return;

        // Solo ejecuta la lógica si un objeto ha sido atrapado
        if (_pickedObject != null)
        {
            // Opcional: Asegúrate de que el objeto no tiene un padre (esto ayuda a evitar problemas)
            if (_pickedObject.transform.parent == null)
            {
                // Verifica que la superficie exista y el objeto tenga el tag correcto
                if (superficieTransform != null && _pickedObject.layer == LayerMask.GetMask("Object"))
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
