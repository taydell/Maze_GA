using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMazes : MonoBehaviour
{
    public int mazeRows, mazeColumns;
    private float size = 6f;
    private int _mazeLength;
    public List<Maze> Mazes = new List<Maze>();
    private List<Mouse> mice = new List<Mouse>();
    private GA_Manager _gA_Manager;
    private MazeCell[,] _mazeCells;

    private void Start()
    {
        _mazeLength = mazeColumns * mazeRows;
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
            
            _mazeCells = mazeLoader.GetMazeCells();

            MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(_mazeCells);
            ma.CreateMaze();

            Mazes.Add(maze);
            var mouse = maze.GetMouse().GetComponent<Mouse>();
            
            mouse.InitMouse(_mazeLength);
            mice.Add(mouse);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var mazeSolver = new MazeSolver(_mazeCells, mazeRows, mazeColumns);
            var bestSolution = mazeSolver.FindBestSolution();
            //_gA_Manager = new GA_Manager(mice);
            //_gA_Manager.InitGA();

            mice[0].InitMouse(bestSolution.Count, bestSolution);
            mice[0].Move();
        }
    }
}
