using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : ISpawner
{
    public GameObject[] PlayerObjects;

    public override void Spawn(Grid grid)
    {
        // Find places for playerObjects.Count go
        // Spawn them

        List<TileCoord> possibilities = new List<TileCoord>();

        List<GameObject> playerList = new List<GameObject>(PlayerObjects);

        if (PlayerContainer.Instance.InitialSpawning)
        {
            PlayerContainer.Instance.InitialSpawning = false;
            PlayerContainer.Instance.PlayerObjects = new List<GameObject>(PlayerObjects);
        }
        else
        {
            playerList = new List<GameObject>(PlayerContainer.Instance.PlayerObjects);
        }

        for (int i = 0; i < grid.Width; ++i)
        {
            possibilities.Add(new TileCoord(i, 0));
            possibilities.Add(new TileCoord(i, grid.Height - 1));
        }

        for (int i = 1; i < grid.Height - 1; ++i)
        {
            possibilities.Add(new TileCoord(0, i));
            possibilities.Add(new TileCoord(grid.Width - 1, i));
        }

        bool foundSolution = false;
        while (!foundSolution && possibilities.Count > 0)
        {
            int randomIndex = Random.Range(0, possibilities.Count);
            List<Tile> validTiles = new List<Tile>();
            if (ValidateTile(grid, grid.GetTile(possibilities[randomIndex]), ref validTiles, playerList.Count))
            {
                for (int i = 0; i < validTiles.Count; ++i)
                {
                    GameObject go = (GameObject)GameObject.Instantiate(playerList[i], validTiles[i].transform.position, PlayerObjects[i].transform.rotation);
                    grid.m_Players.Add(go);
                }
                foundSolution = true;
            }
            else
            {
                possibilities.RemoveAt(randomIndex);
            }
        }
        Debug.Log("Spawned Players...");
    }

    private bool ValidateTile(Grid grid, Tile tile, ref List<Tile> validTiles, int number)
    {
        validTiles = grid.GetNConnectedTileInRange(tile, 1, number);

        if (validTiles.Count != number)
        {
            validTiles.Clear();
        }

        return validTiles.Count == number;
    }
}
