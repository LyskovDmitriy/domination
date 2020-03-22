using System.Collections.Generic;
using UnityEngine;


public abstract class BuildingSettingsBase : ScriptableObject
{
    public abstract BuildingType Type { get; }
}
