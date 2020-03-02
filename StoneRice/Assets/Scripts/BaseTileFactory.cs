using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BASETILETYPE
{
    EMPTY,
    STONEFLOOR = 2,
    STONEWALL
}

public class BaseTileFactory : MonoBehaviour
{
    public GameObject baseTile;
    public Sprite[] sprite;

    private void Awake()
    {
        baseTile = Resources.Load("Prefabs/BaseTile") as GameObject;
        sprite = Resources.LoadAll<Sprite>("Images/Forest_terrain_gray_128px");
    }  
    
    public GameObject createTile(BASETILETYPE _type, float _PosX, float _PosY)
    {
        var oTile = Instantiate(baseTile, new Vector2(_PosX,_PosY), Quaternion.identity);

        oTile.GetComponent<Tile>().tileData.tileType = _type;
        oTile.GetComponent<SpriteRenderer>().sprite = sprite[(int)_type];

        return oTile;
    }
}