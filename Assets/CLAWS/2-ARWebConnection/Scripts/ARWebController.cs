using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARWebController : MonoBehaviour
{
    [SerializeField] private float seconds = 5f;
    [SerializeField] private float duration = 1f;

    private GameObject MainMenuParent;
    private int currentActiveButton;
    private Subscription<LLMCHighlight> newHighlightEvent;

    private Dictionary<int, GameObject> buttonDict = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MainMenuParent = transform.parent.Find("MainMenu").Find("BarMenu").Find("MainMenu").gameObject;
        buttonDict[0] = MainMenuParent.transform.Find("TasksButton").gameObject;
        buttonDict[1] = MainMenuParent.transform.Find("NavigationButton").gameObject;
        buttonDict[2] = MainMenuParent.transform.Find("MessagesButton").gameObject;
        buttonDict[3] = MainMenuParent.transform.Find("SamplesButton").gameObject;
        buttonDict[4] = MainMenuParent.transform.Find("VitalsButton").gameObject;
        buttonDict[5] = MainMenuParent.transform.Find("ModesButton").gameObject;
        currentActiveButton = -1;
        newHighlightEvent = EventBus.Subscribe<LLMCHighlight>(onNewHighlight);
        //StartCoroutine(_testHighlight(0));
    }

    IEnumerator _testHighlight(int id)
    {
        yield return new WaitForSeconds(3);
        LLMCHighlight(id);
        yield return new WaitForSeconds(2);
        LLMCHighlight((id + 2) % 6);
    }

    public void onNewHighlight(LLMCHighlight e)
    {
        LLMCHighlight(e.button_id);
    }

    private void LLMCHighlight(int id)
    {
        if (currentActiveButton != -1)
        {
            // Unhighlight
            if (buttonDict.ContainsKey(currentActiveButton))
            {
                UnHighLight(currentActiveButton);
            }
        }

        // Highlight
        if (buttonDict.ContainsKey(id))
        {
            buttonDict[id].transform.Find("Button").Find("LLMCBackplate").gameObject.SetActive(true);
            currentActiveButton = id;
            StartCoroutine(_WaitToDehighlightButton());
        }

    }

    IEnumerator _WaitToDehighlightButton()
    {
        yield return new WaitForSeconds(seconds);

        if (currentActiveButton != -1)
        {
            // Unhighlight
            if (buttonDict.ContainsKey(currentActiveButton))
            {
                buttonDict[currentActiveButton].transform.Find("Button").Find("LLMCBackplate").gameObject.SetActive(false);
            }
        }

        currentActiveButton = -1;

    }

    private void UnHighLight(int id)
    {
        // Fade out
        GameObject backplate = buttonDict[id].transform.Find("Button").Find("LLMCBackplate").gameObject;
        GameObject quad = backplate.transform.Find("Quad").gameObject;
        Material oldMat = quad.GetComponent<MeshRenderer>().materials[0];
        Material mat = quad.GetComponent<MeshRenderer>().materials[0];

        // Store the initial alpha value
        float startAlpha = mat.color.a;

        // Set the material to be visible initially
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, startAlpha);

        StartCoroutine(_FadeColor(startAlpha, mat));

        backplate.SetActive(false);

        quad.GetComponent<MeshRenderer>().materials[0] = oldMat;
    }

    IEnumerator _FadeColor(float startAlpha, Material mat)
    {
        // Calculate the rate of change
        float rate = 1.0f;

        // Track the elapsed time
        float elapsed = 0f;

        while (elapsed < 10f)
        {
            // Gradually decrease the alpha value
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(startAlpha, 0f, elapsed));

            // Increment the elapsed time
            elapsed += rate * Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }
    }
}
