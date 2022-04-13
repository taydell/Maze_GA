using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeLoader : MonoBehaviour
{
    public int mazeRows, mazeColumns;
    public GameObject wall, startLocation, finishLocation;

    public float size = 2f;

    private MazeCell[,] mazeCells;
    [SerializeField]
    GameObject _floors, _northWalls, _southWalls, _eastWalls, _westWalls;

    // Start is called before the first frame update
    void Start()
    {
        InitializeMaze();
        MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
        ma.CreateMaze();
    }

    private void InitializeMaze()
    {
        mazeCells = new MazeCell[mazeRows, mazeColumns];
        
        for(int r = 0; r < mazeRows; r++)
        {
            for(int c = 0; c < mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();

                if(r == 0 && c == 0)
                {
                    var start = Instantiate(startLocation, new Vector3(r * size , 0, c * size), Quaternion.identity);
                    start.transform.parent = gameObject.transform;
                }

                if (r == mazeRows - 1 && c == mazeColumns - 1)
                {
                    var finish = Instantiate(finishLocation, new Vector3(r * size, 0, c * size), Quaternion.identity);
                    finish.transform.parent = gameObject.transform;
                }

                //For now use the same wall object for the floor
                mazeCells[r, c].floor = Instantiate(wall, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity) as GameObject;
                mazeCells[r, c].floor.name = "Floor " + r + "," + c;
                mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                mazeCells[r, c].floor.transform.parent = _floors.transform;

                if (c == 0) 
                {
                    mazeCells[r, c].westWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) - (size/2f)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].westWall.name = "West Wall " + r + "," + c;
                    mazeCells[r, c].westWall.transform.parent = _westWalls.transform;
                }

                mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity) as GameObject;
                mazeCells[r, c].eastWall.name = "East Wall " + r + "," + c;
                mazeCells[r, c].eastWall.transform.parent = _eastWalls.transform;

                if (r == 0)
                {
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r * size) - (size / 2f), 0, (c * size)), Quaternion.identity) as GameObject;
                    mazeCells[r, c].northWall.name = "North Wall " + r + "," + c;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up * 90f);
                    mazeCells[r, c].northWall.transform.parent = _northWalls.transform;
                }

                mazeCells[r, c].southWall = Instantiate(wall, new Vector3((r * size) + (size / 2f), 0, (c * size)), Quaternion.identity) as GameObject;
                mazeCells[r, c].southWall.name = "South Wall " + r + "," + c;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up * 90f);
                mazeCells[r, c].southWall.transform.parent = _southWalls.transform;
            }
        }
    }
}
