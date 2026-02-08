using System.Linq;
using UnityEngine;

public class CleanGooTask : Task {

    MeshRenderer mr;
    Material gooMat;

    public int waterHitsRequired = 50;
    private float waterHitsStart;

    void Start() {
        waterHitsStart = waterHitsRequired;
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
      if(collision.gameObject.name.Contains("WaterBall") && !completed) {

            Material[] currentMatsA = mr.materials;
            waterHitsRequired = waterHitsRequired - 1;
            float alphaPercent = 2.5f * (waterHitsRequired / waterHitsStart);
            currentMatsA[1].color = new Color(currentMatsA[1].color.r, currentMatsA[1].color.g, currentMatsA[1].color.b, currentMatsA[1].color.r * alphaPercent);
            mr.materials = currentMatsA;

            if(waterHitsRequired <= 0) {
                CompleteTask();
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
