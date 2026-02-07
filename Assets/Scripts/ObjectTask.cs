using System.Collections.Generic;
using UnityEngine;

public class ObjectTask : Task {
    public List<GameObject> requiredObjects;

    private List<GameObject> objectsInArea = new List<GameObject>();

    public bool ShouldObjectiveFailIfObjectsAreRemoved = false;

    void OnTriggerEnter(Collider other)
    {
      if(requiredObjects.Contains(other.gameObject)){
            objectsInArea.Add(other.gameObject);
        }  
    }

    void OnTriggerExit(Collider other)
    {
        if(objectsInArea.Contains(other.gameObject))
        {
            objectsInArea.Remove(other.gameObject);
        }
    }

    void Update() {
        if(objectsInArea.Count == requiredObjects.Count) {
            if(completed == false) {
                completed = true;
                TaskManager.Instance.UpdateTasks(this);
            }
        }
        else if(ShouldObjectiveFailIfObjectsAreRemoved == true){
            if(completed) {
                completed = false;
                TaskManager.Instance.UpdateTasks(this);
            }
        }
    }
}
