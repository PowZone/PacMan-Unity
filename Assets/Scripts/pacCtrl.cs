using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacCtrl : MonoBehaviour
{
    public Texture2D map;

    public float mapScaleFactor = 2f;

    public GameObject[] wallsA;

    public GameObject player, mapCont;

    // ---

    int[] mapData;

    int curDir = 0;

    // ---

    int FindWallSide(int x, int y){

        int res = -1;

        bool tl = false, tc = false, tr = false;
        bool cl = false, cc = false, cr = false;
        bool bl = false, bc = false, br = false;

        if (y > 0)
        {
            if (x > 0) tl = (mapData[(y - 1) * map.width + (x - 1)] == 1);
            tc = (mapData[(y - 1) * map.width + (x + 0)] == 1);
            if (x < map.width - 1) tr = (mapData[(y - 1) * map.width + (x + 1)] == 1);
        }

        if (x>0) cl = (mapData[(y + 0) * map.width + (x - 1)] == 1);
        // bool cc = true;
        if (x < map.width-1) cr = (mapData[(y + 0) * map.width + (x + 1)] == 1);

        if (y < map.height-1)
        {
            if (x > 0) bl = (mapData[(y + 1) * map.width + (x - 1)] == 1);
            bc = (mapData[(y + 1) * map.width + (x + 0)] == 1);
            if (x < map.width - 1) br = (mapData[(y + 1) * map.width + (x + 1)] == 1);
        }

        if (cl && cr && !bc) res = 1;
        if (tc && bc && !cr) res = 3;
        if (tc && bc && !cl) res = 5;
        if (cl && cr && !tc) res = 7;

        if (res < 0)
        {
            if (cr && bc && (!br || !tl)) res = 6;
            if (cl && bc && (!bl || !tr)) res = 8;
            if (tc && cr && (!tr || !bl)) res = 0;
            if (tc && cl && (!tl || !br)) res = 2;
        }

        if (res < 0) res = 4;

        return res;
    }

    // Start is called before the first frame update
    void Start()
    {
        string maps = "";

        mapData = new int[map.width * map.height];

        // ---

        for (int y = map.height; y>=0; y--)
        {
            for (int x = 0; x < map.width;x++){
                Color c = map.GetPixel(x, y);
                // Debug.Log(c);
                if (c.r==1f && c.g==1f && c.b==0f){
                    mapData[y * map.width + x] = 100;
                    maps += "P ";
                }else
                if (c.g==0f && c.b == 1f)
                {
                    mapData[y * map.width + x] = 1;
                    maps += "# ";
                }else{
                    maps += "_ ";
                }
            }
            maps += "\n";
        }

        Debug.Log(maps);

        // ---

        int v,t = 0;
        Vector3 pos;

        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
            {
                v = mapData[y * map.width + x];
                pos = new Vector3(x * mapScaleFactor, y * mapScaleFactor, 1);

                if (v==1){

                    t = FindWallSide(x, y);

                    GameObject o = Instantiate(wallsA[t], mapCont.transform);
                    o.transform.localPosition = pos;

                }else 
                if(v==100){

                    player.transform.localPosition = pos;

                }
            }
        }
      
        // ---

    }

    bool HitWall(Vector3 pos){
        int x = (int)Mathf.Abs(pos.x / mapScaleFactor);
        int y = (int)Mathf.Abs(pos.y / mapScaleFactor);
        bool res = false;

        if (x < 0 || x > map.width - 1) res = true;
        if (y < 0 || y > map.height - 1) res = true;
        if (!res) if (mapData[y * map.width + x] == 1) res = true;

        return (res);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))   curDir = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))  curDir = 2;
        if (Input.GetKeyDown(KeyCode.UpArrow))     curDir = 1;
        if (Input.GetKeyDown(KeyCode.DownArrow))   curDir = 3;

        Vector3 pos;

        if (curDir==0)
        {
            pos = player.transform.localPosition;
            pos.x--;
            player.transform.localPosition = pos;
            //player.transform.localScale = Vector3.left;

            if (HitWall(pos))
            {
                curDir = -1;
                pos.x = Mathf.Abs(pos.x) + 1;
            }


        }
        if (curDir==2)
        {
            pos = player.transform.localPosition;
            pos.x++;
            player.transform.localPosition = pos;
            //player.transform.localScale = Vector3.right;

            if (HitWall(pos))
            {
                curDir = -1;
                pos.x = Mathf.Abs(pos.x) - 1;
            }

        }
        if (curDir==1)
        {
            pos = player.transform.localPosition;
            pos.y++;
            player.transform.localPosition = pos;

            if (HitWall(pos))
            {
                curDir = -1;
                pos.y = Mathf.Abs(pos.y) - 1;
            }

        }
        if (curDir==3)
        {
            pos = player.transform.localPosition;
            pos.y--;
            player.transform.localPosition = pos;

            if (HitWall(pos))
            {
                curDir = -1;
                pos.y = Mathf.Abs(pos.y) + 1;
            }

        }

    }
}
