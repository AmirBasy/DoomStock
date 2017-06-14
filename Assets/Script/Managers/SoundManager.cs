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
        GameManager.I.OnBackMenu += BackMenuSound;
        GameManager.I.OnWoodProducing += AddWoodOnClickSound;

    }
    void OnDisable() {
        GameManager.I.OnGetStone -= GetStoneSound;
        GameManager.I.OnGetFood -= GetFoodSound;
        GameManager.I.OnGetWood -= GetWoodSound;
        GameManager.I.OnConstruction -= GetCostructionSound;
        GameManager.I.OnOpenMenu -= GetOpenMenuSound;
        GameManager.I.OnGetDebris -= GetDebrisSound;
        GameManager.I.OnBackMenu -= BackMenuSound;
        GameManager.I.OnWoodProducing -= AddWoodOnClickSound;
    }

    #region Api

    public void GetStoneSound() {//fatto
        PlaySound(Sounds._getStone);
    }
    public void GetFoodSound() {//fatto
        PlaySound(Sounds._getFood);
    }
    public void GetWoodSound() {//fatto
        PlaySound(Sounds._getWood);
    }
    public void AmbienceSound() { //fatto
        PlaySound(Sounds._ambience);
    }
    public void GetCostructionSound() { //fatto
        PlaySound(Sounds._construction);
    }
    public void GetOpenMenuSound() {//fatto
        PlaySound(Sounds._openMenu);
    }
    public void GetDebrisSound() {//fatto
        PlaySound(Sounds._debris);
    }
    public void BackMenuSound() {//fatto
        PlaySound(Sounds._backMenu);
    }
    public void AddWoodOnClickSound() {//fatto
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
        AudioSource audioTemp = gameObject.AddComponent<AudioSource>();
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
                audioSource.loop = true;
                break;
            case Sounds._construction:
                audioSource.clip = Construction;
                break;
            case Sounds._openMenu:
                audioSource.clip = OpenMenu;
                break;
            case Sounds._debris:
                audioSource.clip = Debris;
                audioSource.loop = false;
                break;
            case Sounds._backMenu:
                audioSource.clip = BackMenu;
                break;
            case Sounds._woodOnClick:
                audioSource.clip = WoodProducing;
                break;
        }
        audioTemp = audioSource;
        audioSource.Play();
    }
}
public enum Sounds {
    _null,
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

