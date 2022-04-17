using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeSolver : MonoBehaviour
{
    private MazeCell[,] _mazecells;
    private List<String> _map;
    private Vector3?[,] _maze_coordinates;
    private List<AStarObject> _bestPath = new List<AStarObject>();
    private int _row, _col;

    public MazeSolver(MazeCell[,] mazeCells, int mazeRows, int mazeColumns)
    {
        _mazecells = mazeCells;
        _row = mazeRows;
        _col = mazeColumns;
    }

    public List<Vector3> FindBestSolution()
    {
        AStar(new Vector3(0, 0, 0));

        return GetBestPathInCardinalDirections();
    }

    public int FindDistanceFromCheese(Vector3 currentPosition)
    {

        return 0;
    }

    private List<Vector3> GetBestPathInCardinalDirections()
    {
        var bestPathInMazeCoordinates = new List<Vector3>();
        for(var i = 0; i < _bestPath.Count; i++)
        {
            if(i%2 != 0)
            {
                var bestPathX = _bestPath[i].X;
                var bestPathZ = _bestPath[i].Z;
                var test = (Vector3)_maze_coordinates[bestPathX, bestPathZ];
                bestPathInMazeCoordinates.Add((Vector3)_maze_coordinates[bestPathX, bestPathZ]);
            }
        }
        bestPathInMazeCoordinates.Add(new Vector3((_row-1),0,(_col-1)) * 6);

        return ChangeCoordinatesIntoCardinalDirections(bestPathInMazeCoordinates);

    }

    private List<Vector3> ChangeCoordinatesIntoCardinalDirections(List<Vector3> coordinates)
    {
        var bestPathInCardinalDirections = new List<Vector3>();
        var previousCoordinate = new Vector3(0,0,0);
        
        foreach(var coordinate in coordinates)
        {
            var cardinalDirection = coordinate - previousCoordinate;
            previousCoordinate = coordinate;
            

            bestPathInCardinalDirections.Add(cardinalDirection);
        }

        return bestPathInCardinalDirections;
    }

    private void AStar(Vector3 currentPosition)
    {
        ConvertMazeCellsTOStringArray();
        SetMazeCoordinates();

        //List<string> _map = new List<string>
        //{
        //    "S|           |         ",
        //    " - ----- - - - --------",
        //    "     | | | | | |       ",
        //    "-- - - - - - - - ----- ",
        //    "   |   | | |   | |   | ",
        //    " ----- - - ----- - --- ",
        //    "     | | | |   | |   | ",
        //    "---- - - --- - - --- - ",
        //    " |   | |     | |   | | ",
        //    " - --- ------- --- - - ",
        //    "     |       |     |  F"
        //};
        var start = new AStarObject();
        start.X = (int)currentPosition.z;
        start.Z = (int)currentPosition.x;


        var finish = new AStarObject();
        finish.X = _map.FindIndex(x => x.Contains("F"));
        finish.Z = _map[finish.X].IndexOf("F");

        start.SetDistance(finish.Z, finish.X);

        var activeTiles = new List<AStarObject>();
        activeTiles.Add(start);
        var visitedTiles = new List<AStarObject>();

        while (activeTiles.Any())
        {
            var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

            if (checkTile.Z == finish.Z && checkTile.X == finish.X)
            {
                //We found the destination and we can be sure (Because the the OrderBy above)
                //That it's the most low cost option. 
                var tile = checkTile;
                while (true)
                {
                    if (_map[tile.X][tile.Z] == ' ')
                    {
                        //var newMapRow = _map[tile.Y].ToCharArray();
                        //newMapRow[tile.X] = '*';
                        //_map[tile.Y] = new string(newMapRow);

                        _bestPath.Insert(0, tile);
                    }
                    tile = tile.Parent;
                    if (tile == null)
                    {
                        return;
                    }
                }
            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);

            var walkableTiles = GetWalkableTiles(_map, checkTile, finish);

            foreach (var walkableTile in walkableTiles)
            {
                //We have already visited this tile so we don't need to do so again!
                if (visitedTiles.Any(x => x.Z == walkableTile.Z && x.X == walkableTile.X))
                    continue;

                //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                if (activeTiles.Any(x => x.Z == walkableTile.Z && x.X == walkableTile.X))
                {
                    var existingTile = activeTiles.First(x => x.Z == walkableTile.Z && x.X == walkableTile.X);
                    if (existingTile.CostDistance > checkTile.CostDistance)
                    {
                        activeTiles.Remove(existingTile);
                        activeTiles.Add(walkableTile);
                    }
                }
                else
                {
                    //We've never seen this tile before so add it to the list. 
                    activeTiles.Add(walkableTile);
                }
            }
        }

        Debug.Log("No Path Found!");
    }

    private void ConvertMazeCellsTOStringArray()
    {
        //only checking east and south walls
        var map = new List<String>();
        for(var i = 0; i < _row; i++)
        {
            var rowString = "";
            var subRowString = "";
            for(var j = 0; j < _col; j++)
            {
                //Make firstspot and wall
                if(i == 0 & j == 0)
                {
                    rowString += "S";
                    if (_mazecells[i, j].eastWall)
                    {
                        rowString += "|";
                    }
                    else
                    {
                        rowString += " ";
                    }
                }

                //last column
                if(i == _row - 1 & j == _col - 1)
                {
                    rowString += "F";
                }

                //check if not in first or last cell
                if(!(i == 0 & j == 0))
                {
                    if(!(i == _row - 1 & j == _col - 1))
                    { 
                         //checks if not in last column
                        if (j != _col - 1)
                        {
                            if (_mazecells[i, j].eastWall)
                            {
                                rowString += " |";
                            }
                            else
                            {
                                rowString += "  ";
                            }
                        }
                        else
                        {
                            rowString += " ";
                        }
                    }
                }

                //check not in last row
                if (i != _row - 1)
                {
                    if(j != _col - 1)
                    {
                        if (_mazecells[i, j].southWall)
                        {
                            subRowString += "--";
                        }
                        else
                        {
                            subRowString += " -";
                        }
                    }
                    else
                    {
                        if (_mazecells[i, j].southWall)
                        {
                            subRowString += "-";
                        }
                        else
                        {
                            subRowString += " ";
                        }
                    }
                }
            }
            
            map.Add(rowString);
            if(subRowString.Length > 0)
            {
                map.Add(subRowString);
            }
        }

        _map = map;
    }

    private void SetMazeCoordinates()
    {
        _maze_coordinates = new Vector3?[_map.Count, _map[0].Length];
        var icount = 0;
        var jcount = 0;
        for(var i = 0; i < _map.Count; i++)
        {
            for(var j = 0; j < _map[0].Length; j++)
            {
                if(i != 0 & i%2 != 0)
                {
                    _maze_coordinates[i, j] = null;
                }
                else
                {
                    if (j % 2 != 0)
                    {
                        if (!(i == 0 & j == 0))
                        {
                            _maze_coordinates[i, j] = null;
                        }
                        else
                        {
                            _maze_coordinates[i, j] = new Vector3(icount, 0, jcount) * 6;
                            jcount++;
                        }
                    }
                    else
                    {
                        _maze_coordinates[i, j] = new Vector3(icount, 0, jcount) * 6;
                        jcount++;
                    }
                }
            }
            if(i % 2 == 0) 
            { 
                icount++;
            }
            jcount = 0;
        }
    }

    private List<AStarObject> GetWalkableTiles(List<string> map, AStarObject currentCell, AStarObject targetCell)
    {
        var possibleTiles = new List<AStarObject>()
            {
                new AStarObject { Z = currentCell.Z, X = currentCell.X - 1, Parent = currentCell, Cost = currentCell.Cost + 1},
                new AStarObject { Z = currentCell.Z, X = currentCell.X + 1, Parent = currentCell, Cost = currentCell.Cost + 1},
                new AStarObject { Z = currentCell.Z - 1, X = currentCell.X, Parent = currentCell, Cost = currentCell.Cost + 1},
                new AStarObject { Z = currentCell.Z + 1, X = currentCell.X, Parent = currentCell, Cost = currentCell.Cost + 1},
            };

        possibleTiles.ForEach(tile => tile.SetDistance(targetCell.Z, targetCell.X));

        var maxX = map.First().Length - 1;
        var maxY = map.Count - 1;

        return possibleTiles
                .Where(tile => tile.Z >= 0 && tile.Z <= maxX)
                .Where(tile => tile.X >= 0 && tile.X <= maxY)
                .Where(tile => map[tile.X][tile.Z] == ' ' || map[tile.X][tile.Z] == 'F')
                .ToList();
    }
}

class AStarObject
{
    public int Z { get; set; }
    public int X { get; set; }
    public int Cost { get; set; }
    public int Distance { get; set; }
    public int CostDistance => Cost + Distance;
    public AStarObject Parent { get; set; }
    //The distance is essentially the estimated distance, ignoring walls to our target. 
    //So how many tiles left and right, up and down, ignoring walls, to get there. 

    public void SetDistance(int targetZ, int targetX)
    {
        this.Distance = Math.Abs(targetZ - Z) + Math.Abs(targetX - X);
    }
}

