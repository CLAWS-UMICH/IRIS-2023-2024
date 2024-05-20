using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

public class SampleCommandHandler : MonoBehaviour
{
    [SerializeField] GameObject ScreenParent;
    GameObject commandScreen;
    [SerializeField] private GameObject command;

    int counter = 0;

    void Start()
    {
        commandScreen = ScreenParent.transform.Find("CommandScreen").gameObject;
    }

    void setCommands(List<string> commands)
    {
        foreach (string c in commands)
        {
            GameObject textObject = Instantiate(command, commandScreen.transform);
            TMP_Text textComponent = textObject.GetComponentInChildren<TMP_Text>();
            textComponent.text = c;
            Vector3 newPosition = textObject.transform.position;
            newPosition.y -= 0.02f * counter + 0.05f;
            textObject.transform.position = newPosition;
        }
        Transform quadTransform = commandScreen.transform.Find("Quad");
        Vector3 currentScale = quadTransform.localScale;
        currentScale.y = 0.02f * commands.Count;
        quadTransform.localScale = currentScale;
    }

}
