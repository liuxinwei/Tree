using UnityEngine;
using System.Collections;

public class TreeNode
{
    public TreeNode leftNode;
    public TreeNode rightNode;
    public int data;

    public int layer;
    public int index;
    public Vector2 point;

    public static int nodeSize = 40;
    public static float wRatio = 100;
    public static float hRatio = 100;

    public void CalculatePosition(int _maxLayer)
    {
        int _maxCount = (int)Mathf.Pow(2, _maxLayer - 1);
        int rowCount = (int)Mathf.Pow(2, layer - 1);
        int nextRountCount = (int)Mathf.Pow(2, layer);
        float curNodeSize = nodeSize * wRatio / 100;
        float startX = _maxCount * curNodeSize / nextRountCount;
        int curindex = GetRowIndex();
        int count = (int)Mathf.Pow(2, _maxLayer - layer);
        float startW = (Screen.width - _maxCount * curNodeSize) / 2;
        point.x = startX + curindex * (count * curNodeSize);
        point.y = layer * nodeSize * 2 * hRatio / 100 + 100;
        point.x = point.x + startW;
    }

    public int GetRowIndex()
    {
        if (layer == 1)
        {
            return 0;
        }
        int curindex = index - ((int)Mathf.Pow(2, layer - 1) - 1) - 1;
        return curindex;
    }

}
