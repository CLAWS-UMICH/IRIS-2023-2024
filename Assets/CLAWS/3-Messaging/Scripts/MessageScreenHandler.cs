using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageReceiveHandler : MonoBehaviour
{

    GameObject parent;
    GameObject textBubble;
    GameObject AstroScreen;
    GameObject LMCCScreen;
    GameObject GroupChatScreen;

    FellowAstronaut fa;
    Messaging msgList;

    List<Message> allMessage;
    List<Message> AstroChat;
    List<Message> LMCCChat;
    List<Message> GroupChat;

    void Start()
    {
        allMessage = msgList.AllMessages;
        EventBus.Subscribe<MessagesAddedEvent>(appendList);

        
    }

    void appendList(MessagesAddedEvent e)
    {
        allMessage = e.NewAddedMessages;
        foreach (Message m in allMessage)
        {
            if (m.sent_to == -2 && m.from != AstronautInstance.User.id)
            {
                GroupChat.Add(m);
            }
            else if (m.from == -1 && m.sent_to == AstronautInstance.User.id)
            {
                LMCCChat.Add(m);
            }
            else if (m.sent_to == AstronautInstance.User.id)
            {
                AstroChat.Add(m);
            }
        }
    }

    void displayAstroMessage()
    {

    }

    void displaLMCCMessage()
    {

    }

    void displayGroupMessage()
    {

    }
}
