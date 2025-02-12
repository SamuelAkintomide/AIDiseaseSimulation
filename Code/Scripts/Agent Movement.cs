using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class Robot_AI : MonoBehaviour
{
    NavMeshAgent agent;
	public bool isAlive = true;
	public bool isHealthy = true;

    void Start(){
        agent = this.GetComponent<NavMeshAgent>();
		agent.stoppingDistance = 5.0f;        
    }
    
	public void SetAliveState(bool alive){
        isAlive = alive;
        if (!isAlive){
            agent.isStopped = true;
        }
    }
	public void SetHealthyState(bool healthy){
        isHealthy = healthy;
    }

	[Task]
    public bool IsAgentAlive(){
        return isAlive;
    }
	[Task]
	public bool IsAgentHealthy(){
        return isHealthy;
    }	
	

	[Task]
	public void PickRandomDestination(){
		Vector3 dest = new Vector3(Random.Range(-30,30), 0, Random.Range(-30,30));
		agent.SetDestination(dest);
		Task.current.Succeed();
	}
	[Task]
	public void MoveToDestination(){
		if(Task.isInspected){
			Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
		}
		if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending){
			Task.current.Succeed();
		}
	}

	[Task]
	public void PickMedicalBox(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30.0f);
        MedicalBox closestMedicalBox = null;
        float closestDistance = float.MaxValue;
        foreach (Collider collider in colliders){
            MedicalBox medicalBox = collider.GetComponent<MedicalBox>();
            if (medicalBox != null){
                float distance = Vector3.Distance(transform.position, medicalBox.transform.position);
                if (distance < closestDistance){
                    closestMedicalBox = medicalBox;
                    closestDistance = distance;
                }
            }
        }
        if (closestMedicalBox != null){
			Vector3 dest = new Vector3(closestMedicalBox.transform.position.x,0,closestMedicalBox.transform.position.z);
			agent.SetDestination(dest);
			Task.current.Succeed();
    	}
    }
}