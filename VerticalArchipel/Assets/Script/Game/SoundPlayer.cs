using UnityEngine;
using System.Collections;
using System;

public class SoundPlayer : MonoBehaviour
{

    public static SoundPlayer Instance;
    private Client Client;

    //Sound sources
    public AudioClip airportSound;
    public AudioClip amusementParkSound;
    public AudioClip churchSound;
    public AudioClip cinemaSound;
    public AudioClip constructionSound;
    public AudioClip factorySound;
    public AudioClip farmSound;
    public AudioClip harborSound;
    public AudioClip hotelsound;
    public AudioClip labSound;
    public AudioClip mineSound;
    public AudioClip oilPlantSound;
    public AudioClip powerPlantSound;
    public AudioClip sawMillSound;
    public AudioClip schoolSound;
    public AudioClip sinkingCargoShipSound;
    public AudioClip sinkingPirateShipSound;
    public AudioClip crashPirateShipSound;
    public AudioClip pirateShipArrivalSound;
    public AudioClip upgradeSound;
    public AudioClip windTurbineSound;

    public AudioClip perturbationSound;
    public AudioClip grasshopperSound;
    public AudioClip thunderSound;
    public AudioClip applauseSound;
    public AudioClip trophySound;

    public AudioClip briefingLetterSound;
    public AudioClip loadingSound;

    public AudioClip startGameSound;
    public AudioClip endGameSound;

    private int soundId = 0;

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundManager!");
        }
        Instance = this;
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSoundEvent += Client_MessageSoundEvent;
    }

    void Update()
    {
        if(this.soundId != 0)
        {
            switch (this.soundId)
            {
                case 1:
                    playAirportSound();
                    break;
                case 2:
                    playAmusementParkSound();
                    break;
                case 3:
                    playCinemaSound();
                    break;
                case 4:
                    playCinemaSound();
                    break;
                case 5:
                    playFactorySound();
                    break;
                case 6:
                    playFarmSound();
                    break;
                case 7:
                    playHarborSound();
                    break;
                case 8:
                    playHotelSound();
                    break;
                case 9:
                    playLabSound();
                    break;
                case 10:
                    playMineSound();
                    break;
                case 11:
                    playOilPlantSound();
                    break;
                case 12:
                    playPowerPlantSound();
                    break;
                case 13:
                    playSawMilSound();
                    break;
                case 14:
                    playSchoolSound();
                    break;
                case 15:
                    playSinkingCargoShipSound();
                    break;
                case 16:
                    playSinkingPirateShipSound();
                    break;
                case 17:
                    playUpgradeSound();
                    break;
                case 18:
                    playWindTurbineSound();
                    break;
                case 19:
                    playConstructionSound();
                    break;
                case 20:
                    playBriefingLetterSound();
                    break;
                case 21:
                    playCrashPirateShipSound();
                    break;
                case 22:
                    playApplauseSound();
                    break;
                case 23:
                    playPerturbationSound();
                    break;
                case 24:
                    playTrophySound();
                    break;
                case 25:
                    playEndGameSound();
                    break;
                case 26:
                    playGrasshopperSound();
                    break;
                case 27:
                    playPirateShipArrivalSound();
                    break;
                case 28:
                    playThunderSound();
                    break;
                case 29:
                    playLoadingSound();
                    break;
                case 30:
                    playStartGameSound();
                    break;
            }
            this.soundId = 0;
        }
    }

    private void Client_MessageSoundEvent(object sender, MessageEventArgs e)
    {
        this.soundId = Int32.Parse((string)e.message.Split('@')[2]);
    }

    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position, 10f);
    }

    public void playAirportSound()
    {
        MakeSound(airportSound);
    }
    public void playAmusementParkSound()
    {
        MakeSound(amusementParkSound);
    }
    public void playChurchSound()
    {
        MakeSound(churchSound);
    }
    public void playCinemaSound()
    {
        MakeSound(cinemaSound);
    }
    public void playConstructionSound()
    {
        MakeSound(constructionSound);
    }
    public void playFactorySound()
    {
        MakeSound(factorySound);
    }
    public void playFarmSound()
    {
        MakeSound(farmSound);
    }
    public void playHarborSound()
    {
        MakeSound(harborSound);
    }
    public void playHotelSound()
    {
        MakeSound(hotelsound);
    }
    public void playLabSound()
    {
        MakeSound(labSound);
    }
    public void playMineSound()
    {
        MakeSound(mineSound);
    }
    public void playOilPlantSound()
    {
        MakeSound(oilPlantSound);
    }
    public void playPowerPlantSound()
    {
        MakeSound(powerPlantSound);
    }
    public void playSawMilSound()
    {
        MakeSound(sawMillSound);
    }
    public void playSchoolSound()
    {
        MakeSound(schoolSound);
    }
    public void playSinkingCargoShipSound()
    {
        MakeSound(sinkingCargoShipSound);
    }
    public void playSinkingPirateShipSound()
    {
        MakeSound(sinkingPirateShipSound);
    }
    public void playUpgradeSound()
    {
        MakeSound(upgradeSound);
    }
    public void playWindTurbineSound()
    {
        MakeSound(windTurbineSound);
    }
    public void playBriefingLetterSound()
    {
        MakeSound(briefingLetterSound);
    }
    public void playCrashPirateShipSound()
    {
        MakeSound(crashPirateShipSound);
    }
    public void playApplauseSound()
    {
        MakeSound(applauseSound);
    }
    public void playPerturbationSound()
    {
        MakeSound(perturbationSound);
    }
    public void playTrophySound()
    {
        MakeSound(trophySound);
    }
    public void playEndGameSound()
    {
        MakeSound(endGameSound);
    }
    public void playGrasshopperSound()
    {
        MakeSound(grasshopperSound);
    }
    public void playPirateShipArrivalSound()
    {
        MakeSound(pirateShipArrivalSound);
    }
    public void playThunderSound()
    {
        MakeSound(thunderSound);
    }
    public void playLoadingSound()
    {
        MakeSound(loadingSound);
    }
    public void playStartGameSound()
    {
        MakeSound(startGameSound);
    }
}
