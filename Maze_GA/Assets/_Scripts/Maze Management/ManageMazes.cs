using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMazes : MonoBehaviour
{
    public int mazeRows, mazeColumns;
    private float size = 6f;
    public List<Maze> Mazes = new List<Maze>();

    private void Start()
    {

        var startingXPosition = mazeRows*size + 10;
        var startingZPosition = mazeColumns*size + 10;
        var currentStartingPostion = new Vector3(0, 0, 0);
        var mazeLoader = gameObject.GetComponent<MazeLoader>();

        for (int i = 0; i < 2; i++)
        {
            var maze = mazeLoader.Init(mazeRows, mazeColumns, size, (i + 1));
            
            if(i % 10 == 0)
            {
                currentStartingPostion.x = 0;
                currentStartingPostion.z += startingZPosition;
            }

            mazeLoader.SetMazePosition(currentStartingPostion);
            currentStartingPostion.x += startingXPosition;
            
            var mazeCells = mazeLoader.GetMazeCells();

            MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
            ma.CreateMaze();

            Mazes.Add(maze);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var mouseComponent = Mazes[0].GetMouse().GetComponent<Mouse>();
            var test = Mazes[1].GetMouse().GetComponent<Mouse>();
            var mazeLength = mazeColumns * mazeRows;

            mouseComponent.InitMouse(mazeLength);
            mouseComponent.Move();

            test.InitMouse(mazeLength);
            test.Move();

            foreach (var maze in Mazes)
            {
                Debug.Log(maze.GetName());
            }
        }
    }
}
