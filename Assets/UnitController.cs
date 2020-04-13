using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class UnitController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float timer = 0.0f;

    [Range(0.0f, 25.0f)]
    public float memoryLength = 10.0f;
    public float memoryLengthUsage;
    public GameObject sensoryRangeDisplay;
    public bool mouthOpen = false;


    [SerializeField]
    float itemXSpread = 30f;

    [SerializeField]
    float itemYSpread = 0;

    [SerializeField]
    float itemZSpread = 30f;

    // Start is called before the first frame update
    public float energy = 100.0f;

    [Range(0.0f, 25.0f)]
    public float speed = 3.0f;
    [Range(0.0f, 25.0f)]
    public float acceleration = 3.0f;
    [Range(0.0f, 25.0f)]
    public float turnspeed = 3.0f;
    [Range(0.0f, 50.0f)]
    public float sensoryRange = 10.0f;
    

    public Collider[] detectedObjects;

    public List<GameObject> detectedFoodObjects = new List<GameObject>();

    public Vector3 targetDestination;
    public Vector3 foodTarget;

    [Range(0.0f, 5.0f)]
    float metabolicRate = 1.0f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        energy -= (Time.deltaTime * metabolicRate);

        if(memoryLengthUsage <= 0)
        {
            memoryLengthUsage = memoryLength;
        }
        else
        {
            memoryLengthUsage -= Time.deltaTime;
        }

        //set scale /2
        sensoryRangeDisplay.transform.localScale = new Vector3((sensoryRange * 2), 0.001f, (sensoryRange * 2));

        //detectedObjects = Physics.OverlapSphere(gameObject.transform.position, sensoryRange);




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
            Wander();
        }

        //perception check
        if (Input.GetKeyDown(KeyCode.W))
        {
            detectedFoodObjects.Clear();
            Debug.Log("Looking around for food");
            detectedObjects = Physics.OverlapSphere(gameObject.transform.position, sensoryRange);

            //% chance of what action I will take
            //-----------------------------------------------

            //if looking for food
            //int i = 0;

            mouthOpen = true;
            foreach (Collider detectedObject in detectedObjects)
                {
                        if(detectedObject.tag == "Food")
                        {
                            detectedFoodObjects.Add(detectedObject.gameObject);
                            detectedFoodObjects = detectedFoodObjects.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();
                        }

                }

            //go to the food
            if(detectedFoodObjects.Count != 0)
            {
                targetDestination = detectedFoodObjects[0].transform.position;
            }
            else
            {
                mouthOpen = false;
                Wander();
            }
            //wander?


        }

    }

    private void Wander()
    {
        targetDestination = new Vector3((Random.Range(-itemXSpread, itemXSpread)), (Random.Range(-itemYSpread, itemYSpread)), (Random.Range(-itemZSpread, itemZSpread)));
    }


    private void OnTriggerStay(Collider collisionobject)
    {
        Debug.Log(collisionobject);
        if (collisionobject.tag == "Food")
        {
            if (mouthOpen == true)
            {
                Debug.Log("energy" + energy);
                Debug.Log("energyContent" + collisionobject.gameObject.GetComponent<food>().energyContent);

                energy += collisionobject.gameObject.GetComponent<food>().energyContent;
                Destroy(collisionobject.gameObject);
                mouthOpen = false;
                //remeber this
                Wander();
            }
        }
    }
}
