using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AstarTile
{
    public TileData tileData;
    public TileData parentTileData;

    public int f; //g+h
    public int g; //from start to this
    public int h; //from this to end    
}

public class Astar : MonoBehaviour
{
    //1. 시작지점으로부터 8방향 검사 각 타일에 f=g+h를 적용, 자신을 부모로 설정, 오픈 리스트에 포함, 자신을 클로즈 리스트에 포함
    //2. f값이 가장 작은 타일을 선택 다시 8방향 검사 오픈 리스트에 없는 타일이면 f=g+h를 적용, 자신을 부모로 설정, 오픈리스트에 포함
    //   오픈 리스트에 있는 타일이라면 추정값을 비교해서 부모노드를 바꿔준다.
    //3. 검사중 도착지점을 발견하면 부모를 자신으로 설정하고 경로를 반환한다.
    //몹한테 경로 전달

    AstarTile[,] astarTiles; //검색용 배열
    int mapHeight;
    int mapWidth; 
    Tile[,] tileMapInfo;

    List<TileData> openList;
    List<TileData> closeList;
    List<TileData> pathList;


    void PathFinding(Position _beginpos, Position _endpos)
    {
        
    }

    void AstarInit()
    {
        mapHeight = TileManager.Instance.mapHeight;
        mapWidth = TileManager.Instance.mapWidth;
        tileMapInfo = TileManager.Instance.tileMapInfoArray;

        astarTiles = new AstarTile[mapWidth, mapHeight];       

        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                astarTiles[j, i].tileData = new TileData();
                astarTiles[j, i].parentTileData = new TileData();
                astarTiles[j, i].tileData = tileMapInfo[j, i].tileData;
            }
        }

        openList = new List<TileData>();
        closeList = new List<TileData>();
    }

    void start()
    {

    }

}
