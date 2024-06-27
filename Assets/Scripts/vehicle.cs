using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Script asociado a los coches y tablones
*/

public class Vehicle : MonoBehaviour
{
    [SerializeField] private float speed; // establecemos la velocidad del objeto
    public bool isLog; // definimos si el objeto es un tablon o coche
    // se utilizara en Player para que el jugador se quede en el tablon

    private void Update() // se llama en cada momento del juego
    {
        // utilizamos translate para mover el objeto en la direccion opuesta al eje Z de donde se ha generado
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
    
}
