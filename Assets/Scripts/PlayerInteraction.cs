using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform interactionZone;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tile tile) && !_tileCloseList.Contains(tile))
        {
            _tileCloseList.Add(tile);

            if (_closestTile) _closestTile.StopHighlight();
            _closestTile = GetCloserTile();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tile tile))
        {
            _tileCloseList.Remove(tile);

            if (_closestTile) _closestTile.StopHighlight();
            _closestTile = GetCloserTile();
        }
    }
    private void GetCloserTile()
    {
        if (_tileCloseList.Count <= 0) return null;

        Tile winnerTile = null;
        float minDistance = Mathf.Infinity;
        foreach (Tile tile in _tileCloseList)
        {
            float newDistance = Vector3.Distance(transform.position, tile.transform.position);
            if (newDistance <= minDistance)
            {
                winnerTile = tile;
                minDistance = newDistance;
            }
        }

        if (winnerTile) winnerTile.StartHighlight();
        return winnerTile;
    }
}
