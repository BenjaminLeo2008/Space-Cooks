using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCatcherScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float detectionRadius;
    [SerializeField] private SphereCollider col;
    [SerializeField] private string layer; // Renamed for clarity, since it's used as a Tag
    [SerializeField] private Vector3 offset;

    private Transform superficieTransform;

    void Start() {
        // Using GetComponent instead of AddComponent to get the existing collider
        col = GetComponent<SphereCollider>();

        // Add the collider if it doesn't exist to avoid a NullReferenceException
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }

        col.radius = detectionRadius;
        col.isTrigger = true;

        // The correct way to find a child transform. 'FindChild' is deprecated.
        superficieTransform = transform.Find("Superficie");

        // Log an error if the child is not found
        if (superficieTransform == null)
        {
            Debug.LogError("Child GameObject named 'Superficie' not found! Make sure it exists.");
        }
    }

  void OnTriggerEnter(Collider other) {
        if (superficieTransform != null && other.gameObject.CompareTag(layer)) {
            
            // 1. Calcula la altura del objeto.
            // other.bounds.extents.y es la mitad de la altura del collider.
            float objectHeight = other.bounds.extents.y;

            // 2. Obtiene la posición de la superficie.
            Vector3 superficiePosition = superficieTransform.position;

            // 3. Calcula la nueva posición del objeto para que su base descanse sobre la superficie.
            // Mantenemos la 'x' y 'z' de la superficie, pero ajustamos la 'y'.
            Vector3 newPosition = new Vector3(superficiePosition.x + offset.x, superficiePosition.y + objectHeight + offset.y, superficiePosition.z + offset.z);

            // 4. Asigna la nueva posición al objeto.
            other.gameObject.transform.position = newPosition;
            
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}