using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    int audioPlayersSize;

    [SerializeField]
    GameObject audioPlayerPrefab;
    [SerializeField]
    AudioClip _tileDestroyed;
    [SerializeField]
    AudioClip _specialTileCreated;
    [SerializeField]
    AudioClip _bombDestroyed;
    [SerializeField]
    AudioClip _missileDestroyed;
    [SerializeField]
    AudioClip _stoneDestroyed;
    [SerializeField]
    AudioClip _specialBombDestroyed;
    [SerializeField]
    AudioClip _missileBombDestroyed;
    [SerializeField]
    AudioClip _chestBought;
    [SerializeField]
    AudioClip _specialChestBought;
    [SerializeField]
    AudioClip _boosterBought;
    [SerializeField]
    AudioClip _coinsBought;

    [SerializeField]
    List<IntEventBus> _boughtEvents = new List<IntEventBus>();

    [SerializeField]
    AudioClip _menuMusic;
    [SerializeField]
    AudioClip _gameMusic;

    List<AudioSource> audioPlayers = new List<AudioSource>();
    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    VolumeOptionsSO volumeOptions;

    [SerializeField]
    EventBus volumeChanged;

    [SerializeField] 
    StringEventBus loadEvent;


    private void Awake()
    {
        volumeChanged.Event += SetVolume;
        loadEvent.Event += SceneChanged;
        BoardEvents.TileDestroyed += BoardEventsTileDestroyed; ;
        BoardEvents.SpecialTileDestroyed += BoardEventsSpecialTileDestroyed;

        volumeOptions.LoadOptions();
        for (int i = 0;i<audioPlayersSize;i++)
        {
            audioPlayers.Add(Instantiate(audioPlayerPrefab, gameObject.transform).GetComponent<AudioSource>());
        }
        foreach (IntEventBus bought in _boughtEvents)
        {
            bought.Event += BoughtEvent; ;
        }
        SetVolume();
    }


    private void BoughtEvent(int index)
    {
        switch (index)
        {
            case 1:
            case 2:
            case 3:
                PlaySound(_boosterBought);
                break;
            case 4: case 5:
                PlaySound(_chestBought);
                break;
            case 6:
                PlaySound(_specialChestBought);
                break;
        }
    }

    private void BoardEventsSpecialTileDestroyed(Vector2Int pos, SpecialTileCombination type)
    {
        switch (type)
        {
            case SpecialTileCombination.ROCKETBOMB:
                PlaySound(_missileBombDestroyed);
                 break;
            case SpecialTileCombination.MEGABOMB:
                PlaySound(_specialBombDestroyed);
                break;
            default:
                break;
        }
    }

    private void BoardEventsTileDestroyed(Vector2 pos, TileType type)
    {
        switch (type)
        {
            case TileType.SHIELD: case TileType.DAGGER: case TileType.ARROW: case TileType.WAND: case TileType.CHALICE:
                PlaySound(_tileDestroyed);
                break;
            case TileType.BOMB:
                PlaySound(_bombDestroyed);
                break;
            case TileType.VERTICAL_ROCKET: case TileType.HORIZONTAL_ROCKET:
                PlaySound(_missileDestroyed);
                break;
            case TileType.BOX:
                PlaySound(_stoneDestroyed);
                break;
        }
    }

    void PlaySound(AudioClip clip)
    {
        AudioSource source = audioPlayers.Find(e => e.isPlaying == false);
        if(source != null)
        {
            source.clip = clip;
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("No available audio source found");
        }
    }

    void SetVolume()
    {
        musicSource.volume = volumeOptions.MasterVolume * volumeOptions.MusicVolume;
        foreach(AudioSource audio in audioPlayers)
        {
            audio.volume = volumeOptions.MasterVolume * volumeOptions.SFXVolume;
        }
    }

    private void SceneChanged(string scene)
    {
        //I dont know lambda
        if (scene =="LevelScene")
        {
            Crossfade(_gameMusic, 4);
        }
        else if(musicSource.clip != _menuMusic)
        {
            Crossfade(_menuMusic, 4);
        }
    }

    private void Crossfade(AudioClip destination, float totalDuration)
    {
        float og_volume = musicSource.volume = volumeOptions.MasterVolume * volumeOptions.MusicVolume;

        musicSource.DOFade(0, totalDuration/2).onComplete = () =>
        {
            musicSource.clip = destination;
            musicSource.Play();
            musicSource.DOFade(og_volume, totalDuration/2);
        };
    }

}
