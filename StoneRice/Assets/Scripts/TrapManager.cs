using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoSingleton<TrapManager>
{
    public List<GameObject> traps; //트랩 오브젝트들
    public List<Trap> trapInfoList; //트랩 스크립트들
    public TrapFactory trapFactory;

    public TileManager m_tileManager;
    public Tile[,] tileMapInfo;

    private void Awake()
    {
        this.gameObject.AddComponent<TrapFactory>();

        trapFactory = GetComponent<TrapFactory>();

        m_tileManager = TileManager.Instance;
        traps = new List<GameObject>();
        trapInfoList = new List<Trap>();
    }

    public void Init()
    {
        tileMapInfo = m_tileManager.tileMapInfoArray;
    }

    public void PlaceTraps(int _trapcount)
    {
        int trapLimit = 0; //트랩 설치 개수 판단 변수
        bool isSet = false; //포문 완료 판단용 

        while (true)
        {
            int trapX = Random.Range(0, m_tileManager.mapWidth);
            int trapY = Random.Range(0, m_tileManager.mapHeight);
            int trapNum = Random.Range(1, 3); //트랩 종류

            //금지영역이거나 계단이면 컨티뉴
            if (tileMapInfo[trapX, trapY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN) continue;
            else if (tileMapInfo[trapX, trapY].tileData.position.PosX == m_tileManager.stairDownPos.PosX &&
                     tileMapInfo[trapX, trapY].tileData.position.PosY == m_tileManager.stairDownPos.PosY) continue;
            else if (tileMapInfo[trapX, trapY].tileData.position.PosX == m_tileManager.stairUpPos.PosX &&
                     tileMapInfo[trapX, trapY].tileData.position.PosY == m_tileManager.stairUpPos.PosY) continue;
            else
            {
                if (traps.Count != 0) //오브젝트리스트에 후보가 있다면
                {
                    for (int i = 0; i < traps.Count; i++)
                    {                      
                        if (traps[i].activeSelf == false) //재사용 가능한 오브젝트라면
                        {
                            traps[i].SetActive(true);
                            traps[i].GetComponent<Trap>().trapData.position.PosX = trapX;
                            traps[i].GetComponent<Trap>().trapData.position.PosY = trapY;
                            traps[i].transform.position = new Vector2(trapX, trapY);
                            traps[i].GetComponent<Trap>().trapData.trapType = (TRAPTYPE)trapNum;
                            switch ((TRAPTYPE)trapNum)
                            {
                                case TRAPTYPE.DART:
                                    traps[i].GetComponent<Trap>().spriteRenderer.sprite = ResourceManager.Instance.spriteAtlas.GetSprite("trap_dart");
                                    break;
                                case TRAPTYPE.NET:
                                    traps[i].GetComponent<Trap>().spriteRenderer.sprite = ResourceManager.Instance.spriteAtlas.GetSprite("trap_net");
                                    break;
                                default:
                                    break;
                            }
                            isSet = true;
                            break;
                        }
                    }
                }
            }

            if (!isSet) //재사용 가능한 오브젝트가 없다면
            {
                traps.Add(trapFactory.CreateTrap((TRAPTYPE)trapNum, trapX, trapY));
            }

            trapLimit += 1;
            isSet = false;
            if (trapLimit >= _trapcount) break;
        }

        for(int i = 0; i < traps.Count; i++)
        {
            if(traps[i].activeSelf) //활성화된 트랩이라면
            {
                trapInfoList.Add(traps[i].GetComponent<Trap>());
            }
        }
    }

    public void DoTrap(int _PosX, int _PosY)
    {
        foreach (Trap T in trapInfoList)
        {
            if (T.trapData.position.PosX == _PosX && T.trapData.position.PosY == _PosY)
            {
                switch (T.trapData.trapType)
                {
                    case TRAPTYPE.DART:
                        T.spriteRenderer.enabled = true;
                        T.trapData.isActive = true;
                        Debug.Log("다트 함정 밟음");
                        break;
                    case TRAPTYPE.NET:
                        if (!T.trapData.isActive)
                        {
                            T.spriteRenderer.enabled = true;
                            T.trapData.isActive = true;
                            Debug.Log("그물 함정 밟음");
                        }
                        break;
                }
            }
        }
    }
}
