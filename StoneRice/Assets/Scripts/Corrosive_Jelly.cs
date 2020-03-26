using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corrosive_Jelly : Enemy
{
    public void Init()
    {
        enemyData.maxHp = 10;
        enemyData.curHp = 10;
        enemyData.maxMp = 10;
        enemyData.maxMp = 10;
        enemyData.atk = 3;
        enemyData.atkRange = 1.5f;
        enemyData.def = 2;
        enemyData.viewRange = 5;
    }

    public override void TurnProgress()
    {
        playerPos = PlayerManager.Instance.player.position; //플레이어 포지션 확인
        
        //상황판단 부분

        //시야에서 플레이어를 탐색
        CalcEnemyFov(TileManager.Instance.tileMapInfoArray);

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
                LogManager.Instance.SimpleLog("누군가가 공격했다");
                //그 뒤에 다시 트래킹으로 변환
                break;
            case ENEMYSTATE.RUNNINGAWAY:
                break;
        }
    }

    void corrosiveAttack()
    {

    }
}
