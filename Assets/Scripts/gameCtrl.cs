using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameCtrl : MonoBehaviour
{
    public Texture2D map;

    public float mapScaleFactor = 2f;

    public GameObject wallsA;
    public Sprite[] wallsSpritesA;

    public GameObject player, mapCont;

    // ---

    public float speedVal = 3f;

    public TextMeshPro debugText, highScoreText;

    // ---

    int[] mapData;

    int oldDir = -1, nextDir = -1, curDir = 0, curScore = 0;

    bool playerStopped = false;

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

                    GameObject o = Instantiate(wallsA, mapCont.transform);
                    // o.GetComponent<SpriteRenderer>().sprite = wallsSpritesA[t];
                    o.transform.localPosition = pos;

                }else 
                if(v==100){

                    player.transform.localPosition = pos;

                }
            }
        }
      
        // ---

    }

    bool HitWall(int dx = 0, int dy = 0){
        Vector3 pos = player.transform.localPosition;
        int x = (int)Mathf.Round(pos.x / mapScaleFactor) +dx;
        int y = (int)Mathf.Round(pos.y / mapScaleFactor) +dy;
        bool res = false;

        // Debug.Log(x + " " + y);

        if (x < 0 || x > map.width - 1) res = true;
        if (y < 0 || y > map.height - 1) res = true;
        if (!res) if (mapData[y * map.width + x] == 1) res = true;

        return (res);
    }

    void PlayerCxPos(){
        Vector3 pos = player.transform.localPosition;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        player.transform.localPosition = pos;
    }

    public void stopPlayer(){
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerCxPos();
        curDir = nextDir = -1;
        playerStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))   nextDir = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))  nextDir = 2;
        if (Input.GetKeyDown(KeyCode.UpArrow))     nextDir = 1;
        if (Input.GetKeyDown(KeyCode.DownArrow))   nextDir = 3;

        if (    (playerStopped && nextDir>=0) 
                || (nextDir == 0 && !HitWall(-1, 0)) 
                || (nextDir == 1 && !HitWall(0, -1)) 
                || (nextDir == 2 && !HitWall(1, 0)) 
                || (nextDir == 3 && !HitWall(0, 1)) 
           )
        {
            curDir = nextDir;
            playerStopped = false;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (oldDir != curDir) PlayerCxPos();

        if (curDir==0)
        {
            rb.velocity = new Vector2(-speedVal, 0f);
        }
        if (curDir==2)
        {
            rb.velocity = new Vector2(speedVal, 0f);
        }
        if (curDir==1)
        {
            rb.velocity = new Vector2(0f, speedVal);
        }
        if (curDir==3)
        {
            rb.velocity = new Vector2(0f, -speedVal);
        }

        debugText.text = nextDir + " (" + playerStopped + ") > " + curDir + " ( " + oldDir + " )\n" +
            "SX: " + HitWall(-1, 0) +" DX: "+ HitWall(1, 0) +"\nUP: "+ HitWall(0,-1) +" DN: "+ HitWall(0,1);

        oldDir = curDir;

    }
}
