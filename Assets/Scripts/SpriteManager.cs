using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
    
    public static void MovingSpriteToResource()
    {

    }

    public static object[] LoadStates()
    {
        return Resources.LoadAll("CreatureStates", typeof(Sprite));
    }
}
