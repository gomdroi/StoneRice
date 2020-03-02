using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public float PosX;
    public float PosY;
}

public class TileData
{
    public Position position;
    public BASETILETYPE tileType;
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
        tileData.position.PosX = transform.position.x;
        tileData.position.PosY = transform.position.y;
    }

    private void Update()
    {
        //플레이어 위치 감지 FOV 처리
    }
}
