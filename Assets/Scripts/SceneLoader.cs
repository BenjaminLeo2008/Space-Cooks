using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
// Si usas TextMeshPro, descomenta estas 2 líneas y usa TMP_Text abajo.
// using TMPro;

public class SceneLoader : MonoBehaviour
{
    [Header("UI")]
    public GameObject loaderUI;
    public Image barraAzul;
    public Text porcentajeText;   // si usas TMP: cambia a TMP_Text

    [Header("Opciones")]
    public float speed = 0.6f;       // velocidad de llenado visual (fill/seg)
    public float minLoadTime = 1.0f; // tiempo mínimo visible de la pantalla

    public void LoadScene(int buildIndex)
    {
        StartCoroutine(LoadScene_Coroutine(buildIndex));
    }

    private IEnumerator LoadScene_Coroutine(int buildIndex)
    {
        // Seguridad: asegurate de empezar desde cero y UI visible
        if (loaderUI != null) loaderUI.SetActive(true);
        if (barraAzul != null) barraAzul.fillAmount = 0f;
        if (porcentajeText != null) porcentajeText.text = "0%";

        // Empezar a cargar sin activar
        AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex);
        op.allowSceneActivation = false;

        float visual = 0f;         // lo que ve el usuario (0→1)
        float t0 = Time.unscaledTime;

        while (!op.isDone)
        {
            // progreso real de Unity (0..0.9) ⇒ normalizado a 0..1
            float real = Mathf.Clamp01(op.progress / 0.9f);

            // suavizar la barra hacia el progreso real
            visual = Mathf.MoveTowards(visual, real, speed * Time.deltaTime);

            if (barraAzul != null) barraAzul.fillAmount = visual;
            if (porcentajeText != null) porcentajeText.text = Mathf.RoundToInt(visual * 100f) + "%";

            // activar solo cuando:
            //  1) Unity ya terminó (real >= 1)
            //  2) La barra llegó a 1 visualmente
            //  3) Pasó el tiempo mínimo de pantalla
            if (real >= 1f && visual >= 1f && (Time.unscaledTime - t0) >= minLoadTime)
            {
                // pequeña pausa estética
                yield return new WaitForSeconds(0.15f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}



