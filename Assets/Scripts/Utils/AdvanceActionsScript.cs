using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceActionsScript : MonoBehaviour
{
    [SerializeField] private IngredientData ingredienteInvolucrado;

    // Prefab que se puede entregar, agrégalo desde el Inspector.
    [SerializeField] private GameObject deliverablePrefab;

    // Diccionario para almacenar todos los prefabs cargados de Resources.
    private Dictionary<string, GameObject> myPrefabs = new Dictionary<string, GameObject>();

    // Diccionario para almacenar todos los IngredientData cargados de Resources.
    private Dictionary<string, IngredientData> allIngredientData = new Dictionary<string, IngredientData>();


    // Referencia al ObjectCatcherScript
    private ObjectCatcherScript objectCatcher;

    // Variable para rastrear si el jugador está dentro del área del collider.
    private bool _isPlayerInTrigger = false;

    #region PUBLIC API
    // ESTA VARIABLE DEBE CONTENER EL SCRIPTABLE OBJECT IngredientData del objeto recogido.
    public IngredientData currentIngredientData;

    // Nombre del GameObject para activar el reemplazo. Configúralo en el Inspector.
    public ETiposMesa tipoDeMesa;

    public PlayerController player;

    public List<GameObject> DeliverablePrefabs;

    public IngredientDatabase ingredientDatabase;

    public enum ETiposMesa {
        Crear,
        Cortar,
        Asar,
        Entregar,
        Desechar,
        Hervir,
        Lavar,
    }
    #endregion
    void Start()
    {
        // Obtiene la referencia al ObjectCatcherScript en el mismo GameObject
        objectCatcher = GetComponent<ObjectCatcherScript>();
        if (objectCatcher == null)
        {
            Debug.LogError("No se encontró ObjectCatcherScript en este GameObject. Asegúrate de que ambos scripts estén juntos.");
        }

        if (ingredientDatabase == null) return;
        foreach (IngredientData ingData in ingredientDatabase.allIngredients) {
            if (!ingData) continue;
            allIngredientData.Add(ingData.Name, ingData);
        }
    }

    void Update()
    {
        currentIngredientData = objectCatcher.IngredientData;
        // Elige la función a ejecutar basándose en el nombre del GameObject
        if (!(_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q))) return;

        bool hasAnObject = (objectCatcher.PickedObject != null);

        switch (tipoDeMesa) {
            case ETiposMesa.Crear:
                if (!hasAnObject) CreateObjectAndGrab();
                break;
            case ETiposMesa.Cortar:
                if (hasAnObject && currentIngredientData != null && currentIngredientData.SimpleNextState != null) 
                    StartCoroutine(ReplacePickedObjectDelayed(2f));
                else 
                    Debug.LogWarning("No se puede reemplazar. El objeto recogido no tiene SimpleNextState definido.");
                break;
            case ETiposMesa.Entregar:
                if (hasAnObject) DeliverObject();
                break;
            case ETiposMesa.Desechar:
                if (hasAnObject) DestroyObject();
                break;
            case ETiposMesa.Lavar:
                if (hasAnObject) StartCoroutine(WashPickedObjectDelayed(2f));
                break;
            case ETiposMesa.Asar:
                break;
            case ETiposMesa.Hervir:
                break;
            default:
                break;
        }
    }

    // Método para detectar si un objeto entra en el collider del GameObject.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInTrigger = true;
        }
    }

    // Método para detectar si un objeto sale del collider del GameObject.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInTrigger = false;
        }
    }

    // Corrutina para reemplazar el objeto después de un retraso
    private IEnumerator ReplacePickedObjectDelayed(float delay)
    {
        // El prefab a instanciar AHORA es el PrefabObject del SimpleNextState del IngredientData
        GameObject nextStatePrefab = currentIngredientData.SimpleNextState.PrefabObject;

        if (nextStatePrefab != null)
        {
            if (player != null)
            {
                player.enabled = false;
            }

            yield return new WaitForSeconds(delay);

            if (objectCatcher.PickedObject != null)
            {
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                Destroy(objectCatcher.PickedObject);

                // Usamos el PrefabObject de SimpleNextState
                GameObject newInstance = Instantiate(nextStatePrefab, currentPosition, currentRotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }

            if (player != null)
            {
                player.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning($"El SimpleNextState de {currentIngredientData.name} no tiene asignado un PrefabObject.");
        }
    }
    // Corrutina para lavar el objeto después de un retraso
    private IEnumerator WashPickedObjectDelayed(float delay)
    {
        // Corrección: Comprobamos si el nombre del objeto sostenido comienza con el nombre del prefab
        // Esto soluciona el problema de que Instantiate agrega "(Clone)" al nombre.
        if (objectCatcher.PickedObject.name.StartsWith("DirtyPlate"))
        {
            // Deshabilita el script del jugador para que no pueda interactuar
            if (player != null)
            {
                player.enabled = false;
            }

            // Espera el tiempo especificado antes de continuar
            yield return new WaitForSeconds(delay);
              
            // Accede al objeto atrapado a través de la propiedad pública "PickedObject"
            // Línea 244: if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("Plate", out GameObject myPrefab))
            // Reemplazamos el uso de myPrefabs por una búsqueda de IngredientData para mantener la consistencia.
            if (objectCatcher.PickedObject != null && allIngredientData.TryGetValue("Plate", out IngredientData plateData) && plateData.PrefabObject != null)
            {
                // Obtiene la posición y rotación del objeto actual
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                // Destruye el objeto actual
                Destroy(objectCatcher.PickedObject);

                // Instancia el nuevo objeto desde el prefab en la misma posición y rotación
                GameObject newInstance = Instantiate(plateData.PrefabObject, currentPosition, currentRotation);

                // Nos aseguramos de que el nuevo objeto tenga un Rigidbody y lo hacemos cinemático.
                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                // Llama a la función del otro script para actualizar el objeto
                objectCatcher.SetPickedObject(newInstance);
            }
            // Habilita el script del jugador al final de la corrutina
            if (player != null)
            {
                player.enabled = true;
            }
        }
    }
    //función para crear un objeto desde cero y agarrarlo.
    private void CreateObjectAndGrab()
    {
        // Busca el IngredientData en el diccionario.
        if (allIngredientData.TryGetValue(ingredienteInvolucrado.Name, out IngredientData dataToCreate) && dataToCreate.IsStartingIngredient)
        {
            if (dataToCreate.PrefabObject != null)
            {
                GameObject newInstance = Instantiate(dataToCreate.PrefabObject, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
            else
            {
                Debug.LogError($"IngredientData '{gameObject.name}' no tiene asignado un PrefabObject.");
            }
        }
        else
        {
            Debug.LogError($"No se encontró un IngredientData válido o no es un ingrediente inicial para: {gameObject.name}");
        }
    }

    //función para eliminar el objeto agarrado y liberar la referencia.
    private void DeliverObject()
    {
        if (allIngredientData.TryGetValue("DirtyPlate", out IngredientData plateData) && plateData.PrefabObject != null)
        {
            // Obtiene la posición y rotación del objeto actual
            Vector3 currentPosition = objectCatcher.finalSuperficieTransform.transform.position;
            Quaternion currentRotation = objectCatcher.finalSuperficieTransform.transform.rotation;

            // Destruye el objeto actual
            Destroy(objectCatcher.PickedObject);

            // Instancia el nuevo objeto desde el prefab en la misma posición y rotación
            GameObject newInstance = Instantiate(plateData.PrefabObject, currentPosition, currentRotation);

            // Nos aseguramos de que el nuevo objeto tenga un Rigidbody y lo hacemos cinemático.
            Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
            if (newRb != null)
            {
                newRb.isKinematic = true;
            }
            // Llama a la función del otro script para actualizar el objeto
            objectCatcher.SetPickedObject(newInstance);
        }
    }

    private void DestroyObject()
    {
        if (objectCatcher.PickedObject != null && objectCatcher.PickedObject.CompareTag("Object") || objectCatcher.PickedObject != null && objectCatcher.PickedObject.CompareTag("Food"))
        {
            if (objectCatcher.PickedObject != null && allIngredientData.TryGetValue("Plate", out IngredientData plateData) && plateData.PrefabObject != null)
            {
                // Obtiene la posición y rotación del objeto actual
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                // Destruye el objeto actual
                Destroy(objectCatcher.PickedObject);

                // Instancia el nuevo objeto desde el prefab en la misma posición y rotación
                GameObject newInstance = Instantiate(plateData.PrefabObject, currentPosition, currentRotation);

                // Nos aseguramos de que el nuevo objeto tenga un Rigidbody y lo hacemos cinemático.
                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                // Llama a la función del otro script para actualizar el objeto
                objectCatcher.SetPickedObject(newInstance);
            }
        }
    }
}