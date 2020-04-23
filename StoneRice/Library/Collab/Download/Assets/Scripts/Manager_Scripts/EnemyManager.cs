using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct EnemyInfo
//{
//    GameObject enemyGameObject;
//    Enemy enemyScript;
//}

public class EnemyManager : MonoSingleton<EnemyManager>
{   
    GameObject enemyPrefab;

    public List<List<GameObject>> stageEnemys;
    public List<Enemy> enemyInfoList;
    public List<List<EnemyData>> stageEnemyInfo;

    public EnemyFactory enemyFactory;

    private void Awake()
    {
        enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
        this.gameObject.AddComponent<EnemyFactory>();

        enemyFactory = GetComponent<EnemyFactory>();
    }

    private void Start()
    {
        enemyInfoList = new List<Enemy>();
        stageEnemys = new List<List<GameObject>>();
    }

    private void Update()
    {
    }

    public void CallRandomEnemy(int _stageNum)
    {
        enemyInfoList.Clear();

        List<GameObject> curStageEnemy = new List<GameObject>();
        int enemyCount = Random.Range(4, 10);

        for(int i = 0; i <= enemyCount; i++)
        {
            int randomNum = Random.Range(0, 2);

            curStageEnemy.Add(enemyFactory.CallEnemy((ENEMYTYPE)randomNum));
        }

        if(_stageNum >= 4)
        {
            int randomNum = Random.Range(0, 2);

            if(randomNum == 1)
            {
                curStageEnemy.Add(enemyFactory.CallEnemy(ENEMYTYPE.ELEPHANTSLUG));
            }
        }

        stageEnemys.Add(curStageEnemy); //층별로 적 게임오브젝트 저장

        //적 목록을 뽑아 저장
        for(int i = 0; i < curStageEnemy.Count; i++)
        {
            enemyInfoList.Add(curStageEnemy[i].GetComponent<Enemy>());
        }
    }

    public void reArrangeEnemy(int _stageNum, bool _isOldNext = false, bool _isBackWard = false)
    {
        enemyInfoList.Clear();

        for (int i = 0; i < stageEnemys[_stageNum].Count; i++)
        {
            stageEnemys[_stageNum][i].SetActive(false);
        }
        
        if(_isOldNext)
        {
            for (int i = 0; i < stageEnemys[_stageNum + 1].Count; i++)
            {
                stageEnemys[_stageNum + 1][i].SetActive(true);               
                enemyInfoList.Add(stageEnemys[_stageNum + 1][i].GetComponent<Enemy>());
            }                
        }
        else if(_isBackWard)
        {
            for (int i = 0; i < stageEnemys[_stageNum - 1].Count; i++)
            {
                stageEnemys[_stageNum - 1][i].SetActive(true);
                enemyInfoList.Add(stageEnemys[_stageNum - 1][i].GetComponent<Enemy>());
            }              
        }       
    }

    public void SaveEnemys(int _stageNum, bool _isNewStage = false)
    {
        if (_isNewStage)
        {
            List<EnemyData> curStageEnemys = new List<EnemyData>();

            //적 데이터 카피
            for (int i = 0; i < enemyInfoList.Count; i++)
            {
                EnemyData enemy = new EnemyData();
                enemy = enemyInfoList[i].enemyData;
                curStageEnemys.Add(enemy);
            }

            stageEnemyInfo.Add(curStageEnemys);
        }
        else if (!_isNewStage)
        {
            stageEnemyInfo[_stageNum].Clear();

            List<EnemyData> curStageEnemys = new List<EnemyData>();

            //적 데이터 카피
            for (int i = 0; i < enemyInfoList.Count; i++)
            {
                EnemyData enemy = new EnemyData();
                enemy = enemyInfoList[i].enemyData;
                stageEnemyInfo[_stageNum].Add(enemy);
            }
        }
    }

    public void Delete_EnemyInfo()
    {
        int curStage = GameManager.Instance.curStage;
        var EL = stageEnemys[curStage];
        

        for (int i = 0; i < enemyInfoList.Count; i++)
        {
            if(enemyInfoList[i].enemyData.curHp == 0)
            {
                GameObject targetGO = enemyInfoList[i].gameObject;
                
                stageEnemys[curStage].Remove(enemyInfoList[i].gameObject);
                enemyInfoList.Remove(enemyInfoList[i]);      
            }         
        }
    }
}
