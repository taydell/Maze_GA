using System;
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
    private int _currentIndexInGene;
    private CoroutineQueue _coroutineQueue;
    private RandomInputGenerator _randomInputGenerator;
    private int _score = 0;
    private Vector3 _directionFacing;
    private float _timeSpentMoving = 0;

    private bool _gotCheese = false;

    public void InitMouse()
    {
        _currentPath = new Queue<IEnumerator>();
        _originalInputs = new List<Vector3>();
        _currentIndexInGene = 0;
        _coroutineQueue = new CoroutineQueue(this);
        _randomInputGenerator = new RandomInputGenerator();
        _coroutineQueue.StartLoop();
        if(_name == null)
            _name = RandomNameSelector.GetName();
        _directionFacing = _randomInputGenerator.GetAllPossibleDirections()[1];
    }

    public void SetGenome()
    {
        _genome = _randomInputGenerator.GetRandomlyGeneratedGenome(ManageMazes.BestSolution.Count);
        _originalInputs = _genome;
        SetPath();
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
        var crossAmount = UnityEngine.Random.Range(0, _genome.Count - 1);
        var momsPartialGenome = mom.GetGenome().Take(crossAmount).ToList();
        var dadsPartialGenome = dad.GetGenome().Skip(crossAmount).ToList();

        momsPartialGenome.AddRange(dadsPartialGenome);
        UpdateGenome(momsPartialGenome);
    }

    public void Mutate(int mutationChance)
    {
        if(UnityEngine.Random.Range(1,100) <= mutationChance)
        {
            var geneToMutate = UnityEngine.Random.Range(0, _genome.Count - 1);
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
        SetPath();
        Move();
    }

    public Vector3 GetFinalPosition()
    {
        return _finalPosition;
    }

    public void ResetPositionToStart()
    {
        //_coroutineQueue.StopCoroutine();
        transform.localPosition = new Vector3(0, 0, 0);
        _previousPosition = new Vector3(0, 0, 0);
        _originalInputs = _genome;
        _currentIndexInGene = 0;

        //_coroutineQueue.StartLoop();
        SetPath();
    }

    public List<Vector3> GetGenome()
    {
        return _genome;
    }

    public string GetName()
    {
        return _name;
    }
    public void SetName(string name)
    {
        _name = name;
    }

    public void DidMouseReachedCheese()
    {
        if (_score == 0)
        {
            _gotCheese = true;
        }
    }

    public bool ReachedCheese()
    {
        return _gotCheese;
    }

    public float GetTimeSpentMoving()
    {
        return _timeSpentMoving;
    }

    private void SetPath()
    {
        var currentPosition = _previousPosition;
        for(int i = _currentIndexInGene; i< _originalInputs.Count; i++)
        {
            currentPosition += _originalInputs[i];
            _currentPath.Enqueue(MoveMouse(currentPosition, 2));
        }
        //foreach (var gene in _originalInputs)
        //{
        //    currentPosition += gene;
        //    _currentPath.Enqueue(MoveMouse(currentPosition, 2));
        //}
    }

    IEnumerator MoveMouse(Vector3 targetPosition, float duration, bool backtracking = false)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;
        RotateMouse(backtracking);
        if (!backtracking) { 
            _previousPosition = transform.localPosition;
            _finalPosition = transform.localPosition;
            //_originalInputs.RemoveAt(0);
            _currentIndexInGene++;
            
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

        if(_originalInputs.Count >= _currentIndexInGene)
        {
            _isMoving = false;
        }
    }

    private void RotateMouse(bool backTracking)
    {
        var directionMoving = new Vector3(0,0,0);
        if (backTracking)
        {
            directionMoving = _directionFacing * -1;
        }
        else
        {
            directionMoving =_originalInputs[_currentIndexInGene];
        }
        

        if(_directionFacing != directionMoving)
        {
            if(directionMoving == _randomInputGenerator.GetAllPossibleDirections()[0]) 
            {
                transform.rotation = Quaternion.Euler(_randomInputGenerator.GetAllPossibleRotationalDirections()[0]);
            }
            else if (directionMoving == _randomInputGenerator.GetAllPossibleDirections()[1]) 
            {
                transform.rotation = Quaternion.Euler(_randomInputGenerator.GetAllPossibleRotationalDirections()[1]);
            }
            else if (directionMoving == _randomInputGenerator.GetAllPossibleDirections()[2]) 
            {
                transform.rotation = Quaternion.Euler(_randomInputGenerator.GetAllPossibleRotationalDirections()[2]);
            }
            else if (directionMoving == _randomInputGenerator.GetAllPossibleDirections()[3]) 
            {
                transform.rotation = Quaternion.Euler(_randomInputGenerator.GetAllPossibleRotationalDirections()[3]);
            }
        }

        _directionFacing = directionMoving;
        
    }
}
