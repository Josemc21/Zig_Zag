using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RotacionMoneda : MonoBehaviour
{
    float speed = 120.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.World);
    }
}
