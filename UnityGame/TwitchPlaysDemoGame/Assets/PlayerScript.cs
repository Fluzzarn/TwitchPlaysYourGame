using UnityEngine;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour {


    public enum Direction
    {
        Stopped,
        Up = 1,
        Down,
        Left,
        Right
    }

    public Direction Dir = Direction.Stopped;

    private Vector3 targetPos = Vector3.zero;

    public int Row;
    public int Col;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {




        switch (Dir)
        {
            case Direction.Stopped:
                targetPos = Vector3.zero * 1.5f;
                break;
            case Direction.Up:
                targetPos = Vector3.up * 1.5f;
                break;
            case Direction.Down:
                targetPos = Vector3.down * 1.5f;
                break;
            case Direction.Left:
                targetPos = Vector3.left * 1.5f;
                break;
            case Direction.Right:
                targetPos = Vector3.right * 1.5f;
                break;
            default:
                break;
        }

        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2);
	}

    internal void SetRowAndCol(int v, int col)
    {
        Row = v; Col = col;
    }
}
