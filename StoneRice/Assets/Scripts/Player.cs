using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    public Position position;
    public Vector3 prePos;
    //실제 타일 배열
    public Tile[,] tileMapInfo;
    public int mapHeight;
    public int mapWidth;
    TurnManager m_TurnManager;
    TileManager m_TileManager;
    FOV fov;

    int viewRange;   

    private void Awake()
    {
        m_TurnManager = TurnManager.Instance;
        m_TileManager = TileManager.Instance;
        fov = new FOV();
    }

    private void Start()
    {
        m_TurnManager.turnState = TURN_STATE.PLAYER_TURN;
        prePos = transform.position;


    }

    //층을 옮길때마다 층의 타일맵 정보를 받아와야함
    public void PlayerInit()
    {
        tileMapInfo = TileManager.Instance.tileMapInfoArray;
        position.PosX = (int)this.gameObject.transform.position.x;
        position.PosY = (int)this.gameObject.transform.position.y;
        mapHeight = TileManager.Instance.mapHeight;
        mapWidth = TileManager.Instance.mapWidth;

        viewRange = 7;

        fov.Init();
        fov.CalcFov(tileMapInfo, position.PosX, position.PosY, viewRange);
    }

    public void TurnOverCheck()
    {
        if(prePos != transform.position)
        {
            prePos = transform.position;
            position.PosX = (int)transform.position.x;
            position.PosY = (int)transform.position.y;
            fov.CalcFov(tileMapInfo, position.PosX, position.PosY, viewRange);
            m_TurnManager.turnState = TURN_STATE.INTERACTIVE;
        }       
    }

    public void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (transform.position.y + 1 < mapHeight) //타일 최상단이 아닐 경우
            {
                if (tileMapInfo[(int)transform.position.x, (int)transform.position.y + 1].tileData.tileRestriction == TILE_RESTRICTION.MOVEABLE)
                {
                    transform.position = tileMapInfo[(int)transform.position.x, (int)transform.position.y + 1].transform.position;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (transform.position.y - 1 >= 0) //타일 최하단이 아닐 경우
            {
                if (tileMapInfo[(int)transform.position.x, (int)transform.position.y - 1].tileData.tileRestriction == TILE_RESTRICTION.MOVEABLE)
                {
                    transform.position = tileMapInfo[(int)transform.position.x, (int)transform.position.y - 1].transform.position;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (transform.position.x - 1 >= 0) //타일 가장 좌측이 아닐 경우
            {
                if (tileMapInfo[(int)transform.position.x - 1, (int)transform.position.y].tileData.tileRestriction == TILE_RESTRICTION.MOVEABLE)
                {
                    transform.position = tileMapInfo[(int)transform.position.x - 1, (int)transform.position.y].transform.position;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (transform.position.x + 1 < mapWidth) //타일 가장 우측이 아닐 경우
            {
                if (tileMapInfo[(int)transform.position.x + 1, (int)transform.position.y].tileData.tileRestriction == TILE_RESTRICTION.MOVEABLE)
                {
                    transform.position = tileMapInfo[(int)transform.position.x + 1, (int)transform.position.y].transform.position;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Comma))
        {         
            if (transform.position.x == m_TileManager.stairDownPos.PosX && transform.position.y == m_TileManager.stairDownPos.PosY)
            {
                GameManager.Instance.GoDownStage();
                transform.position = new Vector2(m_TileManager.stairUpPos.PosX, m_TileManager.stairUpPos.PosY);
            }
            else if(transform.position.x == m_TileManager.stairUpPos.PosX && transform.position.y == m_TileManager.stairUpPos.PosY)
            {
                int preStage = GameManager.Instance.curStage;
                GameManager.Instance.GoUpStage();
                if (preStage != 0)
                {
                    transform.position = new Vector2(m_TileManager.stairDownPos.PosX, m_TileManager.stairDownPos.PosY);
                }
            }
        }
    }
}
