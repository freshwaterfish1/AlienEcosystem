﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject[] itemToSpawn;

    [SerializeField]
    float itemXSpread = 30f;

    [SerializeField]
    float itemYSpread = 0;

    [SerializeField]
    float itemZSpread = 30f;

    public int numbItemsToSpawn = 0;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < numbItemsToSpawn; i++)
            {
                SpreadItem();
            }
        }
    }


    void SpreadItem()
    {
        /*
        GameObject toprightfood = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], new Vector3(itemXSpread, 0, itemZSpread), Quaternion.identity, gameObject.transform);
        GameObject bottomleft = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], new Vector3(-itemXSpread, 0, -itemZSpread), Quaternion.identity, gameObject.transform);
        GameObject bottomright = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], new Vector3(-itemXSpread, 0, itemZSpread), Quaternion.identity, gameObject.transform);
        GameObject topleft = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], new Vector3(itemXSpread, 0, -itemZSpread), Quaternion.identity, gameObject.transform);
        */

        Vector3 randPosition = new Vector3((Random.Range(-itemXSpread, itemXSpread)), (Random.Range(-itemYSpread, itemYSpread)), (Random.Range(-itemZSpread, itemZSpread)));

        GameObject spawnedFood = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], randPosition, Quaternion.identity, gameObject.transform);
        //random size
        //spawnedFood.transform.localScale = new Vector3(Random.Range(.7f, 1.2f), Random.Range(.7f, 1.2f), Random.Range(.7f, 1.2f));
    }

}