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
        // Check if the child transform exists and the tag matches
        if (superficieTransform != null && other.gameObject.CompareTag(layer)) {
            // Set the position of the other object
            other.gameObject.transform.position = superficieTransform.position + offset;
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}