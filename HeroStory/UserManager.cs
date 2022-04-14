using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager self;
    void Start()
    {
        if (!self) self = this;
        else Destroy(this);
    }

    public void SaveData()
    {
        //PlayerPrefs.SetFloat();
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        //PlayerPrefs.GetFloat();
    }
}
