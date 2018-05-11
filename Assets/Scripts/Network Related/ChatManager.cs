using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameSparks.Api.Messages.ScriptMessage.Listener = message =>
        {

            if (!message.HasErrors)
            {
                Debug.Log(message.BaseData.JSON);
            }
			else
			Debug.Log(message.Errors);

        };
    }

    // Update is called once per frame
}
