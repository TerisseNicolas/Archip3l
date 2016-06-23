using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ConstantsLoader : MonoBehaviour {

    public static Dictionary<TypeConstant, string> constantValues = new Dictionary<TypeConstant, string>();
    private string path = "data.txt";

	void Awake()
    {
        if(!this.loadData())
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

    private bool loadData()
    {
        string line = "   ";

        if (File.Exists(this.path))
        {
            StreamReader file = new StreamReader(path);
            while (((line = file.ReadLine()) != null)  && (line.Length > 2))
            {
                try
                {
                    ConstantsLoader.constantValues.Add((TypeConstant)Enum.Parse(typeof(TypeConstant), line.Split('=')[0]), line.Split('=')[1]);
                }
                catch (ArgumentException)
                {
                    Debug.Log("An element with this key already exists.");
                    return false;
                }

            }
            file.Close();
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
}
