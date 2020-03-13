using System.Collections;
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
    public GameObject curUnit;

    PlayerManager m_PlayerManager;
    EnemyManager m_EnemyManager;

    private void Awake()
    {
        m_PlayerManager = PlayerManager.Instance;
        m_EnemyManager = EnemyManager.Instance;
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
                //플레이어 입력             
                //1.플레이어가 이동,공격 기타
                //2.기타 행동일 경우 기타 행동 처리(치료,마법,아이템 사용 등)
                //3.플레이어 위치의 오브젝트 또는 상호작용 오브젝트를 타일매니저의 오브젝트맵을 통해서 확인 상호작용함
                //4.공격일 경우 전투 처리


                break;
            case TURN_STATE.ENEMY_TURN:
                //적을
                foreach(GameObject enemy in m_EnemyManager.enemyList)
                {
                    enemy.GetComponent<Enemy>().TurnProgress();
                }
                turnState = TURN_STATE.PLAYER_TURN;
                break;
        }
    }

    void playerInput()
    {
        
    }
}
