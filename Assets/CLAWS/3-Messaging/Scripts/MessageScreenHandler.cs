using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class MessageReceiveHandler : MonoBehaviour
{

    GameObject parent;
    GameObject chatScreen;
    GameObject AstroScreen;
    GameObject LMCCScreen;
    GameObject GroupChatScreen;
    GameObject emojiParent;
    GameObject emojiScreen;

    [SerializeField] private GameObject textBox;

    TMP_InputField message;

    private int groupChat;

    FellowAstronaut fa;
    Messaging msgList;

    List<Message> allMessage;
    List<Message> AstroChat;
    List<Message> LMCCChat;
    List<Message> GroupChat;

    private int astroCounter = 0;
    private int LMCCCounter = 0;
    private int groupChatCounter = 0;

    private WebsocketDataHandler websocket;

    void Start()
    {
        parent = transform.parent.Find("MessagingScreen").gameObject;
        AstroScreen = parent.transform.Find("AstroScroll").gameObject;
        LMCCScreen = parent.transform.Find("LMCCScroll").gameObject;
        GroupChatScreen = parent.transform.Find("GroupChatScroll").gameObject;

        emojiParent = transform.parent.Find("Messaging").gameObject;
        emojiScreen = emojiParent.transform.Find("emojiScreen").gameObject;

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

        AstroScreen.SetActive(true);
        emojiScreen.SetActive(false);

        StartCoroutine(GenerateAstroBox());
        StartCoroutine(GenerateLMCCBox());
        StartCoroutine(GenerateGroupChatBox());
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
        groupChat = 0;         // temporary set groupchat to -2 for testing
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
            //else if (groupChat == fa.astronaut_id)
            else if (groupChat == 0)
            {
                AstronautInstance.User.MessagingData.AllMessages.Add(m);
                websocket.SendMessageData();
                AstroChat.Add(m);
            }

            message.text = "";
        }
    }

    // Display text boxes
    void generateBox(List<Message> chat, GameObject screen, int size)
    {
        for (int i = size; i < chat.Count(); i++)
        {
            GameObject box = Instantiate(textBox, screen.transform);
            TMP_Text textComponent = box.GetComponentInChildren<TMP_Text>();
            textComponent.text = chat[i].message; Vector3 newPosition = box.transform.position;
            newPosition.y -= 0.02f * i + 0.05f;
            box.transform.position = newPosition;
            if (chat[i].from == AstronautInstance.User.id)
            {
                Vector3 chatPosition = box.transform.position;
                chatPosition.x += 0.07f;
                box.transform.position = chatPosition;
            }
            else
            {
                Vector3 chatPosition = box.transform.position;
                chatPosition.x -= 0.07f;
                box.transform.position = chatPosition;
            }
        }
    }

    IEnumerator GenerateAstroBox()
    {
        Debug.Log(astroCounter);
        while (true)
        {
            if (astroCounter < AstroChat.Count())
            {
                generateBox(AstroChat, AstroScreen, astroCounter);
                astroCounter = AstroChat.Count();
            }
            yield return null;
        }
        
    }

    IEnumerator GenerateLMCCBox()
    {
        while (true)
        {
            if (LMCCCounter < LMCCChat.Count())
            {
                generateBox(LMCCChat, LMCCScreen, LMCCCounter);
                LMCCCounter = LMCCChat.Count();
            }
            yield return null;
        }
    }

    IEnumerator GenerateGroupChatBox()
    {
        while (true)
        {
            if (groupChatCounter < GroupChat.Count())
            {
                generateBox(GroupChat, GroupChatScreen, groupChatCounter);
                groupChatCounter = GroupChat.Count();
            }
            yield return null;
        }
    }


    // Call these functions on button clicks

    public void displayAstroMessage()
    {
        AstroScreen.SetActive(true);
        LMCCScreen.SetActive(false);
        GroupChatScreen.SetActive(false);
        groupChat = fa.astronaut_id;
    }

    public void displaLMCCMessage()
    {
        AstroScreen.SetActive(false);
        LMCCScreen.SetActive(true);
        GroupChatScreen.SetActive(false);
        groupChat = -1;
    }

    public void displayGroupMessage()
    {
        AstroScreen.SetActive(false);
        LMCCScreen.SetActive(false);
        GroupChatScreen.SetActive(true);
        groupChat = -2;
    }

    public void ClickConfirmed()
    {
        message.text = "Confirmed";
    }

    public void ClickRejected()
    {
        message.text = "Rejected";
    }

    public void ClickReady()
    {
        message.text = "Ready";
    }

    public void ClickWarning()
    {
        message.text = "Warning";
    }

    public void CloseEmoji()
    {
        emojiScreen.SetActive(false);
    }
}
