using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public struct StairData
{
    public Position position;
    public STAIRTYPE stairType;
}

public class Stair : MonoBehaviour
{
    public StairData stairData = new StairData();
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
