using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public List<Task> tasks;

    public TMP_Text taskUI;

    public static TaskManager Instance;

    void Start() {
        Instance = this;
        if(tasks.Count == 0) {return;}
        foreach(Task t in tasks) {
            taskUI.text += "Task: " + t.objective + " - completed: " + t.completed + " \n";
        }
    }

    public void UpdateTasks(Task updatedTask){
        taskUI.text = ""; 
        foreach(Task t in tasks) {
            if(t == updatedTask) { t.completed = true;}
            taskUI.text += "Task: " + t.objective + " - completed: " + t.completed + " \n";
        }
    }



 
}
