using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewIngredientDatabase", menuName = "Custom/Ingredient Database")]
public class IngredientDatabase : ScriptableObject {
    public IngredientData[] allIngredients;
}