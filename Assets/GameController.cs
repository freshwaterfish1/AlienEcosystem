using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject[] itemToSpawn;
    public GameObject foodParent;

    public GameObject GeneticsMenu;

    public GameObject WinScreen;
    public GameObject LossScreen;


    public GameObject GameUnits;
    public bool winOrLossOccored;
    public bool isMenuActive = false;




    public TextMeshProUGUI SpeciesAText;
    public TextMeshProUGUI SpeciesAGeneration;
    public GameObject SpeciesAParent;
    public int SpeciesAGenerationCount = 0;

    public TextMeshProUGUI SpeciesBText;
    public TextMeshProUGUI SpeciesBGeneration;
    public GameObject SpeciesBParent;
    public int SpeciesBGenerationCount = 0;

    public TextMeshProUGUI SpeciesCText;
    public TextMeshProUGUI SpeciesCGeneration;
    public GameObject SpeciesCParent;
    public int SpeciesCGenerationCount = 0;

    public GameObject simulationTimer;
    public TextMeshProUGUI simulationTime;

    [SerializeField]
    float itemXSpread = 30f;

    [SerializeField]
    float itemYSpread = 0;

    [SerializeField]
    float itemZSpread = 30f;

    public int numbItemsToSpawn = 0;

    public float timer = 3.0f;
    float timerTicker;

    public TextMeshProUGUI speedAvergeText;
    public TextMeshProUGUI speedGenerationText;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        SpeciesAText.text = "Number Alive: " + SpeciesAParent.transform.childCount;
        SpeciesBText.text = "Number Alive: " + SpeciesBParent.transform.childCount;
        SpeciesCText.text = "Number Alive: " + SpeciesCParent.transform.childCount;

        SpeciesAGeneration.text = "Generation: " + SpeciesAGenerationCount;
        SpeciesBGeneration.text = "Generation: " + SpeciesBGenerationCount;
        SpeciesCGeneration.text = "Generation: " + SpeciesCGenerationCount;
        float simTimeShort = simulationTimer.gameObject.GetComponent<simulationTimer>().simulatedTime;
        simTimeShort = Mathf.Round(simTimeShort * 10f) / 10f;
        simulationTime.text = "Simulation Time: " + System.String.Format("Value: {0:F1}", simTimeShort) + " seconds";


        if (timerTicker <= 0)
        {
            timerTicker = timer;
            SpreadItem();

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

        GeneticsMenu.SetActive(isMenuActive);
        GameUnits.SetActive(!isMenuActive);


        int generationCount = 0;
        int speciesCount = 0;

        float generationValue = 0f;
        float speciesValue = 0f;
        
        foreach (Transform child in SpeciesAParent.transform)
        {
            speciesCount++;
            speciesValue += child.gameObject.GetComponent<UnitController>().speed;

            if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
            {
                generationCount++;
                generationValue += child.gameObject.GetComponent<UnitController>().speed;
            }

            if (child.gameObject.GetComponent<UnitController>().generationCount > SpeciesAGenerationCount)
            {

                SpeciesAGenerationCount = child.gameObject.GetComponent<UnitController>().generationCount;
            }


        }
        //total across species

        float averageSpeed = (speciesValue / speciesCount);
        averageSpeed = Mathf.Round(averageSpeed * 10f) / 10f;
        speedAvergeText.text = "Average Speed: " + System.String.Format("Value: {0:F1}", averageSpeed);

        //generation value
        speedGenerationText.text = "Generation " + SpeciesAGenerationCount  + " Speed:" + (generationValue / generationCount);

        foreach (Transform child in SpeciesBParent.transform)
        {
            if (child.gameObject.GetComponent<UnitController>().generationCount > SpeciesBGenerationCount)
            {
                SpeciesBGenerationCount = child.gameObject.GetComponent<UnitController>().generationCount;
            }
        }

        foreach (Transform child in SpeciesCParent.transform)
        {
            if (child.gameObject.GetComponent<UnitController>().generationCount > SpeciesCGenerationCount)
            {
                SpeciesCGenerationCount = child.gameObject.GetComponent<UnitController>().generationCount;
            }
        }






        if ((SpeciesBParent.transform.childCount <= 0) && (SpeciesCParent.transform.childCount <= 0) && (winOrLossOccored == false))
        {
            Debug.Log("Game Win");
            ToggleWin();
            winOrLossOccored = true;
        }

        if ((SpeciesAParent.transform.childCount <= 0) && (winOrLossOccored == false))
        {
            Debug.Log("Game Loss");
            ToggleLoss();
            winOrLossOccored = true;
        }

    }
    

    public void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
    }

    public void ToggleWin()
    {
        WinScreen.SetActive(true);
    }

    public void ToggleLoss()
    {
        LossScreen.SetActive(true);
    }

    //Player Forced Mutation
    public void mutateSpeed(bool pos)
    {
        if(pos == true)
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    child.gameObject.GetComponent<UnitController>().speed += 1;
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }
        }

        if (pos == false)
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    child.gameObject.GetComponent<UnitController>().speed -= 1;
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
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
