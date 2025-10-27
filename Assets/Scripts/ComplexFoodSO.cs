using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexFood", menuName = "Custom/ComplexFood")]
public class ComplexFoodSO : ScriptableObject
{
    public GameObject complexFoodPrefab;
    public IngredientData[] ingredientsNeededForDish;

    public ComplexFoodSO[] allComplexFoods;
}


[CreateAssetMenu(fileName = "ComplexFood Database", menuName = "Custom/Database")]
public class ComplexFoodDatabaseSO : ScriptableObject
{
    public ComplexFoodSO[] allComplexFoods;
}
