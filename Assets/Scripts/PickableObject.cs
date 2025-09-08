using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool IsPickable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            if (this.transform.parent == null)
            {
                other.GetComponentInParent<PickUpObject>().ObjectToPickUp = this.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            PickUpObject pickUpScript = other.GetComponentInParent<PickUpObject>();
            if (pickUpScript != null && pickUpScript.ObjectToPickUp == this.gameObject)
            {
                pickUpScript.ObjectToPickUp = null;
            }
        }
    }
    private void CenterObject()
    {
        if (IsPickable == false)
        {
            gameObject.GetComponent<Rigidbody>();
        }
    }
}
