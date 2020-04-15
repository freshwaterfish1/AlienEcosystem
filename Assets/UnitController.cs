using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class UnitController : MonoBehaviour
{

    public GameObject SpeciesHolder;
    public NavMeshAgent agent;
    public float timer = 0.0f;

    public float distanceTraveled = 0.0f;
    Vector3 lastPosition;

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
    [Range(0.0f, 5.0f)]
    public float metabolicRate = 1.0f;

    public Collider[] detectedObjects;
    public float movementEfficiency = 0.001f;

    public List<System.Action> actionList = new List<System.Action>();
    public List<GameObject> detectedFoodObjects = new List<GameObject>();

    public Vector3 targetDestination;
    public Vector3 foodTarget;
    public Color unitColor;

    Renderer rend;


    void Start()
    {

        lastPosition = transform.position;
        rend = GetComponent<Renderer>();

        unitColor = new Color((speed / 25), (sensoryRange / 50), (energy / 100), 1.0f);
        rend.material.SetColor("_Color", unitColor);




        actionList.Add(Hunt);
        actionList.Add(Wander);



    }



    // Update is called once per frame
    void Update()
    {

        

        //rend.material.SetColor( "unitcolor", new Color(            ((speed / 25) * 100),            ((sensoryRange / 50) * 100),            ((energy / 100) * 100)));

        timer += Time.deltaTime;
        energy -= (Time.deltaTime * metabolicRate);


        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        energy -= distanceTraveled * movementEfficiency;

        if (memoryLengthUsage <= 0)
        {
            memoryLengthUsage = memoryLength;
            NewAction();
            Debug.Log("New Action");
        }
        else
        {
            memoryLengthUsage -= Time.deltaTime;
        }

        //set scale /2
        sensoryRangeDisplay.transform.localScale = new Vector3((sensoryRange * 2), (sensoryRange * 2), 1);

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
            Hunt();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reproduce();
        }

    }

    private void NewAction()
    {
        actionList[Random.Range(0, actionList.Count)]();
    }

     void Hunt()
    {
        detectedFoodObjects.Clear();
        Debug.Log((this.gameObject.transform.name) + " is Hunting");
        detectedObjects = Physics.OverlapSphere(gameObject.transform.position, sensoryRange);
        mouthOpen = true;
        foreach (Collider detectedObject in detectedObjects)
        {
            if (detectedObject.tag == "Food")
            {
                detectedFoodObjects.Add(detectedObject.gameObject);
                detectedFoodObjects = detectedFoodObjects.OrderBy(x => Vector3.Distance(this.transform.position, x.transform.position)).ToList();
            }

        }

        //go to the food
        if (detectedFoodObjects.Count != 0)
        {
            targetDestination = detectedFoodObjects[0].transform.position;
        }
        else
        {
            mouthOpen = false;
        }
    }


     void Wander()
    {
        Debug.Log((this.gameObject.transform.name) + " is Wandering");
        targetDestination = new Vector3((Random.Range(-itemXSpread, itemXSpread)), (Random.Range(-itemYSpread, itemYSpread)), (Random.Range(-itemZSpread, itemZSpread)));
    }

    void Reproduce()
    {
        GameObject childUnit = Instantiate(gameObject);
        childUnit.transform.parent = SpeciesHolder.transform;
        Debug.Log("child speed was " + childUnit.gameObject.GetComponent<UnitController>().speed);
        childUnit.gameObject.GetComponent<UnitController>().speed += 1;
        Debug.Log("child speed is now " + childUnit.gameObject.GetComponent<UnitController>().speed);

        //Get Colored
        unitColor = new Color((speed / 25), (sensoryRange / 50), (energy / 100), 1.0f);
        rend.material.SetColor("_Color", unitColor);
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
