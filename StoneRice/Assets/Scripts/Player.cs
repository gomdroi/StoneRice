using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct PlayerData
{
    public string playerName;
    public Position position;
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public int strength;
    public int intelligence;
    public int dexterity;

    public float atkRange;
    public int AC;
    public int EV;
    public int XL;
    public int nextEXP;
    public int curEXP;

    public int viewRange;

    public int Gold;

    public List<Debuff> debuffs;
}

public class Player : MonoBehaviour
{
    public PlayerData playerData; //데이터
    public Vector3 prePos; //이전 포지션
    public bool isTurnDone; //턴 종료 판단
    public int NextRest; //휴식 카운트    
    public bool isDead = false;
    public Tile[,] tileMapInfo; //실제 타일 배열
    public PLAYERSTATE playerState;

    int tPosX;
    int tPosY;
    int targetPosX;
    int targetPosY;
    float inputDelay;
    bool isMove;

    public int mapHeight;
    public int mapWidth;
    TurnManager m_TurnManager;
    TileManager m_TileManager;
    EnemyManager m_EnemyManager;
    FOV fov;  

    private void Awake()
    {
        m_TurnManager = TurnManager.Instance;
        m_TileManager = TileManager.Instance;
        m_EnemyManager = EnemyManager.Instance;
        fov = new FOV();
    }

    private void Start() //게임오브젝트 생성시에 
    {
        m_TurnManager.turnState = TURN_STATE.PLAYER_TURN; //플레이어에게 턴을 주고
       prePos = transform.position; //이전 포지션에 생성포지션을 넣고
        playerData.playerName = "김철수"; //이름 주고 (임시)
        SetStats(); //기본 스탯값 입력
        playerData.debuffs = new List<Debuff>();
        inputDelay = 0;
        isMove = true;
        NextRest = 0;
        isTurnDone = false;       
        fov.CalcFov(tileMapInfo, playerData.position.PosX, playerData.position.PosY, playerData.viewRange);
        UIManager.Instance.UI_Update();
    }

    void SetStats()
    {
        playerData.maxHp = 20;
        playerData.curHp = 20;
        playerData.maxMp = 10;
        playerData.curMp = 10;
        playerData.strength = 5;
        playerData.intelligence = 5;
        playerData.dexterity = 5;

        playerData.atkRange = 1;
        playerData.AC = 0;
        playerData.EV = 0;
        playerData.XL = 1;
        playerData.nextEXP = 50;
        playerData.curEXP = 0;

        playerData.viewRange = 9;

        playerData.Gold = 0;
    }

    //층을 옮길때마다 층의 정보를 받아와야함
    public void PlayerInit()
    {
        tileMapInfo = TileManager.Instance.tileMapInfoArray;
        playerData.position.PosX = (int)this.gameObject.transform.position.x;
        playerData.position.PosY = (int)this.gameObject.transform.position.y;
        mapHeight = TileManager.Instance.mapHeight;
        mapWidth = TileManager.Instance.mapWidth;


        fov.Init();       
    }

    public void TurnOverCheck()
    {      
        if (isTurnDone)
        {
            prePos = transform.position; //이전 포지션 저장
            playerData.position.PosX = (int)transform.position.x; //트랜스폼 현재 포지션으로 정보 변경
            playerData.position.PosY = (int)transform.position.y;
            fov.CalcFov(tileMapInfo, playerData.position.PosX, playerData.position.PosY, playerData.viewRange); //시야 변경           
            isTurnDone = false; //턴 상태 돌려줌
            m_TurnManager.turnState = TURN_STATE.INTERACTIVE; //상호작용 턴으로 변경
        }       
    }

