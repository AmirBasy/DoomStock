using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public BuildingView buildingViewPrefab;

    #region Labourers
    private int labourers = 1;

    public int Labourers
    {
        get
        {

            return labourers;
        }
        set
        {
            SetBuildingTime();

            labourers = value;
        }
    }

    #endregion


    /// <summary>
    /// Prende tutti gli Edifici che stanno nella cartella Resources/Cards per evitare che i dati vengano salvati.
    /// </summary>
    /// <returns></returns>
    public static List<BuildingData> GetAllBuildings()
    {   
        BuildingData[] allBuildingfromResources = Resources.LoadAll<BuildingData>("Building");
        List<BuildingData> newBuildingList = new List<BuildingData>();
        foreach (BuildingData buildingPointer in newBuildingList)
        {
            newBuildingList.Add(new BuildingData
            {
                PeopleLimit = buildingPointer.PeopleLimit,
                dimension = buildingPointer.dimension,
                timeMultiplier = buildingPointer.dimension,
                MyResources = buildingPointer.MyResources,
                Population = buildingPointer.Population
            }
            );
        }
        return newBuildingList;
    }


        /// <summary>
        /// Moltiplica i lavoratori per il multiplier del tempo
        /// </summary>
        void SetBuildingTime()
    {
        buildingViewPrefab.Data.timeMultiplier *= Labourers;
    }


    private void Update()
    {

        if (Labourers >= 1)
        {
            //increaseDimension();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Labourers += 1;
        }
        //if (buildingViewPrefab.Data.dimension > 20)/// inserita solo per testare
        //{
        //    this.transform.localScale = new Vector3(1, 2, 1);
        //}
    }

    /// <summary>
    /// fa crescere la dimensione nel tempo
    /// </summary>
    void increaseDimension() { buildingViewPrefab.Data.dimension += Time.deltaTime * buildingViewPrefab.Data.timeMultiplier; }

    public virtual void UpdateGraphic(string newText)
    {
        buildingViewPrefab.ActualPeople.text = newText;
    }
}

