using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    public void Init()
    {
        enemyData.EnemyName = "쥐";
        enemyData.enemyType = ENEMYTYPE.RAT;
        enemyData.maxHp = 8;
        enemyData.curHp = 8;
        enemyData.maxMp = 10;
        enemyData.maxMp = 10;
        enemyData.atk = 2;
        enemyData.atkRange = 1.5f;
        enemyData.def = 0;
        enemyData.viewRange = 11;
        enemyData.expValue = 10;
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
        //if(enemyData.curHp / (float)enemyData.maxHp <= 0.5)
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
