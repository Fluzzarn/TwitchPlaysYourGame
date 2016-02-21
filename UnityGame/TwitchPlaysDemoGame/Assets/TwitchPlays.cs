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
        TwitchPlaysYourGame.TwitchPlays.ChannelName = "fluzzarn";
        TwitchPlaysYourGame.TwitchPlays.NickName = "Fluzzarn";
        TwitchPlaysYourGame.TwitchPlays.ServerAddress = "irc.twitch.tv";
        TwitchPlaysYourGame.TwitchPlays.Password = "oauth:b3pyb0j54uhnkxx4yzflntc1g5ocfn";

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
        targetPos += Vector3.up;
        Player.GetComponent<Animator>().SetTrigger("Up");
        Player.GetComponent<Animator>().SetBool("IsWalking", true);
        Debug.Log("Up");
    }

    void MoveDown()
    {
        targetPos += Vector3.down;
        Player.GetComponent<Animator>().SetBool("IsWalking", true);
        Player.GetComponent<Animator>().SetTrigger("Down");

        Debug.Log("Down");
    }

    void MoveLeft()
    {
        targetPos += Vector3.left;
        Player.GetComponent<Animator>().SetBool("IsWalking", true);
        Player.GetComponent<Animator>().SetTrigger("Left");

        Debug.Log("Left");
    }

    void MoveRight()
    {
        targetPos += Vector3.right;
        Player.GetComponent<Animator>().SetBool("IsWalking", true);
        Player.GetComponent<Animator>().SetTrigger("Right");

        Debug.Log("Right");
    }

    void DoCommand()
    {
        Debug.Log("Command");
        string command = TwitchPlaysYourGame.TwitchPlays.GetMostCommonCommand();
        TwitchPlaysYourGame.TwitchPlays.ExecuteCommand(command);
        TwitchPlaysYourGame.TwitchPlays.ClearCommands();
        Invoke("DoCommand", 5);

    }

}
