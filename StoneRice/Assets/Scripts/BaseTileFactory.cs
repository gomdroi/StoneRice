using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTileFactory : TileFactory
{
    public override void createTile(string _type)
    {
        Tile tile = new Tile();
        switch (_type)
        {           
            case "StoneFloor":
                tile.setBaseTileType(BASETILETYPE.STONEFLOOR);
                break;
            case "StoneWall":
                tile.setBaseTileType(BASETILETYPE.STONEWALL);
                break;
            default:
                break;
        }
    }
}
