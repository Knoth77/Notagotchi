using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureMood : MonoBehaviour
{
    #region Variables

    public int Hunger;
    public int Health;
    public int Happiness;
    public int Cranky;
    public int Index;
    public int PoopIndex;
    public int Hours;
    public int Days;
    public int Years;
    public int Level;

    public SpriteRenderer SRCreature;
    public SpriteRenderer SRSecondary;
    public SpriteRenderer SRBackdrop;

    public Dictionary<string, Sprite> SpriteDict;

    public Button BtnPlay;
    public Button BtnFeed;
    public Button BtnClean;

    public bool JustFed;
    public bool JustPooped;

    private int _levelMod;

    #endregion

    #region Methods

    // Use this for initialization
    void Start()
    {
        SpriteDict = new Dictionary<string, Sprite>();
        LoadStates();

        if (!LoadPlayer())
        {
            Hunger = 100;
            Health = 100;
            Happiness = 100;
            Cranky = 0;
            Index = 0;
            Level = 1;
        }
        else
        {
            SRCreature.sprite = SpriteDict["Content"];
        }

        _levelMod = Years * 25;

        BtnPlay.onClick.AddListener(Play);
        BtnFeed.onClick.AddListener(Feed);
        BtnClean.onClick.AddListener(Clean);

    }

    // Update is called once per frame
    void Update()
    {
        Index++;

        MoodChange();
        StateChange();
    }

    void StateChange()
    {
        if (Health <= 0)
        {
            SRCreature.sprite = SpriteDict["Dead"];
        }
        else if (PoopIndex >= 1)
        {
            SRSecondary.sprite = SpriteDict["Poop"];
            JustPooped = true;
            JustFed = false;
        }
        else if (Hunger < 99)
        {
            SRCreature.sprite = SpriteDict["Hungry"];
        }
    }

    void MoodChange()
    {
        if (JustFed)
        {
            PoopIndex++;
        }

        if (Index % 300 == 0)
        {
            SRCreature.flipX = !SRCreature.flipX;
            Debug.Log(Index + " " + Health + " " + Hunger + " " + PoopIndex);
        }

        if (Index > 3600)
        {
            Index = 0;
            Hours++;
            if (Hours >= 24)
            {
                Hours = 0;
                Days++;

                if (Days >= 365)
                {
                    Years++;
                    Days = 0;
                    LevelUp();
                }
            }
            Hunger--;
        }

        if (Hunger < 50)
        {
            Happiness--;
            Health--;
            Cranky++;
        }
        if (Cranky > 50)
        {
            Health--;
            Happiness--;
        }
        if (Health < 50)
        {
            Cranky++;
            Happiness--;
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
        object[] loadedIcons = Resources.LoadAll("CreatureStates", typeof(Sprite));
        for (int x = 0; x < loadedIcons.Length; x++)
        {
            var name = ((Sprite)loadedIcons[x]).name;
            Debug.Log(name);
            SpriteDict.Add(name, (Sprite)loadedIcons[x]);
        }
    }

    public void OnApplicationQuit()
    {
        SaveSystem.SaveGame(this);
    }


    public bool LoadPlayer()
    {
        var data = SaveSystem.LoadGame();

        if (data == null)
        {
            return false;
        }

        Hunger = data.Hunger;
        Health = data.Health;
        Happiness = data.Happiness;
        Cranky = data.Cranky;
        Index = data.Index;
        PoopIndex = data.PoopIndex;
        Hours = data.Hours;
        Days = data.Days;
        Years = data.Years;
        Level = data.Level;

        JustFed = data.JustFed;
        JustPooped = data.JustPooped;

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
    }

    void Feed()
    {
        if (!JustFed)
        {
            JustFed = true;
            Hunger += 50;
        }

        if (Hunger > 100)
        {
            Hunger = 100 + _levelMod;
            SRCreature.sprite = SpriteDict["Content"];
        }
    }

    void Clean()
    {
        if (JustPooped)
        {
            if (Happiness < 100)
            {
                Happiness += 25;
            }
            if (Happiness > 100)
            {
                Happiness = 100 + _levelMod;
            }

            SRSecondary.sprite = null;
            JustPooped = false;
            PoopIndex = 0;
        }
    }
    #endregion
}
