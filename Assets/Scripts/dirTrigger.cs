using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirTrigger : MonoBehaviour
{
    public bool canMove = false;

    public bool debugMode = false;

    int triggerCnt = 0;

    SpriteRenderer sr;
    Color c1, c0;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        c1 = c0 = Color.white;
        c0.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerCnt++;
        if (debugMode) Debug.Log("Trigger Enter ["+ triggerCnt + "] "+collision.name);
        canMove = false;
        sr.color = c0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (debugMode) Debug.Log("Trigger Stay [" + triggerCnt + "] " + collision.name);
        canMove = false;
        sr.color = c0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerCnt--;
        if (debugMode) Debug.Log("Trigger Exit [" + triggerCnt + "] " + collision.name);
        canMove = (triggerCnt==0);
        sr.color = c1;
    }
    //*/
}
