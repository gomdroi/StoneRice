using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public int mapWidth;
    public int mapHeight;
    public Position stairDownPos;
    public Position stairUpPos;
    public TileData[,] stage;
    //오브젝트 리스트 추가해야함
    //아이템 리스트는 나중에 아이템매니저에서 해결하자 여기선 타일관련해서만 정리
}

public class TileManager : MonoSingleton<TileManager>
{
    public GameObject[,] tileMap; //바닥에 깔려있는 타일맵
    public List<GameObject> objectList; //바닥에 깔려있는 오브젝트 리스트(문,함정,계단을 포함)
    public int mapWidth;
    public int mapHeight;
    public Position stairDownPos;
    public Position stairUpPos;
    public Tile[,] tileMapInfoArray;
    public List<Stage> Stages;

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
            caveGen.Init();

            tileMap = new GameObject[mapWidth, mapHeight];
            tileMapInfoArray = new Tile[mapWidth, mapHeight];
            Stages = new List<Stage>();

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
            ApplyChange();

            Stages.Add(SaveStage());
        }

        //임시 에이스타 초기화
        if (Input.GetKeyDown(KeyCode.A))
        {
            Astar.Instance.AstarInit();
        }

        //스테이지 선택
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadStage(Stages[0]);
            ApplyChange();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadStage(Stages[1]);
            ApplyChange();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadStage(Stages[2]);
            ApplyChange();
        }
    }

    public Stage SaveStage()
    {
        Stage curStage = new Stage();

        curStage.mapHeight = mapHeight;
        curStage.mapWidth = mapWidth;
        TileData[,] tileInfoClone = copyStageData();

        curStage.stage = new TileData[curStage.mapWidth, curStage.mapHeight];
        //오브젝트 리스트 세이브 추가해야함

        curStage.stairDownPos = stairDownPos;
        curStage.stairUpPos = stairUpPos;

        //타일 데이터 카피
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                //curStage.stage[j, i] = tileMapInfoArray[j, i].tileData;
                curStage.stage[j, i] = tileInfoClone[j, i];
            }
        }
        return curStage;
    }

    public TileData[,] copyStageData()
    {
        TileData[,] TileInfoClone = new TileData[mapWidth, mapHeight];

        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                TileInfoClone[j, i] = new TileData();
                TileInfoClone[j, i].position = tileMapInfoArray[j, i].tileData.position;
                TileInfoClone[j, i].tileRestriction = tileMapInfoArray[j, i].tileData.tileRestriction;
                TileInfoClone[j, i].tileType = tileMapInfoArray[j, i].tileData.tileType;
                TileInfoClone[j, i].fov_Value = tileMapInfoArray[j, i].tileData.fov_Value;
            }
        }

        return TileInfoClone;
    }

    void LoadStage(Stage _stageNum)
    {
        mapWidth = _stageNum.mapWidth;
        mapHeight = _stageNum.mapHeight;

        //바뀐 맵크기에 따른 타일 초기화 상태 세팅 코드 필요(오브젝트 풀/타일회수 재배치)

        //오브젝트 리스트 로드 필요함

        stairDownPos = _stageNum.stairDownPos;
        stairUpPos = _stageNum.stairUpPos;

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                tileMapInfoArray[j, i].tileData = _stageNum.stage[j, i];
            }
        }
    }    

    void ApplyChange()
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
