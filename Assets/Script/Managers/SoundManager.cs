using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    #region AudioClipRegion
    AudioSource audioSource;
    public AudioClip OnGameStart, GetStone, GetFood, GetWood, Construction, OpenMenu, BackMenu, Debris, WoodProducing;

    #endregion
    public Sounds DefaultSound;



    void OnEnable() {
        GameManager.I.OnGetStone += GetStoneSound;
        GameManager.I.OnGetFood += GetFoodSound;
        GameManager.I.OnGetWood += GetWoodSound;
        GameManager.I.OnGameStart += AmbienceSound;
        GameManager.I.OnConstruction += GetCostructionSound;
        GameManager.I.OnOpenMenu += GetOpenMenuSound;
        GameManager.I.OnGetDebris += GetDebrisSound;
        GameManager.I.OnBackMenu += SetBackMenuSound;
        GameManager.I.OnWoodProducing += AddWoodOnClickSound;

    }
    void OnDisable() {
        GameManager.I.OnGetStone -= GetStoneSound;
        GameManager.I.OnGetFood -= GetFoodSound;
        GameManager.I.OnGetWood -= GetWoodSound;
        GameManager.I.OnConstruction -= GetCostructionSound;
        GameManager.I.OnOpenMenu -= GetOpenMenuSound;
        GameManager.I.OnGetDebris -= GetDebrisSound;
        GameManager.I.OnBackMenu -= SetBackMenuSound;
        GameManager.I.OnWoodProducing -= AddWoodOnClickSound;
    }

    #region Api

    void GetStoneSound() {
        PlaySound(Sounds._getStone);
    }
    void GetFoodSound() {
        PlaySound(Sounds._getFood);
    }
    void GetWoodSound() {
        PlaySound(Sounds._getWood);
    }
    void AmbienceSound() {
        PlaySound(Sounds._ambience);
    }
    void GetCostructionSound() {
        PlaySound(Sounds._construction);
    }
    void GetOpenMenuSound() {
        PlaySound(Sounds._openMenu);
    }
    void GetDebrisSound() {
        PlaySound(Sounds._debris);
    }
    void SetBackMenuSound() {
        PlaySound(Sounds._backMenu);
    }
    void AddWoodOnClickSound() {
        PlaySound(Sounds._woodOnClick);
    }
    #endregion

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        PlaySound(DefaultSound);
    }

    public void PlaySound(Sounds _soundToPlay) {
        switch (_soundToPlay) {
            case Sounds._getStone:
                audioSource.clip = GetStone;
                break;
            case Sounds._getFood:
                audioSource.clip = GetFood;
                break;
            case Sounds._getWood:
                audioSource.clip = GetWood;
                break;
            case Sounds._ambience:
                audioSource.clip = OnGameStart;
                break;
            case Sounds._construction:
                audioSource.clip = Construction;
                break;
            case Sounds._openMenu:
                audioSource.clip = OpenMenu;
                break;
            case Sounds._debris:
                audioSource.clip = Debris;
                break;
            case Sounds._backMenu:
                audioSource.clip = BackMenu;
                break;
            case Sounds._woodOnClick:
                audioSource.clip = WoodProducing;
                break;
        }

        audioSource.Play();
    }
}
public enum Sounds {
    _getStone,
    _getFood,
    _getWood,
    _ambience,
    _construction,
    _openMenu,
    _debris,
    _backMenu,
    _woodOnClick
}

