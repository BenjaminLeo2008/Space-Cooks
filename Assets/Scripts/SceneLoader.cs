using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Importar si usas TextMeshPro (RECOMENDADO)

public class SceneLoader : MonoBehaviour
{
    // --- Referencias de UI ---
    [Header("Referencias de UI")]
    // Objeto principal de la UI de carga (Canvas o Panel)
    public GameObject loaderUI;

    // La imagen (Image) que actúa como barra, DEBE tener el "Image Type" en Filled (Llenado)
    public Image barraAzul;

    // El texto para mostrar el porcentaje (si usas TMP)
    public TMP_Text porcentajeTexto;

    // --- Configuraciones ---
    [Header("Configuraciones")]
    // Velocidad a la que se mueve la barra visual (solo si la carga real es muy rápida)
    public float visualSpeed = 1.5f;

    // Tiempo mínimo en segundos que el usuario debe ver la pantalla de carga (estético)
    public float minLoadTime = 1.0f;

    // Flag para saber si la escena real ya está lista para activarse (al 90%)
    private bool isSceneReady = false;

    private void Start()
    {
        // Por seguridad, aseguramos que la UI de carga esté oculta al iniciar el juego.
        if (loaderUI != null)
        {
            loaderUI.SetActive(false);
        }
    }

    // --- Método llamado por el botón ---
    // Recibe el índice (Build Index) de la escena a la que quieres ir.
    public void LoadScene(int buildIndex)
    {
        // Aseguramos que el tiempo corra si venimos de un menú en pausa.
        Time.timeScale = 1f;

        // Iniciamos la Coroutine
        StartCoroutine(LoadSceneCoroutine(buildIndex));
    }

    // --- Corutina de Carga Asíncrona ---
    private IEnumerator LoadSceneCoroutine(int buildIndex)
    {
        // 1. Preparación de la UI
        if (loaderUI != null) loaderUI.SetActive(true);
        if (barraAzul != null) barraAzul.fillAmount = 0f;
        if (porcentajeTexto != null) porcentajeTexto.text = "0%";
        isSceneReady = false;

        // 2. Iniciar la Carga Real
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        // ** CLAVE: Prevenir la activación de la escena al 90% **
        operation.allowSceneActivation = false;

        float visualProgress = 0f;
        float timer = 0f;

        // --- Loop Principal de Carga y Animación ---
        while (!operation.isDone)
        {
            // Acumular el tiempo para asegurar el minLoadTime. Usamos Time.unscaledDeltaTime 
            // por si Time.timeScale cambia.
            timer += Time.unscaledDeltaTime;

            // a) Determinar el progreso objetivo
            // Unity solo llega a 0.9. Lo normalizamos para que el 0.9 real sea 1.0 visualmente.
            float targetProgress = operation.progress / 0.9f;

            // Marcamos si la carga real está al 90% (lista para activarse)
            if (operation.progress >= 0.9f)
            {
                isSceneReady = true;
                targetProgress = 1.0f; // Ahora el objetivo visual es el 100%
            }

            // b) Mover la barra visual suavemente hacia el objetivo
            // Usamos MoveTowards para una animación fluida
            visualProgress = Mathf.MoveTowards(visualProgress, targetProgress, Time.unscaledDeltaTime * visualSpeed);

            // c) Actualizar la UI
            if (barraAzul != null) barraAzul.fillAmount = visualProgress;
            if (porcentajeTexto != null) porcentajeTexto.text = Mathf.RoundToInt(visualProgress * 100f) + "%";

            // d) Condición de Activación
            // La escena se activa SOLO si:
            // 1. La barra visual ha llegado al 100% (visualProgress >= 1f)
            // 2. Y el tiempo mínimo estético (minLoadTime) ha pasado.
            if (isSceneReady && visualProgress >= 1f && timer >= minLoadTime)
            {
                // ** CLAVE: Permitir que la escena se cargue finalmente **
                operation.allowSceneActivation = true;
            }

            yield return null; // Esperar al siguiente frame
        }

    }
}




