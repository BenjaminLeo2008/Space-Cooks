using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabberScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Vector3 checkHalfExtents;
    [SerializeField] private Transform checkTransform;
    [SerializeField] private LayerMask checkLayer;
    public bool debug = true;


    [Header("Visualize Variables")]
    public ObjectGrabbed Grabbed;
     
    [System.Serializable]
    public class ObjectGrabbed
    {
        public IngredientData CurrentIngredient;
        public GameObject CurrentObject;
        public PickableObject CurrentPickable;

        public ObjectGrabbed(IngredientData ingData, GameObject obj, PickableObject pickable)
        {
            CurrentIngredient = ingData;
            CurrentObject = obj;
            CurrentPickable = pickable;
        }
    }

    #region PUBLIC API

    public IngredientData CurrentIngredientHeld => Grabbed.CurrentIngredient;
    public GameObject CurrentGameObjHeld => Grabbed.CurrentObject;
    public PickableObject CurrentPickableHeld => Grabbed.CurrentPickable;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }
            
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F)) return; // mata la funcion si no toca F

        (
            IngredientData detectedIng, 
            GameObject ingObj,
            PickableObject pickableObj
        ) = DetectObject();

        bool alreadyDidSomething = false;

        if (Grabbed.CurrentIngredient != null)
        { // esto limipa/libera/borra los datos que teniamos seteados del objeto.
            alreadyDidSomething = true;
            UnglueObjectToPlayer(Grabbed.CurrentObject);
            ClearCurrentIngredient();
        }

        if (detectedIng != null && Grabbed.CurrentIngredient == null && !alreadyDidSomething)
        { // esto setea el current ingredient, con lo que detectamos.
            SetCurrentIngredient(detectedIng, ingObj, pickableObj);
            GlueObjectToPlayer(Grabbed.CurrentObject);
        }
    }


    #region Tracking logic
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

    private void SetCurrentIngredient(IngredientData ingData, GameObject ingObj, PickableObject pickableObj)
    {
        Grabbed = new ObjectGrabbed(ingData, ingObj, pickableObj);
        //Debug.Log("Se seteo un objeto");
    }

    private void ClearCurrentIngredient()
    {
        Grabbed = new ObjectGrabbed(null, null, null);
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
        Grabbed.CurrentPickable.SetPicked(true);
    }

    private void UnglueObjectToPlayer(GameObject obj)
    {
        obj.transform.SetParent(null);
        var rb = obj.GetComponent<Rigidbody>();

        rb.isKinematic = false;

        Debug.Log("Unglued obj!");
    }

    #endregion

    #region ViewGrabZone
    private void OnDrawGizmos()
    {
        if (!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(checkTransform.position, checkHalfExtents);
    }
    #endregion
}
