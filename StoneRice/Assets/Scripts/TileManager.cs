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
            //테스트용 초기화
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    tileMapInfoArray[i, j].tileData.tileType = BASETILETYPE.EMPTY;
                }
            }

            caveGen.GenerateCaveMap();
        }
        
        //적용
        if(Input.GetKeyDown(KeyCode.Space))
        {
            applyChange();
        }

        //방탐색 디버그
        if (Input.GetKeyDown(KeyCode.D))
        {
            bool[,] rc = caveGen.roomCheck;

            for(int i = 0; i < mapHeight; i++)
            {
                for(int j = 0; j < mapWidth; j++)
                {
                    if(rc[j,i])
                    {
                        tileMapInfoArray[j, i].tileData.tileType = BASETILETYPE.DEBUG;
                    }
                }
            }

            applyChange();
        }

        //가장 큰방 남기기
        if (Input.GetKeyDown(KeyCode.F))
        {
            List<Room> rooms = caveGen.rooms;

            for(int i = 1; i < rooms.Count; i++)
            {
                foreach(Tile tile in rooms[i].roomList)
                {
                    tile.tileData.tileType = BASETILETYPE.DEBUG;
                }
            }
        }
    }
    
    void applyChange()
    {
        foreach(Tile tile in tileMapInfoArray)
        {
            switch(tile.tileData.tileType)
            {
                case BASETILETYPE.EMPTY:
                    tile.spriteRenderer.sprite = bTileFactory.baseTile_Sprite[0];
                    break;
                case BASETILETYPE.STONEFLOOR:
                    tile.spriteRenderer.sprite = bTileFactory.baseTile_Sprite[2];
                    break;
                case BASETILETYPE.STONEWALL:
                    tile.spriteRenderer.sprite = bTileFactory.baseTile_Sprite[3];
                    break;
                case BASETILETYPE.STAIR_DOWN:
                    tile.spriteRenderer.sprite = bTileFactory.stair_Sprite[1];
                    break;
                case BASETILETYPE.STAIR_UP:
                    tile.spriteRenderer.sprite = bTileFactory.stair_Sprite[0];
                    break;
                case BASETILETYPE.DEBUG:
                    tile.spriteRenderer.sprite = bTileFactory.xTile_Sprite[0];
                    break;
                default:
                    break;
            }
        }
    }
}
