using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct RecipeTransition
{
    public IngredientData RequiredIngredient;

    public GameObject ResultingPrefab;
}

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Custom/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    public GameObject PrefabObject;

    public IngredientData SimpleNextState;

    public bool IsStartingIngredient = false;

    public List<RecipeTransition> Recipes;
}