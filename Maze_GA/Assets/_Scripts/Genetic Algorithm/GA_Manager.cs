using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Manager : MonoBehaviour
{
    private int _populationSize;
    private int _numGenerations = 700,
                _mutationChance = 20;
    private float _elite = .1f;
    private bool _continue = true;
    private int _currentGeneration = 0;

    private List<Mouse> _mice;
    private Population _population;

    private List<Mouse> _bestInGeneration = new List<Mouse>();

    public GA_Manager(List<Mouse> mice)
    {
        _populationSize = mice.Count;
        _mice = mice;
    }

    public Population GetPopulation()
    {
        return _population;
    }

    public void InitGA()
    {
        _population = new Population(_mice);
        _population.RankPopulation();
    }

    public void WorkPopulation()
    {
        _currentGeneration++;
        _population.GetPopulation().ForEach(mouse => mouse.Move());

        for (int j = 0; j < _populationSize; j++)
        {
            if (!_population.GetPopulation()[j].ReachedCheese())
            {
                var mom = _population.GetEliteParent(_elite);
                var dad = _population.GetEliteParent(_elite);
                //var mom = _population.GetPopulation()[0];
                //var dad = _population.GetPopulation()[1];

                _population.GetPupFromChildPopulation(j).CrossOverChromosome(mom, dad);
                _population.GetPupFromChildPopulation(j).Mutate(_mutationChance);
            }
        }

        _population.CopyPopulation();
        _population.RankPopulation();
        _bestInGeneration.Add(_population.GetBestInPopulation());
        _population.GetPopulation().ForEach(mouse => mouse.DidMouseReachedCheese());
    }

    public bool MiceStill(Population population)
    {
        foreach(var mouse in population.GetPopulation())
        {
            if (mouse.Moving())
            {
                return false;
            }
        }
        return true;
    }

    public Mouse GetBestInPopulation()
    {
        return _population.GetPopulation()[0];
    }

    public bool DidEightyPercentOfMiceReachCheese()
    {
        var miceThatReachedCheeseCount = 0;

        _population.GetPopulation().ForEach(mouse => {
            if (mouse.ReachedCheese())
            {
                miceThatReachedCheeseCount++;
            }
        });

        return miceThatReachedCheeseCount >= _populationSize * .8;
    }
    
    public void MoveSmartMice()
    {
        _population.GetPopulation().ForEach(mouse => mouse.Move());
    }

    public int GetCurrentPercentOfSmartMice()
    {
        float miceThatReachedCheeseCount = 0;

        _population.GetPopulation().ForEach(mouse => {
            if (mouse.ReachedCheese())
            {
                miceThatReachedCheeseCount++;
            }
        });
        return (int)((miceThatReachedCheeseCount / _populationSize) * (100));
    }

    public int GetCurrentGeneration()
    {
        return _currentGeneration;
    }
}
