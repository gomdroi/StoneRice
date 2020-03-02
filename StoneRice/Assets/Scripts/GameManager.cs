using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int GM_mapWidth;
    public int GM_mapHeight;
    TileManager m_TileManager = null;
    EnemyManager m_EnemyManager = null;

    private void Awake()
    {
        m_TileManager = TileManager.Instance;
        m_EnemyManager = EnemyManager.Instance;

        //m_manager.mapWidth = GM_mapWidth;
        //m_manager.mapHeight = GM_mapHeight;
    }
}
