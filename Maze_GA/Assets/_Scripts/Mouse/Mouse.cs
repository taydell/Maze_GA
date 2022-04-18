using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Vector3 _previousPosition, _finalPosition;
    private string _name;
    private bool _isMoving = false;
    [SerializeField]
    private Queue<IEnumerator> _currentPath;
    private List<Vector3> _originalInputs, _genome;
    private CoroutineQueue _coroutineQueue;
    private RandomInputGenerator _randomInputGenerator;
    private int _score = 0;

    public void InitMouse()
    {
        _currentPath = new Queue<IEnumerator>();
        _originalInputs = new List<Vector3>();
        _coroutineQueue = new CoroutineQueue(this);
        _randomInputGenerator = new RandomInputGenerator();
        _coroutineQueue.StartLoop();
        _name = RandomNameSelector.GetName();
    }

    public void SetGenome()
    {
        _genome = _randomInputGenerator.GetRandomlyGeneratedGenome(ManageMazes.BestSolution.Count);

        SetPath(_genome);
    }

    public void UpdateGenome(List<Vector3> genome)
    {
        _genome = genome;
    }

    public void setScore(int score)
    {
        _score = score;
    }

    public int GetScore()
    {
        return _score;
    }

    public void CrossOverChromosome(Mouse mom, Mouse dad)
    {
        var crossAmount = Random.Range(0, _genome.Count - 1);
        var momsPartialGenome = mom.GetGenome().Take(crossAmount).ToList();
        var dadsPartialGenome = dad.GetGenome().Skip(crossAmount).ToList();

        momsPartialGenome.AddRange(dadsPartialGenome);

        _genome = momsPartialGenome;
    }

    public void Mutate(int mutationChance)
    {
        if(Random.Range(1,100) <= mutationChance)
        {
            var geneToMutate = Random.Range(0, _genome.Count - 1);
            var mutation = _randomInputGenerator.GetRandomCardinalDirection();
            _genome[geneToMutate] = mutation;
        }
    }

    public bool Moving()
    {
        return _isMoving;
    }

    public void Move()
    {
        while (_currentPath.Count > 0)
        {
            _isMoving = true;
            IEnumerator moveCoroutine = _currentPath.Dequeue();
            _coroutineQueue.EnqueueAction(moveCoroutine);
        }
    }

    public void Mover(Vector3 position)
    {
        var currentPosition = _previousPosition + position;
        StartCoroutine(MoveMouse(currentPosition, 2));
    }


    public void MoveToPreviousPosition()
    {
        _coroutineQueue.StopCoroutine();
        StartCoroutine(MoveMouse(_previousPosition, 2, true));
        AdjustPath();
        Move();
    }

    public Vector3 GetFinalPosition()
    {
        return _finalPosition;
    }

    public void ResetPositionToStart()
    {
        _coroutineQueue.StopCoroutine();
        _isMoving = false;
        transform.localPosition = new Vector3(0, 0, 0);
        
        SetPath(_genome);
    }

    public List<Vector3> GetGenome()
    {
        return _genome;
    }

    public string GetName()
    {
        return _name;
    }

    private void SetPath(List<Vector3> genome)
    {
        _originalInputs = genome;

        var currentPosition = _previousPosition;
        foreach (var gene in genome)
        {
            currentPosition += gene;
            _currentPath.Enqueue(MoveMouse(currentPosition, 2));
        }
    }

    IEnumerator MoveMouse(Vector3 targetPosition, float duration, bool backtracking = false)
    {
         float time = 0;
         Vector3 startPosition = transform.localPosition;
        if (!backtracking) { 
            _previousPosition = transform.localPosition;
            _finalPosition = transform.localPosition;
            _originalInputs.RemoveAt(0);
        }
         while (time < duration)
         {
             transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
             time += Time.deltaTime;
             yield return 0;
         }
         transform.localPosition = targetPosition;
        if (backtracking)
        {
            _coroutineQueue.StartLoop();
        }
    }

    private void AdjustPath()
    {
        SetPath(_originalInputs);
    }
}
