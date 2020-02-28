using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseTileFactory : MonoBehaviour
{
    public GameObject baseTile;

    private void Awake()
    {
        baseTile = Resources.Load("Prefabs/BaseTile") as GameObject;
    }  
    
    public GameObject createTile(string _type, float _PosX, float _PosY)
    {
        var oTile = Instantiate(baseTile, new Vector2(_PosX,_PosY), Quaternion.identity);
        switch (_type)
        {
            case "Empty":
                oTile.GetComponent<TileCtrl>().tileType = BASETILETYPE.EMPTY;
                break;
            case "StoneFloor":              
                oTile.GetComponent<TileCtrl>().tileType = BASETILETYPE.STONEFLOOR;
                break;
            case "StoneWall":
                oTile.GetComponent<TileCtrl>().tileType = BASETILETYPE.STONEWALL;
                break;
            default:
                break;
                
        }
        return oTile;
    }
}