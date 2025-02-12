using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneSpawner : MonoBehaviour{
    public GameObject agentPrefabs;
    public GameObject[] allAgents;
    public GameObject uiPanel;
    public GameObject[] cameras;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI immunityText;
    public TextMeshProUGUI hospitalProximityText;
    public TextMeshProUGUI timeAliveText;
    public TextMeshProUGUI aliveCountText; 
    public TextMeshProUGUI deadCountText;
    Vector3 startPos;
    public int numAgents;
    private GameObject lastClickedAgent;
    private bool isUpdatingHealth;
    private int aliveAgentCount;
    private int deadAgentCount;
    public static SceneSpawner Instance; 
    
    
    void Awake(){
        Instance = this;
    }

    void Start(){
        numAgents = PlayerPrefs.GetInt("NumAgents");
        allAgents = new GameObject[numAgents];
        aliveAgentCount = numAgents;
        deadAgentCount = 0;
        for (int i = 0; i < numAgents; i++){
            float xPos = Random.Range(-30, 30f);
            float zPos = Random.Range(-30, 30f);
            startPos = new Vector3(xPos, 1f, zPos);
            allAgents[i] = Instantiate(agentPrefabs, startPos, Quaternion.identity);
            allAgents[i].GetComponentInChildren<Camera>().enabled = false;
        }
        UpdateAgentCountText();
    }

    void Update(){
    if (Input.GetMouseButtonDown(0)){
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo)){
            GameObject clickedAgent = GetClickedAgent(hitInfo.collider.gameObject);
            if (clickedAgent != null){
                uiPanel.SetActive(true);
                lastClickedAgent = clickedAgent;
                HealthManager agentHealthManager = clickedAgent.GetComponent<HealthManager>();
                if (agentHealthManager != null){
                    StartCoroutine(UpdateHealthText(agentHealthManager));
                }

                // Enable camera if found
                Camera agentCamera = clickedAgent.GetComponentInChildren<Camera>();
                if (agentCamera != null){
                    agentCamera.enabled = true;
                } else {
                    Debug.LogWarning("Camera component not found in clicked agent's children.");
                }
            } else {
                uiPanel.SetActive(false);
                if (lastClickedAgent != null){
                    Camera lastCamera = lastClickedAgent.GetComponentInChildren<Camera>();
                    if (lastCamera != null){
                        lastCamera.enabled = false;
                    }
                }
            }
        }
    }
}


    IEnumerator UpdateHealthText(HealthManager agentHealthManager){
        isUpdatingHealth = true;
        while (lastClickedAgent == agentHealthManager.gameObject){
            healthText.text = agentHealthManager.healthCount.ToString("F2");
            immunityText.text = agentHealthManager.immunity.ToString("F2");
            timeAliveText.text = agentHealthManager.time.ToString("F2");
            yield return null;
        }
        isUpdatingHealth = false;
    }

    GameObject GetClickedAgent(GameObject clickedObject){
        for (int i = 0; i < numAgents; i++){
            if (clickedObject == allAgents[i]){
                return allAgents[i];
            }
        }
        return null;
    }

    public void AgentDied(){
        deadAgentCount++;
        aliveAgentCount--;
        UpdateAgentCountText();
    }

    void UpdateAgentCountText(){
        aliveCountText.text =  "" + aliveAgentCount;
        deadCountText.text = "" + deadAgentCount;
    }
}
