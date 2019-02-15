using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacCtrl : MonoBehaviour
{
    public Texture2D map;

    public GameObject[] wallsA;

    public GameObject mapCont;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width;x++){
                Color c = map.GetPixel(x, y);
                if (c.b==1f){
                    GameObject o = Instantiate(wallsA[1], mapCont.transform);
                    o.transform.localPosition = new Vector3(x, y, 1);
                }
            }
        }
           

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
