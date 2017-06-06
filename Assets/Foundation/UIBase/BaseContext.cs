using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseContext
{

    public UIType ViewType { get; private set; }

    public BaseContext(UIType viewType)
    {
        ViewType = viewType;
    }
}

