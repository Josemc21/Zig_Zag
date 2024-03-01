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
        if (other.gameObject.CompareTag("Moneda"))
        {
            //TotalMonedas++;
            //Contador.text = "Monedas = " + TotalMonedas";

            // Reproducir el sonido de la moneda
            audioSource.Play();
            other.gameObject.SetActive(false);
        } 
    }
}
