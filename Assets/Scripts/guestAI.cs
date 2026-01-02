using UnityEngine;
using UnityEngine.AI; 

public class GuestAI : MonoBehaviour
{
    public Transform counterLocation; 
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (counterLocation != null)
        {
            agent.SetDestination(counterLocation.position);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Debug.Log("客人已抵達櫃台！");
            FaceCounter();
        }
    }

    void FaceCounter()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}