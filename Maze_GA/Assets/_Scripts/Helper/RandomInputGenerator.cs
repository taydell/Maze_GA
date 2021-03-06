using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInputGenerator : MonoBehaviour
{
    private readonly Vector3[] _cardinalDirections = new Vector3[]
    {
        new Vector3(-6,0,0), //up
        new Vector3(6,0,0),  //down
        new Vector3(0,0,-6), //left
        new Vector3(0,0,6),  //right
    };

    private readonly Vector3[]_rotationalDirection = new Vector3[]
    {
        new Vector3(-90, -90, 90),
        new Vector3(-90, -90, -90),
        new Vector3(-90, -90, 0),
        new Vector3(-90, -90, -180)
    };

    public List<Vector3> GetRandomlyGeneratedGenome(int length)
    {
        var genome = new List<Vector3>();

        for(var i = 0; i< length; i++)
        {
            genome.Add(GetRandomCardinalDirection());
        }

        return genome;
    }

    public Vector3 GetRandomCardinalDirection()
    {
        return _cardinalDirections[Random.Range(0, _cardinalDirections.Length)];

    }

    public Vector3[] GetAllPossibleDirections()
    {
        return _cardinalDirections;
    }
    public Vector3[] GetAllPossibleRotationalDirections()
    {
        return _rotationalDirection;
    }
}
