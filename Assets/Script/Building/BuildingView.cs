using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;

public class BuildingView : MonoBehaviour
{
    #region Proprietà
    [Header("View")]

    /// <summary>
    /// Dato della view.
    /// </summary>
    public BuildingData Data;
    public Mesh Pino;
    MeshFilter CurrentMesh;
    public Mesh Ceppo;
    public Mesh Macerie;
    public Animation animation;
    ParticlesController _particle;

    #endregion

    #region LifeCycle
    private void Start()
    {
        Data.Init();
        if (Data.ID != "Foresta")
        {
            Data._particleController = GetComponentInChildren<ParticlesController>();
            _particle = Data._particleController;
            _particle.Init();
        }
        if (Data.ID != "Foresta")
        {
            SetBuildingStatus(BuildingState.Construction);
        }
        else
        {
            CurrentMesh = GetComponent<MeshFilter>();
            SetBuildingStatus(BuildingState.Ready);
            CurrentMesh.mesh = Pino;
        }
    }

    private void OnEnable()
    {
        TimeEventManager.OnEvent += OnUnitEvent;
    }
    private void OnDisable()
    {
        TimeEventManager.OnEvent -= OnUnitEvent;

    }
    #endregion

    #region API

    private void Update()
    {
        if (Data.Attack > 0)
        {
            Data.FireRateo -= Time.deltaTime;
            if (Data.FireRateo <= 0)
            {
                Data.AttackEnemy();
                Data.FireRateo = Data.StartingFireRateo;
            } 
        }
    }

    bool isAlive = true;

    /// <summary>
    /// Distrugge il building.
    /// </summary>
    public void destroyMe()
    {
        if (isAlive)
        {
            isAlive = false;
            if (OnDestroy != null)
                OnDestroy(this);
            CellDoomstock cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
            cell.SetStatus(CellDoomstock.CellStatus.Debris, cell.building);
            if (Data.ID != "Casa")
                Data.RemoveAllPopulationFromBuilding();
            TimeEventManager.OnEvent -= OnUnitEvent;
            SetBuildingStatus(BuildingState.Destroyed);

            List<Transform> myobject = gameObject.GetComponentsInChildren<Transform>().ToList();
            myobject.Remove(transform);
            if (Data.ID == "Meraviglia")
            {
                GameManager.I.OnMeravigliaDestroyed();
            }
            foreach (Transform go in myobject)
            {
                go.gameObject.SetActive(false);
            }
            CurrentMesh.mesh = Macerie;
            gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            transform.eulerAngles = new Vector3(90, 20, 0); 
        }
    }

