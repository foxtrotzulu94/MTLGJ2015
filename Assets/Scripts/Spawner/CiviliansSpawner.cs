using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CiviliansSpawner : ISpawner
{
    public int MinCivilian = 2;
    public int MaxCivilian = 7;
    public GameObject[] CivilianPrefab;

    public override void Spawn(Grid grid)
    {
        List<Tile> possibilities = new List<Tile>();
        for(int i = 0; i < grid.m_Tiles.Count; ++i)
        {
            for(int j = 0; j < grid.m_Tiles[i].Count; ++j)
            {
                possibilities.Add(grid.m_Tiles[i][j]);
            }
        }
        if(grid.m_Players.Count > 0)
        {
            Tile playerTile = grid.GetTileAtPosition(grid.m_Players[0].transform.position);

            int longestRange = Mathf.Max(grid.Width, grid.Height);
            int range = Mathf.RoundToInt((float)longestRange * 0.75f);

            List<Tile> tilesAroundPlayer = grid.GetConnectedTileInRange(playerTile, range);

            for (int i = 0; i < tilesAroundPlayer.Count; ++i)
            {
                possibilities.Remove(tilesAroundPlayer[i]);
            }

            float sqrRange = range * range;
            for (int i = 0; i < possibilities.Count; ++i)
            {

                float sqrDistance = (playerTile.transform.position - possibilities[i].transform.position).sqrMagnitude;
                if (sqrDistance < sqrRange * 0.5f)
                {
                    possibilities.RemoveAt(i--);
                }
            }

            int numberOfCivilian = Random.Range(MinCivilian, MaxCivilian + 1);
            for (int i = 0; i < numberOfCivilian && possibilities.Count > 0; ++i)
            {
                int randomIndex = Random.Range(0, possibilities.Count);
                int randomCivilian = Random.Range(0, CivilianPrefab.Length);
                GameObject.Instantiate(CivilianPrefab[randomCivilian], possibilities[randomIndex].transform.position, 
                    CivilianPrefab[randomCivilian].transform.rotation);
            }
        }
    }
}
