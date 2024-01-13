using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaypointController : MonoBehaviour
{

    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = getNumGivenString(gameObject.transform.Find("Quad").transform.Find("Text").transform.Find("TextMeshPro").GetComponent<TextMeshPro>().text);
    }

    public void onDelete()
    {

    }

    private int getNumGivenString(string letter)
    {
        int result = 0;
        int length = letter.Length;

        for (int i = 0; i < length; i++)
        {
            char c = letter[i];
            result = result * 26 + (c - 'A' + 1);
        }

        return result - 1;
    }
}
