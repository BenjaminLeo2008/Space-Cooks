using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcherScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private SphereCollider col;
    [SerializeField] private string layer;
    [SerializeField] private Vector3 offset;

    private Transform superficieTransform;
    private Rigidbody caughtRigidbody; // Nuevo: Referencia al Rigidbody del objeto atrapado

    void Start()
    {
        col = GetComponent<SphereCollider>();

        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }

        col.radius = detectionRadius;
        col.isTrigger = true;

        superficieTransform = transform.Find("Superficie");

        if (superficieTransform == null)
        {
            Debug.LogError("Child GameObject named 'Superficie' not found! Make sure it exists.");
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Solo ejecuta el código si no hay un objeto atrapado actualmente
        if (caughtRigidbody == null)
        {
            // Verifica que el objeto detectado no sea hijo de otro objeto.
            if (other.transform.parent == null)
            {
                if (superficieTransform != null && other.gameObject.CompareTag(layer))
                {
                    //Asigna rotación al objeto catched igual al objeto catcher
                    other.transform.rotation = superficieTransform.rotation;
                    //El objeto catcheado se mantiene en la superficie del objeto catcheador
                    float objectHeight = other.bounds.extents.y;
                    Vector3 superficiePosition = superficieTransform.position;
                    Vector3 newPosition = new Vector3(superficiePosition.x + offset.x, superficiePosition.y + objectHeight + offset.y, superficiePosition.z + offset.z);

                    other.gameObject.transform.position = newPosition;

                    Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        caughtRigidbody = rb; // Asigna el Rigidbody del objeto atrapado
                    }
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() == caughtRigidbody)
        {
            caughtRigidbody = null;
        }
    }
}