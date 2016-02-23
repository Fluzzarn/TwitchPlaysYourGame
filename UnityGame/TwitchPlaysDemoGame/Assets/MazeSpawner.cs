using UnityEngine;
using System.Collections;

public class MazeSpawner : MonoBehaviour {


    public const int HEIGHT = 14;
    public const int WIDTH = 16;

    const int PLAYER_ROW = 14;
    const int PLAYER_COL = 13;


    public static int[,] tiles = new int[HEIGHT, WIDTH]
        {
            { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2 },
            { 2,0,0,0,0,0,2,0,0,0,0,0,0,0,0,2 },
            { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
            { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
            { 2,0,0,0,0,0,0,0,0,0,0,2,0,0,0,2 },
            { 2,2,0,0,0,0,0,0,0,0,0,0,0,0,2,2 },
            { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
            { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 2,0,0,0,0,0,0,0,0,0,2,0,0,0,0,1 },
            { 2,0,0,2,0,0,0,0,0,0,0,0,0,0,0,2 },
            { 2,0,0,0,0,2,0,0,0,0,0,0,0,0,2,2 },
            { 2,0,0,0,0,0,0,2,0,0,0,0,0,0,0,2 },
            { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2 },
            { 2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2 }




        };
    public static GameObject[][] TileObjects = new GameObject[HEIGHT][];


    public GameObject IceTile;
    public GameObject GroundTile;
    public GameObject WallTile;
    public GameObject Player;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < HEIGHT; i++)
        {
            TileObjects[i] = new GameObject[WIDTH];
        }



        for (int i = 0; i < tiles.GetLength(0); i++)
        {

            for (int j = 0; j < tiles.GetLength(1); j++)
            {

                GameObject go = IceTile;
                switch (tiles[i,j])
                {
                    case 0:
                        go = IceTile;
                        break;
                    case 1:
                        go = GroundTile;
                        break;
                    case 2:
                        go = WallTile;
                        break;
                }



                var a =(GameObject)Instantiate(go,new Vector3(j * 1.5f , i * 1.5f,2),Quaternion.identity);
                TileObjects[i][j] = a;
                if(i == PLAYER_COL && j == PLAYER_ROW)
                {
                    var player = (GameObject)Instantiate(Player, new Vector3(j * 1.5f, i * 1.5f, 1), Quaternion.identity);
                    player.GetComponent<PlayerScript>().Row = j;
                    player.GetComponent<PlayerScript>().Col = i;
                    GetComponent<TwitchPlays>().Player = player;
                    GetComponent<TwitchPlays>().targetPos = player.transform.position;
                }

            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
