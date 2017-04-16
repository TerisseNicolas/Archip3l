using UnityEngine;
using System.Collections;
using System.IO;

public class NetDebugger : MonoBehaviour {

    public static NetDebugger Instance;

    private string filePath;
    private StreamWriter file;



    void Start()
    {
        this.filePath = "Debug.txt";
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of NetDebugger!");
        }
        Instance = this;

        this.file = new StreamWriter(this.filePath, false);
    }

    public void newLog(string value)
    {
        file.WriteLine(value);
    }

    void Destory()
    {
        this.file.Close();
    }
}
