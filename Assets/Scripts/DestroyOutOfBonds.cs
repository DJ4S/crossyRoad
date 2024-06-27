using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*/

public class DestroyOutOfBounds : MonoBehaviour
{
    public float boundaryZ = 19.5f;  // Establecemos el valor a partir del cual se destruyen los objetos en el mapa

    void Update()
    {
        // verificamos si la position del objeto en Z es superior al limite establecido
        // Mathf.Abs para que se destruya el objeto en ambas direcciones
        if (Mathf.Abs(transform.position.z) > boundaryZ) 
        {
            Destroy(gameObject);
            // si esta fuera del limite se destruye

        }
    }
}
