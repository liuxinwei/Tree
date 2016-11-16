using UnityEngine;
using System.Collections;
using UnityEditor;
using DS.BLL;
using System.Collections.Generic;

public class BSTreeWindow : EditorWindow
{
    BSTree root;
    int maxLayer;
    int delvalue = -1;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("生成树"))
        {
            List<int> list = new List<int> { 50, 30, 70, 10, 40, 90, 80 , 20 , 22, 23, 24};
            root = BSTreeBLL.Create(list);
            maxLayer = 0;
            BSTreeBLL.ResetIndex(root);
            BSTreeBLL.ResetLayer(root, ref maxLayer);
        }
        if (GUILayout.Button("del"))
        {
            if (delvalue != -1)
            {
                BSTreeBLL.Delete(ref root, delvalue);
                if (root != null)
                {
                    maxLayer = 0;
                    root.layer = 1;
                    root.index = 1;
                    BSTreeBLL.ResetIndex(root);
                    BSTreeBLL.ResetLayer(root, ref maxLayer);
                }
                delvalue = -1;
            }
        }
        GUILayout.EndHorizontal();

        BSTree.wRatio = EditorGUILayout.FloatField("wRatio", BSTree.wRatio);
        BSTree.hRatio = EditorGUILayout.FloatField("hRatio", BSTree.hRatio);
        CalculatePosition(root);   
        DrawLine(root);
        DrawNode(root);

        //if (delvalue != -1)
        //{
        //    BSTreeBLL.Delete(ref root, delvalue);
        //    if (root != null)
        //    {
        //        maxLayer = 0;
        //        root.layer = 1;
        //        root.index = 1;
        //        BSTreeBLL.ResetIndex(root);
        //        BSTreeBLL.ResetLayer(root, ref maxLayer);
        //    }
        //    delvalue = -1;
        //}
    }

    void CalculatePosition(BSTree _root)
    {
        if(_root == null)
        {
            return;
        }
        _root.CalculatePosition(maxLayer);
        CalculatePosition(_root.Left);
        CalculatePosition(_root.Right);
    }

    void DrawNode(BSTree _tree)
    {
        if (_tree == null)
        {
            return;
        }
        Rect rect = new Rect(_tree.point.x - BSTree.nodeSize / 2, _tree.point.y - BSTree.nodeSize / 2, BSTree.nodeSize, BSTree.nodeSize);

        if (delvalue == _tree.Data)
        {
            GUI.color = Color.red;
        }
        if(GUI.Button(rect, _tree.Data.ToString()))
        {
            delvalue = _tree.Data;
        }
        GUI.color = Color.white;
        DrawNode(_tree.Left);
        DrawNode(_tree.Right);
    }

    void DrawLine(BSTree _tree)
    {
        if (_tree == null)
        {
            return;
        }
        if(_tree.Left != null)
        {
            Handles.DrawLine(_tree.point, _tree.Left.point);
            DrawLine(_tree.Left);
        }
        if(_tree.Right != null)
        {
            Handles.DrawLine(_tree.point, _tree.Right.point);
            DrawLine(_tree.Right);
        }
    }
}
