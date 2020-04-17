using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodSpawner : MonoBehaviour
{
    public GameObject food;
    public GameObject foodParent;
    public float spawnRatehMin = 1.0f;
    public float spawnRatehMax = 5.0f;
     float memoryLength;
     float memoryLengthUsage;
     float spawnRadius = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        memoryLength = Random.Range(spawnRatehMin, spawnRatehMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (memoryLengthUsage <= 0)
        {
            memoryLengthUsage = memoryLength;

            Vector3 pos = RandomCircle(gameObject.transform.position, spawnRadius);

            Instantiate(food, pos, Quaternion.identity, foodParent.transform);
            //Debug.Log("New Action");
        }
        else
        {
            memoryLengthUsage -= Time.deltaTime;
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

        return pos;
    }

   


}
