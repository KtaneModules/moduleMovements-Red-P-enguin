using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class moduleMovements : MonoBehaviour
{
    public KMBombInfo bomb;
    public KMAudio audio;

    static int ModuleIdCounter = 0;
    int ModuleId;
    private bool moduleSolved = false;
    private bool incorrect = false;

    public KMSelectable[] buttons;
    public GameObject[] lights;
    public string[] text;
    public string[] solvedText;
    public string[] modules;
    public SpriteRenderer display;
    public Sprite[] sprites;
    public TextMesh[] buttonText;
    public Renderer[] stageLights;
    public Material unlitMaterial;
    public Material blackMaterial;

    private string correctText;
    private KMSelectable corButton;
    private int index = 0;
    private int solvedIndex = 0;
    private string selModule;
    private int textNum;
    private int moduleNum;
    private int[,] selMovements;
    private List<int> indexTracker = new List<int>();
    private List<int> solvedTracker = new List<int>();
    private int textNumber = 1;
    private int stageNumber = 0;
    private bool whichIsForbidden = false;

    int[,] forbiddenA = new int[23, 10] {{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                                        { 9, 10, 11, 4, 12, 3, 7, 0, 13, 14},
                                        { 4, 11, 15, 5, 12, 7, 3, 14, 8, 10},
                                        { 7, 1, 13, 9, 16, 6, 3, 0, 4, 15},
                                        { 17, 8, 15, 18, 13, 19, 3, 6, 12, 16},
                                        { 6, 3, 2, 16, 17, 5, 8, 19, 18, 12},
                                        { 16, 4, 8, 0, 13, 7, 15, 10, 14, 18},
                                        { 13, 2, 3, 10, 19, 17, 1, 8, 12, 5},
                                        { 3, 4, 14, 7, 18, 11, 6, 13, 10, 5},
                                        { 13, 7, 6, 11, 5, 16, 10, 4, 19, 3},
                                        { 11, 13, 0, 18, 4, 8, 17, 9, 6, 16},
                                        { 1, 3, 15, 14, 17, 5, 10, 6, 16, 13},
                                        { 18, 13, 8, 9, 11, 15, 7, 4, 3, 14},
                                        { 2, 3, 5, 0, 7, 16, 13, 6, 4, 14},
                                        { 12, 2, 17, 6, 11, 16, 8, 19, 13, 3},
                                        { 16, 19, 1, 10, 13, 5, 0, 12, 2, 6},
                                        { 17, 14, 6, 5, 18, 12, 7, 8, 10, 2},
                                        { 18, 7, 9, 3, 6, 5, 12, 0, 14, 10},
                                        { 12, 17, 10, 5, 19, 8, 16, 14, 15, 18},
                                        { 18, 12, 10, 2, 13, 4, 9, 7, 16, 1},
                                        { 16, 12, 8, 11, 7, 0, 5, 3, 17, 14},
                                        { 19, 16, 1, 12, 18, 3, 5, 2, 14, 13},
                                        { 10, 7, 6, 4, 1, 15, 3, 5, 2, 12}};
    int[,] forbiddenB = new int[23, 10] {{ 4, 11, 15, 5, 12, 7, 3, 14, 8, 10 },
                                        { 16, 4, 8, 0, 13, 7, 15, 10, 14, 18},
                                        { 17, 14, 6, 5, 18, 12, 7, 8, 10, 2},
                                        { 10, 7, 6, 4, 1, 15, 3, 5, 2, 12},
                                        { 16, 19, 1, 10, 13, 5, 0, 12, 2, 6},
                                        { 7, 1, 13, 9, 16, 6, 3, 0, 4, 15},
                                        { 16, 12, 8, 11, 7, 0, 5, 3, 17, 14},
                                        { 12, 17, 10, 5, 19, 8, 16, 14, 15, 18},
                                        { 6, 3, 2, 16, 17, 5, 8, 19, 18, 12},
                                        { 19, 16, 1, 12, 18, 3, 5, 2, 14, 13},
                                        { 13, 2, 3, 10, 19, 17, 1, 8, 12, 5},
                                        { 13, 7, 6, 11, 5, 16, 10, 4, 19, 3},
                                        { 18, 7, 9, 3, 6, 5, 12, 0, 14, 10},
                                        { 18, 12, 10, 2, 13, 4, 9, 7, 16, 1},
                                        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                                        { 12, 2, 17, 6, 11, 16, 8, 19, 13, 3},
                                        { 11, 13, 0, 18, 4, 8, 17, 9, 6, 16},
                                        { 2, 3, 5, 0, 7, 16, 13, 6, 4, 14},
                                        { 17, 8, 15, 18, 13, 19, 3, 6, 12, 16},
                                        { 18, 13, 8, 9, 11, 15, 7, 4, 3, 14},
                                        { 1, 3, 15, 14, 17, 5, 10, 6, 16, 13},
                                        { 9, 10, 11, 4, 12, 3, 7, 0, 13, 14},
                                        { 3, 4, 14, 7, 18, 11, 6, 13, 10, 5}};
    int[,] forbiddenC = new int[23, 10] {{ 17, 8, 15, 18, 13, 19, 3, 6, 12, 16},
                                        { 19, 16, 1, 12, 18, 3, 5, 2, 14, 13},
                                        { 18, 13, 8, 9, 11, 15, 7, 4, 3, 14},
                                        { 13, 2, 3, 10, 19, 17, 1, 8, 12, 5},
                                        { 6, 3, 2, 16, 17, 5, 8, 19, 18, 12},
                                        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                                        { 16, 19, 1, 10, 13, 5, 0, 12, 2, 6},
                                        { 3, 4, 14, 7, 18, 11, 6, 13, 10, 5},
                                        { 4, 11, 15, 5, 12, 7, 3, 14, 8, 10},
                                        { 18, 7, 9, 3, 6, 5, 12, 0, 14, 10},
                                        { 11, 13, 0, 18, 4, 8, 17, 9, 6, 16},
                                        { 17, 14, 6, 5, 18, 12, 7, 8, 10, 2 },
                                        { 9, 10, 11, 4, 12, 3, 7, 0, 13, 14},
                                        { 10, 7, 6, 4, 1, 15, 3, 5, 2, 12},
                                        { 13, 7, 6, 11, 5, 16, 10, 4, 19, 3},
                                        { 2, 3, 5, 0, 7, 16, 13, 6, 4, 14},
                                        { 16, 4, 8, 0, 13, 7, 15, 10, 14, 18},
                                        { 12, 17, 10, 5, 19, 8, 16, 14, 15, 18},
                                        { 1, 3, 15, 14, 17, 5, 10, 6, 16, 13},
                                        { 7, 1, 13, 9, 16, 6, 3, 0, 4, 15},
                                        { 18, 12, 10, 2, 13, 4, 9, 7, 16, 1},
                                        { 12, 2, 17, 6, 11, 16, 8, 19, 13, 3},
                                        { 16, 12, 8, 11, 7, 0, 5, 3, 17, 14}};
    int[,] forbiddenD = new int[23, 10] {{ 16, 19, 1, 10, 13, 5, 0, 12, 2, 6 },
                                        { 13, 7, 6, 11, 5, 16, 10, 4, 19, 3},
                                        { 6, 3, 2, 16, 17, 5, 8, 19, 18, 12},
                                        { 16, 4, 8, 0, 13, 7, 15, 10, 14, 18},
                                        { 17, 14, 6, 5, 18, 12, 7, 8, 10, 2 },
                                        { 18, 12, 10, 2, 13, 4, 9, 7, 16, 1},
                                        { 9, 10, 11, 4, 12, 3, 7, 0, 13, 14},
                                        { 12, 17, 10, 5, 19, 8, 16, 14, 15, 18},
                                        { 16, 12, 8, 11, 7, 0, 5, 3, 17, 14},
                                        { 19, 16, 1, 12, 18, 3, 5, 2, 14, 13},
                                        { 12, 2, 17, 6, 11, 16, 8, 19, 13, 3},
                                        { 1, 3, 15, 14, 17, 5, 10, 6, 16, 13},
                                        { 7, 1, 13, 9, 16, 6, 3, 0, 4, 15},
                                        { 18, 13, 8, 9, 11, 15, 7, 4, 3, 14},
                                        { 2, 3, 5, 0, 7, 16, 13, 6, 4, 14},
                                        { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
                                        { 18, 7, 9, 3, 6, 5, 12, 0, 14, 10},
                                        { 11, 13, 0, 18, 4, 8, 17, 9, 6, 16},
                                        { 17, 8, 15, 18, 13, 19, 3, 6, 12, 16},
                                        { 4, 11, 15, 5, 12, 7, 3, 14, 8, 10},
                                        { 10, 7, 6, 4, 1, 15, 3, 5, 2, 12},
                                        { 3, 4, 14, 7, 18, 11, 6, 13, 10, 5},
                                        { 13, 2, 3, 10, 19, 17, 1, 8, 12, 5}};
    // Use this for initialization
    void Start()
    {

        if (bomb.GetBatteryCount() > 3)
        {
            if (bomb.GetIndicators().Count() > 2)
            {
                selMovements = forbiddenC;
                DebugMsg("The table you're using is Table C.");
            }
            else
            {
                selMovements = forbiddenA;
                DebugMsg("The table you're using is Table A.");
            }
        }
        else if (bomb.GetIndicators().Count() > 2)
        {
            selMovements = forbiddenB;
            DebugMsg("The table you're using is Table B.");
        }
        else
        {
            selMovements = forbiddenD;
            DebugMsg("The table you're using is Table D.");
        }
        init();
    }

    void init()
    {
        whichIsForbidden = false;

        index = UnityEngine.Random.Range(0, 23);
        if (moduleNum != index)
        {
            moduleNum = index;
            display.sprite = sprites[moduleNum];
            DebugMsg("The selected module is " + modules[moduleNum] + ".");

            if (bomb.GetPortCount() % 2 == 0)
            {
                forbiddenGenerator();
            }
            else
            {
                correctGenerator();
            }
        }
        else
        {
            init();
        }
    }

    void correctGenerator()
    {
        index = UnityEngine.Random.Range(0, 20);
        for (int i = 0; i < 10; i++)
        {
            if (selMovements[moduleNum, i] != index)
            {
                if (i >= 9)
                {
                    i = 10;
                    indexTracker.Add(index);
                }
            }
            else
            {
                correctGenerator();
            }
        }
        if (whichIsForbidden == false)
        {
            whichIsForbidden = true;
            DebugMsg("The correct button is " + text[index] + ".");
            forbiddenGenerator();
        }
        else if (indexTracker.Count < 4)
        {
            correctGenerator();
        }
        else if (indexTracker.Count == 4)
        {
            selectCorButton();
        }
    }

    void forbiddenGenerator()
    {
        index = UnityEngine.Random.Range(0, 10);
        if (!indexTracker.Contains(selMovements[moduleNum, index]))
        {
            indexTracker.Add(selMovements[moduleNum, index]);
        }

        if (whichIsForbidden == false)
        {
            whichIsForbidden = true;
            DebugMsg("The correct button is " + text[selMovements[moduleNum, index]] + ".");
            correctGenerator();
        }
        else if (indexTracker.Count < 4)
        {
            forbiddenGenerator();
        }
        else if (indexTracker.Count == 4)
        {
            selectCorButton();
        }
    }

    void selectCorButton()
    {
        index = UnityEngine.Random.Range(0, 4);
        corButton = buttons[index];
        buttonText[index].text = text[indexTracker[0]];
        textOnButtons();
    }

    void textOnButtons()
    {
        index = UnityEngine.Random.Range(0, 4);
        if (buttonText[index].text == "")
        {
            buttonText[index].text = text[indexTracker[textNumber]];
            textNumber++;
            if (textNumber < 4)
            {
                textOnButtons();
            }
        }
        else if (textNumber < 4)
        {
            textOnButtons();
        }
    }

    // Update is called once per frame
    void Awake()
    {
        ModuleId = ModuleIdCounter++;

        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { buttonPressed(pressedButton); return false; };
        }
    }

    void solvedTextSelector()
    {
        index = UnityEngine.Random.Range(0, 4);
        solvedIndex = UnityEngine.Random.Range(0, 17);
        if (buttonText[index].text == "" && solvedTracker.Contains(solvedIndex) != true)
        {
            solvedTracker.Add(solvedIndex);
            buttonText[index].text = solvedText[solvedIndex];
            textNumber++;
            if (textNumber < 4)
            {
                solvedTextSelector();
            }
        }
        else
        {
            solvedTextSelector();
        }
    }

    void buttonPressed(KMSelectable pressedButton)
    {
        pressedButton.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransformWithRef(KMSoundOverride.SoundEffect.ButtonPress, transform);

        textNumber = 1;
        incorrect = false;
        indexTracker.Clear();

        for (int i = 0; i < 4; i++)
        {
            buttonText[i].text = "";
        }

        if (moduleSolved == true)
        {
            return;
        }

        if (pressedButton != corButton)
        {
            incorrect = true;
        }

        if (!incorrect)
        {
            stageLights[stageNumber].material = unlitMaterial;
            if (stageNumber < 2)
            {
                stageNumber++;
                DebugMsg("Correct button. Moving to next stage...");
                init();
            }
            else
            {
                textNumber = 0;
                DebugMsg("Correct button. Module solved!");
                moduleSolved = true;
                GetComponent<KMBombModule>().HandlePass();
                solvedTextSelector();
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                stageLights[i].material = blackMaterial;
            }
            GetComponent<KMBombModule>().HandleStrike();
            DebugMsg("Strike! You pressed " + pressedButton.name + ", when the correct button was " + corButton.name + ".");
            stageNumber = 0;
            init();
        }
    }

    public string TwitchHelpMessage = "Use !{0} press 1 to press the first button. Buttons are labeled in reading order. Use !{0} text to see the text of the buttons in the chat.";
    IEnumerator ProcessTwitchCommand(string cmd)
    {
        var parts = cmd.ToLowerInvariant().Split(new[] { ' ' });

        if (parts.Length == 2 && parts[0] == "press" && parts[1].Length == 1)
        {
            if (parts[1] == "1")
            {
                yield return null;
                buttonPressed(buttons[0]);
            }
            else if (parts[1] == "2")
            {
                yield return null;
                buttonPressed(buttons[1]);
            }
            else if (parts[1] == "3")
            {
                yield return null;
                buttonPressed(buttons[2]);
            }
            else if (parts[1] == "4")
            {
                yield return null;
                buttonPressed(buttons[3]);
            }
        }
        else if (parts.Length == 1 && parts[0] == "text")
        {
            yield return "sendtochat The words are " + text[indexTracker[0]] + ", " + text[indexTracker[1]] + ", " + text[indexTracker[2]] + ", " + text[indexTracker[3]] + ".";
        }
        else
        {
            yield break;
        }
    }

    void DebugMsg(string msg)
{
    Debug.LogFormat("[Module Movements #{0}] {1}", ModuleId, msg);
}
}
