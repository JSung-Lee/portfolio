using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InformationBook : Book
{
    public static UI_InformationBook self;
    public UI_Book ui_Book;

    public void Awake()
    {
        if (self) Destroy(this);
        else self = this;
    }
}
