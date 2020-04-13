using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float timer = 0.0f;


    [SerializeField]
    float itemXSpread = 30f;

    [SerializeField]
    float itemYSpread = 0;

    [SerializeField]
    float itemZSpread = 30f;

    // Start is called before the first frame update
    public float energy = 100.0f;
    public float speed = 3.0f;
    public float acceleration = 3.0f;
    public float turnspeed = 3.0f;
    public float sensoryRange = 10.0f;

    public Collider[] hitColliders;

    public Vector3 targetDestination;

    float consumptionrate = 1.0f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        energy -= (Time.deltaTime * consumptionrate);


        // Set Navmesh Agent setting
        agent.speed = speed;
        agent.angularSpeed = turnspeed;
        agent.acceleration = acceleration;

        if (energy <= 0)
        {
            Debug.Log(gameObject + ("has died"));
            
            Destroy(gameObject);
        }
        agent.SetDestination(targetDestination);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetDestination = new Vector3((Random.Range(-itemXSpread, itemXSpread)), (Random.Range(-itemYSpread, itemYSpread)), (Random.Range(-itemZSpread, itemZSpread)));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            hitColliders = Physics.OverlapSphere(gameObject.transform.position, sensoryRange);
        }

    }
}
