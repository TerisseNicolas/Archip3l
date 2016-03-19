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
    static public int enimgasToBeDone = 0;

    static public Text questionObject { get; private set; }
    static public Text resultObject { get; private set; }
    static public Text answerObject { get; private set; }
    static public string question { get; private set; }
    static public string answer { get; private set; }
    static public string explainations { get; private set; }
    static public int nbAnswers = 0;   //has to be 2

    public TextAsset csv { get; private set; }

    private Client client { get; set; }


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

    void Awake()
    {
        this.client = GameObject.Find("Network").GetComponent<Client>();


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
            Enigma.enimgasToBeDone--;   //doing it just one time
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
        yield return new WaitForSeconds(3);
        
        for (int i = 0; i < 10; i++)
        {
            GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find(i.ToString()).GetComponent<BoxCollider>().enabled = false;
        }
        GameObject.Find("Answer").SetActive(false);

        if (goodAnswer)
        {
            //TODO: finish message
            Enigma.resultObject.text = "Bravo ! votre réponse permet ...";
            this.client.sendData("@35601@" + this.name);
        }
            
        else
        {
            //TODO: finish message
            Enigma.resultObject.text = "Votre mauvaise réponse ...";
            this.client.sendData("@35602@" + this.name);
        }


        //TODO: action of Enigma


        yield return new WaitForSeconds(5);

        if (Enigma.enimgasToBeDone > 0)
        {
            main.addEnigma();
            Destroy(GameObject.Find("Enigma"));
        }
        else
        {
            Destroy(GameObject.Find("Enigma"), 4.1f);
            yield return new WaitForSeconds(4);
            Enigma.enigmaWindowOpen = false;
        }
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
    float TouchTime;

    private MetaGesture gesture;
    private Dictionary<int, int> map = new Dictionary<int, int>();

    public override void CancelTouch(TouchPoint touch, bool @return)
    {
        base.CancelTouch(touch, @return);

        map.Remove(touch.Id);
        if (@return)
        {
            TouchHit hit;
            if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
            map.Add(touch.Id, beginTouch(processCoords(hit.RaycastHit.textureCoord), touch.Tags).Id);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchMoved += touchMovedhandler;
            gesture.TouchCancelled += touchCancelledhandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (gesture)
        {
            gesture.TouchBegan -= touchBeganHandler;
            gesture.TouchMoved -= touchMovedhandler;
            gesture.TouchCancelled -= touchCancelledhandler;
            gesture.TouchEnded -= touchEndedHandler;
        }
    }

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        map.Add(touch.Id, beginTouch(processCoords(touch.Hit.RaycastHit.textureCoord), touch.Tags).Id);
        //this.OnMouseDownSimulation();
        TouchTime = Time.time;
    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        TouchHit hit;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
        moveTouch(id, processCoords(hit.RaycastHit.textureCoord));
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        endTouch(id);
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
    }

    private void touchCancelledhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        cancelTouch(id);
    }

}
