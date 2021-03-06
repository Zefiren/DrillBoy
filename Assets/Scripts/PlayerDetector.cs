﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour {
    public GameObject Player;
    private ProjectileDragging playerDragScript;
	// Use this for initialization
	void Start () {
        playerDragScript = Player.GetComponent<ProjectileDragging>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerDragScript.freeFly)
        {
            print("changing");
            changeToNormal();
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;


        }
        //else
        //    gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        //if (playerDragScript.collidingInDrag)

    }


    //void FixedUpdate()
    //{

    //}
    void changeToNormal()
    {
        CompositeCollider2D cd = GameObject.FindGameObjectWithTag("Ground").GetComponent<CompositeCollider2D>();
        //print(Player.GetComponent<CircleCollider2D>().IsTouchingLayers(LayerMask.NameToLayer("Normal")));
        if (!Player.GetComponent<CircleCollider2D>().IsTouchingLayers(LayerMask.NameToLayer("Normal"))) {
            Player.layer = LayerMask.NameToLayer("Normal");
            playerDragScript.collidingInDrag = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //print(collision.gameObject.name);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("HEY THERE");
        playerDragScript.collidingInDrag = true;
    }
}
