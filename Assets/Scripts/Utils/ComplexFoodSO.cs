using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComplexFood", menuName = "Custom/ComplexFood")]
public class ComplexFoodSO : ScriptableObject
{
    //Cada posible combinación de plato
    public GameObject complexFoodPrefab;
    //Los ingredientes para llegar a ese plato
    public IngredientData[] ingredientsNeededForDish;

}


[CreateAssetMenu(fileName = "ComplexFood Database", menuName = "Custom/Database")]
public class ComplexFoodDatabaseSO : ScriptableObject
{
    public ComplexFoodSO[] allComplexFoods;
}