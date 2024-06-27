using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Script asocido al objeto ExitController
en la escena de Menu
*/

public class ExitGame : MonoBehaviour
{
    // Se llama este metodo al pulsar el boton exit del menu
    public void QuitGame()
    {
        #if UNITY_EDITOR
            // Esto es para que el editor de Unity se detenga al presionar el botón de salida
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // Esto cerrará la aplicación construida
            Application.Quit();
        #endif
    }
}