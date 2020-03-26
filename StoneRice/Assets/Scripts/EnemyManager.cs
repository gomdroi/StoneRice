using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{    
    GameObject enemyPrefab;   

    public List<GameObject> enemyList;
    public List<Enemy> enemyInfoList;

    private void Awake()
    {
        enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    }

    private void Start()
    {
        enemyList = new List<GameObject>();
        enemyInfoList = new List<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Call_Jelly_Enemy();
        }
    }

    void Call_Jelly_Enemy()
    {
        var oEnemy = Instantiate(enemyPrefab, new Vector2(TileManager.instance.stairUpPos.PosX, TileManager.instance.stairUpPos.PosY), Quaternion.identity);
        oEnemy.GetComponent<Corrosive_Jelly>().EnemyInit();
        oEnemy.GetComponent<Corrosive_Jelly>().Init();
        enemyList.Add(oEnemy);
        enemyInfoList.Add(oEnemy.GetComponent<Corrosive_Jelly>());
    }

    //몹 스폰해서 리스트에 넣어두기만하면 됨.

}
