using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtils
{
    public static Color GetColorByRGBA(float r, float g, float b, float a = 255)
    {
        return new Color(r / 255, g / 255, b / 255, a / 255);
    }

}
