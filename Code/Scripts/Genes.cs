using UnityEngine;

public class Genes : MonoBehaviour{
    Renderer myRenderer;
    HealthManager healthManager;

    void Start(){
        myRenderer = GetComponent<Renderer>();
        myRenderer.material.SetColor("_Color", Color.white);
        healthManager = transform.parent.GetComponent<HealthManager>();
    }

    void Update(){
            float healthPercentage = healthManager.healthCount / 100f;
            Color newColor = Color.Lerp(Color.white, new Color(1f, 0.2f, 0.2f), 1f - healthPercentage);
            myRenderer.material.SetColor("_Color", newColor);
    }
}