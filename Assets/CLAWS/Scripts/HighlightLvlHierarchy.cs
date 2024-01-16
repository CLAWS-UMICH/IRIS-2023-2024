using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class HighlightLvlHierarchy : MonoBehaviour
{
    [Tooltip("Add all level names which will be unhighlighted when this button is clicked. Add 'all' to unhighlight all current highlighted buttons")]
    public List<string> levelsToCloseWhenClicked = new List<string>();

    public void OnClick()
    {
        Debug.Log(levelsToCloseWhenClicked);
        EventBus.Publish(new UnHighlight(levelsToCloseWhenClicked));
    }

}


[CustomEditor(typeof(HighlightLvlHierarchy))]
public class HighlightLvlHierarchyEditor : Editor
{
    private SerializedProperty levelsProp;

    private void OnEnable()
    {
        levelsProp = serializedObject.FindProperty("levelsToCloseWhenClicked");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Display the levels in the inspector
        EditorGUILayout.PropertyField(levelsProp, true);

        // Apply changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}