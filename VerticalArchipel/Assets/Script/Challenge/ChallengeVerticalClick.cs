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



public class ChallengeVerticalClick : InputSource
{

    static public string[] row { get; set; }
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
    static public GameObject canvasChallengeObject { get; private set; }
    static public bool answered = false;

    public TextAsset csv { get; private set; }

    private Client client { get; set; }
    private Score Score;

    void Start()
    {
        this.client = GameObject.Find("Network").GetComponent<Client>();
        this.Score = GameObject.Find("Game").GetComponent<Score>();
        
    }

    public void init()
    {
        ChallengeVerticalClick.typeChallenge = (TypeChallenge)Enum.Parse(typeof(TypeChallenge), this.transform.parent.name.Split('_')[1]);
        System.Random ran = new System.Random();
        int aleat;
        ChallengeVerticalClick.quantityToWin = ran.Next(20, 100);
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

        ChallengeVerticalClick.resourceToWin = main.getNameResourceOrStatProduced(tb.ToString());


        //CSV part
        //row[0] : question ; row[1] : answer ; row[2] : explainations ; after : propositions
        //VraiFaux : answer = Proposition0 ou answer = Proposition1
        //QCM : answer = Proposition0 ou answer = Proposition1 ou answer = Proposition2

        //ENCODAGE : UTF8-16-LE
        //last line of file usually blank --> to be removed!
        //csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/" + ChallengeVerticalClick.typeChallenge.ToString() + "/" + ChallengeVerticalClick.typeChallenge.ToString() + "_" + tb.ToString());

        if (main.level == 0) //collège
        {
            csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/College/" + ChallengeVerticalClick.typeChallenge.ToString() + "/" + ChallengeVerticalClick.typeChallenge.ToString() + "_" + tb.ToString());
            Debug.Log("college");
        }
        else
        {
            csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/Lycee/" + ChallengeVerticalClick.typeChallenge.ToString() + "/" + ChallengeVerticalClick.typeChallenge.ToString() + "_" + tb.ToString());
            Debug.Log("lycee");
        }
        Debug.Log("File : " + typeChallenge.ToString() + "_" + tb.ToString());
        row = CSV_reader.GetRandomLine(csv.text);


        if (ChallengeVerticalClick.typeChallenge == TypeChallenge.QCM)
            ChallengeVerticalClick.nbPropositions = 3;
        else
            ChallengeVerticalClick.nbPropositions = 2;


        ChallengeVerticalClick.question = row[0];
        addLineBreaks();
        ChallengeVerticalClick.answer = row[1];
        ChallengeVerticalClick.explainations = row[2];
        ChallengeVerticalClick.propositions = new string[nbPropositions];
        ChallengeVerticalClick.propositions[0] = row[3];
        ChallengeVerticalClick.propositions[1] = row[4];
        if (ChallengeVerticalClick.nbPropositions == 3)
            ChallengeVerticalClick.propositions[2] = row[5];


        
        canvasChallenge = this.transform.parent.GetComponent<Canvas>();

        foreach (Text text in canvasChallenge.GetComponentsInChildren<Text>())
        {
            switch (text.name)
            {
                case "Question":
                    text.text = ChallengeVerticalClick.question;
                    break;
                case "Result":
                    resultText = text;
                    break;
                case "Proposition0":
                    if (typeChallenge == TypeChallenge.QCM)
                        text.text = ChallengeVerticalClick.propositions[0];
                    break;
                case "Proposition1":
                    if (typeChallenge == TypeChallenge.QCM)
                        text.text = ChallengeVerticalClick.propositions[1];
                    break;
                case "Proposition2":
                    if (typeChallenge == TypeChallenge.QCM)
                        text.text = ChallengeVerticalClick.propositions[2];
                    break;
            }
        }

        foreach (SpriteRenderer sp in canvasChallenge.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sp.name == "background")
                ChallengeVerticalClick.background = sp;
        }
    }
    
    void addLineBreaks()
    {
        const int maxChar = 40;
        List<int> spaces = new List<int>();
        int i = 0;
        foreach (char c in ChallengeVerticalClick.question)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        int j = 0;
        i = 1;
        int nbLineBreakAdded = 0;
        while (maxChar * i <= ChallengeVerticalClick.question.Length)
        {
            while (j < spaces.Count && spaces[j] < maxChar * i)
                j++;
            ChallengeVerticalClick.question = question.Substring(0, spaces[j - 1] + nbLineBreakAdded) + "\n" + question.Substring(spaces[j - 1] + nbLineBreakAdded);
            i++;
            nbLineBreakAdded++;
        }
    }



    public void OnMouseDownSimulation()
    {
        if (ChallengeVerticalClick.answered == false)
        {
            ChallengeVerticalClick.answered = true;
            string clickedText = this.name.Split('_')[0];

            //modify Result.text     
            if (clickedText == answer)
            {
                resultText.text = "Réponse correcte !";
                goodAnswer = true;
                main.addNotification("Vous venez de réussir un challenge !");
                this.Score.verticalGoodAnwsers += 1;
                //this.client.sendData("@35401@Challenge" + this.typeChallenge.ToString());
            }
            else {
                resultText.text = "Réponse incorrecte !";
                goodAnswer = false;
                main.addNotification("Vous venez de rater un challenge ...");
                //this.client.sendData("@35402@Challenge" + this.typeChallenge.ToString());
            }
            this.Score.verticalAnswers += 1;

            //Debug.Log(this.Score.verticalGoodAnwsers.ToString() + "/" + this.Score.verticalAnswers.ToString());

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
        yield return new WaitForSeconds(2f);
        /*Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.001f);

            color = background.material.color;
            color.a -= 0.01f;
            background.material.color = color;
        }*/


        if (goodAnswer)
        {
            Canvas challengeWonPrefab = Resources.Load<Canvas>("Prefab/challengeWonCanvas");
            Canvas challengeWon = Instantiate(challengeWonPrefab);
            challengeWon.name = "challengeWonCanvas";
            ChallengeWon.challengeWonWindowName = challengeWon.name;
            Vector3 z = challengeWon.GetComponentInChildren<SpriteRenderer>().gameObject.transform.position;
            z.z = -1;
            challengeWon.GetComponentInChildren<SpriteRenderer>().gameObject.transform.position = z;
            GameObject.Find("challengeWon-sous_ile_1").GetComponent<ChallengeWon>().init(resourceToWin, quantityToWin);
        }


        ChallengeVertical.challengeWindowPresent = false;
        ChallengeVerticalClick.answered = false;

        Destroy(GameObject.Find("Challenge_" + typeChallenge));

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
        if (Time.time - TouchTime < 0.5)
        {
            TouchTime = 0;
            this.OnMouseDownSimulation();
        }
        TouchTime = 0;
    }
    
}
