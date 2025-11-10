using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 checkHalfExtents;
    [SerializeField] private Transform checkTransform;
    [SerializeField] private LayerMask checkLayer;
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
    #region DetectOtherPickableObjects
    private (IngredientData, GameObject, PickableObject) DetectObject()
    {
        Collider[] hits = Physics.OverlapBox(checkTransform.position, checkHalfExtents,
        Quaternion.identity, checkLayer, QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)

        {

            Debug.Log("Detected obj!");
            PickableObject ingredientObj = hits[0].GetComponent<PickableObject>();

            foreach (Collider col in hits)
            {
                if (ingredientObj == null)
                    ingredientObj = col.GetComponent<PickableObject>();
            }

            if (ingredientObj) Debug.Log("Encontro un objeto con PickableObject!");

            IngredientData ingData = ingredientObj.Data;
            GameObject obj = ingredientObj.gameObject;


            return (ingData, obj, ingredientObj);
        }
        else
        {
            return (null, null, null);
        }
    }
    #endregion



    #region viewBox
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(checkTransform.position, checkHalfExtents);
    }
    #endregion
}
