﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DigController : MonoBehaviour {
    public float maxDigPower=1000;
    public float currentDigPower;
    public float digSpeed = 10;
    public float digCost = 10;

    private Rigidbody2D playerRigidbody;
    public GameObject mapGO;
    private Tilemap map;

    private Vector2 diggingDirection;
    private Vector2 lastVelocity;
    private Vector2 preColVelocity;
    private bool collided=false;
    public bool firstCol = true;

    // Use this for initialization
    void Start () {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        currentDigPower = maxDigPower;
        if(mapGO!=null)
            map = mapGO.GetComponent<Tilemap>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.J))
        {
            currentDigPower = maxDigPower;
            playerRigidbody.position = playerRigidbody.position + new Vector2(0f, 5f);
            playerRigidbody.velocity = new Vector2(0f, 5f);
            digSpeed = 10;
        }
	}

    private void FixedUpdate()
    {
        if (firstCol)
        {
            lastVelocity = playerRigidbody.velocity;
            diggingDirection = playerRigidbody.velocity.normalized;
        } 
        if (collided && currentDigPower > 0f)
        {
            collided = false;
            playerRigidbody.position = playerRigidbody.position - diggingDirection ;
            GetComponent<Rigidbody2D>().velocity = diggingDirection * digSpeed;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 hitPos = Vector2.zero;
        if (map != null)
        {
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;
                map.SetTile(map.WorldToCell(hitPos), null);
            }
        }
        collided = true;
        //digSpeed = digSpeed / 2f;
        playerRigidbody.velocity = lastVelocity;
        firstCol = false;

        //print("new   vel" + playerRigidbody.velocity);
        currentDigPower -= digCost;
        if (currentDigPower == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
            GetComponent<ProjectileDragging>().resetVars();
        }
        //print("current" + currentDigPower);
    }
}
