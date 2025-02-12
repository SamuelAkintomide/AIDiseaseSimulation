using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadAgent : MonoBehaviour{
    
    void Start(){
        enabled = false;
    }

    void Update(){
        if (enabled == true){
            FindDead[] allAgents = GameObject.FindObjectsOfType<FindDead>();
            foreach (FindDead agent in allAgents){
                float distance = Vector3.Distance(transform.position, agent.transform.position);
                if (distance < 1.5f){
                    transform.position = new Vector3(transform.position.x, -200f, transform.position.z);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void EnableScript(){
        enabled = true;
    }
}
