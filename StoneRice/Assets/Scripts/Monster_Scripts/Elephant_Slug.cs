using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant_Slug : Enemy
{
    public void Init()
    {
        enemyData.EnemyName = "거대한 민달팽이";
        enemyData.maxHp = 50;
        enemyData.curHp = 50;
        enemyData.maxMp = 10;
        enemyData.maxMp = 10;
        enemyData.atk = 14;
        enemyData.atkRange = 1.5f;
        enemyData.def = 0;
        enemyData.viewRange = 10;
        enemyData.expValue = 100;
    }

    public override void TurnProgress()
    {
        if (isDead)
        {
            DestroySequence();
            return;
        }

        playerPos = PlayerManager.Instance.player.playerData.position; //플레이어 포지션 확인

        //상황판단 부분
        int rndActionNum = Random.Range(0, 10);
               
        if(rndActionNum <= 5) //0,1,2,3,4,5
        {
            if (enemyData.atkRange >= diagonalDistance(enemyData.position.PosX, enemyData.position.PosY, playerPos.PosX, playerPos.PosY))
            {
                enemyState = ENEMYSTATE.ATTACK;
            }
            else
            {
                enemyState = ENEMYSTATE.TRACKING;
            }      
        }
        else if(rndActionNum > 5 && rndActionNum <= 8) //6,7,8
        {
            enemyState = ENEMYSTATE.IDLE;
        }
        else //9
        {
            enemyState = ENEMYSTATE.IDLE;
            LogManager.Instance.SimpleLog("당신은 멀리서 분노한 민달팽이가 울부짖는 소리를 들었다!"); 
        }
     
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                //가만히 있던지 이리저리 돌아다님                            
                break;
            case ENEMYSTATE.TRACKING:
                //플레이어를 대상으로 에이스타 사용 추적 이동               
                TrackPlayer();
                break;
            case ENEMYSTATE.ATTACK:
                //공격행동을 수행함.
                BattleManager.Instance.NormalAttack(PlayerManager.Instance.player, this);
                //그 뒤에 다시 트래킹으로 변환
                break;
            case ENEMYSTATE.RUNNINGAWAY:
                break;
        }
        HideEnemy();
        HpBar_Update();
    }
}
