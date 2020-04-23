using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct Debuff
{
    public DEBUFFTYPE debuffType;
    public int duration;

    public Debuff(DEBUFFTYPE _debufftype, int _duration)
    {
        debuffType = _debufftype;
        duration = _duration;
    }
}

public class BattleManager : Singleton<BattleManager>
{
    LogManager m_logManager;

    public void Init()
    {
        m_logManager = LogManager.Instance;
    }

    public void NormalAttack(Player _player, Enemy _enemy)
    {
        int playerAtk = _player.playerData.strength - _enemy.enemyData.def; //무기 정보 추가 필요 + (계산식 함수 만들어야함)
        int enemyAtk = _enemy.enemyData.atk - _player.playerData.AC;

        switch (TurnManager.Instance.turnState)
        {
            case TURN_STATE.PLAYER_TURN:

                _enemy.enemyData.curHp = _enemy.enemyData.curHp - playerAtk;
                if (_enemy.enemyData.curHp <= 0)
                {
                    _enemy.enemyData.curHp = 0;
                    _enemy.enemyData.isDead = true;
                }
                   
                LogManager.Instance.SimpleLog(
                    _player.playerData.playerName + "이/가 " + _enemy.enemyData.EnemyName + "에게 " + playerAtk + "만큼의 데미지를 주었다"
                    );

                break;
            case TURN_STATE.ENEMY_TURN:

                _player.playerData.curHp = _player.playerData.curHp - enemyAtk;
                if (_player.playerData.curHp <= 0)
                {
                    _player.playerData.curHp = 0;
                    _player.isDead = true;
                }
              
                LogManager.Instance.SimpleLog(
                    _enemy.enemyData.EnemyName + "이/가 당신을 공격했다!"
                    );

                break;
        }            
    }

    public void ActivateTrap(TRAPTYPE _traptype, Player _player)
    {
        switch(_traptype)
        {
            case TRAPTYPE.DART:
                _player.playerData.curHp -= 2;
                LogManager.Instance.SimpleLog("당신은 다트를 맞았다.");
                break;
            case TRAPTYPE.NET:
                int netDuration = Random.Range(2, 4);
                Debuff net_entangle = new Debuff(DEBUFFTYPE.ENTANGLE, netDuration);
                _player.playerData.debuffs.Add(net_entangle);

                LogManager.Instance.SimpleLog("그물이 당신을 덮쳤다!");
                break;
        }
    }
}
