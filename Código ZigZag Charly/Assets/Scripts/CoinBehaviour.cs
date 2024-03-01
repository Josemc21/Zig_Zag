using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectSFX : MonoBehaviour
{
    private AudioSource audioSource; //Componente Audio Source adjunto al objeto de la moneda
    public AudioClip sonidoMoneda;

    void Start()
    {
        // Obtener la referencia al componente Audio Source
        audioSource = GetComponent<AudioSource>();

        // Asignar el clip de sonido al Audio Source
        audioSource.clip = sonidoMoneda;
    }
    void OnTriggerEnter(Collider other)
    {      
            Debug.Log("Moneda tocada");
            Destroy(this.gameObject);
            //TotalMonedas++;
            //Contador.text = "Monedas = " + TotalMonedas";

            // Reproducir el sonido de la moneda
        audioSource.Play();
        this.gameObject.SetActive(false);
            /*if (TotalMonedas == 11)
            {
               
            }*/
            
    }
}
