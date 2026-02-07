using System.Linq;
using UnityEngine;

public class CleanGooTask : Task {

    MeshRenderer mr;
    Material gooMat;

    public int waterHitsRequired = 50;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        if(gooMat == null) {
            gooMat = Resources.Load<Material>("Gooey");
        }
        if(mr.materials.Count() == 1) {
            Material[] currentMats = mr.materials;
            currentMats = currentMats.Append(gooMat).ToArray();
            mr.materials = currentMats;
        }
    }

    void OnCollisionEnter(Collision collision) {
      if(collision.gameObject.name.Contains("WaterBall")) {
            waterHitsRequired = waterHitsRequired - 1;

            if(waterHitsRequired == 0) {
                completed = true;
                TaskManager.Instance.UpdateTasks(this);

                Material[] currentMats = mr.materials;
                currentMats = currentMats.SkipLast(1).ToArray();
                mr.materials=currentMats;
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
