using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Referencia al componente AudioSource
    private AudioSource audioSource;

    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        // Reproducir la música
        audioSource.Play();
    }
}