using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoSingleton<TileManager>
{
    public GameObject[,] tileMap; //바닥에 깔려있는 타일맵
    public List<GameObject> objectList; //바닥에 깔려있는 오브젝트 리스트, 타일 매니저가 갖고있을게 아닌거 같음.
    public int mapWidth;
    public int mapHeight;
    public Tile[,] tileMapInfoArray;
    public BaseTileFactory bTileFactory;
    public CaveMapGenerator caveGen;
    
    private void Awake()
    {
        this.gameObject.AddComponent<BaseTileFactory>();
        this.gameObject.AddComponent<CaveMapGenerator>(); 
        
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
            caveGen.CaveInit();

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
            applyChange();
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
            int spriteNum = 0; //랜덤 스프라이트 선택 변수

            switch (tile.tileData.tileType)
            {
                case BASETILETYPE.EMPTY:
                    tile.spriteRenderer.sprite = bTileFactory.tiles.GetSprite("empty");
                    tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                    break;
                case BASETILETYPE.STONEFLOOR:
                    spriteNum = Random.Range(0, 7);
                    tile.spriteRenderer.sprite = bTileFactory.tiles.GetSprite("orc_floor_" + spriteNum.ToString());
                    tile.tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                    break;
                case BASETILETYPE.STONEWALL:
                    spriteNum = Random.Range(0, 11);
                    tile.spriteRenderer.sprite = bTileFactory.tiles.GetSprite("orc_wall_" + spriteNum.ToString());
                    tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                    break;
                case BASETILETYPE.STAIR_DOWN:
                    tile.spriteRenderer.sprite = bTileFactory.tiles.GetSprite("rock_stairs_down");
                    tile.tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                    break;
                case BASETILETYPE.STAIR_UP:
                    tile.spriteRenderer.sprite = bTileFactory.tiles.GetSprite("rock_stairs_up");
                    tile.tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                    break;
                case BASETILETYPE.OUTOFRANGE:                  
                    spriteNum = Random.Range(0, 3);
                    tile.spriteRenderer.sprite = bTileFactory.tiles.GetSprite("lava_floor_" + spriteNum.ToString());
                    tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                    break;
                default:
                    break;
            }
        }
    }
}
