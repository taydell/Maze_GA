using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Vector3 _previousPosition, _finalPosition;

    [SerializeField]
    private Queue<IEnumerator> _currentPath;
    private List<Vector3> _originalInputs, _genome;
    private CoroutineQueue _coroutineQueue;
    private RandomInputGenerator _randomInputGenerator;
    private int _score = 0;

    public void InitMouse(int genomeLength, List<Vector3> genome = null)
    {
        _currentPath = new Queue<IEnumerator>();
        _originalInputs = new List<Vector3>();
        _coroutineQueue = new CoroutineQueue(this);
        _randomInputGenerator = new RandomInputGenerator();
        _coroutineQueue.StartLoop();
        _genome = genome;

        if(_genome == null)
        {
            _genome = _randomInputGenerator.GetRandomlyGeneratedGenome(genomeLength);
        }
        
        SetPath(_genome);
    }

    public void setScore(int score)
    {
        _score = score;
    }

    private void SetPath(List<Vector3> _genome)
    {
        _originalInputs = _genome;

        var currentPosition = _previousPosition;
        foreach(var gene in _genome)
        {
             currentPosition += gene;
            _currentPath.Enqueue(MoveMouse(currentPosition, 2));
        }
    }

    public void Move()
    {
        while (_currentPath.Count > 0)
        { 
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
