using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;
using System.Collections;
using TouchScript;


public class Enigma : InputSource
{
    static public bool enigmaWindowOpen = false;
    static public string enigmaWindowName = string.Empty;

    static public Text questionObject { get; private set; }
    static public Text resultObject { get; private set; }
    static public Text answerObject { get; private set; }
    static public string question { get; private set; }
    static public string answer { get; private set; }
    static public string explainations { get; private set; }
    static public int nbAnswers = 0;   //has to be 2 to answer

    public TextAsset csv { get; private set; }

    private Client client { get; set; }
    private Score Score;


    void OnMouseDownSimulation()
    {
        if (this.name == Enigma.answerObject.text)  //touch the same number --> remove it
        {
            Enigma.nbAnswers--;
            Enigma.answerObject.text = string.Empty;
        }
        else
        {
            Enigma.nbAnswers++;
            if (Enigma.nbAnswers == 1)
            {
                Enigma.answerObject.text = this.name;
            }
            if (Enigma.nbAnswers == 2)
            {
                Enigma.answerObject.text = (int.Parse(Enigma.answerObject.text) * 10 + int.Parse(this.name)).ToString();
                for (int i = 0; i < 10; i++)
                    GameObject.Find(i.ToString()).GetComponent<BoxCollider>().enabled = false;
                Enigma.questionObject.text = Enigma.explainations;
                StartCoroutine(wait(Enigma.answerObject.text == Enigma.answer));
            }
        }

    }

    void Start()
    {
        this.client = GameObject.Find("Network").GetComponent<Client>();
        this.Score = GameObject.Find("Game").GetComponent<Score>();


        if (this.name == "Answer")
        {
            Enigma.answerObject = this.GetComponent<Text>();
        }
        if (this.name == "Result")
        {
            Enigma.resultObject = this.GetComponent<Text>();
        }
        if (this.name == "Question")
        {
            Enigma.nbAnswers = 0;
            Enigma.questionObject = this.GetComponent<Text>();
            //CSV part
            //row[0] : question ; row[1] : answer ; row[2] : explainations

            //ENCODAGE : UTF8-16-LE
            csv = Resources.Load<TextAsset>("Enigmes/Enigma");

            string[] row = CSV_reader.GetRandomLine(csv.text);

            Enigma.question = row[0];
            Enigma.answer = row[1];
            Enigma.explainations = row[2];
            addLineBreaks();    //for question + explainations

            this.GetComponent<Text>().text = Enigma.question;
        }

    }

    IEnumerator wait(bool goodAnswer)
    {
        yield return new WaitForSeconds(1);
        
        for (int i = 0; i < 10; i++)
        {
            GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find(i.ToString()).GetComponent<BoxCollider>().enabled = false;
        }
        GameObject.Find("Answer").SetActive(false);

        if (goodAnswer)
        {
            Enigma.resultObject.text = "Bonne réponse !";
            main.addNotification("Enigme réussie !");
            this.client.sendData("@35601@" + this.name);
            this.Score.verticalGoodAnwsers += 1;
        }
            
        else
        {
            Enigma.resultObject.text = "Mauvaise réponse !";
            main.addNotification("Enigme ratée !");
            this.client.sendData("@35602@" + this.name);
        }
        this.Score.verticalAnswers += 1;


        yield return new WaitForSeconds(4);
        Enigma.enigmaWindowOpen = false;
        Enigma.nbAnswers = 0;
        Destroy(GameObject.Find("Enigma"));
    
    }

   


    void addLineBreaks()
    {
        const int maxChar = 40;
        List<int> spaces = new List<int>();
        int i = 0;
        foreach (char c in Enigma.question)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        int j = 0;
        i = 1;
        int nbLineBreakAdded = 0;
        while (maxChar * i <= Enigma.question.Length)
        {
            while (j < spaces.Count && spaces[j] < maxChar * i)
                j++;
            Enigma.question = question.Substring(0, spaces[j - 1] + nbLineBreakAdded) + "\n" + question.Substring(spaces[j - 1] + nbLineBreakAdded);
            i++;
            nbLineBreakAdded++;
        }


        spaces = new List<int>();
        i = 0;
        foreach (char c in Enigma.explainations)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        j = 0;
        i = 1;
        nbLineBreakAdded = 0;
        while (maxChar * i <= Enigma.explainations.Length)
        {
            while (j < spaces.Count && spaces[j] < maxChar * i)
                j++;
            Enigma.explainations = explainations.Substring(0, spaces[j - 1] + nbLineBreakAdded) + "\n" + explainations.Substring(spaces[j - 1] + nbLineBreakAdded);
            i++;
            nbLineBreakAdded++;
        }
    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime = 0;

    private MetaGesture gesture;

    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }

    

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (TouchTime == 0)
            TouchTime = Time.time;
    }
    

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
        TouchTime = 0;
    }

}
