using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] public ObjectCatcherScript objectCatcher;

    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform PlayerInteractionZone;
    [SerializeField] private float detectionRadius;
    [SerializeField] private SphereCollider col;
    [SerializeField] private string layer;
    private List<GameObject> _tileCloseList = new List<GameObject>();
    private GameObject _closestTile;
    private Vector3 originalScale;

    // Nueva variable para guardar la referencia al script del objeto.
    private ObjectCatcherScript _caughtCatcherScript;
    private SphereCollider _caughtCol;

    void Start()
    {
        col = GetComponent<SphereCollider>();


        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
        }


        col.radius = detectionRadius;
        col.isTrigger = true;
    }

    void Update()
    {
        if (ObjectToPickUp != null && ObjectToPickUp.GetComponent<PickableObject>().IsPickable == true && PickedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickedObject = ObjectToPickUp;
                PickedObject.GetComponent<PickableObject>().IsPickable = false;

                // Intentamos obtener el script ObjectCatcherScript del objeto.
                _caughtCatcherScript = PickedObject.GetComponent<ObjectCatcherScript>();
                if (_caughtCatcherScript != null)
                {
                    // Si el script existe, lo desactivamos.
                    _caughtCatcherScript.enabled = false;
                }
                _caughtCol = PickedObject.GetComponent<SphereCollider>();
                if (_caughtCol != null)
                {
                    // Si el script existe, lo desactivamos.
                    _caughtCol.enabled = false;
                }
                originalScale = PickedObject.transform.localScale;

                PickedObject.transform.SetParent(PlayerInteractionZone);
                PickedObject.transform.localPosition = Vector3.zero;
                PickedObject.transform.localRotation = Quaternion.identity;
                PickedObject.transform.localScale = originalScale; // Establece la escala local del objeto recogido
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

                // Si teníamos un script guardado, lo volvemos a activar.
                if (_caughtCatcherScript != null)
                {
                    _caughtCatcherScript.enabled = true;
                }
                if (_caughtCol != null)
                {
                    _caughtCol.enabled = true;
                }
                PickedObject = null;
            }
        }
    }
}
