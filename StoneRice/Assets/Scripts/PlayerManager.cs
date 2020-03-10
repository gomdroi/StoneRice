using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    GameObject playerPrefab;

    private void Awake()
    {
        playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
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
        oPlayer.GetComponent<Player>().PlayerInit();
    }
}
