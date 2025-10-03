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

    // Referencia al ObjectCatcherScript
    private ObjectCatcherScript objectCatcher;

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
            myPrefabs[prefab.name] = prefab;
        }
    }

    void Update()
    {
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
                StartCoroutine(ReplacePickedObjectDelayed(2f));
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
        if (objectCatcher.PickedObject.name.StartsWith("RawPotato"))
        {
            if (player != null)
            {
                player.enabled = false;
            }

            yield return new WaitForSeconds(delay);

            if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("CuttedPotato", out GameObject myPrefab))
            {
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                Destroy(objectCatcher.PickedObject);

                GameObject newInstance = Instantiate(myPrefab, currentPosition, currentRotation);

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
        else if (objectCatcher.PickedObject.name.StartsWith("RawSteak"))
        {
            if (player != null)
            {
                player.enabled = false;
            }

            yield return new WaitForSeconds(delay);

            if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("CuttedSteak", out GameObject myPrefab))
            {
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                Destroy(objectCatcher.PickedObject);

                GameObject newInstance = Instantiate(myPrefab, currentPosition, currentRotation);

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
        else if (objectCatcher.PickedObject.name.StartsWith("Dough"))
        {
            if (player != null)
            {
                player.enabled = false;
            }

            yield return new WaitForSeconds(delay);

            if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("DoughDisc", out GameObject myPrefab))
            {
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                Destroy(objectCatcher.PickedObject);

                GameObject newInstance = Instantiate(myPrefab, currentPosition, currentRotation);

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
        else if (objectCatcher.PickedObject.name.StartsWith("CuttedPotatoInFryer"))
        {
            yield return new WaitForSeconds(delay);

            if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("CookedPotatoInFryer", out GameObject myPrefab))
            {
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                Destroy(objectCatcher.PickedObject);

                GameObject newInstance = Instantiate(myPrefab, currentPosition, currentRotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }
        else if (objectCatcher.PickedObject.name.StartsWith("CuttedSteakInSpan"))
        {
            yield return new WaitForSeconds(delay);

            if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("CookedSteakInSpan", out GameObject myPrefab))
            {
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                Destroy(objectCatcher.PickedObject);

                GameObject newInstance = Instantiate(myPrefab, currentPosition, currentRotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
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
            if (objectCatcher.PickedObject != null && myPrefabs.TryGetValue("Plate", out GameObject myPrefab))
            {
                // Obtiene la posición y rotación del objeto actual
                Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
                Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

                // Destruye el objeto actual
                Destroy(objectCatcher.PickedObject);

                // Instancia el nuevo objeto desde el prefab en la misma posición y rotación
                GameObject newInstance = Instantiate(myPrefab, currentPosition, currentRotation);

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
    // función para crear un objeto desde cero y agarrarlo.
    private void CreateObjectAndGrab()
    {
        if (gameObject.name == "Mesa para crear papa")
        {
            if (myPrefabs.TryGetValue("RawPotato", out GameObject myPrefab))
            {
                GameObject newInstance = Instantiate(myPrefab, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }
        else if (gameObject.name == "Mesa para crear carne")
        {
            if (myPrefabs.TryGetValue("RawSteak", out GameObject myPrefab))
            {
                GameObject newInstance = Instantiate(myPrefab, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }
        else if (gameObject.name == "Mesa para crear azucar")
        {
            if (myPrefabs.TryGetValue("Sugar", out GameObject myPrefab))
            {
                GameObject newInstance = Instantiate(myPrefab, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }
        else if (gameObject.name == "Mesa para crear leche")
        {
            if (myPrefabs.TryGetValue("Milk", out GameObject myPrefab))
            {
                GameObject newInstance = Instantiate(myPrefab, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }
        else if (gameObject.name == "Mesa para crear huevo")
        {
            if (myPrefabs.TryGetValue("Egg", out GameObject myPrefab))
            {
                GameObject newInstance = Instantiate(myPrefab, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }
        else if (gameObject.name == "Mesa para crear masa")
        {
            if (myPrefabs.TryGetValue("Dough", out GameObject myPrefab))
            {
                GameObject newInstance = Instantiate(myPrefab, transform.position, transform.rotation);

                Rigidbody newRb = newInstance.GetComponent<Rigidbody>();
                if (newRb != null)
                {
                    newRb.isKinematic = true;
                }
                objectCatcher.SetPickedObject(newInstance);
            }
        }

    }
    //función para eliminar el objeto agarrado y liberar la referencia.
    private void DeliverObject()
    {
        if (deliverablePrefab != null && objectCatcher.PickedObject.name == deliverablePrefab.name)
        {
            Destroy(objectCatcher.PickedObject);
            objectCatcher.SetPickedObject(null);
        }
    }
    private void DestroyObject()
    {
        if (objectCatcher.PickedObject != null && objectCatcher.PickedObject.CompareTag("Object"))
        {
            Destroy(objectCatcher.PickedObject);
            objectCatcher.SetPickedObject(null);
        }
    }
}
