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

    [SerializeField] ScreenType currentScreen;

    FellowAstronaut fa;
    Messaging msgList;

    List<Message> allMessage;
    List<Message> AstroChat;
    List<Message> LMCCChat;
    List<Message> GroupChat;

    void Start()
    {
        parent = transform.parent.Find("MessagingScreen").gameObject;
        AstroScreen = parent.transform.Find("AstroScroll").gameObject;
        LMCCScreen = parent.transform.Find("LMCCScroll").gameObject;
        GroupChatScreen = parent.transform.Find("GroupChatScroll").gameObject;
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

    // Call these functions on button clicks

    public void displayAstroMessage()
    {
        AstroScreen.SetActive(true);
        LMCCScreen.SetActive(false);
        GroupChatScreen.SetActive(false);
    }

    public void displaLMCCMessage()
    {
        AstroScreen.SetActive(false);
        LMCCScreen.SetActive(true);
        GroupChatScreen.SetActive(false);
    }

    public void displayGroupMessage()
    {
        AstroScreen.SetActive(false);
        LMCCScreen.SetActive(false);
        GroupChatScreen.SetActive(true);
    }
}
