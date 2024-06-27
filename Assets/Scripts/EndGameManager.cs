using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/*
Script asociado al EndGameCanvas
*/

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private GameObject endGameCanvas; // referencia a si mismo
    [SerializeField] private Text EndscoreText; // referencia al texto del titulo
    [SerializeField] private Button restartButton; // referencia al boton de replay
    [SerializeField] private Button mainMenuButton; // referencia al boton de menu
    private int scoreDef; // variable auxiliar para imprimir el score final

    private void Start() // se ejcuta al inciar
    {
        endGameCanvas.SetActive(false); // Se desactiva el canvas al empezar para que no moleste
        restartButton.onClick.AddListener(RestartGame); // se añade un evento onClick para el boton restart
        mainMenuButton.onClick.AddListener(ReturnToMainMenu); // se añade un evento onClick para el boton Menu
    }

    public void ShowEndGameScreen(int score) //Funcion para llamar al canvas
    {
        scoreDef = score; // guardamos el score del personaje
        // se añade una espera de 1 segundo para que se vea la animacion de las particulas antes de poner el canvas
        Invoke("ShowEndGameScreenSeg", 1f);  // se llama al canvas
    }

    public void ShowEndGameScreenSeg()
    {
        endGameCanvas.SetActive(true); // se activa el canvas (se pone delante)
        EndscoreText.text = "Score: " + scoreDef; // se escribe el puntuaje en el canvas
        Time.timeScale = 0f; // Se pausa el juego en el momento de muerte
    }
    
    private void RestartGame() // funcion asociada al boton de replay
    {
        Time.timeScale = 1f; // se reanuda el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    private void ReturnToMainMenu() // funcion asociada al boton de menu
    {
        Time.timeScale = 1f; // se reanuda el juego
        SceneManager.LoadScene("Menu"); // Se carga la escena de Menu
    }
}

