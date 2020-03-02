using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public IEnumerator PlayerAction()
    {
        while(true)
        {
            if(!MainGame.playerTurnFinish)
            {

            }

            yield return null;
        }
    }
}
