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
    [SerializeField] private Rigidbody PickedRigidbody;
    private GameObject ObjectToPickUp;
    private GameObject PickedObject;

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
        PickedRigidbody = PickedObject.GetComponent<Rigidbody>();
    }
  
    void Update()
    {
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<PickableObject>().IsPickable == false && PickedObject == null)
        {
            // Verifica que el objeto detectado no sea hijo de otro objeto.
            if (PickedObject.transform.parent == null)
            {
                if (superficieTransform != null && PickedObject.gameObject.CompareTag(layer))
                {
                    //Asigna rotación al objeto catched igual al objeto catcher
                    PickedObject.transform.rotation = superficieTransform.rotation;
                    //El objeto catcheado se mantiene en la superficie del objeto catcheador
                    float objectHeight = PickedObject.GetComponent<Collider>().bounds.extents.y;
                    Vector3 superficiePosition = superficieTransform.position;
                    Vector3 newPosition = new Vector3(superficiePosition.x + offset.x, superficiePosition.y + objectHeight + offset.y, superficiePosition.z + offset.z);

                    PickedObject.gameObject.transform.position = newPosition;

                    Rigidbody rb = PickedRigidbody;
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