using System;
using UnityEngine;

[System.Serializable]
public struct LevelBasedInt
{
    public int baseValue;
    public int bonusPerLevel;
    public int Get(int level) { return baseValue + bonusPerLevel * (level - 1); }
}

[System.Serializable]
public struct LevelBasedFloat
{
    public float baseValue;
    public float bonusPerLevel;
    public float Get(int level) { return baseValue + bonusPerLevel * (level - 1); }
}

[System.Serializable]
public struct LevelBasedMultiplierFloat
{
    public float baseValue;
    public float bonusPerLevel;
    public float Get(int level) { return baseValue * Mathf.Pow((1f + bonusPerLevel), (level - 1)); }
}

[System.Serializable]
public struct MinMaxInt
{
    public int min;
    public int max;
    public int Get() { return UnityEngine.Random.Range(min, max + 1); }
}


[System.Serializable]
public struct LevelBasedMinMaxInt
{
    public LevelBasedInt min;
    public LevelBasedInt max;
    public int Get(int level) {
        int _min = min.Get(level);
        return UnityEngine.Random.Range(_min, Mathf.Max(_min, max.Get(level)) + 1); 
    }
}