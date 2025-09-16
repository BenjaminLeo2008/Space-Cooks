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
    }
}
