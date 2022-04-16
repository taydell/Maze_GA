using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private Vector3 _previousPosition;
    [SerializeField]
    private Queue<IEnumerator> _currentPath;
    private List<Vector3> _originalInputs;
    private CoroutineQueue _coroutineQueue;

    public void InitMouse()
    {
        _currentPath = new Queue<IEnumerator>();
        _originalInputs = new List<Vector3>();
        _coroutineQueue = new CoroutineQueue(this);
        _coroutineQueue.StartLoop();
    }

    public void SetPath(List<Vector3> positions)
    {
        _originalInputs = positions;

        var currentPosition = _previousPosition;
        foreach(var position in positions)
        {
             currentPosition += position;
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

    IEnumerator MoveMouse(Vector3 targetPosition, float duration, bool backtracking = false)
    {
         float time = 0;
         Vector3 startPosition = transform.localPosition;
        if (!backtracking) { 
            _previousPosition = transform.localPosition;
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
