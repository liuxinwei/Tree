using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class PathWindow : EditorWindow
{
    public class Node
    {
        public int id;
        public Node parent;
    }

    Dictionary<int, Node> mHashSet = new Dictionary<int, Node>();
    Queue<Node> mQu = new Queue<Node>();
    public int sx = 10;
    public int sy = 10;

    public int ex = 30;
    public int ey = 30;

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 120, 50), ""))
        {
            Node sn = new Node();
            sn.id = GetPoint(0, sx, sy);
            mQu.Enqueue(sn);
            mHashSet.Add(sn.id, sn);
        }
        if (GUI.Button(new Rect(200, 0, 120, 50), "Next"))
        {

        }
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                if(x == sx && y == sy)
                {
                    GUI.color = Color.green;
                }
                else if(x == ex && y == ey)
                {
                    GUI.color = Color.red;
                }
                else
                {
                    int i = GetPoint(0, x, y);
                    if (mHashSet.ContainsKey(i))
                    {
                        GUI.color = Color.yellow;
                    }
                }
                GUI.Button(new Rect(x * 20, y * 20 + 50, 20, 20), "");
                GUI.color = Color.white;
            }
        }
    }

    void Exenext(Node _node)
    {

    }

    public static int GetPoint(int layer, int x, int y)
    {
        int l = layer << 28;
        int curx = (x << 14) & unchecked((int)0x0fffc000);
        int cury = y & unchecked((int)0x00003fff);
        return l | curx | cury;
    }
}
