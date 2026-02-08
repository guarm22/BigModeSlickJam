using UnityEngine;

public class CleanlinessTask : Task {
    
    public static CleanlinessTask Instance;
    public int gooSlicks;
    private int lastFrame = 0;

    void Awake() {
        Instance = this;
        gooSlicks = 0;
        this.objective = "Keep the house clean of all goo! (-" +  gooSlicks + "%)"; 
    }

    // Update is called once per frame
    void Update() {
        this.objective = "Keep the house clean of all goo! (-" +  gooSlicks + "%)"; 
        if(gooSlicks != lastFrame) {
            TaskManager.Instance.UpdateTasks(this);
        }
        lastFrame = gooSlicks;
    }

    public float GetRemovedPercent() {
        return gooSlicks;
    }
}
