using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyFactory : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyCargo;

    private void Awake()
    {
        enemyPrefab = Resources.Load("Prefabs/Enemy") as GameObject;
        enemyCargo = GameObject.Find("EnemyCargo");       
    }   

    public GameObject CallEnemy(ENEMYTYPE _enemyType)
    {
        Position spawnPos = FindValidateTile();
        var oEnemy = Instantiate(enemyPrefab, new Vector2(spawnPos.PosX, spawnPos.PosY), Quaternion.identity);
        oEnemy.transform.SetParent(enemyCargo.transform);
        switch (_enemyType)
        {
            case ENEMYTYPE.JELLY:
                oEnemy.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("jelly");
                oEnemy.AddComponent<Corrosive_Jelly>();
                oEnemy.GetComponent<Corrosive_Jelly>().EnemyInit();
                oEnemy.GetComponent<Corrosive_Jelly>().Init();
                oEnemy.GetComponent<Corrosive_Jelly>().onDeath += EnemyManager.Instance.Delete_EnemyInfo;
                break;
            case ENEMYTYPE.RAT:
                oEnemy.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("rat");
                oEnemy.AddComponent<Rat>();
                oEnemy.GetComponent<Rat>().EnemyInit();
                oEnemy.GetComponent<Rat>().Init();
                oEnemy.GetComponent<Rat>().onDeath += EnemyManager.Instance.Delete_EnemyInfo;
                break;
            case ENEMYTYPE.ELEPHANTSLUG:
                oEnemy.GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.spriteAtlas.GetSprite("elephant_slug");
                oEnemy.AddComponent<Elephant_Slug>();
                oEnemy.GetComponent<Elephant_Slug>().EnemyInit();
                oEnemy.GetComponent<Elephant_Slug>().Init();
                oEnemy.GetComponent<Elephant_Slug>().onDeath += EnemyManager.Instance.Delete_EnemyInfo;
                break;
        }

        return oEnemy;
    }    

    public Position FindValidateTile()
    {
        TileManager m_tileManager = TileManager.Instance;

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
