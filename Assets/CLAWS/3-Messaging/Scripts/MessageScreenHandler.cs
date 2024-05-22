using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class MessageReceiveHandler : MonoBehaviour
{
    // 22 characters per line

    GameObject chatScreen;
    GameObject AstroScreen;
    GameObject LMCCScreen;
    GameObject GroupChatScreen;

    [SerializeField] GameObject emojiParent;
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
    List<GameObject> boxes;

    GameObject messScreen;

    private int astroCounter = 0;
    private int LMCCCounter = 0;
    private int groupChatCounter = 0;

    private WebsocketDataHandler websocket;

    void Start()
    {
        messScreen = transform.parent.Find("MessagingScreen").gameObject;
        AstroScreen = messScreen.transform.Find("AstroScroll").gameObject;
        LMCCScreen = messScreen.transform.Find("LMCCScroll").gameObject;
        GroupChatScreen = messScreen.transform.Find("GroupChatScroll").gameObject;

        emojiScreen = emojiParent.transform.Find("emojiScreen").gameObject;

        GameObject textFieldObject = messScreen.transform.Find("TextField").gameObject;
        message = textFieldObject.transform.Find("InputField (TMP)").GetComponent<TMP_InputField>();

        msgList = new Messaging();
        allMessage = msgList.AllMessages;
        AstroChat = new List<Message>();
        LMCCChat = new List<Message>();
        GroupChat = new List<Message>();

        fa = AstronautInstance.User.FellowAstronautsData;

        websocket = GameObject.Find("Controller").GetComponent<WebsocketDataHandler>();

        EventBus.Subscribe<MessagesAddedEvent>(appendList);

        AstroScreen.SetActive(true);
        emojiScreen.SetActive(false);

        // Update chat names
        GameObject astroLabel = messScreen.transform.Find("AstroButton").Find("Button").Find("IconAndText").Find("TextMeshPro").gameObject;
        GameObject groupLabel = messScreen.transform.Find("GroupChatButton").Find("Button").Find("IconAndText").Find("TextMeshPro").gameObject;
        if (AstronautInstance.User.id == 0)
        {
            astroLabel.GetComponent<TMP_Text>().text = "Astronaut 2";
            groupLabel.GetComponent<TMP_Text>().text = "LMCC \n & \n Astronaut 2";
        }
        else if (AstronautInstance.User.id == 1)
        {
            astroLabel.GetComponent<TMP_Text>().text = "Astronaut 1";
            groupLabel.GetComponent<TMP_Text>().text = "LMCC \n & \n Astronaut 1";
        }

        StartCoroutine(GenerateAstroBox());
        StartCoroutine(GenerateLMCCBox());
        StartCoroutine(GenerateGroupChatBox());

        Close();
    }

    public void Close()
    {
        AstroScreen.SetActive(true);
        emojiScreen.SetActive(false);
        messScreen.SetActive(false);
        EventBus.Publish<ScreenChangedEvent>(new(Screens.Menu));
    }

    public void Open()
    {
        AstroScreen.SetActive(true);
        emojiScreen.SetActive(false);
        messScreen.SetActive(true);
        EventBus.Publish<ScreenChangedEvent>(new(Screens.Messaging_Astro_BlankMessage));
    }

    //private void OnDestroy()
    //{
    //    foreach (GameObject b in boxes)
    //    {
    //        Destroy(b);
    //    }
    //}

    void appendList(MessagesAddedEvent e)
    {
        allMessage = e.NewAddedMessages;

       foreach (Message m in allMessage)
       {
            if (m.sent_to == -2 && m.from != AstronautInstance.User.id)
            {
                GroupChat.Add(m);
            }
            else if (m.from == -1)// && m.sent_to == AstronautInstance.User.id)
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
        if (AstroScreen.activeSelf)
        {
            groupChat = fa.astronaut_id;
        }
        else if (LMCCScreen.activeSelf)
        {
            groupChat = -1;
        }
        else if (GroupChatScreen.activeSelf)
        {
            groupChat = -2;
        }
        // Exit if message field is empty
        if (!string.Equals(message.text, ""))
        {
            Message m = new Message(groupChat, message.text, AstronautInstance.User.id);
            Debug.Log("sent to: " + m.sent_to + " from: " + m.from);
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
                Debug.Log("message sent to socket");
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
            textComponent.text = chat[i].message;

            Debug.Log("text box created");

            (string formattedText, int newLineCount) = InsertNewLines(chat[i].message, 20);
            textComponent.text = formattedText;

            GameObject boxBackplate = box.transform.Find("TitleBar").gameObject.transform.Find("BackPlate").gameObject;
            Vector3 newScale = boxBackplate.transform.localScale;
            Debug.Log("box backplate scale: " + newScale);
            newScale.y = newLineCount * 0.35f + 0.05f;
            Debug.Log("new box backplate scale: " + newScale + " new line count: " + newLineCount);
            boxBackplate.transform.localScale = newScale;

            Vector3 newPosition = box.transform.position;
            if (newLineCount == 1 || chat.Count == 1)
            {
                newPosition.y -= 0.02f * i + 0.05f;
            }
            else
            {
                newPosition.y -= 0.02f * i + 0.05f + (0.01f * newLineCount / 2);
            }

            //newPosition.y -= 0.02f * i + 0.05f;

            box.transform.position = newPosition;
            if (chat[i].from == AstronautInstance.User.id)
            {
                Debug.Log("chat from: " + chat[i].from + " chat im sending to: " + chat[i].sent_to);
                Vector3 chatPosition = box.transform.position;
                chatPosition.x += 0.03f;
                box.transform.position = chatPosition;
            }
            else
            {
                Debug.Log("chat from: " + chat[i].from + " chat they are sending to: " + chat[i].sent_to);
                Vector3 chatPosition = box.transform.position;
                chatPosition.x -= 0.0f;
                box.transform.position = chatPosition;
            }
        }
    }

    (string, int) InsertNewLines(string text, int maxLineLength)
    {
        if (string.IsNullOrEmpty(text))
        {
            return (text, 0);
        }

        List<string> lines = new List<string>();
        int newLineCount = 1;

        for (int i = 0; i < text.Length; i += maxLineLength)
        {
            if (i + maxLineLength < text.Length)
            {
                lines.Add(text.Substring(i, maxLineLength));
                newLineCount++;
            }
            else
            {
                lines.Add(text.Substring(i));
            }
        }

        return (string.Join("\n", lines), newLineCount);
    }

    IEnumerator GenerateAstroBox()
    {
        //Debug.Log(astroCounter);
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

    public void displayEmojiScreen()
    {
        emojiScreen.SetActive(true);
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
