﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DS.BLL
{
    public class BSTreeBLL
    {
        public static BSTree Create(List<int> list)
        {
            BSTree bsTree = new BSTree()
            {
                Data = list[0],
                Left = null,
                Right = null
            };
            bsTree.layer = 1;
            bsTree.index = 1;
            for (int i = 1; i < list.Count; i++)
            {
                bool isExcute = false;
                Insert(bsTree, list[i], ref isExcute);
            }
            return bsTree;
        }

        public static void Insert(BSTree bsTree, int key, ref bool isExcute)
        {
            if (bsTree == null)
                return;
            if (key < bsTree.Data) 
            {
                Insert(bsTree.Left, key, ref isExcute);
            }
            else
            {
                Insert(bsTree.Right, key, ref isExcute);
            }

            if (!isExcute)
            {
                BSTree current = new BSTree()
                {
                    Data = key,
                    Left = null,
                    Right = null
                };

                if (key < bsTree.Data)
                {
                    bsTree.Left = current;
                }
                else
                {
                    bsTree.Right = current;
                }
                isExcute = true;
            }
        }

        public static void LDR(BSTree bsTree)
        {
            if (bsTree != null)
            {
                LDR(bsTree.Left);
                LDR(bsTree.Right);
            }
        }

        public static bool Search(BSTree bsTree, int key)
        {
            if (bsTree == null) return false;
            if (key == bsTree.Data) return true;
            if (key < bsTree.Data) return Search(bsTree.Left, key);
            else return Search(bsTree.Right, key);
        }

        public static void Delete(ref BSTree bsTree, int key)
        {
            if (bsTree == null) return;
            if (key == bsTree.Data)
            {
                if (bsTree.Left == null && bsTree.Right == null)
                {
                    bsTree = null;
                    return;
                }
                if (bsTree.Left != null && bsTree.Right == null)
                {
                    bsTree = bsTree.Left;
                    return;
                }
                if (bsTree.Left == null && bsTree.Right != null)
                {
                    bsTree = bsTree.Right;
                    return;
                }
                if (bsTree.Left != null && bsTree.Right != null)
                {
                    var node = bsTree.Right;
                    while (node.Left != null)
                    {
                        node = node.Left;
                    }

                    node.Left = bsTree.Left;

                    if (node.Right == null)
                    {
                        Delete(ref bsTree, node.Data);
                        node.Right = bsTree.Right;
                    }
                    bsTree = node;
                }
            }
            if (key < bsTree.Data)
            {
                Delete(ref bsTree.Left, key);
            }
            else
            {
                Delete(ref bsTree.Right, key);
            }
        }

        public static void ResetIndex(BSTree _bsTree)
        {
            if (_bsTree == null)
            {
                return;
            }
            if (_bsTree.Left != null)
            {
                _bsTree.Left.index = _bsTree.index * 2;
                ResetIndex(_bsTree.Left);
            }
            if (_bsTree.Right != null)
            {
                _bsTree.Right.index = _bsTree.index * 2 + 1;
                ResetIndex(_bsTree.Right);
            }
        }

        public static void ResetLayer(BSTree _bsTree, ref int _maxLayer)
        {
            if(_bsTree == null)
            {
                return;
            }
            if (_bsTree.Left != null)
            {
                _bsTree.Left.layer = _bsTree.layer + 1;
                _maxLayer = Mathf.Max(_maxLayer, _bsTree.Left.layer);
                ResetLayer(_bsTree.Left, ref _maxLayer);
            }
            if (_bsTree.Right != null)
            {
                _bsTree.Right.layer = _bsTree.layer + 1;
                _maxLayer = Mathf.Max(_maxLayer, _bsTree.Right.layer);
                ResetLayer(_bsTree.Right, ref _maxLayer);
            }
        }

        public static void Main()
        {
            List<int> list = new List<int> { 50, 30, 70, 10, 40, 90, 80 };

            Debug.Log("***************创建二叉排序树***************");
            BSTree bsTree = BSTreeBLL.Create(list);
            Debug.Log("中序遍历的原始数据:\n");
            BSTreeBLL.LDR(bsTree);

            Debug.Log("\n********************查找节点********************");
            Debug.Log("元素40是否在树中:" + BSTreeBLL.Search(bsTree, 40));

            Debug.Log("\n********************插入节点********************");
            Debug.Log("将元素20插入到树中");
            bool isExcute = false;
            BSTreeBLL.Insert(bsTree, 20, ref isExcute);
            Debug.Log("中序遍历后:\n");
            BSTreeBLL.LDR(bsTree);

            Debug.Log("\n********************删除节点1********************");
            Debug.Log("删除叶子节点20,\n中序遍历后:\n");
            BSTreeBLL.Delete(ref bsTree, 20);
            BSTreeBLL.LDR(bsTree);

            Debug.Log("\n********************删除节点2********************");
            Debug.Log("删除单孩子节点90,\n中序遍历后:\n");
            BSTreeBLL.Delete(ref bsTree, 90);
            BSTreeBLL.LDR(bsTree);

            Debug.Log("\n********************删除节点2********************");
            Debug.Log("删除根节点50,\n中序遍历后:\n");
            BSTreeBLL.Delete(ref bsTree, 50);
            BSTreeBLL.LDR(bsTree);

        }
    }

    public class BSTree
    {
        public int Data;
        public BSTree Left;
        public BSTree Right;
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
            point.y = layer * nodeSize * 2 * hRatio/100;
            point.x = point.x + startW;
        }

        public int GetRowIndex()
        {
            if(layer == 1)
            {
                return 0;
            }
            int curindex = index - ((int)Mathf.Pow(2, layer - 1) - 1) - 1;
            return curindex;
        }
    }
}