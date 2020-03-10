using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    public Position position;
    //실제 타일 배열
    public Tile[,] tileMapInfo;
    public int mapHeight;
    public int mapWidth;

    private void Awake()
    {
        
    }

    //층을 옮길때마다 층의 타일맵 정보를 받아와야함
    public void PlayerInit()
    {
        tileMapInfo = TileManager.Instance.tileMapInfoArray;
        position.PosX = (int)this.gameObject.transform.position.x;
        position.PosY = (int)this.gameObject.transform.position.y;
        mapHeight = TileManager.Instance.mapHeight;
        mapWidth = TileManager.Instance.mapWidth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(transform.position.y + 1 < mapHeight) //타일 최상단이 아닐 경우
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
    }
}
