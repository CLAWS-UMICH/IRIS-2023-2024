using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationController : MonoBehaviour
{
    [SerializeField] List<GameObject> alerts;
    public GameObject prefab;
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
        GameObject newButton = scroll.HandleAddingButton(prefab);
        newButton.transform.Find("Text").transform.Find("Num").GetComponent<TextMeshPro>().text = num.ToString();
        newButton.name = num.ToString();
        alerts.Add(newButton);
        num++;
        StartCoroutine(_DeleteAlert(newButton));
    }

    IEnumerator _DeleteAlert(GameObject alert)
    {
        yield return new WaitForSeconds(Mathf.Floor(Random.Range(3f, 15f)));
        scroll.HandleButtonDeletion(alert);
        alerts.Remove(alert);
    }

}
