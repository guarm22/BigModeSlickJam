using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public List<Task> tasks = new List<Task>();

    public TMP_Text taskUI;

    public static TaskManager Instance;

    public GameObject taskUIObject;
    public GameObject taskUIAnchor;

    public GameObject taskParent;

    private int closedMax = 3;

    private int openMax = 18;

    private bool closed = true;

    public GameObject closedItems;
    public GameObject openItems;

    private List<GameObject> currentList = new List<GameObject>();

    void Start() {
        Instance = this;
        tasks = GameObject.FindObjectsByType<Task>(FindObjectsSortMode.None).ToList<Task>();
        int yVal = 50;
        int index = closed ? closedMax : openMax;
        Vector3 pos = taskUIAnchor.transform.position;
        foreach(Task t in tasks) {
            if(index == 0) {break;}
            index -= 1;
            GameObject newTask = Instantiate(taskUIObject, pos, Quaternion.identity, taskParent.transform);
            pos = new Vector3(pos.x, pos.y-yVal, pos.z);
            newTask.GetComponentInChildren<TMP_Text>().text = t.objective;
            newTask.transform.GetChild(1).gameObject.SetActive(t.completed);
            currentList.Add(newTask);
        }

    }

    public float CalculateCompletionPercent() {
        float completedCount = 0f;
        foreach(Task t in tasks) {
            if(t.completed) { completedCount += 1;}
        }

        return Math.Clamp(((completedCount/(tasks.Count-1))*100) - CleanlinessTask.Instance.GetRemovedPercent(), 0, 100);
    }

    public void UpdateTasks(Task updatedTask){
        int yVal = 50;
        Vector3 pos = taskUIAnchor.transform.position;
        foreach(GameObject g in currentList) {Destroy(g);}

        int index = closed ? closedMax : openMax;
        foreach(Task t in tasks) {
            if(index == 0) {break;}
            index -= 1;
            GameObject newTask = Instantiate(taskUIObject, pos, Quaternion.identity, taskParent.transform);
            pos = new Vector3(pos.x, pos.y-yVal, pos.z);
            newTask.GetComponentInChildren<TMP_Text>().text = t.objective;
            newTask.transform.GetChild(1).gameObject.SetActive(t.completed);
            currentList.Add(newTask);
        }

        //taskUI.text+="Completion Rate: " + CalculateCompletionPercent().ToString("F1");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            closed = !closed;
            closedItems.SetActive(closed);
            openItems.SetActive(!closed);
            UpdateTasks(null);
        }
    }



 
}
