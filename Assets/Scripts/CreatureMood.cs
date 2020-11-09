using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreatureMood : MonoBehaviour
{
    #region Variables

    public int Hunger;
    public int Health;
    public int Happiness;
    public int Cranky;
    public int Obedience;

    public int Seconds;
    public int Minutes;
    public int Hours;
    public int Days;
    public int Years;
    public int Level;

    public DateTime Clock;
    public DateTime IdleClock;
    public DateTime PoopClock;

    public int DeadIndex;
    public int IncubationIndex;
    public int FeedCounter;

    public SpriteRenderer SRCreature;

    public Dictionary<string, Sprite> SpriteDict;
    public Dictionary<string, GameObject> GameObjectDict;

    public Button BtnPlay;
    public Button BtnFeed;
    public Button BtnClean;

    public Button BtnMedicine;
    public Button BtnScold;
    public Button BtnTimeTravel;

    public bool Dead;
    public bool Incubating;
    public bool Sick;
    public bool LoadFeed;
    public bool LoadClean;

    private int _levelMod;

    private float _elapsed = 0f;

    #endregion

    #region Methods

    // Use this for initialization
    void Start()
    {
        /*
         * TODO: Move new files to resource folder for when your character changes
         */
        //var pathNames = AssetDatabase.MoveAsset("Assets/Creatures/Red/", "Assets/Resources/CreatureStates/");
        //UnityEngine.Debug.Log(pathNames);

        //for (int i = 0; i < pathNames.Length; i++)
        //{
        //    UnityEngine.Debug.Log(pathNames[i]);
        //}


        SpriteDict = new Dictionary<string, Sprite>();
        GameObjectDict = new Dictionary<string, GameObject>();
        LoadStates();

        
        if (!LoadPlayer())
        {
            Hunger = 100;
            Health = 100;
            Happiness = 100;
            Obedience = 100;
            Cranky = 0;
            Level = 1;
            SRCreature.sprite = SpriteDict["Content"];
            Clock = DateTime.Now;
            IdleClock = DateTime.Now;
            PoopClock = DateTime.Now;
            BtnClean.interactable = false;
        }
        else
        {
            SRCreature.sprite = SpriteDict["Content"];
        }

        _levelMod = Level * 25;

        BtnPlay.onClick.AddListener(Play);
        BtnFeed.onClick.AddListener(Feed);
        BtnClean.onClick.AddListener(Clean);
        BtnScold.onClick.AddListener(Scold);
        BtnTimeTravel.onClick.AddListener(TimeTravel);
    }

    // Update is called once per frame
    void Update()
    {
        _elapsed += Time.deltaTime;

        if (_elapsed >= 1f)
        {
            DayCycle();
            _elapsed = _elapsed % 1f;
        }

        if (!Dead && !Incubating)
        {
            StateChange();
        }
        else
        {
            DeathCycle();
        }
    }

    void StateChange()
    {
        if (Health <= 0)
        {
            SRCreature.sprite = SpriteDict["Dead"];
            Dead = true;
        }
        else if (Hunger < 50)
        {
            SRCreature.sprite = SpriteDict["Hungry"];
        }

        var dateDiff = DateTime.Now - PoopClock;
        if (PoopClock != DateTime.MinValue && dateDiff.TotalHours >= 2)
        {
            var go = new GameObject("Poop");
            var poop = go.AddComponent<SpriteRenderer>();
            poop.sprite = SpriteDict["Poop"];
            go.transform.position = new Vector3(SRCreature.transform.position.x + 1.5f, SRCreature.transform.position.y - .8f, 0);
            go.transform.localScale = new Vector3(.5f, .5f, .5f);
            poop.sortingLayerName = "Companion";
            GameObjectDict.Add("Poop", go);
            PoopClock = DateTime.MinValue;
            BtnClean.interactable = true;
        }
    }

    private void DeathCycle()
    {
        DayCycle();
        if (Dead)
        {
            DeadIndex++;
        }
        else if (Incubating)
        {
            IncubationIndex++;
        }
        

        if(DeadIndex >= 2592000)
        {
            Dead = false;
            Incubating = true;
            SRCreature.sprite = SpriteDict["Egg"];
        }
        else if(IncubationIndex >= 2592000)
        {

        }
    }

    private void DayCycle()
    {
        Seconds++;
        //Index = 0;
        if (Seconds >= 60)
        {
            Seconds = 0;
            Minutes++;
            if (Minutes >= 60)
            {
                if (!Dead)
                {
                    HourlyChanges();
                }

                if (Hours >= 24)
                {
                    Hours = 0;
                    Days++;

                    if (Days >= 365)
                    {
                        Years++;
                        Days = 0;
                        if (!Dead)
                        {
                            LevelUp();
                        }
                    }
                }
            }
        }
    }

    private void HourlyChanges()
    {
        var rando = new System.Random();

        Obedience -= rando.Next(1, 4);

        Hours++;
        Hunger -= 9;

        if (Hunger <= 0)
        {
            Health -= 10;
        }
        else if (Hunger < 50)
        {
            Health -= 5;
            Happiness -= 2;
        }

        if (GameObjectDict.ContainsKey("Poop"))
        {
            Health -= 5;
        }

        if (Hunger > 50 && Happiness > 50)
        {
            Health += 2;
        }

        if (Health < 50)
        {
            var num = rando.Next(1, 10);

            if (num <= 3)
            {
                Sick = true;
            }

            Happiness -= 2;
        }


        var dateDiff = DateTime.Now - IdleClock;
        if (dateDiff.Hours >= 2)
        {
            Happiness -= 2;
            IdleClock = DateTime.Now;
        }

        if (Obedience <= 0)
        {

        }
    }

    private void LevelUp()
    {
        Happiness += 25;
        Hunger += 25;
        Cranky = 0;
        Happiness += 25;
        _levelMod += 25;

        var oldTransform = SRCreature.transform.localScale;
        SRCreature.transform.localScale = new Vector3(oldTransform.x + .5f, oldTransform.y + .5f, oldTransform.z);
    }

    private void LoadStates()
    {
        object[] loadedIcons =  SpriteManager.LoadStates();
        for (int x = 0; x < loadedIcons.Length; x++)
        {
            var name = ((Sprite)loadedIcons[x]).name;
            UnityEngine.Debug.Log(name);
            SpriteDict.Add(name, (Sprite)loadedIcons[x]);
        }
    }

    public void OnApplicationQuit()
    {
        LoadClean = BtnClean.interactable;
        LoadFeed = BtnFeed.interactable;
        Clock = DateTime.Now;
        SaveSystem.SaveGame(this);
    }


    public bool LoadPlayer()
    {
        var data = SaveSystem.LoadGame();

        if (data == null)
        {
            return false;
        }

        foreach(var companion in data.GameObjectDict)
        {
            var go = new GameObject(companion);
            var poop = go.AddComponent<SpriteRenderer>();
            poop.sprite = SpriteDict["Poop"];
            go.transform.position = new Vector3(SRCreature.transform.position.x + 1.5f, SRCreature.transform.position.y - .8f, 0);
            go.transform.localScale = new Vector3(.5f, .5f, .5f);
            poop.sortingLayerName = "Companion";
            GameObjectDict.Add(companion, go);
        }

        Hunger = data.Hunger;
        Health = data.Health;
        Happiness = data.Happiness;
        Cranky = data.Cranky;

        Seconds = data.Seconds;
        Minutes = data.Minutes;
        Hours = data.Hours;
        Days = data.Days;
        Years = data.Years;
        Level = data.Level;

        Clock = data.Clock;
        IdleClock = data.IdleClock;
        PoopClock = data.PoopClock;

        var dateDiffMainClock = DateTime.Now - data.Clock;
        var tempIdleClock = data.IdleClock;

        Days += dateDiffMainClock.Days;
        Hours += dateDiffMainClock.Hours;
        Minutes += dateDiffMainClock.Minutes;
        Seconds += dateDiffMainClock.Seconds;

        var totalHours = Math.Floor(dateDiffMainClock.TotalHours);
        int i = 0;

        while (totalHours > 0)
        {
            HourlyChanges();
            totalHours--;

            if (i % 2 == 0)
            {
                tempIdleClock.AddHours(2);
                IdleClock = tempIdleClock;
            }

            i++;
        }

        while (Seconds >= 60)
        {
            Seconds -= 60;
            Minutes++;
        }

        while (Minutes >= 60)
        {
            Minutes -= 60;
            Hours++;
        }

        while (Hours >= 24)
        {
            Days++;
            Hours -= 24;
        }

        BtnClean.interactable = data.LoadClean;
        BtnFeed.interactable = data.LoadFeed;

        return true;
    }

    #endregion

    #region Buttons
    void Play()
    {
        if (Happiness < 100)
        {
            Happiness++;
        }
        IdleClock = DateTime.Now;
    }

    void Feed()
    {
        Hunger += 100 + _levelMod;
        PoopClock = DateTime.Now;
        IdleClock = DateTime.Now;
        BtnFeed.interactable = false;
    }

    void Clean()
    {
        Destroy(GameObjectDict["Poop"]);
        GameObjectDict.Remove("Poop");
        IdleClock = DateTime.Now;

        BtnClean.interactable = false;
    }

    void Scold()
    {
        Happiness -= 25;
        Obedience = 100 + _levelMod;
    }

    void TimeTravel()
    {
        if(PoopClock != null && PoopClock > DateTime.MinValue)
        {
            PoopClock = PoopClock.AddHours(-1);
        }
    }
    #endregion
}
