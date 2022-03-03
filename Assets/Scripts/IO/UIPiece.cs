using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIPiece
{
    // the parent gameobject
    private GameObject square;

    // aliases for each part of the gameobject
    private Image background;
    private Image foreground;
    private Image sprite;

    // board position being represented
    private Pos2 pos;

    // public properties
    public GameObject Square
    { get; set; }
    public Color Background
    {
        get { return background.color; }
        set { background.color = value; }
    }
    public Color Foreground
    {
        get { return foreground.color; }
        set { foreground.color = value; }
    }
    public Sprite Sprite
    {
        get { return sprite.sprite; }
        set 
        {
            sprite.sprite = value;
            sprite.enabled = value != null;
        }
    }
    public Pos2 Pos
    { get; set; }

    public UIPiece(GameObject _square)
    {
        square = _square;

        background = square.transform.Find("BACKGROUND").GetComponent<Image>(); 
        foreground = square.transform.Find("FOREGROUND").GetComponent<Image>();
        sprite = square.transform.Find("PIECE").GetComponent<Image>();

        sprite.color = Color.white;
    }
}
