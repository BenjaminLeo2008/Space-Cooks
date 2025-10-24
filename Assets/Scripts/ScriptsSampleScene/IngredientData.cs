using UnityEngine;

// 1. Define el tipo de SO y cómo crearlo en el Editor
[CreateAssetMenu(fileName = "NewIngredient", menuName = "Custom/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    // 2. Referencia al Prefab real (el objeto que se instanciará en el mundo).
    public GameObject PrefabObject;

    // 3. Referencia al Scriptable Object del siguiente estado.
    // Esta es la clave de la transición, reemplazando el diccionario de strings.
    public IngredientData NextState;
}