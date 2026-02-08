using UnityEngine;

public class DontTouchTask : Task {
    
    void Awake() {
        completed = true;
    }

    void OnCollisionEnter(Collision collision) {
        if(!completed) { return; }
        GameObject c = collision.gameObject;
        if(c.tag == "Player" || c.tag == "Ball" || c.tag == "Grabbable") {
            FailTask();
            TaskManager.Instance.UpdateTasks(this);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
