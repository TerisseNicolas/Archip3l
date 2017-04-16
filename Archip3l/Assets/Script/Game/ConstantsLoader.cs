using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

public class ConstantsLoader : MonoBehaviour {

    private static Dictionary<TypeConstant, string> constantValues = new Dictionary<TypeConstant, string>();
    //private string path = "data.txt";
    private string path = "parametres.xml";

    void Awake()
    {
        if(!this.loadData())
        {
            Debug.LogWarning("Could not load all constants!!!");
        }
    }

    void Start()
    {
        //foreach (TypeConstant constant in Enum.GetValues(typeof(TypeConstant)))
        //{
        //    Debug.Log(constant.ToString() + " - " + getConstant(constant));
        //}
    }
    private bool loadData()
    {
        if (File.Exists(this.path))
        {
            ArchipelConstants constants = null;
            XmlSerializer serializer = new XmlSerializer(typeof(ArchipelConstants));
            StreamReader reader = new StreamReader(this.path);
            constants = (ArchipelConstants)serializer.Deserialize(reader);
            reader.Close();

            ConstantsLoader.constantValues.Add(TypeConstant.networkListeningPort, constants.networkListeningPort.value);
            ConstantsLoader.constantValues.Add(TypeConstant.networkSendingPort, constants.networkSendingPort.value);
            ConstantsLoader.constantValues.Add(TypeConstant.networkServerIP, constants.networkServerIP.value);
            ConstantsLoader.constantValues.Add(TypeConstant.pirateBoatsInitInterval, constants.pirateBoatsInitInterval.value);
            ConstantsLoader.constantValues.Add(TypeConstant.pirateBoatsRaisingRate, constants.pirateBoatsIncreaseRate.value);
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

        [System.Xml.Serialization.XmlElement("pirateBoatsInitInterval")]
        public ArchipelConstantValue pirateBoatsInitInterval { get; set; }

        [System.Xml.Serialization.XmlElement("pirateBoatsIncreaseRate")]
        public ArchipelConstantValue pirateBoatsIncreaseRate { get; set; }
    }
}
