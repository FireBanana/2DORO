using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    Queue<GameObject> messageQueue = new Queue<GameObject>();
    public GameObject chatTemplate;
    public GameObject chatContainer;
    public PlayerAuthenticator playerAuth;
    public InputField inpF;
    public int maxMessages = 6;
    string writtenMessage;
    [HideInInspector]
    public string username;

    public void updateMessage(string mssg)
    {
        writtenMessage = mssg;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (writtenMessage == null)
                return;
            playerAuth.sendMessageToAll(username, writtenMessage);
            inpF.Select();
            inpF.text = "";
            writtenMessage = null;
        }
    }

    public void addChatMessage(string name, string message)
    {

        if (string.Equals(message, ""))
            return;

        var newMessage = Instantiate(chatTemplate);
        newMessage.SetActive(true);
        newMessage.transform.SetParent(chatContainer.transform, worldPositionStays: false);
        newMessage.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        newMessage.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0);
        if(name == "Server")
            newMessage.GetComponent<Text>().color = Color.blue;

        foreach (GameObject g in messageQueue)
        {
            g.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 20);
        }

        messageQueue.Enqueue(newMessage);

        newMessage.GetComponent<Text>().text = name + ": " + message;

        if (messageQueue.Count >= maxMessages)
        {
            var lastMessage = messageQueue.Dequeue();
            Destroy(lastMessage);
        }

    }
}
