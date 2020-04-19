using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{



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
    /*
    [SerializeField]
    float itemXSpread = 30f;

    [SerializeField]
    float itemYSpread = 0;

    [SerializeField]
    float itemZSpread = 30f;
    */


    public TextMeshProUGUI AspeedAvergeText;
    public TextMeshProUGUI AspeedGenerationText;

    public TextMeshProUGUI AmemoryAvergeText;
    public TextMeshProUGUI AmemoryGenerationText;

    public TextMeshProUGUI AsenserangeAvergeText;
    public TextMeshProUGUI AsenserangeGenerationText;

    public TextMeshProUGUI BspeedAvergeText;
    //public TextMeshProUGUI BspeedGenerationText;
    public TextMeshProUGUI BmemoryAvergeText;
    //public TextMeshProUGUI BmemoryGenerationText;
    public TextMeshProUGUI BsenserangeAvergeText;
    //public TextMeshProUGUI BsenserangeGenerationText;

    public TextMeshProUGUI evolutionPointsText;


    public float playerEvolutionPoints = 0;

    [Range(0.0f, 1.0f)]
    public float pointsPerSplit = .2f;

    // Start is called before the first frame update
    void Start()
    {
        evolutionPointsText.text = (System.String.Format("{0:F1}", playerEvolutionPoints));
    }

    // Update is called once per frame
    void Update()
    {
        playerEvolutionPoints = Mathf.Round(playerEvolutionPoints * 100f) / 100f;
        SpeciesAText.text = "Number Alive: " + SpeciesAParent.transform.childCount;
        SpeciesBText.text = "Number Alive: " + SpeciesBParent.transform.childCount;
        SpeciesCText.text = "Number Alive: " + SpeciesCParent.transform.childCount;

        SpeciesAGeneration.text = "Generation: " + SpeciesAGenerationCount;
        SpeciesBGeneration.text = "Generation: " + SpeciesBGenerationCount;
        SpeciesCGeneration.text = "Generation: " + SpeciesCGenerationCount;
        float simTimeShort = simulationTimer.gameObject.GetComponent<simulationTimer>().simulatedTime;
        simTimeShort = Mathf.Round(simTimeShort * 10f) / 10f;
        simulationTime.text = "Simulation Time: " + System.String.Format("{0:F1}", simTimeShort) + " seconds";



        GeneticsMenu.SetActive(isMenuActive);
        GameUnits.SetActive(!isMenuActive);


        int AgenerationCount = 0;
        int AspeciesCount = 0;

        float AspeedGenerationValue = 0f;
        float AspeedSpeciesValue = 0f;

        float AmemoryGenerationValue = 0f;
        float AmemorySpeciesValue = 0f;
        
        float AsenserangeGenerationValue = 0f;
        float AsenserangeSpeciesValue = 0f;
        



        //Player Units
        foreach (Transform child in SpeciesAParent.transform)
        {
            AspeciesCount++;
            AspeedSpeciesValue += child.gameObject.GetComponent<UnitController>().speed;
            AmemorySpeciesValue += child.gameObject.GetComponent<UnitController>().memoryLength;
            AsenserangeSpeciesValue += child.gameObject.GetComponent<UnitController>().sensoryRange;

            if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
            {
                AgenerationCount++;
                AspeedGenerationValue += child.gameObject.GetComponent<UnitController>().speed;
                AmemoryGenerationValue += child.gameObject.GetComponent<UnitController>().memoryLength;
                AsenserangeGenerationValue += child.gameObject.GetComponent<UnitController>().sensoryRange;
            }

            if (child.gameObject.GetComponent<UnitController>().generationCount > SpeciesAGenerationCount)
            {
                SpeciesAGenerationCount = child.gameObject.GetComponent<UnitController>().generationCount;
            }


        }

        //total across species
        //total across species
        float AaverageSpeed = (AspeedSpeciesValue / AspeciesCount);
        float AaverageMemory = (AmemorySpeciesValue / AspeciesCount);
        float AaverageSenserange = (AsenserangeSpeciesValue / AspeciesCount);
        //Round the values to be nice
        AaverageSpeed = Mathf.Round(AaverageSpeed * 100f) / 100f;
        AaverageMemory = Mathf.Round(AaverageMemory * 100f) / 100f;
        AaverageSenserange = Mathf.Round(AaverageSenserange * 100f) / 100f;
        //set the value in the correct text boxes
        AspeedAvergeText.text = "Average Speed: " + System.String.Format("{0:F2}", AaverageSpeed);
        AspeedGenerationText.text = "" + (AspeedGenerationValue / AgenerationCount);

        AmemoryAvergeText.text = "Average Memory Length: " + System.String.Format("{0:F2}", AaverageMemory);
        AmemoryGenerationText.text = "" + (AmemoryGenerationValue / AgenerationCount);

        AsenserangeAvergeText.text = "Average Sensory Range: " + System.String.Format("{0:F2}", AaverageSenserange);
        AsenserangeGenerationText.text = "" + (AsenserangeGenerationValue / AgenerationCount);



        //int BgenerationCount = 0;
        int BspeciesCount = 0;

        //float BspeedGenerationValue = 0f;
        float BspeedSpeciesValue = 0f;

        //float BmemoryGenerationValue = 0f;
        float BmemorySpeciesValue = 0f;

        //float BsenserangeGenerationValue = 0f;
        float BsenserangeSpeciesValue = 0f;


        //CPU Units
        foreach (Transform child in SpeciesBParent.transform)
        {
            BspeciesCount++;
            BspeedSpeciesValue += child.gameObject.GetComponent<UnitController>().speed;
            BmemorySpeciesValue += child.gameObject.GetComponent<UnitController>().memoryLength;
            BsenserangeSpeciesValue += child.gameObject.GetComponent<UnitController>().sensoryRange;
            /*
            if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesBGenerationCount)
            {
                
                BgenerationCount++;
                BspeedGenerationValue += child.gameObject.GetComponent<UnitController>().speed;
                BmemoryGenerationValue += child.gameObject.GetComponent<UnitController>().memoryLength;
                BsenserangeGenerationValue += child.gameObject.GetComponent<UnitController>().sensoryRange;
                
            }
            */

            if (child.gameObject.GetComponent<UnitController>().generationCount > SpeciesBGenerationCount)
            {
                SpeciesBGenerationCount = child.gameObject.GetComponent<UnitController>().generationCount;
            }
        }

        //total across species
        //total across species
        float BaverageSpeed = (BspeedSpeciesValue / BspeciesCount);
        float BaverageMemory = (BmemorySpeciesValue / BspeciesCount);
        float BaverageSenserange = (BsenserangeSpeciesValue / BspeciesCount);
        //Round the values to be nice
        BaverageSpeed = Mathf.Round(BaverageSpeed * 100f) / 100f;
        BaverageMemory = Mathf.Round(BaverageMemory * 100f) / 100f;
        BaverageSenserange = Mathf.Round(BaverageSenserange * 100f) / 100f;
        //set the value in the correct text boxes
        BspeedAvergeText.text = "Average Speed: " + System.String.Format("{0:F2}", BaverageSpeed);
        //BspeedGenerationText.text = "" + (BspeedGenerationValue / BgenerationCount);
        BmemoryAvergeText.text = "Average Memory Length: " + System.String.Format("{0:F2}", BaverageMemory);
        //BmemoryGenerationText.text = "" + (BmemoryGenerationValue / BgenerationCount);
        BsenserangeAvergeText.text = "Average Sensory Range: " + System.String.Format("{0:F2}", BaverageSenserange);
        //BsenserangeGenerationText.text = "" + (BsenserangeGenerationValue / BgenerationCount);






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
    
    public void reloadGame()
    {
        SceneManager.LoadScene(0);
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

    public void updatePlayerEvoPoints()
    {
        playerEvolutionPoints--;
        evolutionPointsText.text = (System.String.Format("{0:F1}", playerEvolutionPoints));
    }
    //Player Forced Mutation
    public void mutateSpeed(bool pos)
    {
        if((pos == true) && (playerEvolutionPoints >=1))
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    child.gameObject.GetComponent<UnitController>().speed += 1;
                    updatePlayerEvoPoints();
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }
            
        }

        if ((pos == false) && (playerEvolutionPoints >= 1))
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    if (child.gameObject.GetComponent<UnitController>().speed > 1)
                    {
                        child.gameObject.GetComponent<UnitController>().speed -= 1;
                        updatePlayerEvoPoints();
                    }
 
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }
        }
        
    }

    public void mutateMemory(bool pos)
    {
        if ((pos == true) && (playerEvolutionPoints >= 1))
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    child.gameObject.GetComponent<UnitController>().memoryLength += .1f;
                    child.gameObject.GetComponent<UnitController>().memoryLength = Mathf.Round(child.gameObject.GetComponent<UnitController>().memoryLength * 10f) / 10f;
                    updatePlayerEvoPoints();
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }
            
        }

        if ((pos == false) && (playerEvolutionPoints >= 1))
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    if(child.gameObject.GetComponent<UnitController>().memoryLength > .1f)
                    {
                        child.gameObject.GetComponent<UnitController>().memoryLength -= .1f;
                        child.gameObject.GetComponent<UnitController>().memoryLength = Mathf.Round(child.gameObject.GetComponent<UnitController>().memoryLength * 10f) / 10f;
                        updatePlayerEvoPoints();
                    }
                    
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }

        }
        
    }

    public void mutateSensernage(bool pos)
    {
        if ((pos == true) && (playerEvolutionPoints >= 1))
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    child.gameObject.GetComponent<UnitController>().sensoryRange += 1;
                    updatePlayerEvoPoints();
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }
            
        }

        if ((pos == false) && (playerEvolutionPoints >= 1))
        {
            //find all childern
            foreach (Transform child in SpeciesAParent.transform)
            {

                if (child.gameObject.GetComponent<UnitController>().generationCount >= SpeciesAGenerationCount)
                {
                    if (child.gameObject.GetComponent<UnitController>().sensoryRange > 1)
                    {
                        child.gameObject.GetComponent<UnitController>().sensoryRange -= 1;
                        updatePlayerEvoPoints();
                    }
                        
                }
                child.gameObject.GetComponent<UnitController>().updateColor();
            }
            
        }

    }





}
