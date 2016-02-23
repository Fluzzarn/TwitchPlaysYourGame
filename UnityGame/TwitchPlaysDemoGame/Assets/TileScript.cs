using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {


    public Sprite TileSprite;

    public TileType Tile;

    public enum TileType
    {
        INVALID = -1,
        ICE = 0,
        GROUND,
        WALL
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
