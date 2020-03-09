using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public enum BASETILETYPE
{
    EMPTY,
    STONEFLOOR = 2,
    STONEWALL,
    STAIR_DOWN,
    STAIR_UP,
    OUTOFRANGE
}

public enum TILE_RESTRICTION
{
    FORBIDDEN,
    MOVEABLE,
    FLYONLY
}

public class BaseTileFactory : MonoBehaviour
{
    public GameObject baseTile;
    public SpriteAtlas tiles;

    private void Awake()
    {
        baseTile = Resources.Load("Prefabs/BaseTile") as GameObject;
        tiles = Resources.Load<SpriteAtlas>("Images/Tiles");   
    }  
    
    public GameObject createTile(BASETILETYPE _type, float _PosX, float _PosY)
    {
        var oTile = Instantiate(baseTile, new Vector2(_PosX,_PosY), Quaternion.identity);

        oTile.GetComponent<Tile>().tileData.tileType = _type;
        //oTile.GetComponent<SpriteRenderer>().sprite = baseTile_Sprite[(int)_type];

        return oTile;
    }
}