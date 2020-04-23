using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    GameObject playerPrefab;
    public Player player;
    List<GameObject> minionList;

    //ASTAR 클릭 이동 테스트
    //float rayDistance;

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

        //private void Update()
        //{
        //    AstarTestRayCast();
        //}
    }

    void CallPlayer()
    {
        var oPlayer = Instantiate(playerPrefab, new Vector2(TileManager.instance.stairUpPos.PosX, TileManager.instance.stairUpPos.PosY), Quaternion.identity);
        player = oPlayer.GetComponent<Player>();
        player.PlayerInit();

        //유아이에 생성과 동시에 정보 전달
        UIManager.Instance.playerUI.Init();        

        Camera.main.transform.SetParent(oPlayer.transform);
        Camera.main.transform.position = new Vector3(oPlayer.transform.position.x, oPlayer.transform.position.y, -10);
    }

    //void AstarTestRayCast()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {         
    //        Vector3 mousePosition;
    //        mousePosition = Input.mousePosition;
    //        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

    //        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, rayDistance);
    //        Debug.DrawRay(mousePosition, transform.forward * 15, Color.red, 0.3f);
    //        if (hit)
    //        {
    //            Debug.Log(hit.transform.position);
    //            Position destination;
    //            destination.PosX = (int)hit.transform.position.x;
    //            destination.PosY = (int)hit.transform.position.y;

    //            StopCoroutine("moveMan");
    //            Astar.Instance.AstarTest();
    //            astarPath = Astar.Instance.PathFinding(position, destination);
    //            StartCoroutine("moveMan");
    //        }
    //    }
    //}

    //IEnumerator moveMan()
    //{
    //    for(int i = 0; i < astarPath.Count; i++)
    //    {
    //        position = astarPath[i].position;
    //        transform.position = new Vector2(position.PosX, position.PosY);
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}
}
