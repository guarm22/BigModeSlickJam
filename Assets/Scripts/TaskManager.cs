using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public List<Task> tasks;

    public TMP_Text taskUI;

    public static TaskManager Instance;

    void Start() {
        Instance = this;
        List<Task> allTasks = GameObject.FindObjectsByType<Task>(FindObjectsSortMode.None).ToList<Task>();
        
        taskUI.text = ""; 
        if(tasks.Count == 0) {return;}
        foreach(Task t in allTasks) {
            if(!tasks.Contains(t)) {tasks.Add(t);}

            taskUI.text += "Task: " + t.objective + " - completed: " + t.completed + " \n";
        }
        taskUI.text+="Completion Rate: " + CalculateCompletionPercent().ToString("F1");

    }

    private float CalculateCompletionPercent() {
        float completedCount = 0f;
        foreach(Task t in tasks) {
            if(t.completed) { completedCount += 1;}
        }

        return Math.Clamp(((completedCount/(tasks.Count-1))*100) - CleanlinessTask.Instance.GetRemovedPercent(), 0, 100);
    }

    public void UpdateTasks(Task updatedTask){
        taskUI.text = ""; 

        foreach(Task t in tasks) {
            taskUI.text += "Task: " + t.objective + " - completed: " + t.completed + " \n";
        }
        taskUI.text+="Completion Rate: " + CalculateCompletionPercent().ToString("F1");;
    }



 
}
