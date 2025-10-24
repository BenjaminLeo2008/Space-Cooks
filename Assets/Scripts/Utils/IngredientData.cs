using UnityEngine;
using System.Collections.Generic;

// Estructura para definir las posibles "Recetas"
[System.Serializable]
public struct RecipeTransition
{
    // El OTRO ingrediente que se requiere para esta receta
    public IngredientData RequiredIngredient;

    // El PREFAB final que resulta de esta combinación
    public GameObject ResultingPrefab;
}

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Custom/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    // 1. Prefab del estado actual (ej: Papa Cocida)
    public GameObject PrefabObject;

    // 2. Transición simple (corte, cocción directa, etc. si aplica)
    public IngredientData SimpleNextState;

    // 3. Lista de transiciones complejas/recetas (ej: Papa Cocida + X)
    public List<RecipeTransition> Recipes;
}