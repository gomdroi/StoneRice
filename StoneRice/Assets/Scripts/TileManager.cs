using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int DungeonWidth;
    public int DungeonHeight;

    // Start is called before the first frame update
    void Start()
    {
        TileFactory baseTileFactory = new BaseTileFactory();
        baseTileFactory.createTile("StoneFloor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
