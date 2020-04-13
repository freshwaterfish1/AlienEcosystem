using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public float timer = 0.0f;

    // Start is called before the first frame update
    public float energy = 100.0f;
    public float speed = 3.0f;
    public float sensoryRange = 10.0f;


    float consumptionrate = 1.0f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        energy -= (Time.deltaTime * consumptionrate);

        if(energy <= 0)
        {
            Debug.Log(gameObject + ("has died"));
            
            Destroy(gameObject);
        }
    }
}
