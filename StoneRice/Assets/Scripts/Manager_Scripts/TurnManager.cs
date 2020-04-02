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

public class TurnManager : MonoSingleton<TurnManager>
{
    int globalTurn;
    int preGlobalTurn;
    public TURN_STATE turnState;
    public TURN_STATE preTrunState;
    public GameObject curUnit;

    PlayerManager m_PlayerManager;
    EnemyManager m_EnemyManager;
    TrapManager m_TrapManager;

    UIManager m_UIManager;

    private void Awake()
    {
        m_PlayerManager = PlayerManager.Instance;
        m_EnemyManager = EnemyManager.Instance;
        m_TrapManager = TrapManager.Instance;

        m_UIManager = UIManager.Instance;
    }

    private void Start()
    {
        turnState = TURN_STATE.NONE;
        globalTurn = 0;
        preGlobalTurn = 0;
    }

    private void Update()
    {
        //한바퀴 흐름
        if(preGlobalTurn != globalTurn)
        {           
            m_PlayerManager.player.XL_Check();
            m_PlayerManager.player.RestCheck();
            m_PlayerManager.player.DebuffCheck();
            m_UIManager.UI_Update();
            preGlobalTurn = globalTurn;

            if (m_PlayerManager.player.isDead)
            {
                LogManager.instance.SimpleLog("당신은 죽었습니다....");
            }
        }
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

                if (!m_PlayerManager.player.isDead)
                {
                    m_PlayerManager.player.TurnOverCheck();
                    m_PlayerManager.player.PlayerInput();
                }                             
                break;

            case TURN_STATE.ENEMY_TURN:

                if (preTrunState != TURN_STATE.ENEMY_TURN)
                {
                    preTrunState = TURN_STATE.ENEMY_TURN;
                }

                for(int i = 0; i < m_EnemyManager.enemyInfoList.Count; i++)
                {
                    m_EnemyManager.enemyInfoList[i].TurnProgress();
                }               

                globalTurn += 1;
                turnState = TURN_STATE.PLAYER_TURN;

                break;

            case TURN_STATE.INTERACTIVE:

                m_TrapManager.DoTrap(m_PlayerManager.player); //트랩 체크(나중에 상호작용 합쳐야함?)

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
