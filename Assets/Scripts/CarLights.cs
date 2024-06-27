using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Script asociado al prefab del coche, 
donde se especifican las luces de los faros.
Llamado por el DayNightController.
*/

public class CarLights : MonoBehaviour
{
    // se definen los campos donde se establecen las luces de los faros
    public Light leftHeadlight;
    public Light rightHeadlight;

    public void SetLights(bool state) // recibe si las luces estan apagadas o no
    {
        // cambia el estado de los faros segun el valor recibido
        leftHeadlight.enabled = state; 
        rightHeadlight.enabled = state;
    }
}
