using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    Text hp_Text;
    Text mp_Text;
    Text AC_Text;
    Text EV_Text;
    Text XL_Text;
    Text Gold_Text;
    Text Str_Text;
    Text Int_Text;
    Text Dex_Text;

    Image Hp_Bar_Front;
    Image Mp_Bar_Front;

    Player m_player;

    private void Awake()
    { 
        hp_Text = GameObject.Find("Health_Text").GetComponent<Text>();
        mp_Text = GameObject.Find("Magic_Text").GetComponent<Text>();
        AC_Text = GameObject.Find("AC_Text").GetComponent<Text>();
        EV_Text = GameObject.Find("EV_Text").GetComponent<Text>();
        XL_Text = GameObject.Find("XL_Text").GetComponent<Text>();
        Gold_Text = GameObject.Find("Gold_Text").GetComponent<Text>();
        Str_Text = GameObject.Find("Str_Text").GetComponent<Text>();
        Int_Text = GameObject.Find("Int_Text").GetComponent<Text>();
        Dex_Text = GameObject.Find("Dex_Text").GetComponent<Text>();

        Hp_Bar_Front = GameObject.Find("Hp_Bar_Front").GetComponent<Image>();
        Mp_Bar_Front = GameObject.Find("Mp_Bar_Front").GetComponent<Image>();
    }

    public void Init()
    {
        m_player = PlayerManager.Instance.player;
    }

    public void UI_Update()
    {
        hp_Text.text = "Health : " + m_player.playerData.curHp + " / " + m_player.playerData.maxHp;
        mp_Text.text = "Magic : " + m_player.playerData.curMp + " / " + m_player.playerData.maxMp;
        AC_Text.text = "AC : " + m_player.playerData.AC;
        EV_Text.text = "EV : " + m_player.playerData.EV;
        XL_Text.text = "XL : " + m_player.playerData.XL + "    Next : " + m_player.playerData.nextEXP;
        Gold_Text.text = "Gold : " + m_player.playerData.Gold;
        Str_Text.text = "STR : " + m_player.playerData.strength;
        Int_Text.text = "INT : " + m_player.playerData.intelligence;
        Dex_Text.text = "DEX : " + m_player.playerData.dexterity;

        if (m_player.playerData.curHp != 0) Hp_Bar_Front.fillAmount = m_player.playerData.curHp / (float)m_player.playerData.maxHp;
        else Hp_Bar_Front.fillAmount = 0;

        if (m_player.playerData.curMp != 0) Mp_Bar_Front.fillAmount = m_player.playerData.curMp / (float)m_player.playerData.maxMp;
        else Mp_Bar_Front.fillAmount = 0;
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