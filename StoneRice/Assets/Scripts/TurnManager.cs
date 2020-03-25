﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TURN_STATE
{
    NONE,
    PLAYER_TURN,
    INTERACTIVE,
    ENEMY_TURN,
}

//턴오더 = 플레이어턴 -> 환경 상호작용 -> 적턴 -> 환경 상호작용 -> 1턴 끝?

public class TurnManager : MonoSingleton<TurnManager>
{
    int globalTurn;
    public TURN_STATE turnState;
    public TURN_STATE preTrunState;
    public GameObject curUnit;

    PlayerManager m_PlayerManager;
    EnemyManager m_EnemyManager;
    TrapManager m_TrapManager;

    private void Awake()
    {
        m_PlayerManager = PlayerManager.Instance;
        m_EnemyManager = EnemyManager.Instance;
        m_TrapManager = TrapManager.Instance;
    }

    private void Start()
    {
        turnState = TURN_STATE.NONE;
        globalTurn = 0;
    }

    private void Update()
    {
        //환경 상호작용은 따로 뺄까?
        switch(turnState)
        {
            case TURN_STATE.NONE:
                break;
            case TURN_STATE.PLAYER_TURN:

                if(preTrunState != TURN_STATE.PLAYER_TURN)
                {
                    preTrunState = TURN_STATE.PLAYER_TURN;
                }                
                //플레이어 입력             
                //1.플레이어가 이동,공격 기타
                //2.기타 행동일 경우 기타 행동 처리(치료,마법,아이템 사용 등)
                //3.플레이어 위치의 오브젝트 또는 상호작용 오브젝트를 타일매니저의 오브젝트맵을 통해서 확인 상호작용함
                //4.공격일 경우 전투 처리
                m_PlayerManager.player.TurnOverCheck();
                m_PlayerManager.player.PlayerInput();
            
                break;

            case TURN_STATE.ENEMY_TURN:

                if (preTrunState != TURN_STATE.ENEMY_TURN)
                {
                    preTrunState = TURN_STATE.ENEMY_TURN;
                }

                foreach (Enemy enemy in m_EnemyManager.enemyInfoList)
                {
                    enemy.TurnProgress();
                }

                globalTurn += 1;
                turnState = TURN_STATE.PLAYER_TURN;

                break;

            case TURN_STATE.INTERACTIVE:

                m_TrapManager.DoTrap(m_PlayerManager.player.position.PosX, m_PlayerManager.player.position.PosY); //트랩 체크(나중에 상호작용 합쳐야함?)

                if(preTrunState == TURN_STATE.PLAYER_TURN)
                {
                    turnState = TURN_STATE.ENEMY_TURN;
                }
                else if(preTrunState == TURN_STATE.ENEMY_TURN)
                {
                    turnState = TURN_STATE.PLAYER_TURN;
                }

                break;
        }
    }

    void playerInput()
    {
        
    }
}
