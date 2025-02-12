using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class FindDead : MonoBehaviour{
    NavMeshAgent agent;
    public bool go = true;
    public bool isAlive = true;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 1.0f;
    }

    [Task]
    public bool IsAgentAlive(){
        return isAlive;
    }

    [Task]
    public void MoveToDestination(){
        if (Task.isInspected){
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        }
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending){
            Task.current.Succeed();
        }
    }

    [Task]
    public void PickDeadAgent(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, 130.0f);
        DeadAgent closestDeadAgent = null;
        float closestDistance = float.MaxValue;
        foreach (Collider collider in colliders){
            DeadAgent deadAgent = collider.GetComponent<DeadAgent>();
            if (deadAgent != null && deadAgent.enabled){
                float distance = Vector3.Distance(transform.position, deadAgent.transform.position);
                if (distance < closestDistance){
                    closestDeadAgent = deadAgent;
                    closestDistance = distance;
                }
            }
        }
        if (closestDeadAgent != null){
            Vector3 dest = new Vector3(closestDeadAgent.transform.position.x, 0, closestDeadAgent.transform.position.z);
            agent.SetDestination(dest);
            Task.current.Succeed();
        }
    }
}