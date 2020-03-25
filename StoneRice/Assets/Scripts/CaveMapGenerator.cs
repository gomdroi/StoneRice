using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public List<Tile> roomList;
    public int roomSize;
}

public class CaveMapGenerator : MonoBehaviour
{
    //실제 타일 배열
    public Tile[,] tileMapInfo;
    //월카운트용 배열
    public int[,] wallCount;
    //룸체크용 배열
    public bool[,] roomCheck;
    public List<Room> rooms;
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

    public void Init()
    {
        //월카운트 배열 할당
        wallCount = new int[m_tileManager.mapWidth, m_tileManager.mapHeight];
        //셔플용 배열 할당
        tileIndexArray = new int[m_tileManager.mapWidth * m_tileManager.mapHeight];
        //룸체크 배열 할당
        roomCheck = new bool[m_tileManager.mapWidth, m_tileManager.mapHeight];
        //룸 리스트 할당
        rooms = new List<Room>();

        //그 외 속성 할당
        mapSmoothness = 7;
        wallRatio = 45;
    }

    public void GenerateCaveMap()
    {
        //맵 정보 받아오기
        tileMapInfo = m_tileManager.tileMapInfoArray;
        //월카운트 배열 초기화
        for (int i = 0; i < m_tileManager.mapHeight; i++)
        {
            for (int j = 0; j < m_tileManager.mapWidth; j++)
            {
                wallCount[j, i] = 0;
            }
        }
        //룸체크 배열 초기화
        for (int i = 0; i < m_tileManager.mapWidth; i++)
        {
            for (int j = 0; j < m_tileManager.mapHeight; j++)
            {
                roomCheck[j, i] = false;
            }
        }
        //셔플용 배열 초기화
        for (int i = 0; i < tileIndexArray.Length; ++i)
        {
            tileIndexArray[i] = i;
        }

        //셔플해서 벽배치
        shuffleIndexArry();
        SetRandomWall();
        //동굴 만들기
        for (int i = 0; i < mapSmoothness; i++)
        {
            CaveShaping();
        }
       
        //룸리스트 초기화
        rooms.Clear();
        //방 검색
        AddRooms();

        //계단 놓기
        SetStairs();
    }

    //랜덤한 벽을 배치하기위해 인덱스 셔플
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

    //랜덤한 인덱스의 실제 타일에 벽 배치
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

    //셀룰러 오토마타 알고리즘으로 동굴을 한번 깎는 함수
    private void CaveShaping()
    {
        for (int i = 0; i < m_tileManager.mapHeight; i++)
        {
            for (int j = 0; j < m_tileManager.mapWidth; j++)
            {
                CheckWallCount(j, i);
            }
        }

        for (int i = 0; i < m_tileManager.mapHeight; i++)
        {
            for (int j = 0; j < m_tileManager.mapWidth; j++)
            {
                if (wallCount[j, i] >= 5)
                {
                    tileMapInfo[j, i].tileData.tileType = BASETILETYPE.STONEWALL;
                    tileMapInfo[j, i].tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
                }
                else
                {
                    tileMapInfo[j, i].tileData.tileType = BASETILETYPE.STONEFLOOR;
                    tileMapInfo[j, i].tileData.tileRestriction = TILE_RESTRICTION.MOVEABLE;
                }
            }
        }
    }

    //자신을 포함한 주변 8칸의 벽개수를 찾는 함수
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

