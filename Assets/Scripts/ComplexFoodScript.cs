using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Necesario para usar .OrderBy()

public class ComplexFoodScript : MonoBehaviour
{
    [Header("Settings")]
    public List<IngredientData> possibleIngredients;
    public List<IngredientData> currentIngredients;
    public ComplexFoodDatabaseSO complexFoodDatabase;


    // MODIFICACIÓN: Implementación de la lógica para encontrar el plato complejo.
    public GameObject GetComplexFood()
    {
        // Ordenamos la lista de ingredientes actuales por nombre.
        // Esto permite comparar si dos listas tienen los mismos elementos sin importar el orden.
        List<IngredientData> sortedCurrentIngredients = currentIngredients.OrderBy(i => i.name).ToList();

        foreach (ComplexFoodSO food in complexFoodDatabase.allComplexFoods)
        {

            // 1. Verificamos primero si la cantidad de ingredientes es la misma.
            if (sortedCurrentIngredients.Count != food.ingredientsNeededForDish.Length)
            {
                continue; // No es este plato, pasamos al siguiente.
            }

            // 2. Ordenamos los ingredientes requeridos del plato complejo.
            List<IngredientData> sortedRequiredIngredients = food.ingredientsNeededForDish.OrderBy(i => i.name).ToList();

            // 3. Comparamos las dos listas ordenadas elemento por elemento.
            bool match = true;
            for (int i = 0; i < sortedCurrentIngredients.Count; i++)
            {
                if (sortedCurrentIngredients[i] != sortedRequiredIngredients[i])
                {
                    match = false;
                    break;
                }
            }

            // Si hay un 'match' completo, hemos encontrado el plato.
            if (match)
            {
                // Devolvemos el prefab del plato complejo encontrado.
                return food.complexFoodPrefab;
            }
        }

        // Si el bucle termina sin encontrar un plato, retornamos null.
        return null;
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
        if (!possibleIngredients.Contains(ingredientData))
        {

            return false;
        }
        else
        {
            return true;
        }
    }

    /* COMENTARIO DE CÓDIGO ELIMINADO:
    public void GetComplexFood() {
        foreach (ComplexFoodSO food in complexFoodDatabase.allComplexFoods) {
            // if ( todos los ingredientes son = a los current ingredients ) usar prefab
        }
    }
    */
}