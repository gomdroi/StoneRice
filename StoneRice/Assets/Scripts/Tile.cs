using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Position
{
    public int PosX;
    public int PosY;
}

public class TileData
{
    public Position position;
    public BASETILETYPE tileType;
    public TILE_RESTRICTION tileRestriction;
    public int fov_Value;
}

public class Tile : MonoBehaviour
{
    public TileData tileData = new TileData();
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer FOV_spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FOV_spriteRenderer = GetComponentInChildren<SpriteRenderer>();  
    }

    private void Start()
    {
        tileData.position.PosX = (int)transform.position.x;
        tileData.position.PosY = (int)transform.position.y;
    }

    private void Update()
    {
        //플레이어 위치 감지 FOV 처리
    }
}
