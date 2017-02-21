using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIHelper
{

    // Use this for initialization
    public static int GetDeviceWidth(int width)
    {
        return (int)(((float)width / 1080) * Screen.width);
    }

	 public static int GetDeviceHeight(int height)
    {
        return (int)(((float)height / 1920) * Screen.height);
    }
}
