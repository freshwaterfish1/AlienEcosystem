using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    public float energyContent = 10;

    void Start()
    {
        energyContent = Random.Range(10, 50);
        
        gameObject.transform.localScale = new Vector3(((energyContent - 10) / 40), ((energyContent - 10) / 40), ((energyContent - 10) / 40));
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

}
