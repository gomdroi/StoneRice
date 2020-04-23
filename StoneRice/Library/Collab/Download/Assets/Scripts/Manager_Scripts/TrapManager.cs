using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoSingleton<TrapManager>
{
    public List<GameObject> traps; //트랩 오브젝트들
    public List<Trap> trapInfoList; //트랩 스크립트들
    public TrapFactory trapFactory;
    public List<List<TrapData>> stageTraps;

    public TileManager m_tileManager;
    public Tile[,] tileMapInfo;

    private void Awake()
    {
        this.gameObject.AddComponent<TrapFactory>();

        trapFactory = GetComponent<TrapFactory>();

        m_tileManager = TileManager.Instance;
        traps = new List<GameObject>();
        trapInfoList = new List<Trap>();
        stageTraps = new List<List<TrapData>>();
    }

    public void Init()
    {
        tileMapInfo = m_tileManager.tileMapInfoArray;
    }

    public void PlaceTraps(int _trapCount)
    {
        int trapLimit = 0; //트랩 설치 개수 판단 변수
        bool isSet = false; //포문 완료 판단용 

        trapInfoList.Clear(); //새로 깔기전에 이전 트랩 정보를 지움
        for(int i = 0; i < traps.Count; i++)
        {
            traps[i].SetActive(false);
            traps[i].GetComponent<Trap>().spriteRenderer.enabled = false;
        }

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
            if (trapLimit >= _trapCount) break;
        }

        for(int i = 0; i < traps.Count; i++)
        {
            if(traps[i].activeSelf) //활성화된 트랩이라면
            {
                trapInfoList.Add(traps[i].GetComponent<Trap>());
            }
        }
    }

    public void SaveTraps(int _stageNum, bool _isNewStage = false)
    {
        if(_isNewStage)
        {
            List<TrapData> curStageTraps = new List<TrapData>();

            //트랩 데이터 카피
            for (int i = 0; i < trapInfoList.Count; i++)
            {
                TrapData trap = new TrapData();
                trap = trapInfoList[i].trapData;
                curStageTraps.Add(trap);
            }

            stageTraps.Add(curStageTraps);
        }
        else if(!_isNewStage)
        {
            for (int i = 0; i < trapInfoList.Count; i++)
            {
                stageTraps[_stageNum][i] = (TrapData)trapInfoList[i].trapData;
            }
        }
    }

    public void LoadTraps(int _stageNum)
    {
        trapInfoList.Clear();

        for (int i = 0; i < traps.Count; i++)
        {
            traps[i].SetActive(false);
            traps[i].GetComponent<Trap>().spriteRenderer.enabled = false;
        }

        for (int i = 0; i < stageTraps[_stageNum].Count; i++)
        {
            traps[i].SetActive(true);
            traps[i].GetComponent<Trap>().trapData = (TrapData)stageTraps[_stageNum][i];
            traps[i].transform.position = new Vector2(traps[i].GetComponent<Trap>().trapData.position.PosX, traps[i].GetComponent<Trap>().trapData.position.PosY);
            if(traps[i].GetComponent<Trap>().trapData.isActive) traps[i].GetComponent<Trap>().spriteRenderer.enabled = true;

            switch (traps[i].GetComponent<Trap>().trapData.trapType)
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
        }

        for (int i = 0; i < traps.Count; i++)
        {
            if (traps[i].activeSelf) //활성화된 트랩이라면
            {
                trapInfoList.Add(traps[i].GetComponent<Trap>());
            }
        }
    }

    public void DoTrap(Player _player)
    {
        for(int i = 0; i < trapInfoList.Count; i++)
        {
            if(trapInfoList[i].trapData.position.PosX == _player.playerData.position.PosX && 
                trapInfoList[i].trapData.position.PosY == _player.playerData.position.PosY)
            {
                switch(trapInfoList[i].trapData.trapType)
                {
                    case TRAPTYPE.DART:
                        trapInfoList[i].spriteRenderer.enabled = true;
                        trapInfoList[i].trapData.isActive = true;
                        BattleManager.Instance.ActivateTrap(TRAPTYPE.DART,_player);
                        break;
                    case TRAPTYPE.NET:
                        if (!trapInfoList[i].trapData.isActive)
                        {
                            trapInfoList[i].spriteRenderer.enabled = true;
                            trapInfoList[i].trapData.isActive = true;
                            BattleManager.Instance.ActivateTrap(TRAPTYPE.NET,_player);
                        }
                        break;
                }
            }
        }        
    }
}