    //빈 공간에 계단을 배치
    void SetStairs()
    {
        bool isStairDownSet = false;

        while (true)
        {
            int stairX = Random.Range(0, m_tileManager.mapWidth);
            int stairY = Random.Range(0, m_tileManager.mapHeight);

            if (!isStairDownSet)
            {
                if (tileMapInfo[stairX, stairY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN) continue;
                else
                {
                    //tileMapInfo[stairX, stairY].tileData.tileType = BASETILETYPE.STAIR_DOWN;
                    m_tileManager.stairDownPos.PosX = stairX;
                    m_tileManager.stairDownPos.PosY = stairY;
                    Debug.Log("내려가는 계단 : " + stairX + " , " + stairY);
                    isStairDownSet = true;
                }
            }
            else if (isStairDownSet)
            {
                if (tileMapInfo[stairX, stairY].tileData.tileRestriction == TILE_RESTRICTION.FORBIDDEN) continue;
                else
                {
                    //tileMapInfo[stairX, stairY].tileData.tileType = BASETILETYPE.STAIR_UP;
                    m_tileManager.stairUpPos.PosX = stairX;
                    m_tileManager.stairUpPos.PosY = stairY;
                    Debug.Log("올라가는 계단 : " + stairX + " , " + stairY);
                    break;
                }
            }
        }
    }

    //방을 찾고 방 리스트를 만들어줌
    void AddRooms()
    {
        bool isAllRoomChecked = false;
        bool flag = false;

        while (!isAllRoomChecked)
        {
            flag = false;

            for (int i = 0; i < m_tileManager.mapHeight; i++)
            {
                for (int j = 0; j < m_tileManager.mapWidth; j++)
                {
                    if (tileMapInfo[j, i].tileData.tileType == BASETILETYPE.STONEWALL || roomCheck[j, i] == true) continue;
                    else
                    {
                        //방 하나의 크기를 재서 반환
                        rooms.Add(CheckRoomSize(j, i));
                        //이중 포문 탈출
                        flag = true;
                        break;
                    }
                }
                //아직 더 찾아야되면 여기서 브레이크
                if (flag) break;
                //포문을 다 돌았다면(모든 방을 찾았다면)
                if (i >= m_tileManager.mapHeight - 1) isAllRoomChecked = true;
            }
        }
        Debug.Log(rooms.Count);
        CompareRooms();
    }

    //(벽을 기준으로)방의 크기를 재서 방을 반환
    public Room CheckRoomSize(int _PosX, int _PosY)
    {
        int RoomCount = 0;
        List<Tile> openList = new List<Tile>();
        List<Tile> closeList = new List<Tile>();
        Room room = new Room();
        //HashSet은 인덱스 접근이 불가능, 이터레이터로 순회하면 순회중에 목록을 건드릴 수 없음.
        

        
        //오픈 리스트를 돌면서 주변 타일을 검색해서 다시 오픈리스트에 넣어줌
        //주변 검색을 시도한 타일은 클로즈 리스트로 들어감
        openList.Add(tileMapInfo[_PosX, _PosY]);
        while (openList.Count != 0)
        {
            for (int i = 0; i < openList.Count; i++)
            {
                //상               
                if (!(openList[i].tileData.position.PosY + 1 >= m_tileManager.mapHeight)) //최상단 타일이 아니라면
                {
                    if (!closeList.Contains(tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY + 1])
                    && tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY + 1].tileData.tileType != BASETILETYPE.STONEWALL) //클로즈 리스트에 없고 벽이 아니라면
                    {
                        if(!(openList.Contains(tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY + 1]))) //오픈 리스트에 없다면
                        {
                            openList.Add(tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY + 1]);
                        }                                              
                    }
                }              
                //하
                if (!(openList[i].tileData.position.PosY - 1 < 0)) //최하단 타일이 아니라면
                {
                    if (!closeList.Contains(tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY - 1])
                    && tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY - 1].tileData.tileType != BASETILETYPE.STONEWALL)
                    {
                        if(!(openList.Contains(tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY - 1])))
                        {
                            openList.Add(tileMapInfo[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY - 1]);
                        }                                           
                    }
                }           
                //좌
                if(!(openList[i].tileData.position.PosX - 1 < 0)) //좌측 끝 타일이 아니라면
                {
                    if (!closeList.Contains(tileMapInfo[openList[i].tileData.position.PosX - 1, openList[i].tileData.position.PosY])
                    && tileMapInfo[openList[i].tileData.position.PosX - 1, openList[i].tileData.position.PosY].tileData.tileType != BASETILETYPE.STONEWALL)
                    {
                        if (!(openList.Contains(tileMapInfo[openList[i].tileData.position.PosX - 1, openList[i].tileData.position.PosY])))
                        {
                            openList.Add(tileMapInfo[openList[i].tileData.position.PosX - 1, openList[i].tileData.position.PosY]);
                        }                                             
                    }
                }              
                //우
                if(!(openList[i].tileData.position.PosX + 1 >= m_tileManager.mapWidth))
                {
                    if (!closeList.Contains(tileMapInfo[openList[i].tileData.position.PosX + 1, openList[i].tileData.position.PosY])
                    && tileMapInfo[openList[i].tileData.position.PosX + 1, openList[i].tileData.position.PosY].tileData.tileType != BASETILETYPE.STONEWALL)
                    {
                        if (!(openList.Contains(tileMapInfo[openList[i].tileData.position.PosX +1, openList[i].tileData.position.PosY])))
                        {
                            openList.Add(tileMapInfo[openList[i].tileData.position.PosX + 1, openList[i].tileData.position.PosY]);
                        }                                            
                    }
                }
                //검색 체크 배열 트루
                roomCheck[openList[i].tileData.position.PosX, openList[i].tileData.position.PosY] = true;
                closeList.Add(openList[i]);
                openList.Remove(openList[i]);
                RoomCount += 1;
            }       
        }
        room.roomList = closeList;
        room.roomSize = RoomCount;
        return room;              
    }

    void CompareRooms()
    {
        rooms.Sort(delegate (Room A, Room B)
        {
            if (A.roomSize > B.roomSize) return -1;
            else if (A.roomSize < B.roomSize) return 1;
            return 0;
        });
        
        for(int i = 0;i < rooms.Count; i++)
        {
            Debug.Log(i + "번째 방 개수 : " + rooms[i].roomSize);
        }

        //가장 큰방을 남김
        for (int i = 1; i < rooms.Count; i++)
        {
            foreach (Tile tile in rooms[i].roomList)
            {
                tile.tileData.tileType = BASETILETYPE.OUTOFRANGE;
                tile.tileData.tileRestriction = TILE_RESTRICTION.FORBIDDEN;
            }
        }
    }
}
    
