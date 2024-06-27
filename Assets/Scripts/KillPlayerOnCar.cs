using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Script asociado al coche
*/

public class KillPlayerOnCar : MonoBehaviour
{
    public GameObject carHitParticles; // Referencia a las particulas de impacto de coche
    public AudioClip carCrashSound; // Sonido de choque del coche

    // Se llama este metodo automaticamente al impacto del coche con el player (con Rigidbody con isTrigger)
    private void OnCollisionEnter(Collision collision)
    {
        // Se verifica si el elemento impactado es el jugador
        if (collision.collider.GetComponent<Player>() != null)
        {
            // Se reporduce el sonido en la posicion del impacto
            AudioSource.PlayClipAtPoint(carCrashSound, collision.transform.position);
            // Crea efecto de particulas en la posicion de la colision
            Instantiate(carHitParticles, collision.transform.position, Quaternion.identity);
            // Se mata al jugador
            Destroy(collision.gameObject);
        }
    }
}

