using UnityEngine;
using System.Collections;
using UnityEditor;
using DS.BLL;
using System.Collections.Generic;

public class BSTreeWindow : EditorWindow
{
    TreeNode root;
    int value;
    int maxLayer;
    int delvalue = -1;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();

        TreeNode.wRatio = EditorGUILayout.FloatField("wRatio", TreeNode.wRatio);
        TreeNode.hRatio = EditorGUILayout.FloatField("hRatio", TreeNode.hRatio);
        EditorGUILayout.BeginHorizontal();
        value = EditorGUILayout.IntField("Value", value);
        if(GUILayout.Button("Add Node"))
        {
            List<int> list = new List<int> { 50, 30, 70, 10, 40, 90, 80, 20, 22, 23, 24 };
            for (int i = 0; i < list.Count; i++)
            {
                if (root == null)
                {
                    root = new TreeNode();
                    root.data = list[i];
                    root.layer = 1;
                    root.index = 1;
                }
                else
                {
                    maxLayer = 0;
                    BinaryTree.Insert(root, list[i]);
                    BinaryTree.ResetIndex(root);
                    BinaryTree.ResetLayer(root, ref maxLayer);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        value = EditorGUILayout.IntField("Value", value);
        if (GUILayout.Button("Del Node"))
        {
            BinaryTree.Delete(ref root, value);
            maxLayer = 0;
            root.index = 1;
            root.layer = 1;
            BinaryTree.ResetIndex(root);
            BinaryTree.ResetLayer(root, ref maxLayer);
        }
        EditorGUILayout.EndHorizontal();

        CalculatePosition(root);   
        DrawLine(root);
        DrawNode(root);
    }

    void CalculatePosition(TreeNode _root)
    {
        if(_root == null)
        {
            return;
        }
        _root.CalculatePosition(maxLayer);
        CalculatePosition(_root.leftNode);
        CalculatePosition(_root.rightNode);
    }

    void DrawNode(TreeNode _tree)
    {
        if (_tree == null)
        {
            return;
        }
        Rect rect = new Rect(_tree.point.x - BSTree.nodeSize / 2, _tree.point.y - BSTree.nodeSize / 2, BSTree.nodeSize, BSTree.nodeSize);

        if (delvalue == _tree.data)
        {
            GUI.color = Color.red;
        }
        if (GUI.Button(rect, _tree.data.ToString()))
        {
            delvalue = _tree.data;
        }
        GUI.color = Color.white;
        DrawNode(_tree.leftNode);
        DrawNode(_tree.rightNode);
    }

    void DrawLine(TreeNode _tree)
    {
        if (_tree == null)
        {
            return;
        }
        if(_tree.leftNode != null)
        {
            Handles.DrawLine(_tree.point, _tree.leftNode.point);
            DrawLine(_tree.leftNode);
        }
        if(_tree.rightNode != null)
        {
            Handles.DrawLine(_tree.point, _tree.rightNode.point);
            DrawLine(_tree.rightNode);
        }
    }
}
