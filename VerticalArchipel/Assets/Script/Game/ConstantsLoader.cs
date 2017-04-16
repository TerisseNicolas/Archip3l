using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;

public class ConstantsLoader : MonoBehaviour {

    public static Dictionary<TypeConstant, string> constantValues = new Dictionary<TypeConstant, string>();
    private static string path = "parametres.xml";

	void Awake()
    {
        if(!ConstantsLoader.loadData())
        {
            Debug.LogWarning("Could not load all constants!!!");
        }
    }

    void Start()
    {
        //foreach(TypeConstant constant in Enum.GetValues(typeof(TypeConstant)))
        //{
        //    Debug.Log(constant.ToString() + " - " + getConstant(constant));
        //}
    }


    public static bool loadData()
    {
        String p = ConstantsLoader.path;
        if (File.Exists(p))
        {
            ArchipelConstants constants = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ArchipelConstants));
            StreamReader reader = new StreamReader(p);
            constants = (ArchipelConstants)serializer.Deserialize(reader);
            reader.Close();

            ConstantsLoader.constantValues.Add(TypeConstant.networkListeningPort, constants.networkListeningPort.value);
            ConstantsLoader.constantValues.Add(TypeConstant.networkSendingPort, constants.networkSendingPort.value);
            ConstantsLoader.constantValues.Add(TypeConstant.networkServerIP, constants.networkServerIP.value);
            ConstantsLoader.constantValues.Add(TypeConstant.fileScores, constants.fileScores.value);
            ConstantsLoader.constantValues.Add(TypeConstant.pirateBoatsIncreaseRate, constants.pirateBoatsIncreaseTime.value);
            ConstantsLoader.constantValues.Add(TypeConstant.pirateBoatsStart, constants.pirateBoatsStart.value);
            ConstantsLoader.constantValues.Add(TypeConstant.timerChallenge, constants.timerChallenge.value);
            ConstantsLoader.constantValues.Add(TypeConstant.timerDisturbance, constants.timerDisturbance.value);
            ConstantsLoader.constantValues.Add(TypeConstant.timerGame, constants.timerGame.value);
            return true;
        }
        else
        {
            StreamWriter file = new StreamWriter(path);
            file.Close();
            return false;
        }
    }
    public static string getConstant(TypeConstant constant)
    {
        try
        {
            return ConstantsLoader.constantValues[constant];
        }
        catch (KeyNotFoundException)
        {
            Debug.Log("Dictionary key not found.");
            return string.Empty;
        }
    }

    [Serializable()]
    public class ArchipelConstantValue
    {
        [System.Xml.Serialization.XmlElement("Value")]
        public string value { get; set; }
    }

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("Archipel")]
    public class ArchipelConstants
    {
        [System.Xml.Serialization.XmlElement("networkListeningPort")]
        public ArchipelConstantValue networkListeningPort { get; set; }

        [System.Xml.Serialization.XmlElement("networkSendingPort")]
        public ArchipelConstantValue networkSendingPort { get; set; }

        [System.Xml.Serialization.XmlElement("networkServerIP")]
        public ArchipelConstantValue networkServerIP { get; set; }

        [System.Xml.Serialization.XmlElement("pirateBoatsStart")]
        public ArchipelConstantValue pirateBoatsStart { get; set; }

        [System.Xml.Serialization.XmlElement("pirateBoatsIncreaseTime")]
        public ArchipelConstantValue pirateBoatsIncreaseTime { get; set; }

        [System.Xml.Serialization.XmlElement("timerGame")]
        public ArchipelConstantValue timerGame { get; set; }

        [System.Xml.Serialization.XmlElement("timerChallenge")]
        public ArchipelConstantValue timerChallenge { get; set; }

        [System.Xml.Serialization.XmlElement("timerDisturbance")]
        public ArchipelConstantValue timerDisturbance { get; set; }

        [System.Xml.Serialization.XmlElement("fileScores")]
        public ArchipelConstantValue fileScores { get; set; }
    }
}
