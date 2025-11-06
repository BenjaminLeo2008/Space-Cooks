using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Menu
    public void EmpezarNivel(string NombreNivel)
    {
        SceneManager.LoadScene(NombreNivel);
    }
    public void Salir()
    {
        Application.Quit();
        Debug.Log("Aquí se cierra el juego");
    }
    // Titulo
    public void EmpezarJuego(string NombreMenu)
    {
        SceneManager.LoadScene(NombreMenu);
    }
    // Juego - pausa
    public void VolverAlMenu(string Nombre)
    {
        SceneManager.LoadScene(Nombre);
    }
    public void Reiniciar(string Escena)
    {
        SceneManager.LoadScene(Escena);
    }
}
