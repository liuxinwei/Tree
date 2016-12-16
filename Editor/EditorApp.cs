using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorApp
{
    [MenuItem("Tree/BSTree")]
    public static void BSTree()
    {
       //BSTreeWindow win = EditorWindow.GetWindow<BSTreeWindow>();
       //win.Show();
        PathWindow win = EditorWindow.GetWindow<PathWindow>();
       win.Show();
    }
}
