using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Script asociado a SceneController
*/
public class SceneSwitcher : MonoBehaviour
{
    // Funcion que se ejecuta al pulsar el boton de Play en el Menu
    public void LoadGameScene()
    {
        //Llama la funcion LoadScene de SceneManager para cargar la escena de juego
        SceneManager.LoadScene("Loading"); 
    }

    public void LoadOptionsScene()
    {
        //Llama la funcion LoadScene de SceneManager para cargar la escena de opciones
        SceneManager.LoadScene("Options");
    }
}
