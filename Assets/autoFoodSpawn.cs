using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoFoodSpawn : MonoBehaviour
{
    public GameObject[] itemToSpawn;
    public GameObject foodParent;

    [SerializeField]
    float itemXSpread = 30f;

    [SerializeField]
    float itemYSpread = 0;

    [SerializeField]
    float itemZSpread = 30f;


    public int numbItemsToSpawn = 0;

    public float timer = 3.0f;
    float timerTicker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerTicker <= 0)
        {
            timerTicker = timer;
            for (int i = 0; i < numbItemsToSpawn; i++)
            {
                SpreadItem();
            }

        }
        else
        {
            timerTicker -= Time.deltaTime;
        }

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

        Vector3 randPosition = new Vector3((Random.Range(-itemXSpread, itemXSpread)), (Random.Range(-itemYSpread, itemYSpread)), (Random.Range(-itemZSpread, itemZSpread)));

        GameObject spawnedFood = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], randPosition, Quaternion.identity, gameObject.transform);
        spawnedFood.transform.parent = foodParent.transform;
        //random size
        //spawnedFood.transform.localScale = new Vector3(Random.Range(.7f, 1.2f), Random.Range(.7f, 1.2f), Random.Range(.7f, 1.2f));
    }

}
