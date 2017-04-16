using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using TouchScript.InputSources;

public class ChallengeUpgrade : InputSource
{
    static public bool clicked = false;
    private bool error = false;
    public string question { get; private set; }
    public string answer { get; private set; }
    public string explainations { get; private set; }
    public string[] propositions { get; private set; }
    public int nbPropositions { get; private set; }
    public bool goodAnswer { get; private set; }
    public TypeChallenge typeChallenge { get; private set; }
    public SpriteRenderer background { get; private set; }
    public Text resultText { get; private set; }
    public MinorIsland minorIsland { get; private set; }
    public Building building { get; private set; }
    public Canvas canvasChallenge { get; private set; }

    public TextAsset csv { get; private set; }
    private Client Client;

    public void init(TypeChallenge tc, MinorIsland island, Building myBuilding)
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();

        canvasChallenge = this.transform.parent.GetComponent<Canvas>();

        this.building = myBuilding;
        this.minorIsland = island;

        this.typeChallenge = tc;
        if (typeChallenge == TypeChallenge.QCM)
            this.nbPropositions = 3;
        else
            this.nbPropositions = 2;


        //CSV part
        //row[0] : question ; row[1] : answer ; row[2] : explainations ; after : propositions
        //VraiFaux : answer = Proposition0 ou answer = Proposition1
        //QCM : answer = Proposition0 ou answer = Proposition1 ou answer = Proposition2

        //ENCODAGE : UTF8-16-LE
        //last line of file usually blank --> to be removed!
        //csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/" + typeChallenge.ToString() + "/" + typeChallenge.ToString() + "_" + myBuilding.TypeBuilding.ToString());
        if (RegisterScene.level == 0)   //collège
            csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/College/" + typeChallenge.ToString() + "/" + typeChallenge.ToString() + "_" + myBuilding.TypeBuilding.ToString());
        else
            csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/Lycee/" + typeChallenge.ToString() + "/" + typeChallenge.ToString() + "_" + myBuilding.TypeBuilding.ToString());
        
        Debug.Log("File : " + typeChallenge.ToString() + "_" + myBuilding.TypeBuilding.ToString());


        string[] row = CSV_reader.GetRandomLine(csv.text);

