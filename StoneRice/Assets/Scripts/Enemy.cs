using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    private bool myTurn;
    public IEnumerator EnemyAction()
    {
        while(true)
        {
            if(myTurn)
            {
                //행동이 끝나면 턴 오버
            }

            yield return null;
        }
    }
}
