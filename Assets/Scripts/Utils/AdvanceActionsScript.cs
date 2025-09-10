using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceActionsScript : MonoBehaviour
{
    // Asegúrate de arrastrar el "Objeto Nuevo" en el Inspector de Unity.
    [SerializeField] private GameObject newObjectPrefab;

    // Prefab para la función CreateObjectAndGrab.
    [SerializeField] private GameObject newCreatedPrefab;

    // Nombre del GameObject para activar el reemplazo. Configúralo en el Inspector.
    [SerializeField] private string targetGameObjectName;

    // Prefab que se puede entregar, agrégalo desde el Inspector.
    [SerializeField] private GameObject deliverablePrefab;

    // Referencia al ObjectCatcherScript
    private ObjectCatcherScript objectCatcher;

    // Variable para rastrear si el jugador está dentro del área del collider.
    private bool _isPlayerInTrigger = false;

    void Start()
    {
        // Obtiene la referencia al ObjectCatcherScript en el mismo GameObject
        objectCatcher = GetComponent<ObjectCatcherScript>();
        if (objectCatcher == null)
        {
            Debug.LogError("No se encontró ObjectCatcherScript en este GameObject. Asegúrate de que ambos scripts estén juntos.");
        }
    }

    void Update()
    {
        // Elige la función a ejecutar basándose en el nombre del GameObject
        if (targetGameObjectName == "Mesa para crear")
        {
            // Solo se ejecuta si el "Player" está en el trigger, se presiona 'Q'
            // y no hay ningún objeto en la mano.
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject == null)
            {
                // Si el nombre coincide, ejecuta la función para crear y agarrar un nuevo objeto.
                CreateObjectAndGrab();
            }
        }
        else if (targetGameObjectName == "Mesa para cortar")
        {
            // Solo se ejecuta si el "Player" está en el trigger, se presiona 'Q'
            // y ya se tiene un objeto en la mano.
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                // Si el nombre no coincide, ejecuta la función de reemplazo.
                StartCoroutine(ReplacePickedObjectDelayed(2f));
            }
        }
        else if (targetGameObjectName == "Mesa para entregar")
        {
            // Solo se ejecuta si el "Player" está en el trigger, se presiona 'Q'
            // y ya se tiene un objeto en la mano.
            if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.Q) && objectCatcher.PickedObject != null)
            {
                // Si el nombre no coincide, ejecuta la función de reemplazo.
                DeliverObject();
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
        // Espera el tiempo especificado antes de continuar
        yield return new WaitForSeconds(delay);

        // Accede al objeto atrapado a través de la propiedad pública "PickedObject"
        if (objectCatcher.PickedObject != null && newObjectPrefab != null)
        {
            // Obtiene la posición y rotación del objeto actual
            Vector3 currentPosition = objectCatcher.PickedObject.transform.position;
            Quaternion currentRotation = objectCatcher.PickedObject.transform.rotation;

            // Destruye el objeto actual
            Destroy(objectCatcher.PickedObject);

            // Instancia el nuevo objeto desde el prefab en la misma posición y rotación
            GameObject newInstance = Instantiate(newObjectPrefab, currentPosition, currentRotation);

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

    // Nueva función para crear un objeto desde cero y agarrarlo.
    private void CreateObjectAndGrab()
    {
        if (newCreatedPrefab != null)
        {
            // Instancia el prefab en la posición del GameObject actual.
            GameObject newInstance = Instantiate(newCreatedPrefab, transform.position, transform.rotation);

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

    // Nueva función para eliminar el objeto agarrado y liberar la referencia.
    private void DeliverObject()
    {
        if (objectCatcher.PickedObject != null && deliverablePrefab != null && objectCatcher.PickedObject.name == deliverablePrefab.name)
        {
            Destroy(objectCatcher.PickedObject);
            objectCatcher.SetPickedObject(null);
        }
    }
}
