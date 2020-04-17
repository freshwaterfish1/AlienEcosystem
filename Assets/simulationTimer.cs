using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simulationTimer : MonoBehaviour
{
    public float simulatedTime;

    // Update is called once per frame
    void Update()
    {
        simulatedTime += Time.deltaTime;
    }
}
