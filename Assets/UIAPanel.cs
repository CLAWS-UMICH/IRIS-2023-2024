using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIAPanel : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public List<Transform> Cubes;

    public TextMeshPro CenterTMP;
    public TextMeshPro LowerTMP;
    public UIAProgressBar ProgressBar;

    float extents_x;
    float extents_y;

    Vector3 centerpos = new Vector3(0, 0, -0.62f);
    Vector3 lowerpos = new Vector3(0, -0.1706f, -0.62f);

    public List<UIA_Button> Buttons;
    public GameObject Backplate;


    public void FixTextBounds()
    {
        IEnumerator _delay()
        {
            yield return new WaitForSeconds(0.1f);
            CenterTMP.rectTransform.anchoredPosition3D = centerpos;
            LowerTMP.rectTransform.anchoredPosition3D = lowerpos;
        }
        StartCoroutine(_delay());
    }

    private void Start()
    {
        extents_x = spriteRenderer.bounds.extents.x * 2;
        extents_y = spriteRenderer.bounds.extents.y * 2;
        CenterTMP.rectTransform.anchoredPosition3D = centerpos;
        LowerTMP.rectTransform.anchoredPosition3D = lowerpos;

        Backplate.SetActive(false);
    }

    // 1 2
    // 4 3
    public void Rescale(Vector3 corner1_f, Vector3 corner2_f, Vector3 corner3_f, Vector3 corner4_f)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

#if UNITY_EDITOR
        spriteRenderer.enabled = true;
#endif

        var topy = (corner1_f.y + corner2_f.y) / 2;
        var bottomy = (corner3_f.y + corner4_f.y) / 2;

        var left = (corner1_f + corner4_f) / 2;
        var right = (corner2_f + corner3_f) / 2;

        corner1_f = new Vector3(left.x, topy, left.z);
        corner2_f = new Vector3(right.x, topy, right.z);
        corner3_f = new Vector3(right.x, bottomy, right.z);
        corner4_f = new Vector3(left.x, bottomy, left.z);

        Cubes[0].position = corner1_f;
        Cubes[1].position = corner2_f;
        Cubes[2].position = corner3_f;
        Cubes[3].position = corner4_f;

        // Calculate position
        Vector3 position = (corner1_f + corner2_f + corner3_f + corner4_f) / 4f;

        // Calculate scale (assumes two adjacent corners)
        Vector3 scale = new Vector3(
            Vector3.Distance(corner1_f, corner2_f) / extents_x,
            Vector3.Distance(corner1_f, corner4_f) / extents_y,
            0.1f);

        // Calculate rotation (assumes first corner to last corner)
        Vector3 thirdOrthogonalVector = Vector3.Cross(corner1_f - corner2_f, corner4_f - corner1_f);
        Quaternion rotation = Quaternion.LookRotation(thirdOrthogonalVector, Vector3.up);

        // Apply transformations
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;

        FixTextBounds();
    }

    [ContextMenu("func rescale")]
    public void SetPanelPosition()
    {
        List<Vector3> Positions = new();
        foreach (Transform c in Cubes)
        {
            Positions.Add(c.position);
        }

        Rescale(
            Positions[0],
            Positions[1],
            Positions[2],
            Positions[3]
        );

        Backplate.SetActive(true);
    }

    /// <summary>
    /// sets the tex, location_f is either "center" or "lower"
    /// </summary>
    /// <param name="text_f"></param>
    /// <param name="location_f"></param> 
    public void SetText(string text_f, string location_f)
    {
        if (location_f == "center")
        {
            CenterTMP.gameObject.SetActive(true);
            LowerTMP.gameObject.SetActive(false);
            ProgressBar.gameObject.SetActive(false);
            CenterTMP.text = text_f;
        }
        else
        {
            CenterTMP.gameObject.SetActive(false);
            LowerTMP.gameObject.SetActive(true);
            ProgressBar.gameObject.SetActive(true);
            LowerTMP.text = text_f;
        }
        FixTextBounds();
    }

    public void HideAllButtons()
    {
        foreach (var button in Buttons)
        {
            button.HideButton();
        }
    }

    int curr_button_num = -1;
    bool curr_direction = false;

    public void SetButton(int num, bool up)
    {
        if (num != curr_button_num || curr_direction != up)
        {
            curr_button_num = num;
            curr_direction = up;

            HideAllButtons();

            // show the correct button
            if (up)
            {
                Buttons[num].ShowButton_Up();
            }
            else
            {
                Buttons[num].ShowButton_Down();
            }

        }
    }
}