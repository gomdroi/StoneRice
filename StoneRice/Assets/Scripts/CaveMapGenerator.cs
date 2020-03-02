using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapGenerator : MonoBehaviour
{
    //실제 타일 배열
    public Tile[,] tileMapInfo;
    //월카운트용 배열
    public int[,] wallCount;
    //맵 부드러움 계수
    public int mapSmoothness;
    //셔플용 인덱스 배열
    public int[] tileIndexArray;
    // 벽 생성 비율
    public int wallRatio;

    TileManager m_tileManager = null;

    private void Awake()
    {
        m_tileManager = TileManager.Instance;
    }

    public void GenerateCaveMap()
    {
        //맵 정보 받아오기
        tileMapInfo = m_tileManager.tileMapInfoArray;
        //월카운트 배열 초기화
        wallCount = new int[m_tileManager.mapWidth , m_tileManager.mapHeight];
        //셔플용 배열 초기화
        tileIndexArray = new int[m_tileManager.mapWidth * m_tileManager.mapHeight];
        for (int i = 0; i < tileIndexArray.Length; ++i)
        {
            tileIndexArray[i] = i;
        }

        shuffleIndexArry();
        SetRandomWall();
        for(int i = 0; i < mapSmoothness; i++)
        {
            CaveShaping();
        }
    }

    private void shuffleIndexArry()
    {      
        int temp;
        for (int i = 0; i < tileIndexArray.Length * 2; i++)
        {
            int sour = Random.Range(0, tileIndexArray.Length);
            int dest = Random.Range(0, tileIndexArray.Length);

            //스왑
            temp = tileIndexArray[sour];
            tileIndexArray[sour] = tileIndexArray[dest];
            tileIndexArray[dest] = temp;
        }
    }

    private void SetRandomWall()
    {
        //벽 비율
        float wallTileCount = (float)tileIndexArray.Length / 100 * wallRatio;

        for (int i = 0; i < wallTileCount; i++)
        {
            int tileX = tileIndexArray[i] / m_tileManager.mapWidth;
            int tileY = tileIndexArray[i] % m_tileManager.mapWidth;

            //벽 배치
            tileMapInfo[tileX, tileY].tileData.tileType = BASETILETYPE.STONEWALL;
        }
    }   
   
    private void CaveShaping()
    {
        for (int i = 0; i < m_tileManager.mapWidth; i++)
        {
            for (int j = 0; j < m_tileManager.mapHeight; j++)
            {
                CheckWallCount(i, j);
            }
        }

        for (int i = 0; i < m_tileManager.mapWidth; i++)
        {
            for (int j = 0; j < m_tileManager.mapHeight; j++)
            {
                if (wallCount[i, j] >= 5) tileMapInfo[i, j].tileData.tileType = BASETILETYPE.STONEWALL;
                else tileMapInfo[i, j].tileData.tileType = BASETILETYPE.STONEFLOOR;
            }
        }
    }

    private void CheckWallCount(int _tileX, int _tileY)
    {
        wallCount[_tileX, _tileY] = 0;
        int checkPointX = _tileX - 1;
        int checkPointY = _tileY - 1;

        for (int i = checkPointX; i < checkPointX + 3; i++)
        {
            for (int j = checkPointY; j < checkPointY + 3; j++)
            {
                if (i < 0 || j < 0 || i >= m_tileManager.mapWidth || j >= m_tileManager.mapHeight) wallCount[_tileX, _tileY] += 1;
                else if (tileMapInfo[i, j].tileData.tileType == BASETILETYPE.STONEWALL) wallCount[_tileX, _tileY] += 1;
            }
        }
    }
}
