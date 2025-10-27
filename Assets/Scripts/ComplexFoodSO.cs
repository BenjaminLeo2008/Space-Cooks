using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu("ComplexFoodSO")]
public class ComplexFoodSO : ScriptableObject
{
    public GameObject complexFoodPrefab;
    public IngredientData[] ingredientsNeededForDish;
}
