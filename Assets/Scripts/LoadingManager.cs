using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider; // Asigna el slider desde el inspector
    public string gameSceneName = "Gameplay"; // Nombre de la escena del juego

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName);

        // Mientras la escena no se haya cargado completamente
        while (!operation.isDone)
        {
            // El progreso del cargado está entre 0 y 0.9, se convierte a un rango de 0 a 1
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
