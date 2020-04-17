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
    float speedMin = 0.0f;
    float speedMax = 25.0f;

    [Range(0.0f, 25.0f)]
    public float acceleration = 3.0f;

    [Range(0.0f, 25.0f)]
    public float turnSpeed = 3.0f;

    [Range(0.0f, 50.0f)]
    public float sensoryRange = 10.0f;
    float sensoryRangeMin = 0.0f;
    float sensoryRangeMax = 50.0f;


    [Range(0.0f, 5.0f)]
    public float metabolicRate = 1.0f;

    public float movementEfficiency = 0.0001f;

    public float reproductionChance = 0.0f; //reproduction chance each frame
    public float reproductionCooldown = 2.0f;
    public float reproductionTimer;

    public float decisivness = 1.0f; //the great this is, the more likely a cell is to go for further food
    public float distanceChoice = 0.0f; //more likely to go for nth food.


    public Collider[] detectedObjects;


    public List<System.Action> actionList = new List<System.Action>();
    public List<GameObject> detectedFoodObjects = new List<GameObject>();

    public Vector3 targetDestination;
    public Vector3 foodTarget;
    public Color unitColor;
    public int generationCount;

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
        //Don't Color on Update - do this when they are spawned
        //unitColor = new Color((speed / 25), (sensoryRange / 50), (energy / 100), 1.0f);
        //rend.material.SetColor("_Color", unitColor);


        //rend.material.SetColor( "unitcolor", new Color(            ((speed / 25) * 100),            ((sensoryRange / 50) * 100),            ((energy / 100) * 100)));

        timer += Time.deltaTime;


        //energy consumption
        energy -= (Time.deltaTime * metabolicRate);
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        energy -= distanceTraveled * movementEfficiency;

        //reproduction logic: Update reproduction chance and determine if cell will reproduce

        if (reproductionTimer >= reproductionCooldown)
        {
            reproductionTimer=0.0f;
            reproductionChance += ((energy - 50) / 1000);
            reproductionChance = Mathf.Clamp(reproductionChance, 0.0f, 1.0f);
            if (Random.Range(0.0f, 1.0f) < reproductionChance)
            {
                Reproduce();
            }
        }
        else
        {
            reproductionTimer += Time.deltaTime;
        }

        if (memoryLengthUsage <= 0)
        {
            memoryLengthUsage = memoryLength;
            NewAction();
            //Debug.Log("New Action");
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
        agent.angularSpeed = turnSpeed;
        agent.acceleration = acceleration;
        agent.SetDestination(targetDestination);
        if (energy <= 0)
        {
            //Debug.Log(gameObject + ("has died"));

            Destroy(gameObject);
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {
            Wander();
        }

        //perception check
        if (Input.GetKeyDown(KeyCode.W))
        {
            Hunt();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Random" + NextGaussianDouble());
        }

    }

    private void NewAction()
    {
        Hunt();

        //actionList[Random.Range(0, actionList.Count)]();
    }

     void Hunt()
    {
        detectedFoodObjects.Clear();
        //Debug.Log((this.gameObject.transform.name) + " is Hunting");
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
            float foodChoice = Mathf.Ceil(Mathf.Abs((float)NextGaussianDouble()) * decisivness + distanceChoice);
            foodChoice = Mathf.Clamp(foodChoice, 0.0f, detectedFoodObjects.Count-1);
            targetDestination = detectedFoodObjects[(int)foodChoice].transform.position;
            
        }
        else
        {
            mouthOpen = false;
        }
    }


     void Wander()
    {
        //Debug.Log((this.gameObject.transform.name) + " is Wandering");
        targetDestination = new Vector3((Random.Range(-itemXSpread, itemXSpread)), (Random.Range(-itemYSpread, itemYSpread)), (Random.Range(-itemZSpread, itemZSpread)));
    }

    void Reproduce()
    {
        energy = energy * 0.5f;
        reproductionChance = 0.0f;
        GameObject childUnit = Instantiate(gameObject,gameObject.transform.position,Quaternion.identity);

        //Stops and resets the navmesh agent - ought to fix some bugs.

        childUnit.gameObject.GetComponent<UnitController>().agent.isStopped = true;
        childUnit.gameObject.GetComponent<UnitController>().agent.ResetPath();

        //Parent it to the correct species
        childUnit.transform.parent = SpeciesHolder.transform;
        //Increse it's Generation and then rename it to that generation
        childUnit.gameObject.GetComponent<UnitController>().generationCount += 1;
        childUnit.gameObject.name = ("Unit Generation " + childUnit.gameObject.GetComponent<UnitController>().generationCount);
        childUnit.gameObject.GetComponent<UnitController>().sensoryRange = sensoryRange + (float)NextGaussianDouble() * sensoryRange * 0.05f;
        childUnit.gameObject.GetComponent<UnitController>().speed = speed + (float)NextGaussianDouble() * speed * 0.05f;
        




        //Get Colored based on new values
        unitColor = new Color((speed / 25), (sensoryRange / 50), (memoryLength / 100), 1.0f);
        rend.material.SetColor("_Color", unitColor);
    }

    public static double NextGaussianDouble()
    {
        double u, v, S;

        do
        {
            u = 2.0 * Random.value - 1.0;
            v = 2.0 * Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log((float)S) / (float)S);
        return u * fac;
    }

        private void OnTriggerStay(Collider collisionobject)
    {
        //Debug.Log(collisionobject);
        if (collisionobject.tag == "Food")
        {
            if (mouthOpen == true)
            {
                //Debug.Log("energy" + energy);
                //Debug.Log("energyContent" + collisionobject.gameObject.GetComponent<food>().energyContent);

                energy += collisionobject.gameObject.GetComponent<food>().energyContent;
                Destroy(collisionobject.gameObject);
                mouthOpen = false;
                //remeber this
                //Wander();
                Hunt();
            }
        }
    }
}
