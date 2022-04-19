using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Population : MonoBehaviour
{
    private List<Mouse> _micePopulation, _pupPopulation;

    public Population(List<Mouse> mice)
    {
        _micePopulation = mice;
        _pupPopulation = mice;
    }

    public void RankPopulation()
    {
        _micePopulation.ForEach(mouse => SetFitness(mouse));
        _micePopulation = _micePopulation.OrderBy( mouse => mouse.GetScore()).ToList();
    }

    public Mouse GetEliteParent(float elite)
    {
        var selection = (int)(elite * _micePopulation.Count);
        return _micePopulation.Take(selection).ToList()[Random.Range(0, selection-1)];
    }

    public Mouse GetPupFromChildPopulation(int index)
    {
        return _pupPopulation[index];
    }

    public void CopyPopulation()
    {
        for(var i = 0; i < _micePopulation.Count - 1; i++)
        {
            _micePopulation[i].UpdateGenome(_pupPopulation[i].GetGenome());
        }
    }

    public Mouse GetBestInPopulation()
    {
        return _micePopulation.First();
    }

    public Mouse GetWorstInPopulation()
    {
        return _micePopulation.Last();
    }

    public List<Mouse> GetPopulation()
    {
        return _micePopulation;
    }

    public void ResetMice()
    {
        _micePopulation.ForEach(mouse => mouse.ResetPositionToStart());
    }
    
    private void SetFitness(Mouse mouse)
    {
        var score = 0;
        var count = 0;

        mouse.GetGenome().ForEach(chromosome =>
        {
            score += CompareChromosomes(chromosome, ManageMazes.BestSolution[count]);
            count++;
        });

        mouse.setScore(score);
    }

    private int CompareChromosomes(Vector3 chromosome, Vector3 targetChromosome)
    {
        return chromosome == targetChromosome ? 0:1;
    }
}
