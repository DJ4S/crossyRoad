using UnityEngine;

/*
Script asociado al DayNight controller 
*/

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight; // referencia a la luz direccional de la escana de juego
    public float dayDuration = 120f; // duracion del ciclo de un dia
    public Gradient lightColor; // definimos el color de la luz durante el dia
    public AnimationCurve lightIntensity; // definimos la intensidad de la luz

    public CarLights[] cars; // conjunto de los faros de los coches en escena
    public float nightStart = 2f / 3f; // cuando compienza la noche para el coche
    public float nightEnd = 1f / 6f; // cuando termina la noche para el coche

    private float time; // tiempo actual de ciclo

    void Start()
    {
        time = dayDuration / 2f; // al inicio de play, se inicializa el momento del dia
    }

    void Update()
    {
        time += Time.deltaTime; // suma el tiempo trasncurrido desde el ultimo update
        if (time >= dayDuration) // si se termina el ciclo, se reinicia
        {
            time = 0f; 
        }

        // lo normalizamos para tener el avlor del ciclo de dia actual
        float timeNormalized = time / dayDuration; // tiempo del dia transcurrido
        float sunAngle = timeNormalized * 360f - 90f; 
        // Quaternion para rotaciones en espacio tridimensional
        // rotamos la luz direccional de la escena 
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        sunLight.color = lightColor.Evaluate(timeNormalized); // asignados el color de la luz en funcion del gradiente definido
        sunLight.intensity = lightIntensity.Evaluate(timeNormalized); //asignamos la intensidad de la luz en funcion de la definida

        if (timeNormalized >= nightStart || timeNormalized <= nightEnd) // si estamos de noche
        {
            SetCarLights(true); // encedemos los faros
        }
        else
        {
            SetCarLights(false); // si es de dia, apagamos los faros
        }
    }

    void SetCarLights(bool state) // recibe como deben estar los faros
    {
        foreach (CarLights car in cars) // para cada faro de la escena 
        // car es en cada bucle el coche del momento dentro de todos los del array
        {
            car.SetLights(state); // se establece el valor de las luces
            // llama a la funcion del script CarLights asociado a cada coche
        }
    }
}










