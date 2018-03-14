using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {
    public int mapWidth = 1;
    public int mapHeight = 1;
    //private float xCellScale = 0.1f;
    //private float yCellScale = 0.1f;

    public Tilemap map;

    public Tile[] mapTiles;                           // An array of floor tile prefabs.

    private TileType[][] tiles;
    public int count = 0;
    private int state;
    private Vector2 inputVector;


    // Use this for initialization
    void Start () {
        Grid grid = gameObject.GetComponentInParent<Grid>();
        PolygonCollider2D pol = GameObject.FindGameObjectWithTag("BgSky").GetComponent<PolygonCollider2D>();
        mapWidth = (int)(pol.bounds.max.x * 2f / grid.cellSize.x);
        mapHeight = mapWidth * 2;
        //mapWidth = 5;
        //mapHeight = 5;
        print("width " + mapWidth);
        print("height " + mapHeight);
        //SetupTilesArray();

        //CreateRoomsAndCorridors();

        //SetTilesValuesForRooms();
        //SetTilesValuesForCorridors();

        InstantiateTiles();
        state = 0;
        //InstantiateOuterWalls();
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyUp(KeyCode.Keypad0) ){

        }

	}

    // The type of tile that will be laid in a specific position.
    public enum TileType
    {
        Dirt, Background, Sky
    }


    //void SetupTilesArray()
    //{
    //    tiles = new TileType[mapHeight][];

    //    for (int i = 0; i < tiles.Length; i++)
    //    {
    //        tiles[i] = new TileType[mapWidth];
    //    }
    //}

    TileType SetTileType(int depth)
    {
        if(depth < 50)
        {
            return TileType.Sky;
        }
        else
        {
            return TileType.Dirt;
            count++;
        }
    }

    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                // ... and instantiate a floor tile for it.
                //print(i * xCellScale);
                //print(j * yCellScale);
                
                InstantiateFromArray(mapTiles, j, -i);

                //// If the tile type is Wall...
                //if (tiles[i][j] == TileType.Wall)
                //{
                //    // ... instantiate a wall over the top. 
                //    InstantiateFromArray(wallTiles, i, j);
                //}
            }
        }
    }




    void InstantiateFromArray(Tile[] prefabs, int xCoord, int yCoord)
    {
        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);
        // The position to be instantiated at is based on the coordinates.
        Vector3Int position = new Vector3Int(xCoord, yCoord, 0);
        // Create an instance of the prefab from the random index of the array.

        //GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        TileType tile = SetTileType(Mathf.Abs( yCoord));
        if (tile == TileType.Dirt)
            map.SetTile(position, prefabs[0]);
        if (tile == TileType.Background)
            map.SetTile(position, prefabs[2]);
        if (tile == TileType.Sky)
            return;// map.SetTile(position, prefabs[1]);
        //print("hello" + tileInstance.transform.position);
    }
}



