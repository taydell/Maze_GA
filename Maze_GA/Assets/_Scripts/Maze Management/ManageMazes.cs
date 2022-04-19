using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManageMazes : MonoBehaviour
{
    public int mazeRows, mazeColumns;
    public List<Maze> Mazes = new List<Maze>();
    public static List<Vector3> BestSolution;
    public Camera MainCamera;

    private float size = 6f;
    private int _mazeLength;
    private List<Mouse> _mice = new List<Mouse>();
    private GA_Manager _gA_Manager;
    private MazeCell[,] _mazeCells;
    private bool _isInitialized = false;
    private bool _gA_Started = false;

    private void Start()
    {
        _mazeLength = mazeColumns * mazeRows;
        var startingXPosition = mazeRows*size + 10;
        var startingZPosition = mazeColumns*size + 10;
        var currentStartingPostion = new Vector3(0, 0, 0);
        var mazeLoader = gameObject.GetComponent<MazeLoader>();

        for (int i = 0; i < 5; i++)
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

            var mouse = maze.GetMouseFolder().GetComponent<Mouse>();
            mouse.InitMouse();
            maze.SetMouse(mouse);

            Mazes.Add(maze);
            _mice.Add(mouse);
        }
        Mazes[0].GetMouse().SetName("Taylor");
        Mazes[0].SetMouseFolderName();
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_gA_Started)
            {
                _gA_Manager = new GA_Manager(_mice);
                _gA_Manager.InitGA();
                _gA_Started = true;
            }
            MoveCameraToBestInPopulation();
            if (!_gA_Manager.DidEightyPercentOfMiceReachCheese())
            {
                _gA_Manager.WorkPopulation();
            }
            else
            {
                _gA_Manager.MoveSmartMice();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
             _gA_Manager.GetPopulation().ResetMice();
        }
    }

    private void Initialize()
    {
        _isInitialized = true;

        var mazeSolver = new MazeSolver(_mazeCells, mazeRows, mazeColumns);
        BestSolution = mazeSolver.FindBestSolution();

        _mice.ForEach(mouse => mouse.SetGenome());
    }

    private void MoveCameraToBestInPopulation()
    {
        var bestInPopulation = _gA_Manager.GetBestInPopulation().GetName();
        var maze = Mazes.Where(maze => maze.GetMouse().GetName() == bestInPopulation).ToList();

        var BestMouseMazePosition = maze[0].GetParentFolder().transform.position;
        BestMouseMazePosition.x += (((mazeRows - 1) * 6)/2);
        BestMouseMazePosition.z += (((mazeColumns - 1) * 6)/2);
        StartCoroutine(MoveCamera(BestMouseMazePosition));
    }

    private IEnumerator MoveCamera(Vector3 targetPosition)
    {
        float time = 0;
        var duration = 1;
        Vector3 startPosition = MainCamera.transform.position;
        targetPosition.y = startPosition.y;

        while (time < duration)
        {
            MainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return 0;
        }
        MainCamera.transform.position = targetPosition;
    }
}
