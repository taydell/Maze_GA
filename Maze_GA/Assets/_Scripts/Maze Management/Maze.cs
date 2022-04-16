using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    private GameObject _mouse, _cheese, _ParentFolder;
    private string _name;

    public Maze(GameObject mouse, GameObject cheese, GameObject parentFolder)
    {
        _mouse = mouse;
        _cheese = cheese;
        _ParentFolder = parentFolder;
        _name = parentFolder.name;
    }

    public string GetName()
    {
        return _name;
    }

    public GameObject GetMouse()
    {
        return _mouse;
    }
}
