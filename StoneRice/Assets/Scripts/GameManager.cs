using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int GM_mapWidth;
    public int GM_mapHeight;
    TileManager m_TileManager = null;
    EnemyManager m_EnemyManager = null;
    PlayerManager m_PlayerManager = null;
    TurnManager m_TunrManager = null;

    private void Awake()
    {
        m_TileManager = TileManager.Instance;
        m_PlayerManager = PlayerManager.Instance;
        m_EnemyManager = EnemyManager.Instance;
        m_TunrManager = TurnManager.Instance;
    }

    private void Start()
    {
        m_TileManager.mapWidth = GM_mapWidth;
        m_TileManager.mapHeight = GM_mapHeight;
    }

}
