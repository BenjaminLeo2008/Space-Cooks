using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool IsPickable = true;
    private Rigidbody _rb;
    private IngredientData _data;

    #region PUBLIC API 

    public IngredientData Data => _data;

    #endregion

    private void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            if (this.transform.parent == null)
            {
                other.GetComponentInParent<PlayerInteraction>().ObjectToPickUp = this.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerInteractionZone"))
        {
            PlayerInteraction pickUpScript = other.GetComponentInParent<PlayerInteraction>();
            if (pickUpScript != null && pickUpScript.ObjectToPickUp == this.gameObject)
            {
                pickUpScript.ObjectToPickUp = null;
            }
        }
    }

    private void Update() {
        // Si no es posible agarrar, signfica que alguien lo tiene en la mano
        if (!IsPickable) {
            // por ende, debe ser kinematico 
            _rb.isKinematic = true;
        }
    }
}
