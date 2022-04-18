using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLoader : MonoBehaviour
{
    public GameObject wall, floor, mouse, cheese;

    private MazeCell[,] mazeCells;
    private int _mazeRows, _mazeColumns;
    private float _size;
    private GameObject _mazeFolder, _wallsFolder, _floorsFolder, _northWallsFolder, _southWallsFolder, _eastWallsFolder, _westWallsFolder, 
                       _mouseFolder, _cheese;

    public Maze Init(int mazeRows, int mazeColumns, float size, int mazeId)
    {
        _mazeRows = mazeRows;
        _mazeColumns = mazeColumns;
        _size = size;
       return InitializeMaze(mazeId);
    }

    private Maze InitializeMaze(int mazeId)
    {
        mazeCells = new MazeCell[_mazeRows, _mazeColumns];
        CreateFolderStructure(mazeId);

        for(int r = 0; r < _mazeRows; r++)
        {
            for(int c = 0; c < _mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();
                var rowXPosition = r * _size;
                var columZPosition = c * _size;


                if (r == 0 && c == 0)
                {
                    _mouseFolder = Instantiate(mouse, new Vector3(rowXPosition, 0, columZPosition), Quaternion.identity);

                    _mouseFolder.transform.parent = _mazeFolder.transform;
                }

                if (r == _mazeRows - 1 && c == _mazeColumns - 1)
                {
                    _cheese = Instantiate(cheese, new Vector3(rowXPosition, 0, columZPosition), Quaternion.identity);
                    _cheese.transform.parent = _mazeFolder.transform;
                }

                //For now use the same wall object for the floor
                mazeCells[r, c].floor = Instantiate(floor, new Vector3(rowXPosition, -(_size / 2f), columZPosition), Quaternion.identity) as GameObject;
                mazeCells[r, c].floor.name = "Floor " + r + "," + c;
                mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                mazeCells[r, c].floor.transform.parent = _floorsFolder.transform;

                if (c == 0) 
                {
                    mazeCells[r, c].westWall = Instantiate(wall, new Vector3(rowXPosition, 0, columZPosition - (_size/2f)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;
                    mazeCells[r, c].westWall.transform.parent = _westWallsFolder.transform;
                }

                mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(rowXPosition, 0, columZPosition + (_size / 2f)), Quaternion.identity) as GameObject;
                mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;
                mazeCells[r, c].eastWall.transform.parent = _eastWallsFolder.transform;

                if (r == 0)
                {
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3(rowXPosition - (_size / 2f), 0, columZPosition), Quaternion.identity) as GameObject;
                    mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                    mazeCells[r, c].northWall.transform.parent = _northWallsFolder.transform;
                }

                mazeCells[r, c].southWall = Instantiate(wall, new Vector3(rowXPosition + (_size / 2f), 0, columZPosition), Quaternion.identity) as GameObject;
                mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
                mazeCells[r, c].southWall.transform.parent = _southWallsFolder.transform;
            }
        };

        return new Maze(_mouseFolder, _cheese, _mazeFolder);
    }

    public MazeCell[,] GetMazeCells()
    {
        return mazeCells;
    }

    public void SetMazePosition(Vector3 position)
    {
        _mazeFolder.transform.position = position;
    }

    private void CreateFolderStructure(int mazeId)
    {
        _mazeFolder = new GameObject("Maze" + mazeId);
        _wallsFolder = new GameObject("Walls");
        _floorsFolder = new GameObject("Floors");
        _northWallsFolder = new GameObject("North");
        _southWallsFolder = new GameObject("South");
        _eastWallsFolder = new GameObject("East");
        _westWallsFolder = new GameObject("West");

        _mazeFolder.transform.parent = gameObject.transform;
        _wallsFolder.transform.parent = _mazeFolder.transform;
        _floorsFolder.transform.parent = _mazeFolder.transform;
        _northWallsFolder.transform.parent = _wallsFolder.transform;
        _southWallsFolder.transform.parent = _wallsFolder.transform;
        _eastWallsFolder.transform.parent = _wallsFolder.transform;
        _westWallsFolder.transform.parent = _wallsFolder.transform;
    }
}
