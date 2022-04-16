using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineQueue
{
    MonoBehaviour m_Owner = null;
    Coroutine m_InternalCoroutine = null;
    Queue<IEnumerator> actions = new Queue<IEnumerator>();

    public CoroutineQueue(MonoBehaviour coroutineOwner)
    {
        m_Owner = coroutineOwner;
    }

    public void StartLoop()
    {
        m_InternalCoroutine = m_Owner.StartCoroutine(Process());
    }

    public void StopLoop()
    {
        m_Owner.StopCoroutine(m_InternalCoroutine);
        m_InternalCoroutine = null;
    }

    public void StopCoroutine()
    {
        m_Owner.StopAllCoroutines();
        actions.Clear();
    }

    public void EnqueueAction(IEnumerator aAction)
    {
        actions.Enqueue(aAction);
    }

    private IEnumerator Process()
    {
        while (true)
        {
            if (actions.Count > 0)
                yield return m_Owner.StartCoroutine(actions.Dequeue());
            else
                yield return null;
        }
    }

    /*public void EnqueueWait(float aWaitTime)
    {
        actions.Enqueue(Wait(aWaitTime));
    }

    private IEnumerator Wait(float aWaitTime)
    {
        yield return new WaitForSeconds(aWaitTime);
    }*/
}
