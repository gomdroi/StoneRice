using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public PlayerUI playerUI;

    public GameObject EnemyInfo;
    public GameObject Hp_BarPrefab;

    private void Awake()
    {
        playerUI = new PlayerUI();
        Hp_BarPrefab = Resources.Load("Prefabs/Hp_Bar") as GameObject;
        EnemyInfo = GameObject.Find("Enemy_Info");
    }

    public void UI_Update()
    {
        playerUI.PlayerUI_Update();
    }

    public void GenerateHp_Bar()
    {
        var oHp_Bar = Instantiate(Hp_BarPrefab,EnemyInfo.transform);
    }
}


//namespace Example
//{
//    public class Player
//    {
//        public Action<int> OnChangedHp = null;

//        private int _hp = 0;

//        public void SetHp(int value)
//        {
//            _hp = value;
//            OnChangedHp?.Invoke(_hp);
//        }
//    }

//    public class PlayerUI
//    {
//        private int _label = 0;

//        public PlayerUI(Player player)
//        {

//            player.OnChangedHp = (hp) => {
//                _label = hp;
//            };
//        }

//    }

//    public class Entry
//    {
//        public Entry()
//        {
//            var player = new Player();
//            var ui = new PlayerUI(player);

//        }
//    }

//}