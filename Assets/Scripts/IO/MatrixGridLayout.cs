using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatrixGridLayout : LayoutGroup
{
    public int rows;
    public int columns;

    Vector2 cellSize;

    public override void CalculateLayoutInputVertical()
    {
        float sqrRt = Mathf.Sqrt(transform.childCount);
        rows = columns = Mathf.CeilToInt(sqrRt);

        float parentWidth  = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / columns;
        float cellHeight = parentHeight / rows;

        cellSize.x = cellWidth;
        cellSize.y = cellHeight;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            int col = i / columns;
            int row = i % rows;

            var item = rectChildren[i];

            var xPos = cellSize.x * row;
            var yPos = cellSize.y * col;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
    }
}
