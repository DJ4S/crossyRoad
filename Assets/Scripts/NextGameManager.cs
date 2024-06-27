using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/*
Script asociado al EndGameCanvas
*/

public class NextGameManager : MonoBehaviour
{
    [SerializeField] private GameObject nextGameCanvas; // referencia a si mismo
    [SerializeField] private Button nextButton; // referencia al boton de replay
    [SerializeField] private Button mainMenuButton; // referencia al boton de menu

    private void Start() // se ejcuta al inciar
    {
        nextGameCanvas.SetActive(false); // Se desactiva el canvas al empezar para que no moleste
        nextButton.onClick.AddListener(NextLevel); // se añade un evento onClick para el boton restart
        mainMenuButton.onClick.AddListener(ReturnToMainMenu); // se añade un evento onClick para el boton Menu
    }

    public void ShowNextGameScreen() //Funcion para llamar al canvas
    {
        Invoke("ShowNextGameScreenSeg", 0.5f);  // se llama al canvas
    }

    public void ShowNextGameScreenSeg()
    {
        nextGameCanvas.SetActive(true); // se activa el canvas (se pone delante)
        Time.timeScale = 0f; // Se pausa el juego en el momento
    }
    
    private void NextLevel() // funcion asociada al boton de replay
    {
        Time.timeScale = 1f; // se reanuda el juego
        SceneManager.LoadScene("FinalLevel");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    private void ReturnToMainMenu() // funcion asociada al boton de menu
    {
        Time.timeScale = 1f; // se reanuda el juego
        SceneManager.LoadScene("Menu"); // Se carga la escena de Menu
    }
}