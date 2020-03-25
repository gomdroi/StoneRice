using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum STAIRTYPE
{
    NONE,
    BASE_DOWN_STAIR,
    BASE_UP_STAIR
}

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
