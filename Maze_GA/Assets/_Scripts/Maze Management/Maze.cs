using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    private GameObject _mouseFolder, _cheese, _ParentFolder;
    private string _name;
    private Mouse _mouse = new Mouse();

    public Maze(GameObject mouseFolder, GameObject cheese, GameObject parentFolder)
    {
        _mouseFolder = mouseFolder;
        _cheese = cheese;
        _ParentFolder = parentFolder;
        _name = parentFolder.name;
    }

    public string GetName()
    {
        return _name;
    }

    public GameObject GetMouseFolder()
    {
        return _mouseFolder;
    }
    public Mouse GetMouse()
    {
        return _mouse;
    }
    public void SetMouse(Mouse mouse)
    {
        _mouse = mouse;
        _mouseFolder.name = _mouse.GetName();
    }
    public void SetMouseFolderName()
    {
        _mouseFolder.name = _mouse.name;
    }

    public GameObject GetParentFolder()
    {
        return _ParentFolder;
    }
}
