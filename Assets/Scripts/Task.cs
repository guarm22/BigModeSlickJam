using UnityEngine;

public class Task : MonoBehaviour
{
public bool completed;
public string objective;   


    public void CompleteTask() {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/Task Complete"), this.transform.position);
        completed = true;
    }

    public void FailTask() {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/Task Fail"), this.transform.position);
        completed = false;
    }

}
