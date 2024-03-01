using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

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

        GameObject newFloor;

        if (random > 0.5f)
        {
            if (Type0_Counter < 5) 
            {
                xValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                newFloor = Instantiate(FloorTypeX[0], new Vector3(xValue, 0, zValue), Quaternion.identity * FloorTypeX[0].transform.localRotation);
                Type0_Counter++;
                StartCoroutine(CoinGenerator(newFloor, 0));
            }
            else 
            {
                xValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                newFloor = Instantiate(FloorTypeX[floorSelectorX], new Vector3(xValue, 0, zValue), Quaternion.identity * FloorTypeX[floorSelectorX].transform.localRotation);
                Type0_Counter = 0;
                StartCoroutine(CoinGenerator(newFloor, floorSelectorX));
            }
        } 
        else 
        {
            if (Type0_Counter < 5) 
            {
                zValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                newFloor = Instantiate(FloorTypeZ[0], new Vector3(xValue, 0, zValue), Quaternion.identity);
                Type0_Counter++;
                StartCoroutine(CoinGenerator(newFloor, 0));
            }
            else 
            {
                zValue += 5.0f;
                yield return new WaitForSeconds(0.7f);
                newFloor = Instantiate(FloorTypeZ[floorSelectorZ], new Vector3(xValue, 0, zValue), Quaternion.identity);
                Type0_Counter = 0;
                StartCoroutine(CoinGenerator(newFloor, floorSelectorZ));
            }
        }
        generatingFloor = false;
    }

    IEnumerator CoinGenerator(GameObject Floor, int floorSelector)
    {
        float random = Random.Range(0.0f, 1.0f);

        if (floorSelector != 0)
        {
            if (random < 0.2f)
            {
                if(Floor.gameObject.transform.Find("Coin Left"))
                {
                    Floor.gameObject.transform.Find("Coin Left").gameObject.SetActive(true);
                }
            }
            else if (random > 0.2f && random < 0.4f)
            {
                if(Floor.gameObject.transform.Find("Coin Center"))
                {
                    Floor.gameObject.transform.Find("Coin Center").gameObject.SetActive(true);
                }
            }
            else if (random > 0.4f && random < 0.6f)
            {
                if(Floor.gameObject.transform.Find("Coin Right"))
                {
                    Floor.gameObject.transform.Find("Coin Right").gameObject.SetActive(true);
                }
            }
        }
        else {
            if (random < 0.15f)
            {
                if(Floor.gameObject.transform.Find("Coin Center"))
                {
                    Floor.gameObject.transform.Find("Coin Center").gameObject.SetActive(true);
                }
            }
        }
        
        
        yield return new WaitForSeconds(0.01f);
    }
}
