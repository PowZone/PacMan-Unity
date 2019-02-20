using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonusCtrl : MonoBehaviour
{
    public gameCtrl mainCtrl;

    public bool superBonus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Trigger " + collision.name);
        if (collision.name != "PacMan") return;
        mainCtrl.AddScore(superBonus);
        this.gameObject.SetActive(false);
    }
}
