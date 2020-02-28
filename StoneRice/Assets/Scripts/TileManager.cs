using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public Tile[,] tileMap;
    public BaseTileFactory bTileFactory;

    private void Awake()
    {
        bTileFactory = GetComponent<BaseTileFactory>();
    }

    void Start()
    {
        tileMap = new Tile[mapWidth,mapHeight];

        for (int i = 0; i < mapHeight; i++)
        {
            for(int j = 0; j < mapWidth; j++)
            {
                tileMap[i, j] = new Tile();
                tileMap[i, j].setTilePosition(i,j);
                tileMap[i, j].oTile = bTileFactory.createTile("StoneWall", i, j);
            }
        }
    }
    
}
