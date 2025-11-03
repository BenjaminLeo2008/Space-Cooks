using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ContadorTiempo : MonoBehaviour
{
    public GameObject TiempoUI;
    public PlayerController player;
    public string escenaPuntaje = "EscenaPuntaje";
    public int min, seg;
    public TextMeshProUGUI TimerTxt;

    private float restante;
    private bool enMarcha;

    private void Awake()
    {
        restante = (min * 60) + seg;
        enMarcha = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enMarcha)
        {
            if (restante > 0)
            {
                restante -= Time.deltaTime;
                if (restante < 0)
                {
                    restante = 0;
                }
            int tempMin = Mathf.FloorToInt(restante / 60);
            int tempSeg = Mathf.FloorToInt(restante % 60);
            TimerTxt.text = string.Format("{00:00}:{01:00}", tempMin, tempSeg);
            }
            else
            {
                restante = 0;
                enMarcha = false;
                StartCoroutine(FinDelTiempo());
            }
        }
    }
    IEnumerator FinDelTiempo()
    {
        TiempoUI.SetActive(true);
        if (player != null)
        {
            player.enabled = false;
        }
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(escenaPuntaje);
    }
}
