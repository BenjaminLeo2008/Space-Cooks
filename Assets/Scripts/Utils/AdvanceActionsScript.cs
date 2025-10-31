using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceActionsScript : MonoBehaviour
{
    // Nombre del GameObject para activar el reemplazo. Configúralo en el Inspector.
    [SerializeField] private string targetGameObjectName;

    // Prefab que se puede entregar, agrégalo desde el Inspector.
    [SerializeField] private GameObject deliverablePrefab;

    // Diccionario para almacenar todos los prefabs cargados de Resources.
    private Dictionary<string, GameObject> myPrefabs = new Dictionary<string, GameObject>();

    // Diccionario para almacenar todos los IngredientData cargados de Resources.
    private Dictionary<string, IngredientData> allIngredientData = new Dictionary<string, IngredientData>();


    // Referencia al ObjectCatcherScript
    private ObjectCatcherScript objectCatcher;

    // ESTA VARIABLE DEBE CONTENER EL SCRIPTABLE OBJECT IngredientData del objeto recogido.
    public IngredientData ingredientData;

    // Variable para rastrear si el jugador está dentro del área del collider.
    private bool _isPlayerInTrigger = false;

    public PlayerController player;

    void Start()
    {
        // Obtiene la referencia al ObjectCatcherScript en el mismo GameObject
        objectCatcher = GetComponent<ObjectCatcherScript>();
        if (objectCatcher == null)
        {
            Debug.LogError("No se encontró ObjectCatcherScript en este GameObject. Asegúrate de que ambos scripts estén juntos.");
        }

        // Carga todos los prefabs de la carpeta "Resources" una sola vez al inicio del juego.
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("");
        foreach (GameObject prefab in loadedPrefabs)
        {
            // Línea 45: myPrefabs[prefab.name] = prefab;
            // Se eliminó la carga directa de prefabs aquí, ya que los IngredientData ahora manejan la referencia.
        }

        // Carga todos los IngredientData de la carpeta "Resources" al inicio.
        IngredientData[] loadedIngredients = Resources.LoadAll<IngredientData>("");
        foreach (IngredientData data in loadedIngredients)
        {
            allIngredientData[data.name] = data;
        }

        // Línea 51:
        // Se eliminó la línea vacía.
    }

    void Update()
    {
        ingredientData = objectCatcher.IngredientData;
        // Elige la función a ejecutar basándose en el nombre del GameObject
        if (targetGameObjectName == "Mesa para crear")
        {
            // Solo se ejecuta si el "Player" está en el trigger, se presiona 'Q'
            // y no hay ningún objeto en la mano.
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                // Si el nombre coincide, ejecuta la función para crear y agarrar un nuevo objeto.
                CreateObjectAndGrab();
            }
        }
        else if (targetGameObjectName == "Mesa para reemplazar comida")
        {
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                // Solo inicia la corrutina si tenemos un IngredientData y una transición definida.
                if (ingredientData != null && ingredientData.SimpleNextState != null)
                {
                    StartCoroutine(ReplacePickedObjectDelayed(2f));
                }
                else
                {
                    Debug.LogWarning("No se puede reemplazar. El objeto recogido no tiene SimpleNextState definido.");
                }
            }
        }
        else if (targetGameObjectName == "Mesa para entregar")
        {
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                DeliverObject();
            }
        }
        else if (targetGameObjectName == "Mesa para eliminar")
        {
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                DestroyObject();
            }
        }
        else if (targetGameObjectName == "Mesa para lavar")
        {
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                StartCoroutine(WashPickedObjectDelayed(2f));
            }
        }
        else
        {
            Debug.Log("El GameObject de nombre " + gameObject.name + " no tiene acciones avanzadas");
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
        GameObject nextStatePrefab = ingredientData.SimpleNextState.PrefabObject;

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
            Debug.LogWarning($"El SimpleNextState de {ingredientData.name} no tiene asignado un PrefabObject.");
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
        // Extraemos el nombre del ingrediente de la mesa.
        // Ejemplo: "Mesa para crear papa" -> "papa"
        // Asegúrate de que los IngredientData en Resources tengan el mismo nombre ("Papa").
        string ingredientName = targetGameObjectName.Replace("Mesa para crear ", "");

        // Convertimos la primera letra a mayúscula para que coincida con el nombre del Scriptable Object (SO)
        if (ingredientName.Length > 0)
        {
            ingredientName = char.ToUpper(ingredientName[0]) + ingredientName.Substring(1);
        }
        // Busca el IngredientData en el diccionario.
        if (allIngredientData.TryGetValue(ingredientName, out IngredientData dataToCreate) && dataToCreate.IsStartingIngredient)
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
                Debug.LogError($"IngredientData '{ingredientName}' no tiene asignado un PrefabObject.");
            }
        }
        else
        {
            Debug.LogError($"No se encontró un IngredientData válido o no es un ingrediente inicial para: {ingredientName}");
        }
    }
    //función para eliminar el objeto agarrado y liberar la referencia.
    private void DeliverObject()
    {
        if (deliverablePrefab != null && objectCatcher.PickedObject.name == deliverablePrefab.name)
        {
            if (objectCatcher.PickedObject != null && allIngredientData.TryGetValue("DirtyPlate", out IngredientData plateData) && plateData.PrefabObject != null)
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