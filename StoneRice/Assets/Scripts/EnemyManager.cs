using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{
    //플레이어턴이 끝나면
    //에너미 리스트 돌면서 턴오버 체크
    //다 돌면 플레이어 코루틴 다시 돌려주기

    private List<GameObject> EnemyList;
  
}
