using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AstarTile
{
    public Position position;
    public TileData tileData;
    public TileData parentTileData;

    public bool isListed;

    public int F; //g+h
    public int G; //from start to this
    public int H; //from this to end   
    
    public void Init()
    {
        isListed = false;
        F = 5000;
        G = 0;
        H = 0;

        tileData = new TileData();
        parentTileData = new TileData();
    }

    public void SetTile(AstarTile _lastindex, List<AstarTile> _openlist,Position _endpos)
    {
        if (tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN) return; //이동 할 수 없는 타일이면 리턴

        if (!isListed) //오픈 리스트에 없다면
        {
            isListed = true; //트루로 바꾸고
            _openlist.Add(this); //오픈 리스트에 추가
            CalcH(_endpos); //H값 계산 적용
            G = _lastindex.G + 14; //G값 계산 적용
            CalcF(); //F계산
            parentTileData = _lastindex.tileData; //부모를 검색타일로 설정            
        }
        else //오픈 리스트에 있다면
        {
            if (_lastindex.G + 14 < G) //기존 G보다 새로운 G가 작다면
            {
                G = _lastindex.G + 14; //G값 다시 적용
                CalcF(); //F계산
                parentTileData = _lastindex.tileData; //부모를 검색타일로 설정  
            }
        }
    }

    void CalcH(Position _endpos)
    {
        int vertical = Mathf.Abs(_endpos.PosX - position.PosX) * 10;//가로H 값
        int horizontal = Mathf.Abs(_endpos.PosX - position.PosX) * 10;//세로 H값
  
        H = vertical + horizontal;   //총 h값 :  가로+세로 H    
    }

    void CalcF()
    {
        F = G + H;
    }
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
    Tile[,] tileMapInfo; //실제 타일 배열

    int lastIndex;
    List<AstarTile> openList;
    List<AstarTile> closeList;
    List<TileData> pathList;    

    void PathFinding(Position _beginpos, Position _endpos)
    {
        bool isDone = false;

        closeList.Add(astarTiles[_beginpos.PosX, _beginpos.PosY]);          

        while(!isDone)
        {
            AddOpenList(_endpos);
        }
    }

    void AstarInit()
    {
        //해당 층에서 필요한 타일맵 정보를 받아온다.

        mapHeight = TileManager.Instance.mapHeight;
        mapWidth = TileManager.Instance.mapWidth;
        tileMapInfo = TileManager.Instance.tileMapInfoArray;

        astarTiles = new AstarTile[mapWidth, mapHeight];       

        for(int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                astarTiles[j, i] = new AstarTile();
                astarTiles[j, i].Init();
                astarTiles[j, i].position.PosX = j;
                astarTiles[j, i].position.PosY = i;
                astarTiles[j, i].tileData = tileMapInfo[j, i].tileData; //참조
            }
        }

        lastIndex = 0;
        openList = new List<AstarTile>();
        closeList = new List<AstarTile>();
    }

    
    void AddOpenList(Position _endpos)
    {
        Position searchPosition = closeList[lastIndex].position;

        int checkPointX = closeList[lastIndex].position.PosX - 1;
        int checkPointY = closeList[lastIndex].position.PosY - 1;

        for (int i = checkPointX; i < checkPointX + 3; i++)
        {
            for (int j = checkPointY; j < checkPointY + 3; j++)
            {
                if (i < 0 || j < 0 || i >= mapWidth || j >= mapHeight) continue; //배열범위에서 벗어나거나 
                else if (i == 4 && j == 4) continue; //자신이면 컨티뉴
                else astarTiles[checkPointX, checkPointY].SetTile(closeList[lastIndex], openList, _endpos);
            }
        }      
    }

    void AddOpenList0()
    {
        Position searchPosition = closeList[lastIndex].position;

        //좌상단 검색
        if (MoveableCheck(searchPosition, -1, 1)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //상단 검색
        if (MoveableCheck(searchPosition, 0, 1)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX, searchPosition.PosY + 1].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX, searchPosition.PosY + 1].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX, searchPosition.PosY + 1].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX, searchPosition.PosY + 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX, searchPosition.PosY + 1].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX, searchPosition.PosY + 1].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX, searchPosition.PosY + 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //우상단 검색
        if (MoveableCheck(searchPosition, 1, 1)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX + 1, searchPosition.PosY + 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //좌측 검색
        if (MoveableCheck(searchPosition, -1, 0)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX - 1, searchPosition.PosY].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX - 1, searchPosition.PosY]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX - 1, searchPosition.PosY].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX - 1, searchPosition.PosY].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX - 1, searchPosition.PosY].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //우측 검색
        if (MoveableCheck(searchPosition, +1, 0)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX + 1, searchPosition.PosY].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX + 1, searchPosition.PosY]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX + 1, searchPosition.PosY].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX + 1, searchPosition.PosY].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX + 1, searchPosition.PosY].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //좌하단 검색
        if (MoveableCheck(searchPosition, -1, -1)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX - 1, searchPosition.PosY - 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //하단 검색
        if (MoveableCheck(searchPosition, 0, -1)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX, searchPosition.PosY - 1].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX, searchPosition.PosY - 1].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX - 1, searchPosition.PosY + 1]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX, searchPosition.PosY - 1].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX, searchPosition.PosY - 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX, searchPosition.PosY - 1].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX, searchPosition.PosY - 1].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX, searchPosition.PosY - 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }

        //우하단 검색
        if (MoveableCheck(searchPosition, 1, -1)) //이동 가능한 타일인지 체크
        {
            if (!astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].isListed) //오픈 리스트에 없다면
            {
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].isListed = true; //트루로 바꾸고
                openList.Add(astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1]); //오픈 리스트에 추가
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].G = closeList[lastIndex].G + 14; //G값 계산 적용
                astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정                              
            }
            else //오픈 리스트에 있다면
            {
                if (closeList[lastIndex].G + 14 < astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].G) //기존 G보다 새로운 G가 작다면
                {
                    astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].G = closeList[lastIndex].G + 14; //G값 다시 적용
                    astarTiles[searchPosition.PosX + 1, searchPosition.PosY - 1].parentTileData = closeList[lastIndex].tileData; //부모를 검색타일로 설정  
                }
            }
        }
    } //초안 
    public bool MoveableCheck(Position _position, int _targetX, int _targetY, bool _isfly = false)
    {
        //타일 배열 벗어났는지 체크
        if(_position.PosX - 1 < 0 || _position.PosY - 1 < 0 || _position.PosX + 1 > mapWidth || _position.PosY + 1 > mapHeight)
        {
            return false;
        }
        
        //이동 가능 타일인지 체크
        if(astarTiles[_position.PosX + _targetX, _position.PosY + _targetY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN)
        {
            return false;
        }

        return true;
    } //이동가능 체크 초안
}
