using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Manager : MonoBehaviour
{
    private int _populationSize;
    private int _numGenerations = 700,
                _mutationChance = 20;
    private float _elite = .1f;

    private List<Mouse> _mice;

    private List<Mouse> _bestInGeneration;

    public GA_Manager(List<Mouse> mice)
    {
        _populationSize = mice.Count;
        _mice = mice;
    }

    public void InitGA()
    {
        for(var i = 0; i < _numGenerations; i++)
        {
            for(int j = 0; j < _populationSize; j++)
            {
                if(i == 0)
                {

                }
                else
                {

                }
            }
        }
    }
}
