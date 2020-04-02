using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFactory : MonoBehaviour
{
    public GameObject trapPrefab;
    public GameObject trapCargo;

    private void Awake()
    {      
        trapPrefab = Resources.Load("Prefabs/Trap") as GameObject;
        trapCargo = GameObject.Find("TrapPool");
    }

    public GameObject CreateTrap(TRAPTYPE _traptype, int _PosX, int _PosY)
    {
        var oTrap = Instantiate(trapPrefab, new Vector2(_PosX, _PosY), Quaternion.identity);
        oTrap.transform.SetParent(trapCargo.transform);

        oTrap.GetComponent<Trap>().trapData.position.PosX = _PosX;
        oTrap.GetComponent<Trap>().trapData.position.PosY = _PosY;
        oTrap.GetComponent<Trap>().trapData.trapType = _traptype;

        switch (_traptype)
        {
            case TRAPTYPE.DART:
                oTrap.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("trap_dart");
                break;
            case TRAPTYPE.NET:
                oTrap.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("trap_net");
                break;
            default:
                break;
        }
        
        return oTrap;
    }
}
