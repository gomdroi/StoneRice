using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Position position;
    List<TileData> astarPath;

    public void EnemyInit()
    {
        position.PosX = (int)this.gameObject.transform.position.x;
        position.PosY = (int)this.gameObject.transform.position.y;
        astarPath = new List<TileData>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            astarPath = Astar.Instance.PathFinding(position, TileManager.Instance.stairDownPos);
            //position = astarPath[0].position;
            //transform.position = new Vector2(position.PosX, position.PosY);
            StartCoroutine("moveMan");
        }
    }
    
    IEnumerator moveMan()
    {
        for(int i = 0; i < astarPath.Count; i++)
        {
            position = astarPath[i].position;
            transform.position = new Vector2(position.PosX, position.PosY);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