        try
        {
            this.question = row[0];
        }
        catch
        {
            error = true;
        }
        finally
        {
            if (!error)
            {
                addLineBreaks();
                this.answer = row[1];
                this.explainations = row[2];
                this.propositions = new string[nbPropositions];
                this.propositions[0] = row[3];
                this.propositions[1] = row[4];
                if (this.nbPropositions == 3)
                    this.propositions[2] = row[5];


                foreach (Text text in canvasChallenge.GetComponentsInChildren<Text>())
                {
                    switch (text.name)
                    {
                        case "Question":
                            text.text = this.question;
                            break;
                        case "Result":
                            resultText = text;
                            break;
                        case "Proposition0":
                            if (typeChallenge == TypeChallenge.QCM)
                                text.text = this.propositions[0];
                            break;
                        case "Proposition1":
                            if (typeChallenge == TypeChallenge.QCM)
                                text.text = this.propositions[1];
                            break;
                        case "Proposition2":
                            if (typeChallenge == TypeChallenge.QCM)
                                text.text = this.propositions[2];
                            break;
                    }
                }

                foreach (SpriteRenderer sp in canvasChallenge.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (sp.name == "background")
                        this.background = sp;
                }
            }
            else
            {
                minorIsland.displayPopup("Oups ! Une erreur s'eest produite, veuillez ré-essayer ...", 3);
                Destroy(GameObject.Find("Challenge_" + typeChallenge + "_" + minorIsland.nameMinorIsland));
            }
        }
    }

    void addLineBreaks()
    {
        const int maxChar = 45;
        List<int> spaces = new List<int>();
        int i = 0;
        foreach (char c in this.question)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        int j = 0;
        i = 1;
        int nbLineBreakAdded = 0;
        while (maxChar * i <= this.question.Length)
        {
            while (j < spaces.Count && spaces[j] < maxChar * i)
                j++;
            this.question = question.Substring(0, spaces[j - 1] + nbLineBreakAdded) + "\n" + question.Substring(spaces[j - 1] + nbLineBreakAdded);
            i++;
            nbLineBreakAdded++;
        }
    }

    private void refreshAttributes()    //for objects (expect background) which have not been initialized
    {
        background = this.transform.parent.Find("background").gameObject.GetComponent<SpriteRenderer>();
        ChallengeUpgrade backgroundChallengeUpgrade = this.transform.parent.Find("background").gameObject.GetComponent<ChallengeUpgrade>();
        this.question = backgroundChallengeUpgrade.question;
        this.answer = backgroundChallengeUpgrade.answer;
        this.explainations = backgroundChallengeUpgrade.explainations;
        this.propositions = backgroundChallengeUpgrade.propositions;
        this.nbPropositions = backgroundChallengeUpgrade.nbPropositions;
        this.typeChallenge = backgroundChallengeUpgrade.typeChallenge;
        this.resultText = backgroundChallengeUpgrade.resultText;
        this.minorIsland = backgroundChallengeUpgrade.minorIsland;
        this.goodAnswer = backgroundChallengeUpgrade.goodAnswer;
        this.canvasChallenge = backgroundChallengeUpgrade.canvasChallenge;
        this.building = backgroundChallengeUpgrade.building;
        this.Client = backgroundChallengeUpgrade.Client;
    }


    void OnMouseDownSimulation()
    {
        if (!clicked)
        {
            clicked = true;
            refreshAttributes();
            string clickedText = this.name.Split('_')[0];

            //modify Result.text     
            if (clickedText == answer)
            {
                resultText.text = "Réponse correcte !";
                goodAnswer = true;
                minorIsland.nbGoodAnswersChallenges++;
            }
            else {
                resultText.text = "Réponse incorrecte !";
                goodAnswer = false;
            }
            minorIsland.nbAnswersChallenges++;

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
        yield return new WaitForSeconds(1f);
        /*Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.001f);

            color = background.material.color;
            color.a -= 0.01f;
            background.material.color = color;
        }*/

        minorIsland.challengePresent = false;
        clicked = false;

        //ChallengeUpgrade.building.name is a string --> conversion necessary + split (name like: "sous_ile_X_nameBuilding")
        if (Enum.IsDefined(typeof(TypeBuilding), this.building.name.Split('_')[3]))
        {

            if (goodAnswer)
            {
                //withdrawal of resources needed for the upgrading
                switch (building.level)
                {
                    case 0:
                        foreach (Tuple<TypeResource, int> tuple in building.upgrade1ResourceNeeded)
                        {
                            minorIsland.resourceManager.changeResourceStock(tuple.First, -tuple.Second);
                            this.Client.sendData("@2" + minorIsland.nameMinorIsland.Split('_')[2] + "355@" + tuple.First.ToString() + "@" + (-tuple.Second).ToString());
                            yield return new WaitForSeconds(0.05f);
                        }
                        break;
                    case 1:
                        foreach (Tuple<TypeResource, int> tuple in building.upgrade2ResourceNeeded)
                        {
                            minorIsland.resourceManager.changeResourceStock(tuple.First, -tuple.Second);
                            this.Client.sendData("@2" + minorIsland.nameMinorIsland.Split('_')[2] + "355@" + tuple.First.ToString() + "@" + (-tuple.Second).ToString());
                            yield return new WaitForSeconds(0.05f);
                        }
                        break;
                    case 2:
                        foreach (Tuple<TypeResource, int> tuple in building.upgrade3ResourceNeeded)
                        {
                            minorIsland.resourceManager.changeResourceStock(tuple.First, -tuple.Second);
                            this.Client.sendData("@2" + minorIsland.nameMinorIsland.Split('_')[2] + "355@" + tuple.First.ToString() + "@" + (-tuple.Second).ToString());
                            yield return new WaitForSeconds(0.05f);
                        }
                        break;
                }

                building.level += 1;
                this.Client.sendData("@2" + this.minorIsland.nameMinorIsland.Split('_')[2] + "345@" + building.resourceProduced.TypeResource.ToString() + "@" + building.resourceProduced.Production);
                building.changeProduction(building.quantityProduced);
                minorIsland.displayPopup("Bonne réponse ! Votre bâtiment passe au niveau " + building.level.ToString() + " !", 3, explainations);

                //upgrading animation
                if (building.level != 0)
                    StartCoroutine(building.launchUpgradeAnimation(this.transform.parent.gameObject));
                this.transform.parent.position = new Vector3(1000, 1000, 1000);
                //Destroy(this.transform.parent.gameObject, building.constructionTime + 2);

            }
            else
            {
                if (building.level > 0)
                {
                    building.level -= 1;
                    building.changeProduction(-building.quantityProduced / 2);
                    building.quantityProduced /= 2;
                    minorIsland.displayPopup("Mauvaise réponse ! Votre bâtiment redescend au niveau " + building.level.ToString() + " ...", 3, explainations);
                    this.Client.sendData("@2" + minorIsland.name.Split('_')[2] + "122@" + building.TypeBuilding.ToString()  + "@" + building.level.ToString());
                }
                else
                    minorIsland.displayPopup("Mauvaise réponse ! L'amélioration n'a donc pas pu se faire ...", 3, explainations);

                Destroy(this.transform.parent.gameObject);
            }

           
        }       

    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime = 0;

    private TapGesture gesture;


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<TapGesture>();
        gesture.Tapped += pressedHandler;
    }


    private void pressedHandler(object sender, EventArgs e)
    {
        this.OnMouseDownSimulation();
    }
}
