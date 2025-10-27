using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexFoodScript : MonoBehaviour
{
    [Header("Settings")]
    public List<IngredientData> possibleIngredients;
    public List<IngredientData> currentIngredients;
    public ComplexFoodDatabaseSO complexFoodDatabase;


    public ComplexFoodSO GetComplexFood() {
        foreach (ComplexFoodSO food in complexFoodDatabase.allComplexFoods) {
            // if ( todos los ingredientes son = a los current ingredients ) usar prefab
        }
    }


    public void UpdatePossibleIngredients(IngredientData ingredientData)
    {
        possibleIngredients.Clear();

        foreach (RecipeTransition recipe in ingredientData.Recipes)
        {
            possibleIngredients.Add(recipe.RequiredIngredient);
        }
    }

    public void AddIngredient(IngredientData ingredientData)
    {
        currentIngredients.Add(ingredientData);
    }

    public bool IsValidIngredient(IngredientData ingredientData)
    {
        if (!possibleIngredients.Contains(ingredientData)) {

            return false;
        } else
        {
            return true;
        }
    }

}
