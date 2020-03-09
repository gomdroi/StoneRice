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
}
