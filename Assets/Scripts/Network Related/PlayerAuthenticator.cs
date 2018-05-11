using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerAuthenticator : MonoBehaviour
{

    bool gameSparksAvailable;
    string username, password;
    string matchID = null;
    void Start()
    {
        Application.runInBackground = true;
        GameSparks.Core.GS.GameSparksAvailable = available =>
        {

            if (available)
            {
                gameSparksAvailable = true;
                Debug.Log("Servers are available");
            }

        };

    }

    // Update is called once per frame
    public void changeUsername(string name)
    {
        username = name;
    }
    public void changePassword(string pass)
    {
        password = pass;
    }

    public void createNewPlayer()
    {
        if (gameSparksAvailable)
        {
            Debug.Log("Registering...");
            new RegistrationRequest()
            .SetUserName(username)
            .SetDisplayName(username)
            .SetPassword(password)
            .Send(response =>
            {

                if (response.HasErrors)
                {
                    Debug.LogError(response.Errors.JSON);
                }
                else
                {
                    Debug.Log("Registered");
                }

            });
        }
    }

    public void authorizePlayer()
    {
        if (gameSparksAvailable == true)
        {
            new AuthenticationRequest()
            .SetUserName(username)
            .SetPassword(password)
            .Send(response =>
            {
                if (response.HasErrors)
                {
                    Debug.LogError(response.Errors.JSON);
                }
                else
                {
                    joinLobby();
                    //SceneManager.LoadScene("FightingRoom");


                }
            });
        }
        else
        {
            Debug.Log("Not connected to gamesparks");
        }
    }

    public void joinLobby()
    {
        new GameSparks.Api.Requests.MatchmakingRequest()
        .SetMatchShortCode("LOB")
        .SetSkill(0)
        .Send(response =>
        {

            if (!response.HasErrors)
            {
                Debug.Log("Matchmaking request succedful");
                matchUpdatedListener();
                startChatListener();
            }
            else
                Debug.LogError(response.Errors.JSON);
        });
    }

    void matchUpdatedListener()
    {
        GameSparks.Api.Messages.MatchUpdatedMessage.Listener = message =>
        {
            matchID = message.MatchId;
        };
    }

    void startChatListener()
    {
        GameSparks.Api.Messages.ScriptMessage.Listener = message =>
        {

            if (!message.HasErrors)
            {
                Debug.Log(message.Data.GetString("Message"));
            }
            else
                Debug.Log(message.Errors);

        };
    }

    public void sendMessageToAll()
    {
        if (matchID == null)
            return;

        new GameSparks.Api.Requests.LogEventRequest()
         .SetEventKey("Chat_ToAll")
         .SetEventAttribute("Message", "lolgay")
         .SetEventAttribute("MatchID", matchID)
         .Send(response =>
         {
             Debug.LogError("Message sending unsuccesful");
         });
    }

}
