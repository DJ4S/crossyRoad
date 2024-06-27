using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
Script asociado al OptionsController
*/

public class OptionsManager : MonoBehaviour
{
    public Button musicToggleButton; // referencia al boton de musica
    private bool isMusicOn = true; // booleano que define si la musica esta activa
    
    // al iniciar el juego se llama a la funcion para definir el estado de la musica
    void Start()
    {
        UpdateMusicButton(); 
    }

    // funcion si se da al boton de Return
    public void ReturnToMainMenu()
    {
        //Llama la funcion LoadScene de SceneManager para cargar la escena de Menu
        SceneManager.LoadScene("Menu");
    }

    // funcion si se toca el boton de musica
    public void ToggleMusic()
    {
        // se cambia el valor, si estaba on, se pasa a off y viceversa
        isMusicOn = !isMusicOn;
        // se actualiza el valor del texto del boton a la nueva situacion
        UpdateMusicButton();
        // Añadir la logica para detener la musica
        // AudioListener.pause = !isMusicOn;
    }

    private void UpdateMusicButton()
    {
        // actualiza el texto de la del boton en funcion de si la musica esta activa o no
        musicToggleButton.GetComponentInChildren<Text>().text = "Music: " + (isMusicOn ? "On" : "Off");
    }
}



