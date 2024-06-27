using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
Script asociado al agua
*/

public class KillPlayerOnTouch : MonoBehaviour
{
    public AudioClip waterSound; // Referencia a las particulas de impacto con agua
    public GameObject waterSplashParticles; // Referencia a particulas de agua

    // Se llama este metodo automaticamente al impacto del coche con el player (con Rigidbody con isTrigger)
    private void OnCollisionEnter(Collision collision)
    {
        // Se verifica si el elemento impactado es el jugador
        if (collision.collider.GetComponent<Player>() != null)
        {
            Destroy(collision.gameObject); // Se mata al juagdor
            // Se emite el sonide de caida al agua en la posicion del jugador
            AudioSource.PlayClipAtPoint(waterSound, transform.position);
            // Emite las particulas de agua en la posicion del jugador
            Instantiate(waterSplashParticles, collision.transform.position, Quaternion.identity);
        }
    }
}
