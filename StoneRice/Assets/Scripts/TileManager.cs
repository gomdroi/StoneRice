using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoSingleton<TileManager>
{
    public GameObject[,] tileMap;
    public int mapWidth;
    public int mapHeight;
    public Tile[,] tileMapInfoArray;
    public BaseTileFactory bTileFactory;
    public CaveMapGenerator caveGen;
    
    
    private void Awake()
    {
        bTileFactory = GetComponent<BaseTileFactory>();
        caveGen = GetComponent<CaveMapGenerator>();
    }

    void Start()
    {
    }

    private void Update()
    {
        //임시 초기화
        if(Input.GetKeyDown(KeyCode.S))
        {
            tileMap = new GameObject[mapWidth, mapHeight];
            tileMapInfoArray = new Tile[mapWidth, mapHeight];

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    tileMap[i, j] = bTileFactory.createTile(BASETILETYPE.EMPTY, i, j);
                    tileMapInfoArray[i, j] = tileMap[i, j].GetComponent<Tile>();
                }
            }
        }

        //맵생성
        if(Input.GetKeyDown(KeyCode.C))
        {
            caveGen.GenerateCaveMap();
        }
        
        //적용
        if(Input.GetKeyDown(KeyCode.Space))
        {
            applyChange();
        }
    }

    void applyChange()
    {
        foreach(Tile tile in tileMapInfoArray)
        {
            switch(tile.tileData.tileType)
            {
                case BASETILETYPE.EMPTY:
                    tile.spriteRenderer.sprite = bTileFactory.sprite[0];
                    break;
                case BASETILETYPE.STONEFLOOR:
                    tile.spriteRenderer.sprite = bTileFactory.sprite[2];
                    break;
                case BASETILETYPE.STONEWALL:
                    tile.spriteRenderer.sprite = bTileFactory.sprite[3];
                    break;
                default:
                    break;
            }
        }
    }
}
