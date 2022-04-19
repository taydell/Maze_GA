using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public TextMeshProUGUI CurrentGen;
    public TextMeshProUGUI BestInGeneration;
    public TextMeshProUGUI Percent;

    // Start is called before the first frame update
    void Start()
    {
        CurrentGen.text = "Current Generation: 0";
        Percent.text = "Percent: 0";
    }
}
