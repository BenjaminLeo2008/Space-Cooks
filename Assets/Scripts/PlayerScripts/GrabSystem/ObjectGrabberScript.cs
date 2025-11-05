using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabberScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Vector3 checkHalfExtents;
    [SerializeField] private Transform checkTransform;
    [SerializeField] private LayerMask checkLayer;

    private IngredientData _currentIngredient;
    private GameObject _currentObj;

    #region PUBLIC API

    public IngredientData CurrentIngredientHeld => _currentIngredient;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }
            
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return;

        IngredientData detectedIng = DetectObject();

        if (detectedIng && _currentIngredient == null)
        {
            SetCurrentIngredient(detectedIng);
            GlueObjectToPlayer(_currentObj);
        }

        if (_currentIngredient != null)
        {
            UnglueObjectToPlayer(_currentObj);
            ClearCurrentIngredient();
        }
    }


    #region Tracking logic
    private IngredientData DetectObject()
    {
        Collider[] hits = Physics.OverlapBox(checkTransform.position, checkHalfExtents,
        Quaternion.identity, checkLayer, QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)
        
        {

            Debug.Log("Detected obj!");
            PickableObject ingredientObj = hits[0].GetComponent<PickableObject>();
            IngredientData ingData = ingredientObj.Data;

            _currentObj = ingredientObj.gameObject;

        
            return ingData;
        }
        else
        {
            return null;
        }
    }

    private void SetCurrentIngredient(IngredientData ingData)
    {
        _currentIngredient = ingData;
    }

    private void ClearCurrentIngredient()
    {
        _currentIngredient = null;
    }
    #endregion

    #region Object logic
    private void GlueObjectToPlayer(GameObject obj)
    {
        obj.transform.position = checkTransform.position;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        obj.transform.SetParent(gameObject.transform);
        var rb = obj.GetComponent<Rigidbody>();

        rb.isKinematic = true;

        Debug.Log("Gluing obj!");
    }

    private void UnglueObjectToPlayer(GameObject obj)
    {
        obj.transform.SetParent(null);
        var rb = obj.GetComponent<Rigidbody>();

        rb.isKinematic = false;
    }

    #endregion

}
