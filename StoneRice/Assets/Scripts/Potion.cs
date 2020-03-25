using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum POTIONCOLOR
{
    WHITE,
    DARK,
    PINK,
    BUBBLY
}

public enum POTIONTYPE
{
    CURING,
    MIGHT,
    AGILITY,
    DEGENERATION
}

public class PotionData
{
    public Position position;
    public POTIONCOLOR potionColor;
    public POTIONTYPE potionType;
    public int[] rndPotionNum;
}

public class Potion : MonoBehaviour
{
    public PotionData potionData = new PotionData();
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //private void Start()
    //{
    //    for(int i = 0; i < potionData.rndPotionNum.Length; i++)
    //    {
    //        if(potionData.potionType == (POTIONTYPE)i)
    //        {
    //            potionData.potionColor = (POTIONCOLOR)potionData.rndPotionNum[i];
    //        }
    //    }
    //}
}
