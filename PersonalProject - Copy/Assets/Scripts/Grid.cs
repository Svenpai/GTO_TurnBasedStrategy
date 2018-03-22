﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Grid : MonoBehaviour
{

    [Space(10)]
    public Tile Tile;
    public Corner Corner;

    [Space(10)]
    public int Length; //X
    public int Width; //Y

    [Space(10)]
    public Tile[,] TileArray;
    public Corner[,] CornerArray;

    // Use this for initialization
    void Start()
    {
        SpawnGrid();
    }

    //Spawns the field tile and corner prefabs for the length and width given.
    void SpawnGrid()
    {
        GameObject Field = new GameObject("Field");

        // Spawning tiles
        GameObject Tiles = new GameObject("Tiles");
        Tiles.transform.SetParent(Field.transform);
        TileArray = new Tile[Length, Width];
        for (int x = 0; x < Length; x++)
        {
            for (int y = 0; y < Width; y++)
            {
                TileArray[x, y] = Instantiate(Tile, new Vector3(x * 10, 0, y * 10), Quaternion.identity, Tiles.transform);
                TileArray[x, y].Position = new Point(x, y);
            }
        }

        //Spawning corners
        GameObject Corners = new GameObject("Corners");
        Corners.transform.SetParent(Field.transform);
        CornerArray = new Corner[Length + 1, Width + 1];
        for (int x = 0; x < Length + 1; x++)
        {
            for (int y = 0; y < Width + 1; y++)
            {
                CornerArray[x, y] = Instantiate(Corner, new Vector3(x * 10 - 5, 0.1f, y * 10 - 5), Quaternion.identity, Corners.transform);
            }
        }
    }

    public Point GetFreeTilePos()
    {
        List<Tile> FreeTiles = new List<Tile>();
        foreach (Tile t in TileArray)
        {
            if (t.transform.childCount == 0)
            {
                FreeTiles.Add(t);
            }
        }

        if (FreeTiles.Count > 0)
        {
            var number = Random.Range(0, FreeTiles.Count);

            for (int x = 0; x < Length; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    if (TileArray[x, y] == FreeTiles[number])
                    {
                        return new Point(x, y);
                    }
                }
            }
        }

        print("No free tiles left!");
        return null;
    }

    public Tile GetFreeTile()
    {
        List<Tile> FreeTiles = new List<Tile>();
        foreach (Tile t in TileArray)
        {
            if (t.transform.childCount == 0 && t.Owner == null)
            {
                FreeTiles.Add(t);
            }
        }

        if (FreeTiles.Count > 0)
        {
            var number = Random.Range(0, FreeTiles.Count);

            return FreeTiles[number];
        }

        print("No free tiles left!");
        return null;
    }

    public Point TileToPoint(Tile t)
    {
        for (int x = 0; x < Length; x++)
        {
            for (int y = 0; y < Width; y++)
            {
                if (TileArray[x, y] == t)
                {
                    return new Point(x, y);
                }
            }
        }

        Debug.LogError("Error: Tile does not exist in grid!");
        return null;
    }

    public void AddPlayerTiles(Tile tile, Player player)
    {
        print("Update terrain");
        Terrainer structure = (Terrainer)tile.transform.GetComponentInChildren<Terrainer>();
        int range = structure.Range;

        List<Tile> rangetiles = new List<Tile>();
        if (TileArray[tile.Position.X, tile.Position.Y].Owner == player)
        {
            rangetiles.Add(TileArray[tile.Position.X, tile.Position.Y]);

            //counts down range
            for (int i = range; i > 0; i--)
            {
                List<Tile> TempList = new List<Tile>();
                foreach (Tile t in rangetiles)
                {
                    //Get each tile around the tile, if tile is not in list already, add it.
                    if (t.Position.X + 1 < Length)
                    {
                        if (!rangetiles.Contains(TileArray[t.Position.X + 1, t.Position.Y]))
                        {
                            TempList.Add(TileArray[t.Position.X + 1, t.Position.Y]);
                        }
                    }

                    if (t.Position.X - 1 >= 0)
                    {
                        if (!rangetiles.Contains(TileArray[t.Position.X - 1, t.Position.Y]))
                        {
                            TempList.Add(TileArray[t.Position.X - 1, t.Position.Y]);
                        }
                    }

                    if (t.Position.Y + 1 < Width)
                    {
                        if (!rangetiles.Contains(TileArray[t.Position.X, t.Position.Y + 1]))
                        {
                            TempList.Add(TileArray[t.Position.X, t.Position.Y + 1]);
                        }
                    }

                    if (t.Position.Y - 1 >= 0)
                    {
                        if (!rangetiles.Contains(TileArray[t.Position.X, t.Position.Y - 1]))
                        {
                            TempList.Add(TileArray[t.Position.X, t.Position.Y - 1]);
                        }
                    }
                }

                rangetiles.AddRange(TempList);
            }
        }

        foreach (Tile t in rangetiles)
        {
            t.SetOwner(player);
        }
    }
}
