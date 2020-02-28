using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BASETILETYPE
{
    EMPTY,
    STONEFLOOR,
    STONEWALL
}

public class TileCtrl : MonoBehaviour
{
    public BASETILETYPE tileType;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprite;

    private void Awake()
    {
        sprite = Resources.LoadAll<Sprite>("Images/Forest_terrain_gray_128px");
        spriteRenderer = GetComponent<SpriteRenderer>();       
    }

    private void Start()
    {
        switch(tileType)
        {
            case BASETILETYPE.EMPTY:
                spriteRenderer.sprite = sprite[0];
                break;
            case BASETILETYPE.STONEFLOOR:
                spriteRenderer.sprite = sprite[2];
                break;
            case BASETILETYPE.STONEWALL:
                spriteRenderer.sprite = sprite[3];
                break;
            default:
                break;

        }
       
    }

}
