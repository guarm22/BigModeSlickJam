using UnityEngine;
using System.Collections.Generic;

public class ObjectTaskTimer : Task {
    public GameObject requiredObject;

    private bool objectInArea = false;

    public bool ShouldObjectiveFailIfObjectsAreRemoved = false;

    public float minTimeRequired = 15f;

    public float maxTimeBeforeFail = 25f;

    private bool permaFail = false;

    private float elapsedTime = 0f;

    void OnTriggerEnter(Collider other)
    {
      if(requiredObject == other.gameObject) {
            objectInArea = true;
        }  
    }

    void OnTriggerExit(Collider other)
    {
        if(requiredObject == other.gameObject){
            objectInArea = false;
        }
    }

    void Update() {
        if(permaFail) {return;}

        if(objectInArea) {
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= maxTimeBeforeFail && completed) {
                completed = false;
                permaFail = true;
                TaskManager.Instance.UpdateTasks(this);
                return;
            }

            if(completed == false && elapsedTime >= minTimeRequired) {
                completed = true;
                TaskManager.Instance.UpdateTasks(this);
            }
        }
        
    }
}
