using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public float PosX;
    public float PosY;
}

public class Tile
{
    public Position position;
    public GameObject oTile; 

    public void setTilePosition(float _PosX, float _PosY)
    {
        position.PosX = _PosX;
        position.PosY = _PosY;
    }

    
}
