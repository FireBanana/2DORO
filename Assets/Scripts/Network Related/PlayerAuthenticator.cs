using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using GameSparks.RT;
using DG.Tweening;
using GameSparks.Core;
using Org.BouncyCastle.Cms;
using Random = UnityEngine.Random;

public class PlayerAuthenticator : MonoBehaviour
{
    public static PlayerAuthenticator instance;

    bool gameSparksAvailable;
    string username, password;
    string matchID = null;
    private string userID; //For cloudcode player removal
    public ChatManager chatmanager;
    public GameObject dialogBox;
    public Text dialogText;
    public Fighter fighterScript;
    private AudioSource musicPlayer;
    public AudioClip inGameMusic;

    public string playerClass;
    public int rank;
    public int points;

    public Text menuNameText, menuRankText, menuPointsText, menuClassText;

    //Realtime Session Stuff
    private int port;
    private string accessToken;
    private string host;
    private GameSparksRTUnity RTClass;
    public bool connectedToSession;
    public GameObject enemyPrefab;
    Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

    //Current match details
    public string currentCreatorId;
    public int currentPlayerNumb;

    //character creation
    CharacterSelectionButton[] activeButtons = new CharacterSelectionButton[3];

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        SceneManager.activeSceneChanged += setListenersOnSceneChange;
        Application.runInBackground = true;
        GameSparks.Core.GS.GameSparksAvailable = available =>
        {

            if (available)
            {
                gameSparksAvailable = true;
                Debug.Log("Servers are available");
            }

        };

        musicPlayer = GetComponent<AudioSource>();
        startChatListener(); //DIRTY REMOVE CHAMBER BELOW
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

    int captchaNumb1, captchaNumb2;

    public void runCaptcha()
    {
        dialogBox.SetActive(true);
        captchaNumb1 = Random.Range(0, 9);
        captchaNumb2 = Random.Range(0, 9);
        string dialogTxt = "What is " + captchaNumb1.ToString() + " + " + captchaNumb2.ToString() + "?";
        dialogText.text = dialogTxt;
        dialogBox.GetComponent<RectTransform>().DOAnchorPosY(30, 1);
    }

    public void verifyCaptcha(int ans)
    {
        if (ans == (captchaNumb1 + captchaNumb2))
        {
            //createNewPlayer();
            characterCreationScreen.SetActive(true);
            dialogBox.GetComponent<RectTransform>().DOAnchorPosY(-100, 1);
        }
    }

    public void characterEdited(CharacterSelectionButton newButton, int row)
    {
        if (row == 1)
        {
            if (activeButtons[0] != null)
                activeButtons[0].changeToNonSelected();
            newButton.changeToSelected();
            activeButtons[0] = newButton;
        }
        else if (row == 2)
        {
            if (activeButtons[1] != null)
                activeButtons[1].changeToNonSelected();
            newButton.changeToSelected();
            activeButtons[1] = newButton;
        }
        else if (row == 3)
        {
            if (activeButtons[2] != null)
                activeButtons[2].changeToNonSelected();
            newButton.changeToSelected();
            activeButtons[2] = newButton;
        }
    }

    public GameObject characterCreationScreen;

    public void createNewPlayer()
    {

        playerClass = activeButtons[1].nameID;
        rank = 1;
        points = 0;


        var classData = new GameSparks.Core.GSRequestData().AddString("classData", playerClass);

        if (gameSparksAvailable)
        {
            Debug.Log("Registering...");
            new RegistrationRequest()
                .SetUserName(username)
                .SetDisplayName(username)
                .SetPassword(password)
                .SetScriptData(classData)
                .Send(response =>
                {

                    if (response.HasErrors)
                    {
                        Debug.LogError(response.Errors.JSON);
                        dialogBox.GetComponent<RectTransform>().DOAnchorPosY(-100, 1);
                    }
                    else
                    {
                        Debug.Log("Registered");
                        dialogBox.GetComponent<RectTransform>().DOAnchorPosY(-100, 1);
                        characterCreationScreen.SetActive(true);
                        userID = response.UserId;
                        SceneManager.LoadScene("Chamber");
                        musicPlayer.clip = inGameMusic;
                        musicPlayer.Play();
                    }

                });
        }
    }

