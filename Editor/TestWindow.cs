using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public enum MoveDir
{
    Up,
    Down,
    Left,
    Right,
}

public class TestWindow : EditorWindow
{
    List<PathNode> pathNodes = new List<PathNode>();
    public int nodeindex = 0;

    void OnGUI()
    {
        ShowGUI(new Vector2(0, 0), PathNode.curData);
        ShowGUI(new Vector2(0, 180), PathNode.targetData);

        if (pathNodes.Count > 0)
        {
            ShowGUI(new Vector2(300, 0), pathNodes[nodeindex].data);
        }

        if (GUI.Button(new Rect(200, 0, 100, 30), "执行"))
        {
            nodeindex++;
            nodeindex = Mathf.Min(nodeindex, pathNodes.Count - 1);
        }
        if (GUI.Button(new Rect(200, 50, 100, 30), "Start"))
        {
            nodeindex = 0;
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closeList = new List<PathNode>();
            PathNode pathNode = new PathNode(PathNode.curData);
            closeList.Add(pathNode);
            while (pathNode.data != PathNode.targetData)
            {
                int index = pathNode.GetIndex('0');
                int x = index % 3;
                int y = index / 3;
                for (int i = 0; i < 4; i++)
                {
                    switch ((MoveDir)i)
                    {
                        case MoveDir.Up:
                            {
                                if (y > 0)
                                {
                                    char[] cs = pathNode.data.ToCharArray();
                                    char curc = cs[index];
                                    char lastc = cs[index - 3];
                                    cs[index - 3] = curc;
                                    cs[index] = lastc;
                                    string curdata = new string(cs);

                                    PathNode closeNode = closeList.Find((node) => { return node.data == curdata; });
                                    if (closeNode == null)
                                    {
                                        closeNode = openList.Find((node) => { return node.data == curdata; });
                                    }

                                    if (closeNode != null)
                                    {
                                        if (closeNode.g > pathNode.g + 1)
                                        {
                                            closeNode.g = pathNode.g + 1;
                                            closeNode.parentNode = pathNode;
                                        }
                                    }
                                    else
                                    {
                                        PathNode node = new PathNode(curdata);
                                        node.parentNode = pathNode;
                                        node.g = pathNode.g + 1;
                                        openList.Add(node);
                                    }
                                }
                            }
                            break;
                        case MoveDir.Down:
                            {
                                if (y < 2)
                                {
                                    char[] cs = pathNode.data.ToCharArray();
                                    char curc = cs[index];
                                    char lastc = cs[index + 3];
                                    cs[index + 3] = curc;
                                    cs[index] = lastc;
                                    string curdata = new string(cs);
                                    PathNode closeNode = closeList.Find((node) => { return node.data == curdata; });
                                    if (closeNode == null)
                                    {
                                        closeNode = openList.Find((node) => { return node.data == curdata; });
                                    }
                                    if (closeNode != null)
                                    {
                                        if (closeNode.g > pathNode.g + 1)
                                        {
                                            closeNode.g = pathNode.g + 1;
                                            closeNode.parentNode = pathNode;
                                        }
                                    }
                                    else
                                    {
                                        PathNode node = new PathNode(curdata);
                                        node.parentNode = pathNode;
                                        node.g = pathNode.g + 1;
                                        openList.Add(node);
                                    }
                                }
                            }
                            break;
                        case MoveDir.Left:
                            {
                                if(x > 0)
                                {
                                    char[] cs = pathNode.data.ToCharArray();
                                    char curc = cs[index];
                                    char lastc = cs[index - 1];
                                    cs[index - 1] = curc;
                                    cs[index] = lastc;
                                    string curdata = new string(cs);
                                    PathNode closeNode = closeList.Find((node) => { return node.data == curdata; });
                                    if (closeNode == null)
                                    {
                                        closeNode = openList.Find((node) => { return node.data == curdata; });
                                    }
                                    if (closeNode != null)
                                    {
                                        if (closeNode.g > pathNode.g + 1)
                                        {
                                            closeNode.g = pathNode.g + 1;
                                            closeNode.parentNode = pathNode;
                                        }
                                    }
                                    else
                                    {
                                        PathNode node = new PathNode(curdata);
                                        node.parentNode = pathNode;
                                        node.g = pathNode.g + 1;
                                        openList.Add(node);
                                    }
                                }
                            }
                            break;
                        case MoveDir.Right:
                            {
                                if(x < 2)
                                {
                                    char[] cs = pathNode.data.ToCharArray();
                                    char curc = cs[index];
                                    char lastc = cs[index + 1];
                                    cs[index + 1] = curc;
                                    cs[index] = lastc;
                                    string curdata = new string(cs);
                                    PathNode closeNode = closeList.Find((node) => { return node.data == curdata; });
                                    if (closeNode == null)
                                    {
                                        closeNode = openList.Find((node) => { return node.data == curdata; });
                                    }
                                    if (closeNode != null)
                                    {
                                        if (closeNode.g > pathNode.g + 1)
                                        {
                                            closeNode.g = pathNode.g + 1;
                                            closeNode.parentNode = pathNode;
                                        }
                                    }
                                    else
                                    {
                                        PathNode node = new PathNode(curdata);
                                        node.parentNode = pathNode;
                                        node.g = pathNode.g + 1;
                                        openList.Add(node);
                                    }
                                }
                            }
                            break;
                    }
                }
                if (openList.Count == 0)
                {
                    break;
                }
                openList.Sort((a, b) => { return a.f.CompareTo(b.f); });
                pathNode = openList[0];
                openList.RemoveAt(0);
                closeList.Add(pathNode);
            }
            PathNode curNode = pathNode;
            while (curNode != null)
            {
                pathNodes.Add(curNode);
                curNode = curNode.parentNode;
            }
            pathNodes.Reverse();
        }
    }

    void ShowGUI(Vector2 vec, string _data)
    {
        char[] c = _data.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == '0')
            {
                GUI.color = Color.red;
            }
            GUI.Button(new Rect(vec.x + (i % 3) * 50, vec.y + (i / 3) * 50, 50, 50), c[i].ToString());
            GUI.color = Color.white;
        }
    }
}

public class PathNode
{
    public string data;
    public static string targetData = "123456780";
    public static string curData = "603125478";
    public PathNode parentNode;

    public PathNode(string _data)
    {
        data = _data;
    }

    public int h
    {
        get
        {
            int curH = 0;
            for (int i = 0; i < data.Length; i++)
            {
                int x = i % 3;
                int y = i / 3;
                char c = data[i];
                int index = GetTargetIndex(c);
                int curX = index % 3;
                int curY = index / 3;
                int value = Mathf.Abs(curX - x) + Mathf.Abs(curY - y);
                curH += value;
            }
            return curH;
        }
    }

    public int g;

    public int f
    {
        get { return g + h; }
    }

    public int GetTargetIndex(char _c)
    {
        for (int i = 0; i < targetData.Length; i++)
        {
            if (targetData[i] == _c)
            {
                return i;
            }
        }
        return 0;
    }

    public int GetIndex(char _c)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == _c)
            {
                return i;
            }
        }
        return 0;
    }
}