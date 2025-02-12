using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour{
    public float healthCount;
    public float healthLossRate;
    public float immunity;
    public float DiseaseSeverity;
    float BaseImmunity;
    public bool contaminationOccured = false; 
    public bool isDead = false;
    public DeadAgent deadAgent;
    Animator anim;
    //public GameObject uiPanel;
    private int code = 0;
    private double increaseTime = 0.015; 
    public double time = 0f;

    void Start(){
        anim = GetComponent<Animator>();
        BaseImmunity = PlayerPrefs.GetFloat("BaseImmunity");
        immunity = Random.Range(BaseImmunity, 0.600f);
        healthCount = Random.Range(25, 101);
        DiseaseSeverity = PlayerPrefs.GetFloat("DiseaseSev");
        
    }

    void Update(){
        healthCount -= healthLossRate;
        time += increaseTime;
        anim.SetFloat("Health", healthCount);
        if (healthCount >= 70){
            healthyState();
        } else if (healthCount < 70 && healthCount >= 40){ 
            sickState();
        } else if (healthCount < 40 && healthCount > 0){ 
            contagiousState();
        } else{ 
            deadState();
        }
    }

    void healthyState(){
        healthLossRate = 0.001f;
            CheckAtHospital();
             if (!contaminationOccured){
                CheckForContamination();
            }
            GetComponent<Robot_AI>().SetHealthyState(true);
    }

    void sickState(){
        healthLossRate = 0.005f * DiseaseSeverity;
        CheckAtHospital();
        GetComponent<Robot_AI>().SetHealthyState(false);
    }

    void contagiousState(){
        healthLossRate = 0.01f * DiseaseSeverity;
        CheckAtHospital();
        GetComponent<Robot_AI>().SetHealthyState(false);
    }

    void deadState(){
        healthLossRate = 0.0f;
        healthCount = 0f;
        increaseTime = 0;
        transform.Rotate(90, 0, 0);
        transform.position = new Vector3(transform.position.x, 0.6f, transform.position.z);
        GetComponent<Robot_AI>().SetAliveState(false);
        deadAgent.EnableScript();
        //Movement_Test findDeadComponent = GetComponent<Movement_Test>();
        if (code == 0){
            SceneSpawner.Instance.AgentDied();
            code++;
        }  
    }
 
    void CheckForContamination(){ 
        HealthManager[] allAgents = GameObject.FindObjectsOfType<HealthManager>();
        foreach (HealthManager agent in allAgents){
            if (agent != this && agent.healthCount < 40 && healthCount > 0 ){
                float distance = Vector3.Distance(transform.position, agent.transform.position);
                if (distance < 2.0f && Random.value < (1-immunity)){ //Distance and Probability of catching
                    ReduceHealth(20.0f);
                    contaminationOccured = true; 
                }
            }
        }
    }

    void CheckAtHospital(){ 
        MedicalBox[] allMedicalBoxes = GameObject.FindObjectsOfType<MedicalBox>();
        foreach (MedicalBox medBox in allMedicalBoxes){
            if (healthCount < 90 && healthCount > 0 ){
                float distance = Vector3.Distance(transform.position, medBox.transform.position);
                if (distance < 4.0f){
                    IncreaseHealth(2.0f);
                }
            }
        }
    }

    public void IncreaseHealth(float amount){
        healthCount += amount;
        if(immunity<= 0.85){
            immunity += 0.0005f;
        }
    }

    public void ReduceHealth(float amount){
        healthCount -= amount;
    }
}
