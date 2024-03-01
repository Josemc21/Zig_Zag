using UnityEngine;

public class MoverFondo : MonoBehaviour
{
    public Transform pelota; // Referencia al transform de la pelota
    public float factorMovimiento = 0.5f; // Factor de movimiento para ajustar la velocidad del fondo

    private Vector3 offset; // Distancia inicial entre la pelota y el fondo

    void Start()
    {
        offset = transform.position - pelota.position; // Calcula la distancia inicial entre la pelota y el fondo
    }

    void LateUpdate()
    {
        Vector3 targetPos = pelota.position + offset; // Calcula la posición objetivo del fondo
        transform.position = Vector3.Lerp(transform.position, targetPos, factorMovimiento * Time.deltaTime); // Mueve suavemente el fondo hacia la posición objetivo
    }
}