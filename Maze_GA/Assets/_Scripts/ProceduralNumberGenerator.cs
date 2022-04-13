using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralNumberGenerator
{
    public static int currentPosition;
    public const string key = "123424123342421432233144441212334432121223344";

    public static int GetNextNumber()
    {
        string currentNum = key.Substring(currentPosition++ % key.Length, 1);
        return int.Parse(currentNum);
        //return Random.Range(1, 5);
    }
}
