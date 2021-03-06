﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

//[RequireComponent(typeof(LineRenderer))]
public class UnitController : MonoBehaviour
{
    [Header("Main Variables")]
    public bool playerCell = false;
    public GameObject SpeciesHolder;
    public GameObject GameController;
    public NavMeshAgent agent;
    public float timer = 0.0f;
    public float distanceTraveled = 0.0f;
    public GameObject sensoryRangeDisplay;
    bool mouthOpen = false;
    Vector3 lastPosition;

    [Range(0.0f, 25.0f)]
    public float memoryLength = 10.0f;
    public float memoryLengthMin = 0.0f;
    public float memoryLengthMax = 30.0f;
    float memoryLengthUsage;
    
    

    
    //[SerializeField]
    float itemXSpread = 30f;
    //[SerializeField]
    float itemYSpread = 0;
    //[SerializeField]
    float itemZSpread = 30f;

    // Start is called before the first frame update
    public float energy = 100.0f;
    
    [Header("Variables")]
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

    public float mutationRate = 0.2f; //Mutation Rate
    public float mutationRateMin = 0.00001f;
    public float mutationRateMax = 1.0f;

    public float movementEfficiency = 0.00003f;

    public float reproductionChance = 0.0f; //reproduction chance each frame
    public float reproductionCooldown = 2.0f;
    public float reproductionTimer;
    public float reproductionRate = 100.0f;
    public float reproductionRateMin = 50.0f;
    public float reproductionRateMax = 150.0f;

    public float decisivness = 1.0f; //the greater this is, the more likely a cell is to go for further food
    public float decisivnessmin = 0.0f;
    public float decisivnessmax = 10.0f;

    public float distanceChoice = 0.0f; //more likely to go for nth food.

    public float lifespan = 30.0f;
    public float lifeTimer = 0.0f;

    [Header("Lists")]
    public Collider[] detectedObjects;
    public List<System.Action> actionList = new List<System.Action>();
    public List<GameObject> detectedFoodObjects = new List<GameObject>();

    public Vector3 targetDestination;
    public Vector3 foodTarget;
    public Color unitColor;
    public int generationCount;

    Renderer rend;

    /*
     int segments = 10;
     float xradius = 5;
    public float yradius = 5;
    LineRenderer line;
    */

    void Start()
    {
        acceleration = speed;
        turnSpeed = speed;


        lastPosition = transform.position;
        rend = GetComponent<Renderer>();

        updateColor();

        actionList.Add(Hunt);
        actionList.Add(Wander);

        /*
        //make circle
        line = gameObject.GetComponent<LineRenderer>();
        //line.SetVertexCount(segments + 1);
        line.positionCount = (segments + 1);
        line.useWorldSpace = false;
        CreateCircle();
        */
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
        energy -= distanceTraveled * movementEfficiency * (speed / speedMax);

        //reproduction logic: Update reproduction chance and determine if cell will reproduce

        if(playerCell == true)
        {
            //Do the player defined mutations
            // check if this is the newest generation
            //if it is, set the value in the GUI
            

            sensoryRange = sensoryRange + (float)NextGaussianDouble() * sensoryRange * 0;
            speed = speed + (float)NextGaussianDouble() * speed * 0;
            memoryLength = memoryLength + (float)NextGaussianDouble() * memoryLength * 0;
            decisivness = decisivness + (float)NextGaussianDouble() * decisivness * 0;
        }



        if (reproductionTimer >= reproductionCooldown)
        {
            reproductionTimer=0.0f;
            reproductionChance += ((energy - 50) / 1000) * (reproductionRate / 100.0f);
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

        if(lifeTimer >= lifespan)
        {
            Destroy(gameObject);
        }
        else
        {
            lifeTimer += Time.deltaTime;
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
            //detectedFoodObjects[0].gameObject.GetComponent<food>().energyContent - future code to allow choice by energy content
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


    public static float Mutate(float cellTrait, float traitMin, float traitMax, float mutationRate)
   {
        return Mathf.Clamp(cellTrait + (float)NextGaussianDouble() * cellTrait * mutationRate, traitMin, traitMax);
   }
    void Reproduce()
    {

        energy = (energy - 5.0f) * 0.5f;

        reproductionChance = 0.0f;
        GameObject childUnit = Instantiate(gameObject,gameObject.transform.position,Quaternion.identity);

        //Stops and resets the navmesh agent - ought to fix some bugs.

        childUnit.gameObject.GetComponent<UnitController>().agent.isStopped = true;
        childUnit.gameObject.GetComponent<UnitController>().agent.ResetPath();

        //Parent it to the correct species
        childUnit.transform.parent = SpeciesHolder.transform;
        //Increse it's Generation and then rename it to that generation
        childUnit.gameObject.GetComponent<UnitController>().generationCount += 1;
        childUnit.gameObject.GetComponent<UnitController>().lifeTimer = 0.0f;
        childUnit.gameObject.name = ("Unit Generation " + childUnit.gameObject.GetComponent<UnitController>().generationCount);
        reproductionTimer += Random.value; //offsets reproduction time for child so the species doesn't reproduce in descrete increments.

        if (playerCell == false)
        {
            //Mutates the child if it's not a player cell
            childUnit.gameObject.GetComponent<UnitController>().speed = Mutate(speed, speedMin, speedMax, mutationRate);
                childUnit.gameObject.GetComponent<UnitController>().acceleration = childUnit.gameObject.GetComponent<UnitController>().speed;
                childUnit.gameObject.GetComponent<UnitController>().turnSpeed = childUnit.gameObject.GetComponent<UnitController>().speed;

            childUnit.gameObject.GetComponent<UnitController>().memoryLength = Mutate(memoryLength, memoryLengthMin, memoryLengthMax, mutationRate);
            childUnit.gameObject.GetComponent<UnitController>().sensoryRange = Mutate(sensoryRange, sensoryRangeMin, sensoryRangeMax, mutationRate);
            childUnit.gameObject.GetComponent<UnitController>().decisivness = Mutate(decisivness, decisivnessmin, decisivnessmax, mutationRate);
            //childUnit.gameObject.GetComponent<UnitController>().mutationRate = Mutate(mutationRate, mutationRateMin, mutationRateMax, mutationRate);  //allows variable mutation rate
            childUnit.gameObject.GetComponent<UnitController>().reproductionRate = Mutate(reproductionRate, reproductionRateMin, reproductionRateMax, mutationRate);
        }
        if (playerCell == true)
        {
            //add evolution points
            GameController.gameObject.GetComponent<GameController>().playerEvolutionPoints = GameController.gameObject.GetComponent<GameController>().playerEvolutionPoints + GameController.gameObject.GetComponent<GameController>().pointsPerSplit;
            GameController.gameObject.GetComponent<GameController>().evolutionPointsText.text = ""+GameController.gameObject.GetComponent<GameController>().playerEvolutionPoints;
        }

            //Make circle
            //childUnit.gameObject.GetComponent<UnitController>().CreateCircle()

            //Make circle
            //childUnit.gameObject.GetComponent<UnitController>().CreateCircle();




            //Get Colored based on new values
            updateColor();

    }
    public void updateColor()
    {
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
    /*
    void CreateCircle()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * sensoryRange;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * sensoryRange;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }
    */

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
