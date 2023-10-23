using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum TileState
{
    None,
    X,
    O
}

public enum Direction
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    LeftDown,
    Left,
    UpLeft
}


public class TileController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] SpriteRenderer mySpriteRenderer;
    
    [SerializeField] Sprite xSprite;
    [SerializeField] Sprite oSprite;

    [SerializeField] Color xColor;
    [SerializeField] Color oColor;


    public TileState MyState { get; set; }

    public Vector2 coordinate;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MyState != TileState.None) { return; }
        if(GameManager.instance.isGameOver) { return; }

        var state = GameManager.instance.turn % 2 == 0 ? TileState.X : TileState.O;
        SetState(state);
        GameManager.instance.turn++;

        var result = GameManager.instance.HasWinner();
        if(result.Item1)
        {
            Debug.Log($"Winner is: {result.Item2}");
            Invoke("ResetGame",2);
            GameManager.instance.isGameOver = true;
        }
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetState(TileState state)
    {
        MyState = state;
        mySpriteRenderer.color = state == TileState.X ? xColor : oColor;
        mySpriteRenderer.sprite = state == TileState.X ? xSprite : oSprite;
    }

    public TileController GetNextTile(Direction direction)
    {
        var nextTileCoordinate = coordinate;
        switch(direction)
        {
            case Direction.Up:
                nextTileCoordinate.y++;
                break;
            case Direction.UpRight:
                nextTileCoordinate.y++;
                nextTileCoordinate.x++;
                break;
            case Direction.Right:
                nextTileCoordinate.x++;
                break;
            case Direction.DownRight:
                nextTileCoordinate.x++;
                nextTileCoordinate.y--;
                break;
            case Direction.Down:
                nextTileCoordinate.y--;
                break;
            case Direction.LeftDown:
                nextTileCoordinate.y--;
                nextTileCoordinate.x--;
                break;
            case Direction.Left:
                nextTileCoordinate.x--;
                break;
            case Direction.UpLeft:
                nextTileCoordinate.x--;
                nextTileCoordinate.y++;
                break;
        } 

        return GameManager.instance.ListTileController.Find(tile => tile.coordinate == nextTileCoordinate);
    }

}
