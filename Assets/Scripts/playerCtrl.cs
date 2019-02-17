using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCtrl : MonoBehaviour
{
    public gameCtrl mainCtrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("Collision Enter");
        mainCtrl.stopPlayer();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Debug.Log("Collision Stay");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Debug.Log("Collision Exit");
    }

}
