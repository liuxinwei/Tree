using UnityEngine;
using UnityEditor;
using DS.BLL;
using System.Collections.Generic;

public class MainInspector : Editor
{
    BSTree bsTree;
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("生成树"))
        {
            List<int> list = new List<int> { 50, 30, 70, 10, 40, 90, 80 };
            BSTree bsTree = BSTreeBLL.Create(list);
            BinaryTree<string> k = new BinaryTree<string>();
        }
    }
}
