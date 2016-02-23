using UnityEngine;
using System.Collections;
using TwitchPlaysYourGame;
using UnityEngine.UI;
public class TwitchPlays : MonoBehaviour
{


    public GameObject Player;
    public Vector3 targetPos;
    public InputField iField;

    // Use this for initialization
    void Start()
    {

        Application.runInBackground = true;
    }

    void Awake()
    {


        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("up", MoveUp);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("down", MoveDown);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("left", MoveLeft);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("right", MoveRight);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("reset", Reset);

        Invoke("DoCommand", 5);
    }

    public void Connect()
    {
        TwitchPlaysYourGame.TwitchPlays.ChannelName = "fluzzarn";
        TwitchPlaysYourGame.TwitchPlays.NickName = "Fluzzarn";
        TwitchPlaysYourGame.TwitchPlays.ServerAddress = "irc.twitch.tv";
        TwitchPlaysYourGame.TwitchPlays.Password = iField.text;
        iField.text = "";
        TwitchPlaysYourGame.TwitchPlays.Connect();
    }

    void OnApplicationQuit()
    {
        Debug.Log("QUITTING");
        TwitchPlaysYourGame.TwitchPlays.Disconnect();
    }

    // Update is called once per frame
    void Update()
    {

        Player.transform.position = Vector3.Lerp(Player.transform.position, targetPos, Time.deltaTime * 2);

        if (Vector3.Distance(Player.transform.position, targetPos) < .05)
        {
            Player.transform.position = targetPos;
            Player.GetComponent<Animator>().SetBool("IsWalking", false);
        }


        if(Input.GetKey(KeyCode.DownArrow))
        {
            MoveDown();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    void MoveUp()
    {
        var tile = GetTile(Player.GetComponent<PlayerScript>().Row, Player.GetComponent<PlayerScript>().Col + 1).GetComponent<TileScript>();
        Move("Up", 0, 1, tile);

        if (tile.Tile == TileScript.TileType.ICE)
        {
            MoveUp();
        }




    }

    void MoveDown()
    {
        var tile = GetTile(Player.GetComponent<PlayerScript>().Row , Player.GetComponent<PlayerScript>().Col - 1).GetComponent<TileScript>();
        Debug.Log(tile.Tile);
        Move("Down", 0, -1, tile);

        if (tile.Tile == TileScript.TileType.ICE)
        {
            MoveDown();
        }
    }

    void MoveLeft()
    {
        var tile = GetTile(Player.GetComponent<PlayerScript>().Row -1, Player.GetComponent<PlayerScript>().Col).GetComponent<TileScript>();
        Move("Left", -1, 0, tile);

        if (tile.Tile == TileScript.TileType.ICE)
        {
            MoveLeft();
        }
    }

    void MoveRight()
    {

        var tile = GetTile(Player.GetComponent<PlayerScript>().Row + 1, Player.GetComponent<PlayerScript>().Col).GetComponent<TileScript>();
        Move("Right", 1,0, tile);

        if (tile.Tile == TileScript.TileType.ICE)
        {
            MoveRight();
        }
    }



    void Move(string dir, int RowMod,int ColMod, TileScript tile)
    {
        if (!(tile.Tile == TileScript.TileType.WALL))
        {
            targetPos = tile.transform.position + new Vector3(0,0,-2);

            Player.GetComponent<PlayerScript>().Row = Player.GetComponent<PlayerScript>().Row + RowMod;
            Player.GetComponent<PlayerScript>().Col = Player.GetComponent<PlayerScript>().Col + ColMod;

            Player.GetComponent<Animator>().SetBool("IsWalking", true);
            Player.GetComponent<Animator>().SetTrigger(dir);

            Debug.Log(dir);

        }

    }
    void DoCommand()
    {
        Debug.Log("Command");
        string command = TwitchPlaysYourGame.TwitchPlays.GetMostCommonCommand();
        TwitchPlaysYourGame.TwitchPlays.ExecuteCommand(command);
        TwitchPlaysYourGame.TwitchPlays.ClearCommands();
        Invoke("DoCommand", 5);

    }
    GameObject GetTile(int row, int col)
    {
        
        if (row >= 0 && col >= 0)
        {
            if (col < MazeSpawner.HEIGHT && row <= MazeSpawner.WIDTH)
            {
                return MazeSpawner.TileObjects[col][row];
            }
        }

        return null;
    }


    void Reset()
    {
        Player.GetComponent<PlayerScript>().SetRowAndCol(MazeSpawner.PLAYER_ROW, MazeSpawner.PLAYER_COL);
        targetPos = GetTile(MazeSpawner.PLAYER_ROW, MazeSpawner.PLAYER_COL).transform.position + new Vector3(0,0,-2);
    }

}
