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
    TextMeshPro commandsText;
    GameObject backplate;
    [SerializeField] private GameObject command;

    List<string> testString;
    List<string> testString2;

    int counter = 0;

    void Start()
    {
        Debug.Log("wtfffff");
        backplate = ScreenParent.transform.Find("BackPlate").gameObject;
        commandsText = ScreenParent.transform.Find("commandTexts").gameObject.GetComponent<TextMeshPro>();

        testString = new List<string>
        {
            "First String",
            "Second String",
            "Third String",
            "Fourth String",
            "Fifth String"
        };
        testString2 = new List<string>
        {
            "lets",
            "fucking",
            "go!!!!"
        };

        Debug.Log("sample command script is called");
        setCommands(testString2);
    }

    void setCommands(List<string> commands)
    {
        foreach (string c in commands)
        {
            Debug.Log(c);
            commandsText.text += c + "\n";
        }
        Transform quadTransform = backplate.transform.Find("Quad");
        Vector3 currentScale = quadTransform.localScale;
        currentScale.y = 0.0075f * commands.Count;
        quadTransform.localScale = currentScale;

        Vector3 currentPosition = backplate.transform.localPosition;
        currentPosition.y -= currentScale.y / 4;
        backplate.transform.localPosition = currentPosition;
    }

}
