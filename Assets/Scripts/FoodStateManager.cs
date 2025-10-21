using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStateManager : MonoBehaviour
{
    public bool IsPickable = true;
    private Rigidbody _rb;
    
    private int _foodState;
    
    private AdvanceActionsScript advanceActionsScript;
    public GameObject FoodState => _foodState;
    private Dictionary<string, GameObject> myPrefabs = new Dictionary<string, GameObject>();

    void Start(){
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("");
        foreach (GameObject prefab in loadedPrefabs)
        {
            myPrefabs[prefab.name] = prefab;
        }
    }
    void Update(){
        if (gameObject != null){
            if(_foodState == 1)
            {
                
            }
        }
    }

}