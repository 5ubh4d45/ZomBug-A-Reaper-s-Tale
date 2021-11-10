using System;
using UnityEngine;
using UnityDebug = UnityEngine.Debug;

namespace GameEssentials.Debug
{
    public static class UtilsClass
    {
        public static string ColorToHex(Color color)
        {
            string r = Mathf.RoundToInt(color.r * 255).ToString("X2");
            string g = Mathf.RoundToInt(color.g * 255).ToString("X2");
            string b = Mathf.RoundToInt(color.b * 255).ToString("X2");
            string a = Mathf.RoundToInt(color.a * 255).ToString("X2");
            return $"#{r}{g}{b}{a}";
        }
    }
}