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
    public List<GameObject> JellyPool;
    public List<GameObject> RatPool;
    public List<GameObject> SlugPool;

    public List<Enemy> enemyInfoList;
    public List<List<EnemyData>> stageEnemys;

    public EnemyFactory enemyFactory;

    private void Awake()
    {
        this.gameObject.AddComponent<EnemyFactory>();

        enemyFactory = GetComponent<EnemyFactory>();
    }

    private void Start()
    {
        JellyPool = new List<GameObject>();
        RatPool = new List<GameObject>();
        SlugPool = new List<GameObject>();

        enemyInfoList = new List<Enemy>();
        stageEnemys = new List<List<EnemyData>>();
    }

    private void Update()
    {
    }

    void PoolOff()
    {
        for (int i = 0; i < JellyPool.Count; i++)
        {
            JellyPool[i].SetActive(false);
        }
        for (int i = 0; i < RatPool.Count; i++)
        {
            RatPool[i].SetActive(false);
        }
        for (int i = 0; i < SlugPool.Count; i++)
        {
            SlugPool[i].SetActive(false);
        }
    }

    public void CallRandomEnemy(int _stageNum)
    {
        PoolOff();
        enemyInfoList.Clear();
     
        int enemyCount = Random.Range(4, 10);
        Debug.Log("현재 층 생성된 몬스터 수 : " + enemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            
            Position spawnPos = enemyFactory.FindValidateTile();
            int randomNum = 0;
            bool isSet = false;

            if (_stageNum < 4)
            {
                randomNum = Random.Range(0, 2);
            }
            else if(_stageNum >= 4)
            {
                randomNum = Random.Range(0, 3);
            }

            switch((ENEMYTYPE)randomNum)
            {
                case ENEMYTYPE.JELLY:
                    for(int j = 0; j < JellyPool.Count; j++)
                    {
                        if (JellyPool[j].activeSelf) continue;
                        JellyPool[j].SetActive(true);
                        JellyPool[j].transform.position = new Vector2(spawnPos.PosX, spawnPos.PosY);
                        JellyPool[j].GetComponent<Corrosive_Jelly>().EnemyInit();
                        JellyPool[j].GetComponent<Corrosive_Jelly>().Init();
                        isSet = true;
                        break;
                    }
                    if(isSet == false)
                    {
                        JellyPool.Add(enemyFactory.CallEnemy(ENEMYTYPE.JELLY));
                    }
                    break;
                case ENEMYTYPE.RAT:
                    for(int j = 0; j < RatPool.Count; j++)
                    {
                        if (RatPool[j].activeSelf) continue;
                        RatPool[j].SetActive(true);
                        RatPool[j].transform.position = new Vector2(spawnPos.PosX, spawnPos.PosY);
                        RatPool[j].GetComponent<Rat>().EnemyInit();
                        RatPool[j].GetComponent<Rat>().Init();
                        isSet = true;
                        break;
                    }
                    if(isSet == false)
                    {
                        RatPool.Add(enemyFactory.CallEnemy(ENEMYTYPE.RAT));
                    }
                    break;
                case ENEMYTYPE.ELEPHANTSLUG:
                    for(int j = 0; j < SlugPool.Count; j++)
                    {
                        if (SlugPool[j].activeSelf) continue;
                        SlugPool[j].SetActive(true);
                        SlugPool[j].transform.position = new Vector2(spawnPos.PosX, spawnPos.PosY);
                        SlugPool[j].GetComponent<Elephant_Slug>().EnemyInit();
                        SlugPool[j].GetComponent<Elephant_Slug>().Init();
                        isSet = true;
                        break;
                    }
                    if(isSet == false)
                    {
                        SlugPool.Add(enemyFactory.CallEnemy(ENEMYTYPE.ELEPHANTSLUG));
                    }
                    break;
            }          
        }

        for(int i = 0; i < JellyPool.Count; i++)
        {
            if(JellyPool[i].activeSelf)
            {
                enemyInfoList.Add(JellyPool[i].GetComponent<Corrosive_Jelly>());
            }
        }

        for(int i = 0; i < RatPool.Count; i++)
        {
            if(RatPool[i].activeSelf)
            {
                enemyInfoList.Add(RatPool[i].GetComponent<Rat>());
            }
        }

        for(int i = 0; i < SlugPool.Count; i++)
        {
            if(SlugPool[i].activeSelf)
            {
                enemyInfoList.Add(SlugPool[i].GetComponent<Elephant_Slug>());
            }
        }

        List<EnemyData> curStageEnemy = new List<EnemyData>();
        for(int i = 0; i < enemyInfoList.Count; i++)
        {
            curStageEnemy.Add(enemyInfoList[i].enemyData);
        }
        stageEnemys.Add(curStageEnemy);
    }

    public void SaveEnemys(int _stageNum)
    {        
        stageEnemys[_stageNum].Clear();

        //적 데이터 카피
        for (int i = 0; i < enemyInfoList.Count; i++)
        {
            EnemyData enemy = new EnemyData();
            enemy = enemyInfoList[i].enemyData;
            stageEnemys[_stageNum].Add(enemy);
        }    
    }

    public void LoadEnemys(int _stageNum)
    {
        PoolOff();
        enemyInfoList.Clear();

        for(int i = 0; i < stageEnemys[_stageNum].Count; i++)
        {
            switch(stageEnemys[_stageNum][i].enemyType)
            {
                case ENEMYTYPE.JELLY:
                    for(int j = 0; j < JellyPool.Count; j++)
                    {
                        if (JellyPool[j].activeSelf) continue;
                        JellyPool[j].SetActive(true);
                        JellyPool[j].GetComponent<Corrosive_Jelly>().enemyData = stageEnemys[_stageNum][i];
                        JellyPool[j].transform.position = 
                            new Vector2(JellyPool[j].GetComponent<Corrosive_Jelly>().enemyData.position.PosX, JellyPool[j].GetComponent<Corrosive_Jelly>().enemyData.position.PosY);
                        enemyInfoList.Add(JellyPool[j].GetComponent<Corrosive_Jelly>());
                        break;
                    }
                    break;
                case ENEMYTYPE.RAT:
                    for(int j = 0; j < RatPool.Count; j++)
                    {
                        if (RatPool[j].activeSelf) continue;
                        RatPool[j].SetActive(true);
                        RatPool[j].GetComponent<Rat>().enemyData = stageEnemys[_stageNum][i];
                        RatPool[j].transform.position =
                            new Vector2(RatPool[j].GetComponent<Rat>().enemyData.position.PosX, RatPool[j].GetComponent<Rat>().enemyData.position.PosY);
                        enemyInfoList.Add(RatPool[j].GetComponent<Rat>());
                        break;
                    }
                    break;
                case ENEMYTYPE.ELEPHANTSLUG:
                    for(int j =0; j < SlugPool.Count; j++)
                    {
                        if (SlugPool[j].activeSelf) continue;
                        SlugPool[j].SetActive(true);
                        SlugPool[j].GetComponent<Elephant_Slug>().enemyData = stageEnemys[_stageNum][i];
                        SlugPool[j].transform.position =
                            new Vector2(SlugPool[j].GetComponent<Elephant_Slug>().enemyData.position.PosX, SlugPool[j].GetComponent<Elephant_Slug>().enemyData.position.PosY);
                        enemyInfoList.Add(SlugPool[j].GetComponent<Elephant_Slug>());
                        break;
                    }
                    break;
            }
        }
    }

    public void Delete_EnemyInfo()
    {
        for (int i = 0; i < enemyInfoList.Count; i++)
        {
            if(enemyInfoList[i].enemyData.curHp == 0)
            {
                enemyInfoList.Remove(enemyInfoList[i]);      
            }         
        }
    }
}
