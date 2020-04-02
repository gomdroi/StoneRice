using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    FLYONLY,
    OCCUPIED
}

public class BaseTileFactory :MonoBehaviour
{
    public GameObject baseTile;
    public GameObject stairPrefab;

    public GameObject tileCargo;
    private void Awake()
    {
        baseTile = Resources.Load("Prefabs/BaseTile") as GameObject;
        stairPrefab = Resources.Load("Prefabs/Stair") as GameObject;
        tileCargo = GameObject.Find("TilePool");
    }  
    
    public GameObject createTile(BASETILETYPE _type, int _PosX, int _PosY)
    {
        var oTile = Instantiate(baseTile, new Vector2(_PosX,_PosY), Quaternion.identity);
        oTile.transform.SetParent(tileCargo.transform);

        oTile.GetComponent<Tile>().tileData.tileType = _type;
        oTile.GetComponent<Tile>().tileData.position.PosX = _PosX;
        oTile.GetComponent<Tile>().tileData.position.PosY = _PosY;
        oTile.GetComponent<Tile>().tileData.isSeen = false;
        oTile.GetComponent<Tile>().tileData.isSighted = false;
        //oTile.GetComponent<SpriteRenderer>().sprite = baseTile_Sprite[(int)_type];

        return oTile;
    }

    public GameObject CreateStairs(STAIRTYPE _stairtype, int _PosX, int _PosY)
    {
        var oObject = Instantiate(stairPrefab, new Vector2(_PosX, _PosY), Quaternion.identity);
        oObject.transform.SetParent(tileCargo.transform);

        oObject.GetComponent<Stair>().stairData.position.PosX = _PosX;
        oObject.GetComponent<Stair>().stairData.position.PosY = _PosY;
        oObject.GetComponent<Stair>().stairData.stairType = _stairtype;
        //예시
        if (_stairtype == STAIRTYPE.BASE_DOWN_STAIR)
        {
            oObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("rock_stairs_down");
        }
        else if (_stairtype == STAIRTYPE.BASE_UP_STAIR)
        {
            oObject.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("rock_stairs_up");
        }

        return oObject;
    }
}