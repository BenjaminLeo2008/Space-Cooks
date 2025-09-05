using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform interactionZone;

    private Vector3 originalScale; 

    void Update()
    {
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<PickableObject>().IsPickable == true && PickedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickedObject = ObjectToPickUp;
                PickedObject.GetComponent<PickableObject>().IsPickable = false;

                
                originalScale = PickedObject.transform.localScale;

                PickedObject.transform.SetParent(interactionZone);
                PickedObject.transform.localPosition = Vector3.zero;
                PickedObject.transform.localRotation = Quaternion.identity;
                PickedObject.GetComponent<Rigidbody>().useGravity = false;
                PickedObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else if (PickedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickedObject.GetComponent<PickableObject>().IsPickable = true;
                PickedObject.transform.SetParent(null);
                PickedObject.GetComponent<Rigidbody>().useGravity = true;
                PickedObject.GetComponent<Rigidbody>().isKinematic = false;

           
                PickedObject.transform.localScale = originalScale;

                PickedObject = null;
            }
        }
    }
}
