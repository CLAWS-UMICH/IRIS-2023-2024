using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationController : MonoBehaviour
{
    List<GameObject> alerts = new List<GameObject>();
    public GameObject prefab;
    public GameObject prefab1;
    private NewScroll scroll;
    int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        //scroll = transform.Find("Scroll").GetComponent<ScrollHandler>();
        scroll = transform.Find("Scroll").GetComponent<NewScroll>();
    }

    public void AddAlert()
    {
        GameObject newButton = null;
        if (num % 2 == 0)
        {
            newButton = scroll.HandleAddingButton(prefab);
        } else
        {
            newButton = scroll.HandleAddingButton(prefab1);
        }
        newButton.transform.Find("Text").transform.Find("Num").GetComponent<TextMeshPro>().text = num.ToString();
        newButton.name = num.ToString();
        alerts.Add(newButton);
        num++;
        StartCoroutine(_DeleteAlert(newButton));
    }

    IEnumerator _DeleteAlert(GameObject alert)
    {
        yield return new WaitForSeconds(Mathf.Floor(Random.Range(3f, 10f)));
        scroll.HandleButtonDeletion(alert);
        alerts.Remove(alert);
    }

}