    public void authorizePlayer()
    {
        if (gameSparksAvailable == true)
        {
            if (username == "Server" ||username ==  "server") //Add error message
                return;
            
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
                        // joinMatch();
                        playerClass = response.ScriptData.GetString("class");
                        rank = (int) response.ScriptData.GetNumber("rank");
                        points = (int) response.ScriptData.GetNumber("points");
                        userID = response.UserId;
                        SceneManager.LoadScene("Chamber");
                        musicPlayer.clip = inGameMusic;
                        musicPlayer.Play();
                    }
                });
        }
        else
        {
            Debug.Log("Not connected to gamesparks");
        }
    }

    public void joinMatch(string shortCode)
    {
        matchFoundListener();
        new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode(shortCode)
            .SetSkill(0)
            .Send(response =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Matchmaking request succedful");
                }
                else
                    Debug.LogError(response.Errors.JSON);
            });
    }
    

    #region BattleMatch
    public void createBattleMatch()
    {
        
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("CREATEMATCH")
            .SetEventAttribute("Creator" , username)
            .SetEventAttribute("Mode", "PVP")
            .SetEventAttribute("PlayerCount", 1)
            .Send(response =>
            {
                if(response.HasErrors)
                {
                    print("Error: " + response.Errors.JSON);
                    return;
                }
                
                var createScreenManager =
                    DataHolder.Instance.roomWaitingScreen.GetComponent<CreateBattleMenuManager>();
                createScreenManager.setNextAvailableName(username);

                currentCreatorId = userID;
                currentPlayerNumb = 1;

            });
    }

    public void searchBattleMatches()
    {

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("SEARCHMATCHES")
            .Send(response =>
            {
                if(response.HasErrors)
                {
                    print("Error: " + response.Errors.JSON);
                    return;
                }
                
                var serverBrowserManager = DataHolder.Instance.serverBrowserScreen.GetComponent<ServerBrowserManager>();

                serverBrowserManager.totalMatches = response.ScriptData.GetGSDataList("Matches");

                serverBrowserManager.initializeList();
                serverBrowserManager.populateInitialList();
            });
    }

    public void joinBattleMatch(string creatorID)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("JOINMATCH")
            .SetEventAttribute("CreatorId" , creatorID)
            .SetEventAttribute("Name", username)
            .Send(response =>
            {
                if (response.HasErrors)
                {
                    print("Error: " + response.Errors.JSON);
                    return;
                }

                currentCreatorId = response.ScriptData.GetGSData("responseData").GetString("CreatorId");
                currentPlayerNumb = (int)response.ScriptData.GetGSData("responseData").GetInt("PlayerCount");
                var matchDetails = response.ScriptData.GetGSData("responseData");
                DataHolder.Instance.roomWaitingScreen.SetActive(true);
                
                var createScreenManager =
                    DataHolder.Instance.roomWaitingScreen.GetComponent<CreateBattleMenuManager>();
                
                createScreenManager.cleanSlots();
                
                createScreenManager.setNextAvailableName(matchDetails.GetString("Creator"));

                for (int i = 2; i <= matchDetails.GetInt("PlayerCount"); i++)
                {
                    createScreenManager.setNextAvailableName(matchDetails.GetGSData("Players").GetString("Player" + i.ToString()));
                }
                
            });
    }
    
    public void leaveBattleMatch()
    {
        
        print(currentCreatorId + " " + currentPlayerNumb);
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("LEAVEMATCH")
            .SetEventAttribute("CreatorId" , currentCreatorId)
            .SetEventAttribute("PlayerNumb", currentPlayerNumb)
            .Send(response =>
            {
                if (response.HasErrors)
                {
                    print("Error: " + response.Errors.JSON);
                    return;
                }
                
                DataHolder.Instance.roomWaitingScreen.SetActive(false);

            });
    }

    public void initiateBattleMatch()
    {
        new LogEventRequest()
            .SetEventKey("STARTMATCH")
            .SetEventAttribute("CreatorId", currentCreatorId)
            .Send(response =>
            {
                if (response.HasErrors)
                {
                    print(response.Errors.JSON);
                    return;
                }
                SceneManager.LoadScene(2);
            });
    }

    public void startBattleMatch(string creatId)
    {
        print("seaerching");
        new MatchmakingRequest()
            .SetMatchShortCode("BATTLE")
            .SetMatchGroup(creatId)
            .SetSkill(0)
            .Send(response =>
            {
                if (response.HasErrors)
                {
                    print(response.Errors.JSON);
                    return;
                }
                fighterScript = DataHolder.Instance.fighter;
                fighterScript.isAllowedToFight = true;
                fighterScript.healthBar = BattleManager.Instance.assignHealthBar();
            });
    }
    #endregion
    
    public void matchFoundListener()
    {
        GameSparks.Api.Messages.MatchFoundMessage.Listener = message =>
        {
            if (!message.HasErrors)
            {
                matchID = message.MatchId;
                port = (int) message.Port;
                accessToken = message.AccessToken;
                host = message.Host;
                createSession();
            }
        };
    }

    public string previousScene;
    public void setListenersOnSceneChange(Scene old, Scene news)
    {
        if (RTClass != null)
        {
            RTClass.Disconnect();
            players.Clear();
            removePlayerFromMatch();
        }

        if (news.name == "FightingRoom")
        {
            startBattleMatch(currentCreatorId);
        }
        else if (news.name == "Chamber")
        {
            menuNameText = GameObject.Find("PlayerNameText").GetComponent<Text>();
            menuRankText = GameObject.Find("RankText").GetComponent<Text>();
            menuPointsText = GameObject.Find("PointsText").GetComponent<Text>();
            menuClassText = GameObject.Find("ClassText").GetComponent<Text>();
            menuNameText.text = " ";
            GameObject.Find("Inventory").SetActive(false);
            return;
        }
        else if (news.name == "Hallway")
        {
            joinMatch("LOB");
        }
        else if (news.name == "Armory")
        {
            joinMatch("ARM");
        }
        else if (news.name == "Lobby")
        {
            joinMatch("OPCENT");
        }
        else if (news.name == "TechLab")
            joinMatch("TECH");

        var elev = GameObject.Find("Elevator");

        if (elev != null)
        {
            if (previousScene != "Chamber")
            {
                print("found");
                var elevAnim = elev.GetComponent<Animator>();
                elevAnim.Play("Elevator_Close");
            }
        }
        else
        {
            print("not found");
        }
        

        menuNameText = GameObject.Find("PlayerNameText").GetComponent<Text>();
        menuRankText = GameObject.Find("RankText").GetComponent<Text>();
        menuPointsText = GameObject.Find("PointsText").GetComponent<Text>();
        menuClassText = GameObject.Find("ClassText").GetComponent<Text>();
        menuNameText.text = " ";
        GameObject.Find("Inventory").SetActive(false);


        if (chatmanager == null)
            chatmanager = GameObject.Find("ChatInput").GetComponent<ChatManager>();
        chatmanager.username = username;
        chatmanager.playerAuth = this;
        matchUpdatedListener();
        startChatListener();
    }

    void matchUpdatedListener()
    {
        GameSparks.Api.Messages.MatchUpdatedMessage.Listener = message =>
        {
            if (message.HasErrors)
            {
                print(message.Errors.JSON);
                return;
            }
            matchID = message.MatchId;
            
        };
    }

    void startChatListener()
    {
        GameSparks.Api.Messages.ScriptMessage.Listener = message =>
        {

            //   if (SceneManager.GetActiveScene().name == "Chamber") //VERY DIRTY
            //      return;
               
            if (!message.HasErrors)
            {
                if (message.ExtCode == "Chat")
                {
                    var mssg = message.Data.GetString("Message");
                    var dname = message.Data.GetString("displayName");
                    chatmanager.addChatMessage(dname, mssg);
                }
                else if (message.ExtCode == "joined")
                {
                    var playerName = message.Data.GetString("playerName");
                    chatmanager.addChatMessage("Server", playerName + " has joined the room.");
                }

                else if (message.ExtCode == "left")
                {
                    var playerName = message.Data.GetString("playerName");
                    chatmanager.addChatMessage("Server", playerName + " has left the room.");
                }
                else if (message.ExtCode == "PlayerJoinMatch")
                {
                    var nameToAdd = message.Data.GetString("Name");
                    var createScreenManager =
                        DataHolder.Instance.roomWaitingScreen.GetComponent<CreateBattleMenuManager>();
                    createScreenManager.setNextAvailableName(nameToAdd);
                }
                else if (message.ExtCode == "PlayerLeftMatch")
                {
                    DataHolder.Instance.roomWaitingScreen.GetComponent<CreateBattleMenuManager>().removeName((int)message.Data.GetInt("PlayerNumb"));
                    
                }
                else if (message.ExtCode == "StartMatch")
                {
                    print("Started match");
                    currentCreatorId = message.Data.GetString("CreatorId");
                    SceneManager.LoadScene(2);
                }
            }
            else
                Debug.Log(message.Errors);

        };
    }

    public void removePlayerFromMatch()
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("REMOVE")
            .SetEventAttribute("matchID", matchID)
            .SetEventAttribute("playerID", userID)
            .Send(response => { });
    }

    public void sendMessageToAll(string leName, string leMessage)
    {
        if (matchID == null)
            return;

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("Chat_ToAll")
            .SetEventAttribute("Message", leMessage)
            .SetEventAttribute("MatchID", matchID)
            .Send(response => { });
    }

    public void sendLoginNotification(string status)
    {
        if (matchID == null)
            return;

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("PlayerJoined")
            .SetEventAttribute("MatchId", matchID)
            .SetEventAttribute("Status", status)
            .Send(response =>
            {
                if(response.HasErrors)
                    print(response.Errors.JSON);
            });
    }

    public void setInventoryInfo()
    {
        var Lclass = playerClass;
        var Lrank = rank;
        var Lpoints = points;

        menuNameText.text = username;
        menuClassText.text = Lclass;
        menuRankText.text = Lrank.ToString();
        menuPointsText.text = Lpoints.ToString();
    }

    #region RealtimeMultiplayer

    public void createSession()
    {
        if (RTClass == null)
            RTClass = gameObject.AddComponent<GameSparksRTUnity>();
        RTClass.Configure(host, port, accessToken, OnPacket: pack => packetReceived(pack),
            OnPlayerConnect: pack => playerConnected(pack),
            OnPlayerDisconnect: pack => playerDisconnected(pack),
            OnReady: pack => playersReady(pack));
        RTClass.Connect();
        connectedToSession = true;
        fighterScript.StartCoroutine(fighterScript.positionSendToPacket());

    }

    public void playersReady(bool status)
    {
        foreach (var enemies in RTClass.ActivePeers)
        {
            print(enemies);
            if (enemies != RTClass.PeerId)
            {
                var newEnemy = Instantiate(enemyPrefab);
                newEnemy.GetComponent<Enemy>().id = enemies;
                players.Add(enemies, newEnemy);
            }
        }
        sendLoginNotification("joined");
    }

    public void playerConnected(int id)
    {
        var newEnemy = Instantiate(enemyPrefab);
        newEnemy.GetComponent<Enemy>().id = id;
        players.Add(id, newEnemy);

    }

    public void playerDisconnected(int id)
    {
        GameObject discPlayer;
        players.TryGetValue(id, out discPlayer);
        Destroy(discPlayer);
        players.Remove(id);

    }

    public bool pausePackets;
    public bool specialRunning;

    public void packetReceived(RTPacket pack)
    {
        
        GameObject enemyToChange;
        players.TryGetValue(pack.Sender, out enemyToChange);
        
        if (pack.OpCode == 100)
        {
            if (enemyToChange != null)
            {
                //Movement code
                if(!pausePackets)
                    enemyToChange.transform.DOMove((Vector2) pack.Data.GetVector2(1), Time.deltaTime * 10).SetEase(Ease.Linear);
       
                //Animation code
                if (!string.Equals(pack.Data.GetString(2), "null"))
                {
                    if(!specialRunning)
                       enemyToChange.GetComponent<Animator>().Play(pack.Data.GetString(2), 0);
                    
                    if (pack.Data.GetString(2) == "Enemy_special1")
                        specialRunning = true;
                }

                //Sprite flip code
                if (pack.Data.GetInt(3) == 0)
                {
                    if (enemyToChange.GetComponent<SpriteRenderer>().flipX)
                        enemyToChange.GetComponent<SpriteRenderer>().flipX = false;

                }
                else if (pack.Data.GetInt(3) == 1)
                {
                    if (!enemyToChange.GetComponent<SpriteRenderer>().flipX)
                        enemyToChange.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else
                print("Error: No enemy to change");
        }
            else if (pack.OpCode == 110)
        {
            enemyToChange.GetComponent<Enemy>().createHitEffect((Vector2)pack.Data.GetVector2(4));
            var hurtType = pack.Data.GetString(1);
            var dir = (Vector2)pack.Data.GetVector2(2);
            var damage = pack.Data.GetFloat(3);
            fighterScript.receiveAttack(Char.Parse(hurtType), dir, (float)damage);
        }
    }

    public void sendMovementPacket(Vector3 pos, string animName, int isFlipped)
    {
        using (RTData data = RTData.Get())
        {
            data.SetVector2(1, pos);
            data.SetString(2, animName);
            data.SetInt(3, isFlipped);
            RTClass.SendData(100, GameSparksRT.DeliveryIntent.UNRELIABLE_SEQUENCED, data);
        }
    }

    public void sendDamagePacket(char hurtType, Vector2 hitDir, float damage, Vector2 hitLoc)
    {
        using (RTData data = RTData.Get())
        {
            data.SetString(1, hurtType.ToString());
            data.SetVector2(2, hitDir);
            data.SetFloat(3, damage);
            data.SetVector2(4, hitLoc);
            RTClass.SendData(110, GameSparksRT.DeliveryIntent.UNRELIABLE_SEQUENCED, data);
        }
    }
    
    #endregion

}