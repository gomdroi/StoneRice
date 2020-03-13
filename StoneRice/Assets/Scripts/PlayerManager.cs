using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    GameObject playerPrefab;
    public Player player;
    List<GameObject> minionList;

    private void Awake()
    {
        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
    }

    private void Start()
    {
        minionList = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CallPlayer();
        }
    }

    void CallPlayer()
    {
        var oPlayer = Instantiate(playerPrefab, new Vector2(TileManager.instance.stairDownPos.PosX, TileManager.instance.stairDownPos.PosY), Quaternion.identity);
        player = oPlayer.GetComponent<Player>();
        player.PlayerInit();
    }
}
