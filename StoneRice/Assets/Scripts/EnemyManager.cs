using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{    
    GameObject enemy1Prefab;   

    private List<GameObject> EnemyList;

    private void Awake()
    {
        enemy1Prefab = Resources.Load("Prefabs/Enemy1") as GameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CallEnemy();
        }
    }

    void CallEnemy()
    {
        var oEnemy = Instantiate(enemy1Prefab, new Vector2(TileManager.instance.stairUpPos.PosX, TileManager.instance.stairUpPos.PosY), Quaternion.identity);
        oEnemy.GetComponent<Enemy>().EnemyInit();
    }
    //플레이어턴이 끝나면
    //에너미 리스트 돌면서 턴오버 체크
}
