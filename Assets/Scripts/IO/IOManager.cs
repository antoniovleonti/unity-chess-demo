using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IOManager : MonoBehaviour
{
    // defined from Unity editor
    public GameObject boardPrefab;
    public GameObject squarePrefab;
    public GameObject draggingPrefab;
    public PieceSet pieceSet;

    private MouseIO mouseio = new MouseIO();
    
    public const uint Ny = 8; // number of squares
    public const uint Nx = 8;

    private GameObject boardPanel;
    private UIPiece[,] squares = new UIPiece[Ny, Nx];
    private Game game;

    int player = 1;

    private void Awake()
    {
        // initialize new game from default position
        game = new Game();

        // init UI board

        boardPanel = Instantiate(boardPrefab, transform);

        Color light = Color.Lerp(Color.white, Color.grey, 0.2f);
        Color dark = Color.Lerp(Color.green, Color.grey, 0.8f);

        for (int i = 0; i < Ny; i++) 
        {
            for (int j = 0; j < Nx; j++) 
            {
                // create square
                var sqr = Instantiate(squarePrefab, boardPanel.transform);
                // determine color of square
                Color color = (i & 1) == (j & 1) ? light : dark;

                // create object for neat reference
                var tmp = new UIPiece(sqr);

                tmp.Background = color;

                // set attributes of square

                // create event triggers
                AddEvent(sqr, EventTriggerType.PointerEnter, delegate { OnPointerEnter(tmp); });
                AddEvent(sqr, EventTriggerType.BeginDrag, delegate { OnDragStart(tmp); });
                AddEvent(sqr, EventTriggerType.EndDrag, delegate { OnDragEnd(tmp); });
                AddEvent(sqr, EventTriggerType.Drag, delegate { OnDrag(tmp); });

                squares[i, j] = tmp;
            }
        }

        UpdateSprites(1);
    }
    private void UpdateSprites(int player)
    {
        int q = (player > 0) ? 1 : 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                // get coords we want to display with this square
                int y = q * 7 - player * i;
                int x = q * 7 - player * j;

                // get appropriate sprite
                Sprite s = pieceSet.IntToSprite(game.board.value[y, x]);

                // assign sprite to square
                squares[i, j].Sprite = s;

                // update square being displayed
                squares[i, j].Pos = new Pos2(y, x);
            }
        }
    }
    // simplifies process of adding events
    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);

        trigger.triggers.Add(eventTrigger);
    }

    private void OnPointerEnter(UIPiece square)
    {
        Pos2 yx = square.Pos;

        mouseio.dst = new Pos2(yx);
    }

    private void OnDragStart(UIPiece square)
    {
        Pos2 yx = square.Pos;
        int y = yx.y;
        int x = yx.x;


        if (game.board.value[y, x] != 0)
        {
            Sprite s = pieceSet.IntToSprite(game.board.value[y, x]);

            mouseio.dragging = Instantiate(draggingPrefab, transform);
            mouseio.dragging.transform.Find("PIECE").GetComponent<Image>().sprite = s;

            square.Sprite = null;
        }
    }
    private void OnDragEnd(UIPiece square)
    {
        Pos2 yx = square.Pos;
        int y = yx.y;
        int x = yx.x;

        if (mouseio.dragging != null)
        {
            Object.Destroy(mouseio.dragging);

            Move input = new Move(new Pos2(yx), mouseio.dst);

            //Debug.Log($"{{{input.ToString()}}}");

            game.TryMove(input);
        }
        // update board
        UpdateSprites(1);
    }
    private void OnDrag(UIPiece obj)
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseio.dragging != null)
        {
            mouseio.dragging.transform.position = new Vector3(p.x, p.y, 1);
        }

        //Debug.Log($"{new Vector3(p.x, p.y, 0)} vs. {mouseio.dragging.transform.position}");
    }
}

public class MouseIO
{
    public GameObject dragging;
    public Pos2 dst;
}