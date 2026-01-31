using System.Collections.Generic;
using UnityEngine;

public class ObjectTask : Task {
    public List<GameObject> requiredObjects;

    [HideInInspector]
    public bool complete;
    private List<GameObject> objectsInArea = new List<GameObject>();

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
            if(complete == false)
            {
                this.complete = true;
                TaskManager.Instance.UpdateTasks(this);
            }
        }
    }
}
