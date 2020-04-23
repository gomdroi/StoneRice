using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct TrapData
{
    public Position position;
    public bool isActive;
    public TRAPTYPE trapType;
}

public class Trap : MonoBehaviour
{
    public TrapData trapData = new TrapData();
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        trapData.isActive = false;
    }

    private void OnDisable()
    {
        trapData.isActive = false;
        trapData.trapType = TRAPTYPE.NONE;
    }
}
