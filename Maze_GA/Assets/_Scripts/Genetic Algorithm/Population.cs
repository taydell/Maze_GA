using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    private List<Mouse> _micePopulation, _pupPopulation;

    public Population(List<Mouse> mice)
    {
        _micePopulation = mice;
        _pupPopulation = mice;
    }
}
