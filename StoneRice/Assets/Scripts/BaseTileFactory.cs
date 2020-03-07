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
    DEBUG
}

public class BaseTileFactory : MonoBehaviour
{
    public GameObject baseTile;
    public Sprite[] baseTile_Sprite;
    public Sprite[] xTile_Sprite;
    public Sprite[] stair_Sprite;

    private void Awake()
    {
        baseTile = Resources.Load("Prefabs/BaseTile") as GameObject;
        baseTile_Sprite = Resources.LoadAll<Sprite>("Images/Forest_terrain_gray_128px");
        xTile_Sprite = Resources.LoadAll<Sprite>("Images/Forest_terrain_blue_128px");
        stair_Sprite = Resources.LoadAll<Sprite>("Images/Forest_terrain_slope_gray_snow_128px");
    }  
    
    public GameObject createTile(BASETILETYPE _type, float _PosX, float _PosY)
    {
        var oTile = Instantiate(baseTile, new Vector2(_PosX,_PosY), Quaternion.identity);

        oTile.GetComponent<Tile>().tileData.tileType = _type;
        oTile.GetComponent<SpriteRenderer>().sprite = baseTile_Sprite[(int)_type];

        return oTile;
    }
}