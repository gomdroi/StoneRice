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

        //자신의 HP상태가 얼마남지 않았으면 러닝어웨이로 전환(특정 몹 한정)

        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                //가만히 있던지 이리저리 돌아다님

                //플레이어와 거리가 인식범위 이내로 들어오면 트래킹으로 전환   
                CalcEnemyFov(TileManager.Instance.tileMapInfoArray);
                break;
            case ENEMYSTATE.TRACKING:
                //공격 사거리내에 플레이어가 있고 LOS가 나오면 어택으로 전환
                //플레이어를 대상으로 에이스타 사용 추적 이동
                TrackPlayer();
                
                break;
            case ENEMYSTATE.ATTACK:
                //공격행동을 수행함.
                break;
            case ENEMYSTATE.RUNNINGAWAY:
                break;
        }
    }

    void corrosiveAttack()
    {

    }
}
