using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
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

    public bool JustFed;
    public bool JustPooped;

    public PlayerData(CreatureMood mood)
    {
        Hunger = mood.Hunger;
        Health = mood.Health;
        Happiness = mood.Happiness;
        Cranky = mood.Cranky;
        Index = mood.Index;
        PoopIndex = mood.PoopIndex;
        Hours = mood.Hours;
        Days = mood.Days;
        Years = mood.Years;
        Level = mood.Level;

        JustFed = mood.JustFed;
        JustPooped = mood.JustPooped;

    }
}
