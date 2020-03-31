using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoSingleton<EnemyManager>
{    
    GameObject enemyPrefab;   

    //public List<GameObject> enemyList;
    public List<Enemy> enemyInfoList;
    public List<List<Enemy>> stageEnemy;

    private void Awake()
    {
        enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
    }

    private void Start()
    {
        //enemyList = new List<GameObject>();
        enemyInfoList = new List<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Call_Jelly_Enemy();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Call_Rat_Enemy();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Call_Snail_Enemy();
        }
    }

    void Call_Jelly_Enemy()
    {
        Position spawnPos = FindValidateTile();
        var oEnemy = Instantiate(enemyPrefab, new Vector2(spawnPos.PosX, spawnPos.PosY), Quaternion.identity);
        oEnemy.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("jelly");
        oEnemy.AddComponent<Corrosive_Jelly>();
        oEnemy.GetComponent<Corrosive_Jelly>().EnemyInit();
        oEnemy.GetComponent<Corrosive_Jelly>().Init();
        oEnemy.GetComponent<Corrosive_Jelly>().onDeath += Delete_EnemyInfo;
        //enemyList.Add(oEnemy);
        enemyInfoList.Add(oEnemy.GetComponent<Corrosive_Jelly>());
    }

    void Call_Snail_Enemy()
    {
        Position spawnPos = FindValidateTile();
        var oEnemy = Instantiate(enemyPrefab, new Vector2(spawnPos.PosX, spawnPos.PosY), Quaternion.identity);
        oEnemy.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("elephant_slug");
        oEnemy.AddComponent<Elephant_Slug>();
        oEnemy.GetComponent<Elephant_Slug>().EnemyInit();
        oEnemy.GetComponent<Elephant_Slug>().Init();
        oEnemy.GetComponent<Elephant_Slug>().onDeath += Delete_EnemyInfo;
        //enemyList.Add(oEnemy);
        enemyInfoList.Add(oEnemy.GetComponent<Elephant_Slug>());
    }

    void Call_Rat_Enemy()
    {
        Position spawnPos = FindValidateTile();
        var oEnemy = Instantiate(enemyPrefab, new Vector2(spawnPos.PosX, spawnPos.PosY), Quaternion.identity);
        oEnemy.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("rat");
        oEnemy.AddComponent<Rat>();
        oEnemy.GetComponent<Rat>().EnemyInit();
        oEnemy.GetComponent<Rat>().Init();
        oEnemy.GetComponent<Rat>().onDeath += Delete_EnemyInfo;
        //enemyList.Add(oEnemy);
        enemyInfoList.Add(oEnemy.GetComponent<Rat>());
    }

    void Delete_EnemyInfo()
    {       
        for(int i = 0; i < enemyInfoList.Count; i++)
        {
            if(enemyInfoList[i].enemyData.curHp == 0)
            {
                enemyInfoList.Remove(enemyInfoList[i]);
            }         
        }
    }

    Position FindValidateTile()
    {
        TileManager m_tileManager = TileManager.instance;

        Position validatePosition = new Position();

        while (true)
        {
            int posX = Random.Range(0, m_tileManager.mapWidth);
            int posY = Random.Range(0, m_tileManager.mapHeight);

            
            if (m_tileManager.tileMapInfoArray[posX, posY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN ||
                m_tileManager.tileMapInfoArray[posX, posY].tileData.tileRestriction == TILE_RESTRICTION.OCCUPIED) continue;
            else
            {
                validatePosition.PosX = posX;
                validatePosition.PosY = posY;
                break;
            }                    
        }

        return validatePosition;
    }


}
