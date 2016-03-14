using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;
using TouchScript;



public class ChallengeVertical : InputSource
{
    static public bool challengeWindowPresent = false;

    static public string[] rowSent { get; set; }    //initialized at the reception
    static public string resourceToWin { get; set; }
    static public int quantityToWin { get; set; }
    static public string question { get; private set; }
    static public string answer { get; private set; }
    static public string explainations { get; private set; }
    static public string[] propositions { get; private set; }
    static public int nbPropositions { get; private set; }
    static public TypeChallenge typeChallenge { get; set; }
    static public SpriteRenderer background { get; private set; }
    static public Text resultText { get; private set; }
    static public bool goodAnswer { get; private set; }
    static public Canvas canvasChallenge { get; private set; }

    public TextAsset csv { get; private set; }

    private Client client { get; set; }

    void Awake()
    {
        this.client = GameObject.Find("Network").GetComponent<Client>();
    }


    //initialize typeChallenge, rowSent, resourceToWin, quantityToWin
    private void getInitParameters()
    {
        //random type of ChallengeVertical
        TypeChallenge tc;
        System.Random ran = new System.Random();
        int aleat = ran.Next(0, 2);
        if (aleat == 0)
            tc = TypeChallenge.VraiFaux;
        else
            tc = TypeChallenge.QCM;

        int quantityToWin = ran.Next(0, 50);

        //random type of Building
        TypeBuilding tb;
        aleat = ran.Next(0, Enum.GetNames(typeof(TypeBuilding)).Length);
        tb = (TypeBuilding)Enum.Parse(typeof(TypeBuilding), Enum.GetNames(typeof(TypeBuilding))[aleat], true);
        //here we only use building with material resources
        while (Enum.IsDefined(typeof(TypeBuildingStat), tb.ToString()))
        {
            aleat = ran.Next(0, Enum.GetNames(typeof(TypeBuilding)).Length);
            tb = (TypeBuilding)Enum.Parse(typeof(TypeBuilding), Enum.GetNames(typeof(TypeBuilding))[aleat], true);
        }

        string resourceToWin = main.getNameResourceOrStatProduced(tb.ToString());


        //CSV part
        //row[0] : question ; row[1] : answer ; row[2] : explainations ; after : propositions
        //VraiFaux : answer = Proposition0 ou answer = Proposition1
        //QCM : answer = Proposition0 ou answer = Proposition1 ou answer = Proposition2

        //ENCODAGE : UTF8-16-LE
        //last line of file usually blank --> to be removed!
        csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/" + tc.ToString() + "/" + tc.ToString() + "_" + tb.ToString());
        Debug.Log("Challenges/ChallengesFiles/" + tc.ToString() + "/" + tc.ToString() + "_" + tb.ToString());
        string[] row = CSV_reader.GetRandomLine(csv.text);

        init(tc, row, resourceToWin, quantityToWin);


    }


