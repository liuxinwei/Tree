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


//open=[Start]
//closed=[]
//while open不为空 
//{
//    从open中取出估价值f最小的结点n
//    if n == Target then
//    return 从Start到n的路径 // 找到了！！！
//    else 
//    {
//        for n的每个子结点x 
//        {
//            if x in open 
//            {
//                计算新的f(x)
//                比较open表中的旧f(x)和新f(x)
//                if 新f(x) < 旧f(x)
//                {
//                    删掉open表里的旧f(x)，加入新f(x)
//                }
//            }
//            else if x in closed 
//            {
//                计算新的f(x)
//                比较closed表中的旧f(x)和新f(x)
//                if 新f(x) < 旧f(x)
//                {
//                    remove x from closed
//                    add x to open
//                }
//            }   比较新f(x)和旧f(x) 实际上比的就是新旧g(x),因h(x)相等
//            else 
//            {
//                // x不在open，也不在close，是遇到的新结点
//                计算f(x) add x to open
//            }
//        }
//        add n to closed
//    }
//}
//open表为空表示搜索结束了，那就意味着无解
