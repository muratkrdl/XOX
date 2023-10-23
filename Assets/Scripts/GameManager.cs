using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public static GameManager instance { get; private set; }

    public int turn { get; set; }

    public List<TileController> ListTileController => listTileController;
    [SerializeField] List<TileController> listTileController;
    [SerializeField] Canvas canvas;

    public bool isGameOver = false;

    void Awake() 
    {
        instance = this;   
    }

    public (bool, TileState) HasWinner()
    {
        foreach(var tile in listTileController)
        {
            if(tile.MyState == TileState.None) continue;

            foreach(var direction in Enum.GetValues(typeof(Direction)))
            {
                var nextTile = tile.GetNextTile((Direction)direction);
                if(!nextTile) continue;

                if(tile.MyState != nextTile.MyState) continue;

                var lastTile = nextTile.GetNextTile((Direction)direction);
                if(!lastTile) continue;
                if(lastTile.MyState != tile.MyState) continue;

                return (true, tile.MyState);
            }
        }

        return (false, TileState.None);
    }

}
