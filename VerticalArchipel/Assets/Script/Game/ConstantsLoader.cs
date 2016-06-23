using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ConstantsLoader : MonoBehaviour {

    public static Dictionary<TypeConstant, string> constantValues = new Dictionary<TypeConstant, string>();
    private static string path = "data.txt";

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
        //Debug.Log("load");
        string line = "   ";
        Dictionary<TypeConstant, string> constantValuesTemp = new Dictionary<TypeConstant, string>();

        if (File.Exists(ConstantsLoader.path))
        {
            StreamReader file = new StreamReader(path);
            while (((line = file.ReadLine()) != null)  && (line.Length > 2))
            {
                try
                {
                    constantValuesTemp.Add((TypeConstant)Enum.Parse(typeof(TypeConstant), line.Split('=')[0]), line.Split('=')[1]);
                }
                catch (ArgumentException)
                {
                    Debug.Log("An element with this key already exists.");
                    return false;
                }

            }
            file.Close();
            ConstantsLoader.constantValues = constantValuesTemp;
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
