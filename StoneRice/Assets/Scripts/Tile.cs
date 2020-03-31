using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Position
{
    public int PosX;
    public int PosY;
}

public struct TileData : ICloneable
{
    public Position position;
    public BASETILETYPE tileType;
    public TILE_RESTRICTION tileRestriction;
    public bool isSeen;
    public bool isSighted;

    public object Clone()
    {
        return new TileData()
        {
            position = this.position,
            tileType = this.tileType,
            tileRestriction = this.tileRestriction,
            isSeen = this.isSeen,
            isSighted = this.isSighted
        };
    }
}

public class Tile : MonoBehaviour
{
    public TileData tileData = new TileData();
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer FOV_spriteRenderer;

    //디버깅
    public TILE_RESTRICTION DT;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FOV_spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();  
    }

    private void Update()
    {
        DT = tileData.tileRestriction;
    }
}
