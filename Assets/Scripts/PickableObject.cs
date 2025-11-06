using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool IsPickable = true;
    private Rigidbody _rb;
    [SerializeField] private IngredientData _data;
    [SerializeField] private bool _isPicked;

    #region PUBLIC API 

    public IngredientData Data => _data;
    public bool IsPicked => _isPicked;
    public Rigidbody Rb => _rb;

    public void SetPicked(bool isPicked)
    {
        _isPicked = isPicked;
    }

    #endregion

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    /*
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
    */
}