    /// <summary>
    /// Toglie le macerie, rende libera la cella e recupera 1/4 del materiale.
    /// </summary>
    public void RemoveDebris()
    {
        GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y].SetStatus(CellDoomstock.CellStatus.Empty);
        GameManager.I.GetResourceDataByID("Wood").Value += Data.GetActualWoodValue();
        GameManager.I.GetResourceDataByID("Stone").Value += Data.GetActualWoodValue();
        if (OnRemoveDebris != null)
            OnRemoveDebris(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// inizializzazione del buildingData.
    /// </summary>
    /// <param name="_buildingData"></param>
    public void Init(BuildingData _buildingData)
    {
        Data.isAttacking = false;
        Data = _buildingData;
        TimeEventManager.OnEvent += OnUnitEvent;
        Data.StartingFireRateo = Data.FireRateo;
    }

    public void SetBuildingStatus(BuildingState _status)
    {
        Data.CurrentState = _status;
        OnStatusChanged();
    }
    /// <summary>
    /// Chiamata ogni volta che viene settato lo status di un edificio.
    /// </summary>
    public void OnStatusChanged()
    {
        switch (Data.CurrentState)
        {
            case BuildingState.Construction:
                GameManager.I.soundManager.GetCostructionSound();
                CurrentMesh = GetComponent<MeshFilter>();
                if (Data.ID != "Meraviglia")
                {
                    transform.DOMoveY(transform.position.y + 1, Data.BuildingTime).OnComplete(() => { SetBuildingStatus(BuildingState.Built); });
                }
                else
                {
                    SetBuildingStatus(BuildingState.Built);
                }
                break;
            case BuildingState.Built:
                Data.Delay = 0;
                if (Data.Population.Count < 1)
                    AnimationStop(Data);
                if (Data.ID == "Foresta")
                    CurrentMesh.mesh = Pino;
                break;
            case BuildingState.Producing:
                AnimationStart(Data);
                if (Data.ID != "Foresta") ;
                break;
            case BuildingState.Ready:
                AnimationStop(Data);
                CellDoomstock cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
                switch (Data.ID)
                {
                    case "Foresta":
                        CurrentMesh.mesh = Pino;
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitWood, cell);
                        break;
                    case "Cava":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitStone, cell);
                        break;
                    case "Chiesa":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitFaith, cell);
                        break;
                    case "Fattoria":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitFood, cell);
                        break;
                    case "Muro":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitSpirit, cell);
                        break;
                    case "Torretta":
                        GameManager.I.messagesManager.ShowiInformation(MessageLableType.LimitSpirit, cell);
                        break;
                    default:
                        break;
                }
                break;
            case BuildingState.Waiting:


                if (Data.ID == "Foresta")
                    CurrentMesh.mesh = Ceppo;

                break;
            case BuildingState.Destroyed:
                CurrentMesh.mesh = Macerie;
                break;
            default:
                break;
        }
    }

    public void LimitReached(BaseResourceData res)
    {
        if (res.Value >= res.Limit)
        {
            res.Value = 0;
            SetBuildingStatus(BuildingState.Ready);

        }
    }


    #region Animation
    public void AnimationStart(BuildingData _building)
    {

        if (animation != null)
        {
            animation = GetComponent<Animation>();
            animation.Play();

            foreach (AnimationState state in animation)
            {
                state.speed = 0.3f;
            }
        }

    }
    public void AnimationStop(BuildingData _building)
    {
        StartCoroutine(StopAnimation(2));

    }

    IEnumerator StopAnimation(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        if (animation != null)
        {
            animation.Stop();
        }
    }
    #endregion


    #endregion

    #region Events

    public delegate void BuildingEvent(BuildingView _buildingView);
    public static BuildingEvent OnDestroy;
    public static BuildingEvent OnRemoveDebris;


    void OnUnitEvent(TimedEventData _eventData)
    {
        switch (_eventData.ID)
        {
            case "Production":
                CellDoomstock cell = null;
                if (Data.CurrentState == BuildingState.Producing && Data.ID != "Foresta" && Data.ID != "Meraviglia")
                {
                    cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += (int)(Data.Population.Count * 1);
                        LimitReached(res);
                        switch (res.ID)
                        {
                            case "Faith":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FaithProduction, cell, true, "1");
                                break;
                            case "Stone":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.StoneProduction, cell, true, "1");
                                break;
                            case "Food":
                                GameManager.I.messagesManager.ShowiInformation(MessageLableType.FoodProduction, cell, true, "1");
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (Data.CurrentState == BuildingState.Producing && Data.ID == "Foresta")
                {
                    cell = GameManager.I.gridController.Cells[(int)Data.GetGridPosition().x, (int)Data.GetGridPosition().y];
                    foreach (var res in Data.BuildingResources)
                    {
                        res.Value += 1;
                        LimitReached(res);
                        if (res.ID == "Wood")
                        {
                            GameManager.I.messagesManager.ShowiInformation(MessageLableType.WoodProduction, cell, true, "1");
                        }
                    }
                }
                break;
            case "Delay":
                if (Data.CurrentState == BuildingState.Waiting)
                {
                    Data.Delay++;
                    if (Data.Population.Count > 0)
                        SetBuildingStatus(BuildingState.Producing);
                    if (Data.Delay >= Data.DelayLimit)
                    {
                        SetBuildingStatus(BuildingState.Built);
                    }
                }
                break;
            default:
                break;
        }
    }

    #endregion


    //public void BarrettaSetColor()
    //{
    //    if (BarrettaGrow <= 0.25)
    //    {
    //        barretta.color = Color.red;
    //    }
    //    if (BarrettaGrow >= 0.25 && BarrettaGrow <= 0.75)
    //    {
    //        barretta.color = Color.yellow;
    //    }
    //    if (BarrettaGrow >= 0.75)
    //    {
    //        barretta.color = Color.green;
    //        BarrettaGrow = 1;
    //    }
    //}
}
