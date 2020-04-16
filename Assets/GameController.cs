using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject[] itemToSpawn;
    public TextMeshProUGUI SpeciesAText;
    public GameObject SpeciesAParent;
    public GameObject GeneticsMenu;
    public bool isMenuActive = false;

    public TextMeshProUGUI SpeciesBText;
    public GameObject SpeciesBParent;

    public TextMeshProUGUI SpeciesCText;
    public GameObject SpeciesCParent;

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
        SpeciesAText.text = "Species A: " + SpeciesAParent.transform.childCount;
        SpeciesBText.text = "Species B: " + SpeciesBParent.transform.childCount;
        SpeciesCText.text = "Species C: " + SpeciesCParent.transform.childCount;

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < numbItemsToSpawn; i++)
            {
                SpreadItem();
            }
        }

        GeneticsMenu.SetActive(isMenuActive);
        SpeciesAParent.SetActive(!isMenuActive);
        SpeciesBParent.SetActive(!isMenuActive);
        SpeciesCParent.SetActive(!isMenuActive);

        /*
        if(isMenuActive == true)
        {
            GeneticsMenu.SetActive(true);
        }
        else
        {
            GeneticsMenu.SetActive(false);
        }
        */
    }
    

    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
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
