using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    public Position position;
    //실제 타일 배열
    public Tile[,] tileMapInfo;

    private void Awake()
    {
        tileMapInfo = TileManager.Instance.tileMapInfoArray;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (tileMapInfo[position.PosX, position.PosY - 1].tileData.tileRestriction == TILE_RESTRICTION.MOVEABLE)
            {
                transform.position = tileMapInfo[position.PosX, position.PosY - 1].transform.position;
            }

            
        }
    }
}
