using UnityEngine;
using System.Collections;
using System;

public class SoundPlayer : MonoBehaviour {

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
    public AudioClip sinkingCargoShipSounf;
    public AudioClip sinkingPirateShipSound;
    public AudioClip upgradeSound;
    public AudioClip windTurbineSound;

    public AudioClip startGameSound;
    public AudioClip endGameSound;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundManager!");
        }
        Instance = this;
        this.Client = GameObject.Find("NetWork").GetComponent<Client>();
        this.Client.MessageSoundEvent += Client_MessageSoundEvent;
    }

    private void Client_MessageSoundEvent(object sender, MessageEventArgs e)
    {
        int soundId = Int32.Parse((string)e.message.Split('@')[2]);

        switch(soundId)
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
        }
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
        MakeSound(sinkingCargoShipSounf);
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
}
