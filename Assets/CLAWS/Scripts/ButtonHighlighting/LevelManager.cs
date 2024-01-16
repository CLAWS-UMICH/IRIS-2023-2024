using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public string levelName;
    public List<GameObject> gameObjects = new List<GameObject>();
}

public class LevelManager : MonoBehaviour
{
    private Subscription<HighlightButton> highlightButtonEvent;
    private Subscription<UnHighlight> unHighlightEvent;

    private Dictionary<GameObject, LevelData> gameObjectToLevelMap = new Dictionary<GameObject, LevelData>();
    private Dictionary<string, GameObject> levelToHighlight = new Dictionary<string, GameObject>();
    public List<LevelData> levels = new List<LevelData>();
    private List<GameObject> activeButtons = new List<GameObject>();

    private void Start()
    {
        highlightButtonEvent = EventBus.Subscribe<HighlightButton>(onNewButtonHighlight);
        unHighlightEvent = EventBus.Subscribe<UnHighlight>(onUnHighlight);
        initializeDictionary();
    }

    private void initializeDictionary()
    {
        foreach (LevelData levelData in levels)
        {
            foreach (GameObject gameObject in levelData.gameObjects)
            {
                gameObjectToLevelMap[gameObject] = levelData;
                levelToHighlight[levelData.levelName] = null;
            }
        }
    }

    public void onNewButtonHighlight(HighlightButton e)
    {
        if (gameObjectToLevelMap.ContainsKey(e.button))
        {
            unHighLightButton(e.button);
            highLightButton(e.button);
        }
    }

    public void onUnHighlight(UnHighlight e)
    {
        foreach (string lvlName in e.levelnames)
        {
            if (lvlName == "all")
            {
                UnHighlightAll();
                return;
            }
            else if (levelToHighlight.ContainsKey(lvlName) && levelToHighlight[lvlName] != null)
            {
                levelToHighlight[lvlName].GetComponent<ButtonHighlight>().unHighlight();
                GetRidOfActiveButton(levelToHighlight[lvlName]);
            }
        }
    }

    private void unHighLightButton(GameObject button)
    {
        if (gameObjectToLevelMap.ContainsKey(button))
        {
            LevelData levelData = gameObjectToLevelMap[button];
            if (levelToHighlight.ContainsKey(levelData.levelName))
            {
                if (levelToHighlight[levelData.levelName] != null)
                {
                    // Unhighlight button
                    levelToHighlight[levelData.levelName].GetComponent<ButtonHighlight>().unHighlight();
                    GetRidOfActiveButton(levelToHighlight[levelData.levelName]);
                }
            }
        }
    }

    private void highLightButton(GameObject button)
    {
        LevelData levelData = gameObjectToLevelMap[button];
        levelToHighlight[levelData.levelName] = button;

        // Highlight Button
        button.GetComponent<ButtonHighlight>().highLight();
        AddButtonToActive(button);

    }


    public void addButtonFromScroll(GameObject scrollObject, GameObject button)
    {
        if (gameObjectToLevelMap.ContainsKey(scrollObject))
        {
            LevelData levelData = gameObjectToLevelMap[scrollObject];
            gameObjectToLevelMap[button] = levelData;
            levelData.gameObjects.Add(button);
        }
    }

    public void clearButtonList(GameObject scrollObject)
    {
        if (gameObjectToLevelMap.ContainsKey(scrollObject))
        {
            LevelData levelData = gameObjectToLevelMap[scrollObject];
            levelData.gameObjects.Clear();
        }
    }

    private void AddButtonToActive(GameObject button)
    {
        activeButtons.Add(button);
    }

    private void GetRidOfActiveButton(GameObject button)
    {
        activeButtons.Remove(button);
    }

    public void UnHighlightAll()
    {
        foreach (GameObject button in activeButtons)
        {
            button.GetComponent<ButtonHighlight>().unHighlight();
        }
        activeButtons.Clear();
    }
}
