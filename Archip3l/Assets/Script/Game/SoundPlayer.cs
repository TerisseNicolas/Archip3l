using UnityEngine;
using System.Collections;
using System;

public class SoundPlayer : MonoBehaviour {

    public static SoundPlayer Instance;
    private Client Client;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundManager!");
            Destroy(gameObject);
        }
        Instance = this;
        if (GameObject.Find("Network") != null)
            this.Client = GameObject.Find("Network").GetComponent<Client>();
    }

    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position, 10f);
    }

    private void playBuildingSoung(string typeBuilding)
    {
        switch(typeBuilding)
        {
            case "Airport":
                playAirportSound();
                break;
            case "AmusementPark":
                playAmusementParkSound();
                break;
            case "Church":
                playChurchSound();
                break;
            case "Cinema":
                playCinemaSound();
                break;
            case "Factory":
                playFactorySound();
                break;
            case "Farm":
                playFarmSound();
                break;
            case "Harbor":
                playHarborSound();
                break;
            case "Hotel":
                playHotelSound();
                break;
            case "Lab":
                playLabSound();
                break;
            case "GoldMine":
            case "StoneMine":
                playMineSound();
                break;
            case "OilPlant":
                playOilPlantSound();
                break;
            case "PowerPlant":
                playPowerPlantSound();
                break;
            case "School":
                playSchoolSound();
                break;
            case "SawMill":
                playSawMilSound();
                break;
            
        }
    }

    public void playAirportSound()
    {
        this.Client.sendData("@30800@1");
    }
    public void playAmusementParkSound()
    {
        this.Client.sendData("@30800@2");
    }
    public void playChurchSound()
    {
        this.Client.sendData("@30800@3");
    }
    public void playCinemaSound()
    {
        this.Client.sendData("@30800@4");
    }
    public void playConstructionSound()
    {
        this.Client.sendData("@30800@19");
    }
    public void playFactorySound()
    {
        this.Client.sendData("@30800@5");
    }
    public void playFarmSound()
    {
        this.Client.sendData("@30800@6");
    }
    public void playHarborSound()
    {
        this.Client.sendData("@30800@7");
    }
    public void playHotelSound()
    {
        this.Client.sendData("@30800@8");
    }
    public void playLabSound()
    {
        this.Client.sendData("@30800@9");
    }
    public void playMineSound()
    {
        this.Client.sendData("@30800@10");
    }
    public void playOilPlantSound()
    {
        this.Client.sendData("@30800@11");
    }
    public void playPowerPlantSound()
    {
        this.Client.sendData("@30800@12");
    }
    public void playSawMilSound()
    {
        this.Client.sendData("@30800@13");
    }
    public void playSchoolSound()
    {
        this.Client.sendData("@30800@14");
    }
    public void playSinkingCargoShipSound()
    {
        this.Client.sendData("@30800@15");
    }
    public void playSinkingPirateShipSound()
    {
        this.Client.sendData("@30800@16");
    }
    public void playUpgradeSound()
    {
        this.Client.sendData("@30800@17");
    }
    public void playWindTurbineSound()
    {
        this.Client.sendData("@30800@18");
    }
    public void playBriefingLetterSound()
    {
        this.Client.sendData("@30800@20");
    }
    public void playCrashPirateShipSound()
    {
        this.Client.sendData("@30800@21");
    }
    public void playApplauseSound()
    {
        this.Client.sendData("@30800@22");
    }
    public void playPerturbationSound()
    {
        this.Client.sendData("@30800@23");
    }
    public void playTrophySound()
    {
        this.Client.sendData("@30800@24");
    }
    public void playEndGameSound()
    {
        this.Client.sendData("@30800@25");
    }
    public void playGrasshopperSound()
    {
        this.Client.sendData("@30800@26");
    }
    public void playPirateShipArrivalSound()
    {
        this.Client.sendData("@30800@27");
    }
    public void playThunderSound()
    {
        this.Client.sendData("@30800@28");
    }
    public void playLoadingSound()
    {
        this.Client.sendData("@30800@29");
    }
}
