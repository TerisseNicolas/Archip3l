using UnityEngine;
using System.Collections;
using System;

public class SoundPlayer : MonoBehaviour {

    public static SoundPlayer Instance;
    private Client Client;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundManager!");
        }
        Instance = this;
        this.Client = GameObject.Find("Network").GetComponent<Client>();
    }

    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position, 10f);
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
}