    public void init(TypeChallenge tc, string[] row, string resourceToWin, int quantityToWin)
    {

        canvasChallenge = this.transform.parent.GetComponent<Canvas>();

        ChallengeVertical.rowSent = row;
        ChallengeVertical.resourceToWin = resourceToWin;
        ChallengeVertical.quantityToWin = quantityToWin;
        ChallengeVertical.typeChallenge = tc;
        if (typeChallenge == TypeChallenge.QCM)
            ChallengeVertical.nbPropositions = 3;
        else
            ChallengeVertical.nbPropositions = 2;


        ChallengeVertical.question = row[0];
        addLineBreaks();
        ChallengeVertical.answer = row[1];
        ChallengeVertical.explainations = row[2];
        ChallengeVertical.propositions = new string[nbPropositions];
        ChallengeVertical.propositions[0] = row[3];
        ChallengeVertical.propositions[1] = row[4];
        if (ChallengeVertical.nbPropositions == 3)
            ChallengeVertical.propositions[2] = row[5];


        foreach (Text text in canvasChallenge.GetComponentsInChildren<Text>())
        {
            switch (text.name)
            {
                case "Question":
                    text.text = ChallengeVertical.question.Replace('*', '\n');        //in CSV: '*' replace a line break ('\n')
                    break;
                case "Result":
                    resultText = text;
                    break;
                case "Proposition0":
                    text.text = ChallengeVertical.propositions[0];
                    break;
                case "Proposition1":
                    text.text = ChallengeVertical.propositions[1];
                    break;
                case "Proposition2":
                    text.text = ChallengeVertical.propositions[2];
                    break;
            }
        }

        foreach (SpriteRenderer sp in canvasChallenge.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sp.name == "background")
                ChallengeVertical.background = sp;
        }

    }

    void addLineBreaks()
    {
        const int maxChar = 40;
        List<int> spaces = new List<int>();
        int i = 0;
        foreach (char c in ChallengeVertical.question)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        int j = 0;
        i = 1;
        int nbLineBreakAdded = 0;
        while (maxChar * i <= ChallengeVertical.question.Length)
        {
            while (j < spaces.Count && spaces[j] < maxChar * i)
                j++;
            ChallengeVertical.question = question.Substring(0, spaces[j - 1] + nbLineBreakAdded) + "\n" + question.Substring(spaces[j - 1] + nbLineBreakAdded);
            i++;
            nbLineBreakAdded++;
        }
    }


    public void OnMouseDownSimulation()
    {
        if ((this.name == "Challenge1") || (this.name == "Challenge2") || (this.name == "Challenge3"))
        {
            if (!ChallengeVertical.challengeWindowPresent && !Trophy.infoWindowPresent && !Enigma.enigmaWindowOpen && !Disturbance.disturbanceWindowOpen && !Island.infoIslandPresent)
            {
                ChallengeVertical.challengeWindowPresent = true;
                Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + ChallengeVertical.typeChallenge.ToString());
                Canvas canvasChallenge = Instantiate(challengePrefab);
                canvasChallenge.name = "Challenge_" + ChallengeVertical.typeChallenge.ToString();
                canvasChallenge.GetComponentInChildren<ChallengeVertical>().getInitParameters();
                main.removeChallenge(GameObject.Find(this.name));
            }
        }
        else
        {
            string clickedText = this.name.Split('_')[0];

            //modify Result.text     
            if (clickedText == answer)
            {
                resultText.text = "Réponse correcte !";
                goodAnswer = true;
                main.addNotification("Vous venez de réussir un challenge !");
                //this.client.sendData("@35401@Challenge" + this.typeChallenge.ToString());
            }
            else {
                resultText.text = "Réponse incorrecte !";
                goodAnswer = false;
                main.addNotification("Vous venez de rater un challenge ...");
                //this.client.sendData("@35402@Challenge" + this.typeChallenge.ToString());
            }

            //modify Propositions background
            if (typeChallenge == TypeChallenge.VraiFaux)
            {
                foreach (Image background in canvasChallenge.GetComponentsInChildren<Image>())
                {
                    if (background.name == answer + "_background")
                        background.sprite = Resources.Load<Sprite>("Challenges/VraiFaux/case" + answer + "Clic");
                    else if (background.name.Contains("_background"))
                        background.sprite = Resources.Load<Sprite>("Challenges/VraiFaux/case" + background.name.Split('_')[0] + "Grise");
                }
            }
            else
            {
                foreach (Image background in canvasChallenge.GetComponentsInChildren<Image>())
                {
                    if (background.name == answer + "_background")
                        background.sprite = Resources.Load<Sprite>("Challenges/QCM/case" + answer + "Clic");
                    else if (background.name.Contains("_background"))
                        background.sprite = Resources.Load<Sprite>("Challenges/QCM/case" + background.name.Split('_')[0] + "Grise");
                }
            }

            StartCoroutine(wait());
        }
            
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.001f);

            color = background.material.color;
            color.a -= 0.01f;
            background.material.color = color;
        }

        
        if (goodAnswer)
        {
            Canvas challengeWonPrefab = Resources.Load<Canvas>("Prefab/challengeWonCanvas");
            Canvas challengeWon = Instantiate(challengeWonPrefab);
            challengeWon.name = "challengeWonCanvas";
            GameObject.Find("challengeWon-sous_ile_1").GetComponent<ChallengeWon>().init(resourceToWin, quantityToWin);
        }


        ChallengeVertical.challengeWindowPresent = false;

        Destroy(GameObject.Find("Challenge_" + typeChallenge));

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
        if (Time.time - TouchTime < 0.5)
        {
            TouchTime = 0;
            this.OnMouseDownSimulation();
        }
        else if (Time.time - TouchTime < 1.5)
        {
            TouchTime = 0;
        }
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
