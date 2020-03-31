using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ENEMYTYPE
{
    JELLY,
    KOBOLD
}

public enum ENEMYSTATE
{
    IDLE,
    TRACKING,
    ATTACK,
    RUNNINGAWAY
}

public struct EnemyData
{
    public Position position;
    public ENEMYTYPE enemyType;
    public string EnemyName;
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public int atk;
    public float atkRange;
    public int def;
    public int viewRange;
    public int expValue;
}

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData; 

    protected List<TileData> astarPath;
    public Position playerPos;
    public ENEMYSTATE enemyState;

    public SpriteRenderer spriteRenderer;
    public Image Hp_Bar_Front;

    public Action onDeath;

    public bool isDead = false;

    int mapWidth;
    int mapHeight;  

    public void EnemyInit()
    {
        enemyData.position.PosX = (int)this.gameObject.transform.position.x;
        enemyData.position.PosY = (int)this.gameObject.transform.position.y;
        astarPath = new List<TileData>();
        enemyState = ENEMYSTATE.IDLE;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hp_Bar_Front = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

        TileManager.Instance.tileMapInfoArray[enemyData.position.PosX, enemyData.position.PosY].tileData.tileRestriction = TILE_RESTRICTION.OCCUPIED;

        //꼭 필요할까
        mapWidth = TileManager.Instance.mapWidth;
        mapHeight = TileManager.Instance.mapHeight;          
    }
  

    //몬스터 타입에 따라 한턴 프로그레스 내용 변경? 
    //스텟은 어떻게?
    //적한테 인식번호를 줘야되나? 그래서 다른 곳에서 데이터를 관리?


    public virtual void TurnProgress()
    {     
        //자신의 HP상태가 얼마남지 않았으면 러닝어웨이로 전환(특정 몹 한정)

        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                //가만히 있던지 이리저리 돌아다님
                
                //플레이어와 거리가 인식범위 이내로 들어오면 트래킹으로 전환                
                break;
            case ENEMYSTATE.TRACKING:
                //플레이어를 대상으로 에이스타 사용 추적 이동
                //공격 사거리내에 플레이어가 있고 LOS가 나오면 어택으로 전환
                break;
            case ENEMYSTATE.ATTACK:
                //공격행동을 수행함.
                break;
            case ENEMYSTATE.RUNNINGAWAY:
                break;
        }
    }

    protected void TrackPlayer()
    {
        Astar.Instance.ClearData();
        astarPath = Astar.Instance.PathFinding(enemyData.position, playerPos); //플레이어로 길 탐색

        //이동전에 검색 가능하게
        TileManager.Instance.tileMapInfoArray[enemyData.position.PosX, enemyData.position.PosY].tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE; 

        enemyData.position = astarPath[0].position; //한칸만 이동
        transform.position = new Vector2(enemyData.position.PosX, enemyData.position.PosY);

        //이동후에 검색 불가능하게
        TileManager.Instance.tileMapInfoArray[enemyData.position.PosX, enemyData.position.PosY].tileData.tileRestriction = TILE_RESTRICTION.OCCUPIED;
    }

    protected void RandomMovement()
    {        
        int rndNum = UnityEngine.Random.Range(0, 8);
    }
  
    protected float distance(float _a, float _b)
    {
        return _a > _b ? _a : _b;
    }

    protected float diagonalDistance(int _x0, int _y0, int _x1, int _y1)
    {
        int dx = _x1 - _x0;
        int dy = _y1 - _y0;

        return distance(Mathf.Abs(dx), Mathf.Abs(dy));
    }

    public void CalcEnemyFov(Tile[,] _tilemap)
    {
        bool isFindPlayer = false;

        for (int i = 0; i < 360; i++) //1도씩 360도 계산
        {
            //플레이어를 찾았으면 시야 검색 중지
            if (isFindPlayer) break;

            float degree = i * (Mathf.PI / 180);

            int nx = Mathf.RoundToInt(Mathf.Cos(degree) * enemyData.viewRange) + enemyData.position.PosX;
            int ny = Mathf.RoundToInt(Mathf.Sin(degree) * enemyData.viewRange) + enemyData.position.PosY;

            float distance = diagonalDistance(enemyData.position.PosX, enemyData.position.PosY, nx, ny); //각도당 시야 거리 계산

            for (int j = 0; j < (int)distance; j++)
            {
                int tileX = Mathf.RoundToInt(Mathf.Lerp(enemyData.position.PosX, nx, j / distance)); //러프를 이용해서 걸리는 타일을 뽑는다.
                int tileY = Mathf.RoundToInt(Mathf.Lerp(enemyData.position.PosY, ny, j / distance));

                if (tileX < 0 || tileX >= mapWidth) continue;
                if (tileY < 0 || tileY >= mapHeight) continue;

                if (_tilemap[tileX, tileY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN) //벽을 만나면
                {                 
                    break; //그 뒤로는 검색 중지
                }
                else
                {
                    //시야거리 안에 플레이어가 있다면
                    if (tileX == playerPos.PosX && tileY == playerPos.PosY)
                    {
                        //공격 사거리 안에 있다면
                        if(enemyData.atkRange >= diagonalDistance(enemyData.position.PosX, enemyData.position.PosY, playerPos.PosX, playerPos.PosY))
                        {
                            //공격
                            enemyState = ENEMYSTATE.ATTACK;
                            isFindPlayer = true;
                            break;
                        }
                        //그냥 보이기만 하는거라면
                        else
                        {
                            //추적
                            enemyState = ENEMYSTATE.TRACKING;
                            isFindPlayer = true;
                            break;
                        }
                    }
                    //아무것도 안 보인다면
                    else
                    {
                        enemyState = ENEMYSTATE.IDLE;
                    }
                }
            }
        }       
    }   

    public void HideEnemy()
    {
        if(!TileManager.Instance.tileMapInfoArray[enemyData.position.PosX, enemyData.position.PosY].tileData.isSighted)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            spriteRenderer.enabled = false;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            spriteRenderer.enabled = true;
        }
    }

    public void HpBar_Update()
    {
        if (enemyData.curHp != 0) Hp_Bar_Front.fillAmount = enemyData.curHp / (float)enemyData.maxHp;
        else Hp_Bar_Front.fillAmount = 0;
    }

    public void DestroySequence()
    {
        TileManager.Instance.tileMapInfoArray[enemyData.position.PosX, enemyData.position.PosY].tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
        Destroy(this.gameObject);
        onDeath();
        PlayerManager.Instance.player.playerData.nextEXP -= enemyData.expValue; //몹이 죽을 때 경험치를 줌 (여기 있으면 안 됨*)
    }
}
