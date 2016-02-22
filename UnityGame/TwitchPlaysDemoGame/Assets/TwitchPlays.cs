using UnityEngine;
using System.Collections;
using TwitchPlaysYourGame;

public class TwitchPlays : MonoBehaviour
{


    public GameObject Player;
    public Vector3 targetPos;


    // Use this for initialization
    void Start()
    {

        Application.runInBackground = true;
    }

    void Awake()
    {

        TwitchPlaysYourGame.TwitchPlays.ChannelName = "fluzzarn";
        TwitchPlaysYourGame.TwitchPlays.NickName = "Fluzzarn";
        TwitchPlaysYourGame.TwitchPlays.ServerAddress = "irc.twitch.tv";
        TwitchPlaysYourGame.TwitchPlays.Password = "oauth:b3pyb0j54uhnkxx4yzflntc1g5ocfn";

        TwitchPlaysYourGame.TwitchPlays.Disconnect();
        TwitchPlaysYourGame.TwitchPlays.Connect();
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("up", MoveUp);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("down", MoveDown);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("left", MoveLeft);
        TwitchPlaysYourGame.TwitchPlays.AddCommandToFunction("right", MoveRight);



        targetPos = Player.transform.position;
        Invoke("DoCommand", 5);
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

    }

    void MoveUp()
    {
        Debug.Log("Up");
        if (!(GetTile(Vector3.up * 1.5f + Player.transform.position) == TileScript.TileType.WALL))
        {
            targetPos += Vector3.up * 1.5f;
            Player.GetComponent<Animator>().SetTrigger("Up");
            Player.GetComponent<Animator>().SetBool("IsWalking", true);
           
        }
        
    }

    void MoveDown()
    {
        if (!(GetTile(Vector3.down + Player.transform.position) == TileScript.TileType.WALL))
        {
            targetPos += Vector3.down * 1.5f;
            Player.GetComponent<Animator>().SetBool("IsWalking", true);
            Player.GetComponent<Animator>().SetTrigger("Down");

            Debug.Log("Down");

        }
    }

    void MoveLeft()
    {
        if (!(GetTile(Vector3.left + Player.transform.position) == TileScript.TileType.WALL))
        {
            targetPos += Vector3.left * 1.5f;
            Player.GetComponent<Animator>().SetBool("IsWalking", true);
            Player.GetComponent<Animator>().SetTrigger("Left");

            Debug.Log("Left");

        }
    }

    void MoveRight()
    {
        if (!(GetTile(Vector3.right + Player.transform.position) == TileScript.TileType.WALL))
        {
            targetPos += Vector3.right * 1.5f;
            Player.GetComponent<Animator>().SetBool("IsWalking", true);
            Player.GetComponent<Animator>().SetTrigger("Right");

            Debug.Log("Right");

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
    TileScript.TileType GetTile(Vector2 Pos)
    {
        var hit = Physics2D.Raycast(Pos, Vector2.zero);

        if (hit)
        {
            GameObject go = hit.transform.gameObject;

            if (go.GetComponent<TileScript>())
            {
                var tileComp = go.GetComponent<TileScript>();

                return tileComp.Tile;
            }
        }

        return TileScript.TileType.INVALID;
    }


}
