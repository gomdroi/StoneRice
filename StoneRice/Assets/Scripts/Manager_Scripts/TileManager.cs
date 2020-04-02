using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Stage
{
    public int mapWidth;
    public int mapHeight;
    public Position stairDownPos;
    public Position stairUpPos;
    public TileData[,] stage;
    
    //public Stage(int width, int height)
    //{
    //    mapWidth = width;
    //    mapHeight = height;
    //}
}

public class TileManager : MonoSingleton<TileManager>
{
    public GameObject[,] tileMap; //바닥에 깔려있는 타일맵
    
    public int mapWidth;
    public int mapHeight;
    public Position stairDownPos;
    public Position stairUpPos;
    public Tile[,] tileMapInfoArray;
    public List<Stage> Stages;
    public List<GameObject> stairs;

    public BaseTileFactory bTileFactory;
    public CaveMapGenerator caveGen;  
    
    private void Awake()
    {
        this.gameObject.AddComponent<BaseTileFactory>();
        this.gameObject.AddComponent<CaveMapGenerator>(); 
        
        bTileFactory = GetComponent<BaseTileFactory>();
        caveGen = GetComponent<CaveMapGenerator>();       
    }

    public void Init()
    {
        caveGen.Init();

        tileMap = new GameObject[mapWidth, mapHeight];
        tileMapInfoArray = new Tile[mapWidth, mapHeight];
        Stages = new List<Stage>();
        stairs = new List<GameObject>();

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                tileMap[j, i] = bTileFactory.createTile(BASETILETYPE.EMPTY, j, i);
                tileMapInfoArray[j, i] = tileMap[j, i].GetComponent<Tile>();
            }
        }
    }

    public void CreateCaveMap()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                tileMapInfoArray[j, i].tileData.tileType = BASETILETYPE.EMPTY;
                tileMapInfoArray[j, i].tileData.isSeen = false;
                tileMapInfoArray[j, i].tileData.isSighted = false;
            }
        }

        caveGen.GenerateCaveMap();

        //외곽 블락처리
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                if(i == 0 || j == 0 || i == mapHeight - 1 || j == mapWidth - 1)
                {
                    tileMapInfoArray[j, i].tileData.tileType = BASETILETYPE.STONEWALL;
                    tileMapInfoArray[j, i].tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                }
            }
        }

        ApplyChange();
    }

    public void MakeStairs()
    {
        //계단 위치에 게임 오브젝트를 생성 (리턴 받는 오브젝트는 나중에 쓰자)
        stairs.Add(bTileFactory.CreateStairs(STAIRTYPE.BASE_DOWN_STAIR, stairDownPos.PosX, stairDownPos.PosY));
        stairs.Add(bTileFactory.CreateStairs(STAIRTYPE.BASE_UP_STAIR, stairUpPos.PosX, stairUpPos.PosY));
    }

    public void FindStairs()
    {
        //맵이 전환됐을 때 계단을 재배치
        foreach (GameObject GO in stairs)
        {
            if (GO.GetComponent<Stair>().stairData.stairType == STAIRTYPE.NONE) continue;
            if (GO.GetComponent<Stair>().stairData.stairType == STAIRTYPE.BASE_DOWN_STAIR)
            {
                GO.GetComponent<Stair>().stairData.position = stairDownPos;
                GO.transform.position = new Vector2(GO.GetComponent<Stair>().stairData.position.PosX, GO.GetComponent<Stair>().stairData.position.PosY);
            }
            else if (GO.GetComponent<Stair>().stairData.stairType == STAIRTYPE.BASE_UP_STAIR)
            {
                GO.GetComponent<Stair>().stairData.position = stairUpPos;
                GO.transform.position = new Vector2(GO.GetComponent<Stair>().stairData.position.PosX, GO.GetComponent<Stair>().stairData.position.PosY);
            }
        }
    }

    public void SaveStage(int _stageNum, bool _isNewStage = false)
    {
        if(_isNewStage)
        {
            Stage curStage = new Stage();

            curStage.mapHeight = mapHeight;
            curStage.mapWidth = mapWidth;

            curStage.stage = new TileData[curStage.mapWidth, curStage.mapHeight];

            curStage.stairDownPos = stairDownPos;
            curStage.stairUpPos = stairUpPos;

            //타일 데이터 카피
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    curStage.stage[j, i] = (TileData)tileMapInfoArray[j, i].tileData.Clone();
                }
            }

            Stages.Add(curStage);
        }
        else if(!_isNewStage)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    Stages[_stageNum].stage[j, i] = (TileData)tileMapInfoArray[j, i].tileData.Clone();
                }
            }
        }
        
    }

    public void LoadStage(int _stageNum)
    {
        mapWidth = Stages[_stageNum].mapWidth;
        mapHeight = Stages[_stageNum].mapHeight;

        //바뀐 맵크기에 따른 타일 초기화 상태 세팅 코드 필요(오브젝트 풀/타일회수 재배치)
        
        stairDownPos = Stages[_stageNum].stairDownPos;
        stairUpPos = Stages[_stageNum].stairUpPos;

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                tileMapInfoArray[j, i].tileData = (TileData)Stages[_stageNum].stage[j, i].Clone();
                //tileMapInfoArray[j, i].tileData.position = _stageNum.stage[j, i].position;
                //tileMapInfoArray[j, i].tileData.tileType = _stageNum.stage[j, i].tileType;
                //tileMapInfoArray[j, i].tileData.tileRestriction = _stageNum.stage[j, i].tileRestriction;
                //tileMapInfoArray[j, i].tileData.fov_Value = _stageNum.stage[j, i].fov_Value;
            }
        }

        ApplyChange();
    }    

    public void ApplyChange()
    {
        ResourceManager resourceManager = ResourceManager.Instance;
        foreach(Tile tile in tileMapInfoArray)
        {           
            int spriteNum = 0; //랜덤 스프라이트 선택 변수

            switch (tile.tileData.tileType)
            {
                case BASETILETYPE.EMPTY:
                    tile.spriteRenderer.sprite = resourceManager.spriteAtlas.GetSprite("empty");
                    tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                    break;
                case BASETILETYPE.STONEFLOOR:
                    spriteNum = Random.Range(0, 7);
                    tile.spriteRenderer.sprite = resourceManager.spriteAtlas.GetSprite("orc_floor_" + spriteNum.ToString());
                    tile.tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                    break;
                case BASETILETYPE.STONEWALL:
                    spriteNum = Random.Range(0, 11);
                    tile.spriteRenderer.sprite = resourceManager.spriteAtlas.GetSprite("orc_wall_" + spriteNum.ToString());
                    tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                    break;
                case BASETILETYPE.STAIR_DOWN:
                    tile.spriteRenderer.sprite = resourceManager.spriteAtlas.GetSprite("rock_stairs_down");
                    tile.tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                    break;
                case BASETILETYPE.STAIR_UP:
                    tile.spriteRenderer.sprite = resourceManager.spriteAtlas.GetSprite("rock_stairs_up");
                    tile.tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                    break;
                case BASETILETYPE.OUTOFRANGE:                  
                    spriteNum = Random.Range(0, 3);
                    tile.spriteRenderer.sprite = resourceManager.spriteAtlas.GetSprite("lava_floor_" + spriteNum.ToString());
                    tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                    break;
                default:
                    break;
            }
        }

        foreach (Tile tile in tileMapInfoArray)
        {
            if(!tile.tileData.isSeen)
            {
                tile.FOV_spriteRenderer.color = new Color(255, 255, 255, 1);
            }
            else if(tile.tileData.isSeen)
            {
                tile.FOV_spriteRenderer.color = new Color(255, 255, 255, 0.5f);
            }
        }
    }
}
