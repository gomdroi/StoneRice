using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    float PosX;
    float PosY;
}

public enum BASETILETYPE
{
    NONE,
    STONEFLOOR,
    STONEWALL
}

public class Tile
{

    public GameObject baseTile;

    public void setBaseTileType(BASETILETYPE _tileType)
    {
        switch(_tileType)
        {
            case BASETILETYPE.STONEFLOOR:
                baseTile.GetComponent<SpriteRenderer>().sprite = Resources.Load("Images/Forest_terrain_gray_128px_2") as Sprite;
                break;
            case BASETILETYPE.STONEWALL:
                break;
            default:
                break;
        }
    }

    public void setTilePosition(int _PosX, int _PosY)
    {
    }
}
