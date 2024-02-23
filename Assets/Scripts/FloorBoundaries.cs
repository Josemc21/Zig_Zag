using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBoundaries : MonoBehaviour
{

    public static float leftSide = -1.875f;
    public static float rightSide = 1.875f;
    public float internalLeft;
    public float internalRight;

    void Update()
    {
        internalLeft = leftSide;
        internalRight = rightSide;
    }

}
