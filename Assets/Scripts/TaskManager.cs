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
        float completedCount = 0;
        if(tasks.Count == 0) {return;}
        foreach(Task t in allTasks) {
            if(!tasks.Contains(t)) {tasks.Add(t);}
            if(t.completed) { completedCount += 1;}

            taskUI.text += "Task: " + t.objective + " - completed: " + t.completed + " \n";
        }
        taskUI.text+="Completion Rate: " + ((completedCount/tasks.Count)*100).ToString("F1");

    }

    public void UpdateTasks(Task updatedTask){
        taskUI.text = ""; 
        float completedCount = 0;

        foreach(Task t in tasks) {
            if(t.completed) { completedCount += 1;}
            taskUI.text += "Task: " + t.objective + " - completed: " + t.completed + " \n";
        }
        taskUI.text+="Completion Rate: " + ((completedCount/tasks.Count)*100).ToString("F1");
    }



 
}
