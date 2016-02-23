using UnityEngine;
using System.Collections;

public class MazeSpawner : MonoBehaviour {


    const int HEIGHT = 20;
    const int WIDTH = 39;

    public static int[][] tiles = new int[HEIGHT][];

    public GameObject IceTile;
    public GameObject GroundTile;
    public GameObject WallTile;
	// Use this for initialization
	void Start () {

        for (int i = 0; i < HEIGHT; i++)
        {
            tiles[i] = new int[WIDTH];
        }

        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                if(i == 0 || j == 0 || i == HEIGHT - 1 || j == WIDTH - 1)
                {
                    tiles[i][j] = 2;
                }
                else
                tiles[i][j] = 1;
            }
        }

        for (int i = 0; i < tiles.Length; i++)
        {

            for (int j = 0; j < tiles[i].Length; j++)
            {

                GameObject go = IceTile;
                switch (tiles[i][j])
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

                Instantiate(go,new Vector3(j * 1.5f - 19.5f, i * 1.5f - 7.5f,2),Quaternion.identity);
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
