using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpawner : ISpawner
{
    //public int MinFireSpot = 2;
    //public int MaxFireSpot = 7;
    public int MinFlamePerSpot = 3;
    public int MaxFlamePerSpot = 8;

    public override void Spawn(Grid grid)
    {
        List<Tile> possibilities = new List<Tile>();
        for (int i = 0; i < grid.m_Tiles.Count; ++i)
        {
            for (int j = 0; j < grid.m_Tiles[i].Count; ++j)
            {
                possibilities.Add(grid.m_Tiles[i][j]);
            }
        }

        if (grid.m_Players.Count > 0)
        {
            Tile playerTile = grid.GetTileAtPosition(grid.m_Players[0].transform.position);

            int longestRange = Mathf.Max(grid.Width, grid.Height);
            int range = Mathf.RoundToInt((float)longestRange * 0.5f);

            List<Tile> tilesAroundPlayer = grid.GetConnectedTileInRange(playerTile, range);

            for (int i = 0; i < tilesAroundPlayer.Count; ++i)
            {
                possibilities.Remove(tilesAroundPlayer[i]);
            }

            foreach (GameObject civilian in grid.m_Civilian)
            {
                Tile civilianTile = grid.GetTileAtPosition(civilian.transform.position);

                List<Tile> tilesAroundCivilian = grid.GetConnectedTileInRange(civilianTile, 4);

                for (int i = 0; i < tilesAroundCivilian.Count; ++i)
                {
                    possibilities.Remove(tilesAroundCivilian[i]);
                }
            }

            int numberOfFireSpot = Mathf.Max(1, (PlayerContainer.Instance.level+1) / 2);// Random.Range(MinFireSpot, MaxFireSpot + 1);
            for (int i = 0; i < numberOfFireSpot && possibilities.Count > 0; ++i)
            {
                int randomIndex = Random.Range(0, possibilities.Count);

                int numberOfFlame = Random.Range(MinFlamePerSpot, MaxFlamePerSpot);
                List<Tile> tiles = grid.GetNConnectedTileInRange(possibilities[randomIndex], 3, numberOfFlame);

                for (int j = 0; j < tiles.Count; ++j)
                {
                    Flammable flammable = tiles[j].GetComponent<Flammable>();
                    flammable.Ignite();
                }
            }
        }
    }
}
