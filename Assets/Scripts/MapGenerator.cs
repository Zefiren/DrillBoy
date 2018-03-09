using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public float mapWidth = 1;
    public float mapHeight = 1;
	// Use this for initialization
	void Start () {
        Grid grid = gameObject.GetComponentInParent<Grid>();
        PolygonCollider2D pol = GameObject.FindGameObjectWithTag("BgSky").GetComponent<PolygonCollider2D>();
        mapWidth = pol.bounds.max.x * 2f / grid.cellSize.x;
        mapHeight = mapWidth * 10;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
