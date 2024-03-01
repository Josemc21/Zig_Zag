using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenerateFloor : MonoBehaviour
{
    public GameObject[] FloorTypeX;
    public GameObject[] FloorTypeZ;
    private float zValue = 25;
    private float xValue = 0;
    private bool generatingFloor = false;
    private int TotalTypesX;
    private int TotalTypesZ;
    public int Type0_Counter = 0;

    void Start() 
    { 
        TotalTypesX = FloorTypeX.Length; 
        TotalTypesZ = FloorTypeZ.Length;
    }

    void Update()
    {
        if (generatingFloor == false) 
        {
            generatingFloor = true;
            StartCoroutine(FloorGenerator());
        }
    }

    IEnumerator FloorGenerator()
    {
        float random = Random.Range(0.0f, 1.1f);
        int floorSelectorX = Random.Range(0,TotalTypesX);
        int floorSelectorZ = Random.Range(0,TotalTypesZ);

        if (random > 0.5f)
        {
            if (Type0_Counter < 5) 
            {
                xValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                Instantiate(FloorTypeX[0], new Vector3(xValue, 0, zValue), Quaternion.identity);
                Type0_Counter++;
            }
            else 
            {
                xValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                Instantiate(FloorTypeX[floorSelectorX], new Vector3(xValue, 0, zValue), Quaternion.identity);
                Type0_Counter = 0;
            }
        } 
        else 
        {
            if (Type0_Counter < 5) 
            {
                zValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                Instantiate(FloorTypeZ[0], new Vector3(xValue, 0, zValue), Quaternion.identity);
                Type0_Counter++;
            }
            else 
            {
                zValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                Instantiate(FloorTypeZ[floorSelectorZ], new Vector3(xValue, 0, zValue), Quaternion.identity);
                Type0_Counter = 0;
            }
        }
        generatingFloor = false;
    }
}
