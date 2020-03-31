using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public int GM_mapWidth;
    public int GM_mapHeight;
    //모노싱글톤
    TileManager m_TileManager = null;
    EnemyManager m_EnemyManager = null;
    PlayerManager m_PlayerManager = null;
    TurnManager m_TunrManager = null;
    TrapManager m_TrapManager = null;
    LogManager m_LogManager = null;
    UIManager m_UIManager = null;
    //싱글톤
    Astar m_Astar = null;
    BattleManager m_BattleManager = null;
    public int curStage;

    //임시
    public Text stageText;

    private void Awake()
    {
        m_TileManager = TileManager.Instance;
        m_PlayerManager = PlayerManager.Instance;
        m_EnemyManager = EnemyManager.Instance;        
        m_TrapManager = TrapManager.Instance;
        m_LogManager = LogManager.Instance;
        m_UIManager = UIManager.Instance;
        m_TunrManager = TurnManager.Instance;

        m_Astar = Astar.Instance;
        m_BattleManager = BattleManager.Instance;
    }

    private void Start()
    {
        m_TileManager.mapWidth = GM_mapWidth;
        m_TileManager.mapHeight = GM_mapHeight;
        curStage = 0;
    }

    private void Update()
    {
        //초기화 0층 생성
        if (Input.GetKeyDown(KeyCode.S))
        {
            ResourceManager.Instance.LoadAtlas(); //이미지 정보 로드
            m_TileManager.Init(); //타일맵 생성 빈타일 오브젝트 배치
            m_TileManager.CreateCaveMap(); //동굴생성 타일에 속성 적용
            m_TileManager.SaveStage(curStage, true); //첫층 저장
            Astar.Instance.AstarInit(); //맵정보 받아서 에이스타용 배열 생성및 초기화
            m_TileManager.MakeStairs();
            m_TrapManager.Init(); //트랩 설치를 위해 맵정보 받아오기
            m_TrapManager.PlaceTraps(10); //함정 설치
            m_TrapManager.SaveTraps(curStage, true); //초기 함정 상태 저장

            m_BattleManager.Init(); //배틀 매니저 초기 설정
        }

        //스테이지 선택
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_TileManager.LoadStage(0);
            m_TileManager.FindStairs();
            m_TileManager.ApplyChange();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_TileManager.LoadStage(1);
            m_TileManager.FindStairs();
            m_TileManager.ApplyChange();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_TileManager.LoadStage(2);
            m_TileManager.FindStairs();
            m_TileManager.ApplyChange();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            m_TileManager.CreateCaveMap();
            m_TileManager.SaveStage(curStage, true);
            m_TileManager.FindStairs();         
            
        }
    }

    //다음층으로 내려갈때
    public void GoDownStage()
    {           
        if(m_TileManager.Stages.Count <= curStage + 1) //다음층에 간적이 없다면
        {
            m_TileManager.SaveStage(curStage); //현재층 맵 상태를 저장
            m_TrapManager.SaveTraps(curStage); //현재층 트랩 상태를 저장  

            m_TileManager.CreateCaveMap(); //새로운 맵을 만듬
            m_TileManager.SaveStage(curStage, true); //만든 맵을 저장
            m_TileManager.FindStairs(); //계단 재배치
                     
            curStage += 1; //스테이지 증가

            m_TrapManager.PlaceTraps(10); //새로운 트랩을 깜
            m_TrapManager.SaveTraps(curStage, true); //새로운 스테이지의 트랩을 저장
        }        
        else //해당층에 간적이 있다면
        {
            m_TileManager.SaveStage(curStage); //현재층 맵 상태를 저장
            m_TrapManager.SaveTraps(curStage); //현재층 트랩 상태를 저장
            curStage += 1; //스테이지 증가
            m_TileManager.LoadStage(curStage); //다음층의 스테이지를 로드
            m_TrapManager.LoadTraps(curStage); //다음층의 트랩 로드
            m_TileManager.FindStairs(); //계단 재배치
        }

        stageText.text = "Floor : " + curStage.ToString();
    }

    //이전 층으로 돌아갈때
    public void GoUpStage()
    {
        if (curStage <= 0) //첫층이라면
        {
            m_LogManager.SimpleLog("아직은 떠날 수 없다");
        }
        else //첫층이 아니라면
        {
            m_TileManager.SaveStage(curStage); //현재층 맵정보 저장
            m_TrapManager.SaveTraps(curStage); //현재층 트랩정보 저장
            curStage -= 1; //스테이지 감소
            m_TileManager.LoadStage(curStage); //이전층을 로드
            m_TrapManager.LoadTraps(curStage); //이전층 트랩 로드
            m_TileManager.FindStairs(); //계단 재배치          
        }

        stageText.text = "Floor : " + curStage.ToString();
    }


}
