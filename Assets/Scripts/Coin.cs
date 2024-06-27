using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/*
Script asociado al player
*/

public class Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText; // texto del canvas que va actualizando las monedas durante el juego
    public int cantidadMonedas; // valor de las monedas recogidas
    public AudioClip coinSound; // sonido al recoger la moneda
    private Animator animator; // elemento que gestionara las animaciones
    
    // Declaración del evento para notificar la recolección de monedas
    public event Action<int> CoinCollected;

    /* 
    Se llama automaticamente cuando el player entra en contacto con otro objeto
    con collider y rigidbody (isTrigger)
    */

    private void Start() // al iniciar el juego
    {
        animator = GetComponent<Animator>(); // se obtiene el animator adjunto al player

        if(CompareTag("Moneda"))
        {
            StartCoroutine(AnimateCoin());
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moneda")) // verifica si el objeto colisionada tiene el tag moneda
        {
            cantidadMonedas++; // se aumenta las monedas recogidas
            AudioSource.PlayClipAtPoint(coinSound, transform.position); //suena el sonido
            Destroy(other.gameObject); // elimina la moneda
            coinText.text = "Coins: " + cantidadMonedas; // actualiza el valor de monedas del canvas
            CoinCollected?.Invoke(cantidadMonedas);
        }
    }

    private IEnumerator AnimateCoin()
    {
        while (true)
        {
            animator.SetTrigger("Jump");
            yield return new WaitForSeconds(4f); // Tiempo entre saltos
        }
    }
}