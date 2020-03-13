using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public Position position;
    List<TileData> astarPath;
    public Position playerPos;

    //ASTAR 테스트
    float rayDistance;

    public void EnemyInit()
    {
        position.PosX = (int)this.gameObject.transform.position.x;
        position.PosY = (int)this.gameObject.transform.position.y;
        astarPath = new List<TileData>();

        rayDistance = 15f;
    }

    private void Update()
    {
        AstarTestRayCast();
    }

    public void TurnProgress()
    {
        if(CheckAtkRange()) //공격 범위에 적이없으면
        {

        }
        else //이동
        {
            Astar.Instance.AstarTest();
            playerPos = PlayerManager.Instance.player.position; //플레이어 포지션 확인
            astarPath = Astar.Instance.PathFinding(position, playerPos); //그곳으로 길탐색
            position = astarPath[0].position; //한칸만 이동
            transform.position = new Vector2(position.PosX, position.PosY);
        }
    }

    bool CheckAtkRange()
    {
        return false;
    }

    void AstarTestRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {         
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, rayDistance);
            Debug.DrawRay(mousePosition, transform.forward * 15, Color.red, 0.3f);
            if (hit)
            {
                Debug.Log(hit.transform.position);
                Position destination;
                destination.PosX = (int)hit.transform.position.x;
                destination.PosY = (int)hit.transform.position.y;

                StopCoroutine("moveMan");
                Astar.Instance.AstarTest();
                astarPath = Astar.Instance.PathFinding(position, destination);
                StartCoroutine("moveMan");
            }
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
