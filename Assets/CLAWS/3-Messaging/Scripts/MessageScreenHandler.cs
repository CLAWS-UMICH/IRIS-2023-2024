using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class MessageReceiveHandler : MonoBehaviour
{

    GameObject parent;
    GameObject textBubble;
    GameObject AstroScreen;
    GameObject LMCCScreen;
    GameObject GroupChatScreen;

    GameObject textBox;

    TMP_InputField message;

    private int groupChat;

    FellowAstronaut fa;
    Messaging msgList;

    List<Message> allMessage;
    List<Message> AstroChat;
    List<Message> LMCCChat;
    List<Message> GroupChat;

    private WebsocketDataHandler websocket;

    void Start()
    {
        parent = transform.parent.Find("MessagingScreen").gameObject;
        AstroScreen = parent.transform.Find("AstroScroll").gameObject;
        LMCCScreen = parent.transform.Find("LMCCScroll").gameObject;

        GroupChatScreen = parent.transform.Find("GroupChatScroll").gameObject;
        GameObject textFieldObject = parent.transform.Find("TextField").gameObject;
        message = textFieldObject.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();

        msgList = new Messaging();
        allMessage = msgList.AllMessages;
        AstroChat = new List<Message>();
        LMCCChat = new List<Message>();
        GroupChat = new List<Message>();

        fa = AstronautInstance.User.FellowAstronautsData;

        websocket = transform.parent.parent.parent.Find("Controller").GetComponent<WebsocketDataHandler>();

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

    public void sendMessage()
    {
        Debug.Log(message);
        groupChat = -2;
        // Exit if message field is empty
        if (!string.Equals(message.text, ""))
        {
            Message m = new Message(groupChat, message.text, AstronautInstance.User.id);

            // Currently in group chat
            if (groupChat == -2)
            {
                AstronautInstance.User.MessagingData.AllMessages.Add(m);
                websocket.SendMessageData();
                GroupChat.Add(m);
            }
            // Currently in LMCC chat
            else if (groupChat == -1)
            {
                AstronautInstance.User.MessagingData.AllMessages.Add(m);
                websocket.SendMessageData();
                LMCCChat.Add(m);
            }
            // Currently in astronaut chat
            else if (groupChat == fa.astronaut_id)
            {
                AstronautInstance.User.MessagingData.AllMessages.Add(m);
                websocket.SendMessageData();
                AstroChat.Add(m);
            }

            message.text = "";
        }
    }

    // Display text boxes
    void generateBox(List<Message> chat, GameObject screen)
    {
        foreach (var c in chat)
        {
            GameObject box = Instantiate(textBox, screen.transform);
            TMP_Text textComponent = box.GetComponentInChildren<TMP_Text>();
            textComponent.text = c.message;
        }
    }


    // Call these functions on button clicks

    public void displayAstroMessage()
    {
        AstroScreen.SetActive(true);
        LMCCScreen.SetActive(false);
        GroupChatScreen.SetActive(false);
        groupChat = fa.astronaut_id;

        generateBox(AstroChat, AstroScreen);
    }

    public void displaLMCCMessage()
    {
        AstroScreen.SetActive(false);
        LMCCScreen.SetActive(true);
        GroupChatScreen.SetActive(false);
        groupChat = -1;

        generateBox(LMCCChat, LMCCScreen);
    }

    public void displayGroupMessage()
    {
        AstroScreen.SetActive(false);
        LMCCScreen.SetActive(false);
        GroupChatScreen.SetActive(true);
        groupChat = -2;

        generateBox(GroupChat, GroupChatScreen);
    }
}