    public void PlayerInput_T()
    {
        if (isMove)
        {
            tPosX = (int)transform.position.x;
            tPosY = (int)transform.position.y;
            targetPosX = 0;
            targetPosY = 0;
            playerState = PLAYERSTATE.INPUT;
            isMove = false;
        }

        switch (playerState)
        {
            case PLAYERSTATE.INPUT:
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Keypad8))
                {
                    targetPosX = tPosX;
                    targetPosY = tPosY + 1;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Keypad2))
                {
                    targetPosX = tPosX;
                    targetPosY = tPosY - 1;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4))
                {
                    targetPosX = tPosX - 1;
                    targetPosY = tPosY;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6))
                {
                    targetPosX = tPosX + 1;
                    targetPosY = tPosY;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKey(KeyCode.Keypad7))
                {
                    targetPosX = tPosX - 1;
                    targetPosY = tPosY + 1;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKey(KeyCode.Keypad9))
                {
                    targetPosX = tPosX + 1;
                    targetPosY = tPosY + 1;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKey(KeyCode.Keypad1))
                {
                    targetPosX = tPosX - 1;
                    targetPosY = tPosY - 1;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKey(KeyCode.Keypad3))
                {
                    targetPosX = tPosX + 1;
                    targetPosY = tPosY - 1;
                    playerState = PLAYERSTATE.OUTPUT;
                }
                else if (Input.GetKeyDown(KeyCode.Comma))
                {
                    if (tPosX == m_TileManager.stairDownPos.PosX && tPosY == m_TileManager.stairDownPos.PosY)
                    {
                        GameManager.Instance.GoDownStage();
                        transform.position = new Vector2(m_TileManager.stairUpPos.PosX, m_TileManager.stairUpPos.PosY);
                    }
                    else if (tPosX == m_TileManager.stairUpPos.PosX && tPosY == m_TileManager.stairUpPos.PosY)
                    {
                        int preStage = GameManager.Instance.curStage;
                        GameManager.Instance.GoUpStage();
                        if (preStage != 0)
                        {
                            transform.position = new Vector2(m_TileManager.stairDownPos.PosX, m_TileManager.stairDownPos.PosY);
                        }
                    }
                    isTurnDone = true;
                    playerState = PLAYERSTATE.DELAY;
                }
                else if (Input.GetKey(KeyCode.Space)) //휴식
                {
                    NextRest += 2;

                    isTurnDone = true;
                    playerState = PLAYERSTATE.DELAY;
                }
                break;               
            case PLAYERSTATE.OUTPUT:
                //방향에 적이 있으면
                for (int i = 0; i < m_EnemyManager.enemyInfoList.Count; i++)
                {
                    if (m_EnemyManager.enemyInfoList[i].enemyData.position.PosX == targetPosX && m_EnemyManager.enemyInfoList[i].enemyData.position.PosY == targetPosY)
                    {
                        BattleManager.Instance.NormalAttack(this, m_EnemyManager.enemyInfoList[i]);
                        isTurnDone = true;
                        break;
                    }
                }
                if (isTurnDone)
                {
                    playerState = PLAYERSTATE.DELAY;
                    return;
                }

                //인탱글 체크       
                for (int i = 0; i < playerData.debuffs.Count; i++)
                {
                    if (playerData.debuffs[i].debuffType == DEBUFFTYPE.ENTANGLE)
                    {
                        LogManager.Instance.SimpleLog("묶여서 움직일 수 없다");
                        isTurnDone = true;
                    }
                }
                if (isTurnDone)
                {
                    playerState = PLAYERSTATE.DELAY;
                    return;
                }


                //없으면 이동체크
                if (targetPosX < 0 || targetPosX >= mapWidth)
                {
                    LogManager.Instance.SimpleLog("그곳으로 갈 수 없다");
                    return;
                }
                else if (targetPosY < 0 || targetPosY >= mapHeight)
                {
                    LogManager.Instance.SimpleLog("그곳으로 갈 수 없다");
                    return;
                }

                if (tileMapInfo[targetPosX, targetPosY].tileData.tileRestriction == TILE_RESTRICTION.MOVEABLE)
                {
                    transform.position = tileMapInfo[targetPosX, targetPosY].transform.position;
                    isTurnDone = true;
                }
                else if (tileMapInfo[targetPosX, targetPosY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN)
                {
                    LogManager.Instance.SimpleLog("그곳으로 갈 수 없다");
                    isTurnDone = true;
                }

                NextRest += 1;
                playerState = PLAYERSTATE.DELAY;
                break;
            case PLAYERSTATE.DELAY:
                inputDelay += Time.deltaTime;
                if (inputDelay >= 0.1f)
                {
                    inputDelay = 0;
                    isMove = true;
                }
                break;
        }
    }  

    public void XL_Check()
    {
        if(playerData.nextEXP <= 0)
        {
            playerData.nextEXP = 45;
            playerData.nextEXP = playerData.nextEXP * (playerData.XL * 2);
            LevelUp();          
        }
    }

    public void LevelUp()
    {
        playerData.XL += 1;
        playerData.maxHp = playerData.maxHp + playerData.strength;
        playerData.curHp = playerData.curHp + playerData.strength;
    }

    public void DebuffCheck()
    {
        if (playerData.debuffs == null) return;       
        for(int i = 0; i < playerData.debuffs.Count; i++)
        {          
            Debuff temp = playerData.debuffs[i];
            temp.duration = playerData.debuffs[i].duration - 1;
            playerData.debuffs[i] = temp;

            if(playerData.debuffs[i].duration <= 0)
            {
                playerData.debuffs.Remove(playerData.debuffs[i]);
            }
        }
    }

    public void RestCheck()
    {
        if (NextRest >= 4)
        {
            playerData.curHp += 1;
            if (playerData.curHp >= playerData.maxHp)
            {
                playerData.curHp = playerData.maxHp;
            }
            NextRest = 0;
        }
    }
}
