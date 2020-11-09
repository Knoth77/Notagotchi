using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Hunger;
    public int Health;
    public int Happiness;
    public int Cranky;
    public int Obedience;

    public int Index;
    public int PoopIndex;
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

    public bool LoadFeed;
    public bool LoadClean;

    public bool Dead;
    public bool Incubating;
    public bool Sick;

    public List<string> SpriteDict;
    public List<string> GameObjectDict;

    public PlayerData(CreatureMood mood)
    {
        Hunger = mood.Hunger;
        Health = mood.Health;
        Happiness = mood.Happiness;
        Cranky = mood.Cranky;
        Obedience = mood.Obedience;

        Seconds = mood.Seconds;
        Minutes = mood.Minutes;
        Hours = mood.Hours;
        Days = mood.Days;
        Years = mood.Years;
        Level = mood.Level;

        Clock = mood.Clock;
        IdleClock = mood.IdleClock;
        PoopClock = mood.PoopClock;

        DeadIndex = mood.DeadIndex;
        IncubationIndex = mood.IncubationIndex;
        FeedCounter = mood.FeedCounter;

        LoadFeed = mood.LoadFeed;
        LoadClean = mood.LoadClean;

        Dead = mood.Dead;
        Incubating = mood.Incubating;
        Sick = mood.Sick;

        SpriteDict = mood.SpriteDict.Keys.ToList();
        GameObjectDict = mood.GameObjectDict.Keys.ToList();

    }
}
