using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PotionFactory : MonoBehaviour
{
    public GameObject potionPrefab;
    
    public int potionKindNum;
    public int[] rndPotionNum;

    private void Awake()
    {
        potionPrefab = Resources.Load("Prefabs/Potion") as GameObject;
        
    }

    private void Start()
    {
        potionKindNum = 4;
        rndPotionNum = new int[potionKindNum];
        for(int i = 0; i < potionKindNum; i++)
        {
            rndPotionNum[i] = i;
        }
        RandomizePotion();
    }

    public GameObject CreatePotion(POTIONTYPE _potiontype, int _PosX, int _PosY)
    {
        var oObject = Instantiate(potionPrefab, new Vector2(_PosX, _PosY), Quaternion.identity);

        oObject.GetComponent<Potion>().potionData.position.PosX = _PosX;
        oObject.GetComponent<Potion>().potionData.position.PosY = _PosY;
        oObject.GetComponent<Potion>().potionData.potionType = _potiontype;

        for(int i = 0; i < rndPotionNum.Length; i++)
        {
            if(oObject.GetComponent<Potion>().potionData.potionType == (POTIONTYPE)i)
            {
                oObject.GetComponent<Potion>().potionData.potionColor = (POTIONCOLOR)rndPotionNum[i];
            }
        }
        return oObject;
    }

    void RandomizePotion()
    {
        int temp;
        for (int i = 0; i < rndPotionNum.Length * 2; i++)
        {
            int sour = UnityEngine.Random.Range(0, rndPotionNum.Length);
            int dest = UnityEngine.Random.Range(0, rndPotionNum.Length);

            //스왑
            temp = rndPotionNum[sour];
            rndPotionNum[sour] = rndPotionNum[dest];
            rndPotionNum[dest] = temp;
        }
    }
}
