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

    void Start() {
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

    void OnTriggerEnter(Collider other) {
        // Verifica que el objeto detectado no sea hijo de otro objeto.
        if (other.transform.parent != null)
        {
            // Opcional: puedes agregar un log para saber que no se movió el objeto.
            Debug.Log($"Ignorando el objeto '{other.gameObject.name}' porque es hijo de otro objeto.");
            return; // Sale de la función, el resto del código no se ejecutará.
        }

        if (superficieTransform != null && other.gameObject.CompareTag(layer)) {
            
            float objectHeight = other.bounds.extents.y;
            Vector3 superficiePosition = superficieTransform.position;
            Vector3 newPosition = new Vector3(superficiePosition.x + offset.x, superficiePosition.y + objectHeight + offset.y, superficiePosition.z + offset.z);

            other.gameObject.transform.position = newPosition;
            
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = true;
            }
        }
    }
}
