using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A 星算法，公式：f = g + h;
/// </summary>
public class AStarUtils
{
    /// <summary>
    /// 直角移动的 g 值
    /// </summary>
    public const int STRAIGHT_COST = 1;

    /// <summary>
    /// 对角移动的 g 值
    /// </summary>
    public const int DIAG_COST = 14;

    /// <summary>
    /// 地图节点
    /// </summary>
    public Dictionary<int, AStarNode> nodes = new Dictionary<int, AStarNode>();

    /// <summary>
    /// 地图的宽度(列数)
    /// </summary>
    private int numCols;

    /// <summary>
    /// 地图的高度(行数)
    /// </summary>
    private int numRows;

    /// <summary>
    /// 当前节点到结束节点的估价函数
    /// </summary>
    private IAStarHeuristic iAStarHeuristic;

    /// <summary>
    /// 当前的寻路编号 
    /// </summary>
    private int searchPathCheckNum;

    /// <summary>
    /// 当前查找可移动范围的编号
    /// </summary>
    private int walkableRangeCheckNum;

    /// <summary>
    /// 是否是四向寻路，默认为八向寻路
    /// </summary>
    public bool isFourWay;

    /// <summary>
    /// 存放 "openList" 的最小二叉堆
    /// </summary>
    private BinaryHeapUtils binaryHeapUtils;

    public bool lockAstar = false;

    /// <summary>
    /// 获取节点
    /// </summary>
    /// <returns>The node.</returns>
    /// <param name="nodeX">Node x.</param>
    /// <param name="nodeY">Node y.</param>
    public AStarNode GetNode(int nodeX, int nodeY)
    {
        int nodeKey = GetNodeKey(nodeX, nodeY);
        AStarNode node = null;
        nodes.TryGetValue(nodeKey, out node);
        return node;
    }

    public AStarNode GetNode(int nodeKey)
    {
        if (this.nodes.ContainsKey(nodeKey))
        {
            return this.nodes[nodeKey];
        }
        return null;
    }

    /// <summary>
    /// 组装 Star Key
    /// </summary>
    /// <returns>The node key.</returns>
    /// <param name="nodeX">Node x.</param>
    /// <param name="nodeY">Node y.</param>
    private int GetNodeKey(int nodeX, int nodeY)
    {
        int l = nodeX << 16;

        return l | nodeY;
        //byte[] xbyte = System.BitConverter.GetBytes((short)nodeX);
        //byte[] ybyte = System.BitConverter.GetBytes((short)nodeY);
        //return System.BitConverter.ToInt32(new byte[4] { xbyte[0], xbyte[1], ybyte[0], ybyte[1] }, 0);

    }

    public static int[] dX = new int[] { 0, 1, 0, -1, -1, -1, 1, 1 };
    public static int[] dY = new int[] { 1, 0, -1, 0, -1, 1, -1, 1 };
    public static readonly int dir = 4;
    List<int> findDir = new List<int>() { 0, 1, 2, 3 /*, 4, 5, 6, 7*/ };
    /// <summary>
    /// 获取节点的相邻节点
    /// </summary>
    /// <returns>The adjacent nodes.</returns>
    /// <param name="node">Node.</param>
    private void GetAdjacentNodes(AStarNode node)
    {
        int x = node.nodeX;
        int y = node.nodeY;
        int targetX = 0;
        int targetY = 0;
        node.adjacent.Clear();
        for (int i = 0; i < findDir.Count; i++)
        {
            targetX = x + dX[findDir[i]];
            targetY = y + dY[findDir[i]];
            AStarNode varNode = null;
            if (nodes.TryGetValue(GetNodeKey(targetX, targetY), out varNode))
            {
                node.adjacent.Add(varNode);
            }
        }
    }

    //private void GetAdjacentNodesCallback(AStarNode node, System.Action<AStarNode> returnData)
    //{
    //    int x = node.nodeX;
    //    int y = node.nodeY;
    //    int targetX = 0;
    //    int targetY = 0;

    //    for (int i = 0; i < findDir.Count; i++)
    //    {
    //        targetX = x + dX[findDir[i]];
    //        targetY = y + dY[findDir[i]];
    //        AStarNode varNode = null;
    //        if (nodes.TryGetValue(this.GetNodeKey(targetX, targetY), out varNode))
    //        {
    //            //adjacentNodes.Add(varNode);
    //            if (returnData != null)
    //            {
    //                returnData(varNode);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 刷新节点的 links 属性
    /// </summary>
    /// <param name="node">Node.</param>
    private void RefreshNodeLinks(AStarNode node)
    {
        //Profiler.BeginSample("44444444444444444444444444444");
        //IList<AStarNode> adjacentNodes = GetAdjacentNodes(node);
        //Profiler.EndSample();

        GetAdjacentNodes(node);
        int cost = 0;
        int index = 0;
        for (int i = 0; i < node.adjacent.size; i++)
        {
            if (node.linksPoint[i] != null)
            {
                node.linksPoint[i].node = null;
                node.linksPoint[i].cost = 0;
            }
            AStarNode nodeItem = node.adjacent[i];
            if (nodeItem.walkable)
            {
                if (node.nodeX != nodeItem.nodeX && node.nodeY != nodeItem.nodeY)
                {
                    int key1 = this.GetNodeKey(node.nodeX, nodeItem.nodeY);
                    int key2 = this.GetNodeKey(nodeItem.nodeX, node.nodeY);
                    AStarNode node1 = null;
                    AStarNode node2 = null;
                    if (!nodes.TryGetValue(key1, out node1) || !nodes.TryGetValue(key2, out node2))
                    {
                        continue;
                    }
                    else if (!this.nodes[key1].walkable || !this.nodes[key2].walkable)
                    {
                        continue;
                    }
                    else
                    {
                        cost = DIAG_COST;
                    }
                }
                else
                {
                    cost = STRAIGHT_COST;
                }

                if (node.linksPoint[index] == null)
                {
                    node.linksPoint[index] = new AStarLinkNode(nodeItem, cost);
                }
                else
                {
                    node.linksPoint[index].node = nodeItem;
                    node.linksPoint[index].cost = cost;
                }
                index++;
            }
        }
    }

    /// <summary>
    /// 刷新节点的相邻节点的 links 属性
    /// </summary>
    /// <param name="node">Node.</param>
    private void RefreshLinksOfAdjacentNodes(AStarNode node)
    {
        GetAdjacentNodes(node);
        for (int i = 0; i < node.adjacent.size; i++)
        {
            RefreshNodeLinks(node.adjacent[i]);
        }
    }

    /// <summary>
    /// 刷新所有节点的 links 属性
    /// </summary>
    private void RefreshLinksOfAllNodes()
    {
        var e = nodes.GetEnumerator();      
        while (e.MoveNext())
        {
            RefreshNodeLinks(e.Current.Value);
        }
    }

    /// <summary>
    /// 搜索路径
    /// </summary>
    /// <returns><c>true</c>, if base binary heap was searched, <c>false</c> otherwise.</returns>
    /// <param name="startNode">Start node.</param>
    /// <param name="endNode">End node.</param>
    /// <param name="nowCheckNum">Now check number.</param>
    private bool SearchBaseBinaryHeap(AStarNode startNode, AStarNode endNode, int nowCheckNum)
    {
        int STATUS_CLOSED = nowCheckNum + 1;

        this.binaryHeapUtils.Reset();

        startNode.g = 0;
        startNode.f = startNode.g + this.iAStarHeuristic.Heuristic(startNode.nodeX, startNode.nodeY, endNode.nodeX, endNode.nodeY);
        startNode.searchPathCheckNum = STATUS_CLOSED;

        int g = 0;
        AStarNode node = startNode;
        AStarNode nodeItem;

        while (node != endNode)
        {
            //IList<AStarLinkNode> links = node.links;
            for (int i = 0; i < findDir.Count; i ++ )
            {
                AStarLinkNode linkNode = node.linksPoint[findDir[i]];

                if (linkNode == null || linkNode.node == null)
                {
                    continue;
                }

                //if (findDir[i] >= links.Count)
                //    continue;
                nodeItem = linkNode.node;
                g = node.g + linkNode.cost;

                // 如果已被检查过
                if (nodeItem.searchPathCheckNum >= nowCheckNum)
                {
                    if (nodeItem.g > g)
                    {
                        nodeItem.f = g + this.iAStarHeuristic.Heuristic(nodeItem.nodeX, nodeItem.nodeY, endNode.nodeX, endNode.nodeY);
                        nodeItem.g = g;
                        nodeItem.parentNode = node;
                        if (nodeItem.searchPathCheckNum == nowCheckNum)
                        {
                            this.binaryHeapUtils.ModifyNode(nodeItem.binaryHeapNode);
                        }
                    }
                }
                else
                {
                    nodeItem.f = g + this.iAStarHeuristic.Heuristic(nodeItem.nodeX, nodeItem.nodeY, endNode.nodeX, endNode.nodeY);
                    nodeItem.g = g;
                    nodeItem.parentNode = node;

                    nodeItem.binaryHeapNode = this.binaryHeapUtils.InsertNode(nodeItem);
                    nodeItem.searchPathCheckNum = nowCheckNum;
                }
            }

            if (this.binaryHeapUtils.headNode != null)
            {
                node = this.binaryHeapUtils.PopNode();
                node.searchPathCheckNum = STATUS_CLOSED;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 寻路
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="startNode">Start node.</param>
    /// <param name="endNode">End node.</param>
    public IList<AStarNode> FindPath(AStarNode startNode, AStarNode endNode, int _dir = 0)
    {
        findDir.Sort((a, b) => {
            int ca = a == _dir ? 0 : 1;
            int cb = b == _dir ? 0 : 1;
            if (ca != cb)
                return ca.CompareTo(cb);
            return a.CompareTo(b);
        });

        this.searchPathCheckNum += 2;
        if (this.SearchBaseBinaryHeap(startNode, endNode, searchPathCheckNum))
        {
            AStarNode currentNode = endNode;
            List<AStarNode> pathList = new List<AStarNode>() { endNode };
            while (currentNode != startNode)
            {
                currentNode = currentNode.parentNode;
                pathList.Add(currentNode);
            }
            pathList.Reverse();
            //pathList[pathList.Count - 1] = endNode;
            return pathList;
        }
        return null;
    }

    ///// <summary>
    ///// 返回节点在指定的代价内可移动的范围
    ///// </summary>
    ///// <returns>The range.</returns>
    ///// <param name="startNode">Start node.</param>
    ///// <param name="costLimit">Cost limit.</param>
    //public IList<AStarNode> WalkableRange(AStarNode startNode, int costLimit)
    //{
    //    this.walkableRangeCheckNum++;

    //    int maxStep = (int)(costLimit / STRAIGHT_COST);

    //    int startX = Mathf.Max(startNode.nodeX - maxStep, 0);
    //    int endX = Mathf.Min(startNode.nodeX + maxStep, this.numCols - 1);


    //    int startY = Mathf.Max(startNode.nodeY - maxStep, 0);
    //    int endY = Mathf.Min(startNode.nodeY + maxStep, this.numRows - 1);

    //    IList<AStarNode> rangeList = new List<AStarNode>();
    //    for (int i = startX; i <= endX; i++)
    //    {
    //        for (int j = startY; j <= endY; j++)
    //        {
    //            AStarNode nodeItem = this.nodes[this.GetNodeKey(i, j)];
    //            if (nodeItem.walkable && nodeItem.walkableRangeCheckNum != walkableRangeCheckNum)
    //            {
    //                IList<AStarNode> pathList = this.FindPath(startNode, nodeItem);
    //                if (pathList != null && pathList[pathList.Count - 1].f <= costLimit)
    //                {
    //                    foreach (AStarNode node in pathList)
    //                    {
    //                        if (node.walkableRangeCheckNum != walkableRangeCheckNum)
    //                        {
    //                            node.walkableRangeCheckNum = walkableRangeCheckNum;
    //                            rangeList.Add(node);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return rangeList;
    //}

    //public AStarUtils(int numCols, int numRows, bool isFourWay = false)
    //{
    //    this.numCols = numCols;
    //    this.numRows = numRows;
    //    this.isFourWay = isFourWay;
    //    this.iAStarHeuristic = new AStarManhattanHeuristic();
    //    //this.iAStarHeuristic = new AStarDiagonalHeuristic ();

    //    AStarNode node = null;
    //    this.nodes = new Dictionary<int, AStarNode>();
    //    for (int i = 0; i < this.numCols; i++)
    //    {
    //        for (int j = 0; j < this.numRows; j++)
    //        {
    //            node = new AStarNode(i, j);
    //            node.AddHeuristic(this.RefreshLinksOfAdjacentNodes, node);
    //            this.nodes.Add(this.GetNodeKey(i, j), node);
    //        }
    //    }
    //    this.RefreshLinksOfAllNodes();
    //    this.binaryHeapUtils = new BinaryHeapUtils(numCols * numRows / 2);
    //}

    public AStarNode Insert(int _x, int _y)
    {
        AStarNode node = new AStarNode(_x, _y);
        node.AddHeuristic(this.RefreshLinksOfAdjacentNodes, node);
        this.nodes.Add(this.GetNodeKey(_x, _y), node);
        return node;
    }

    public void CompleteInsert()
    {
        if (iAStarHeuristic == null)
        {
            iAStarHeuristic = new AStarManhattanHeuristic();
        }
        this.RefreshLinksOfAllNodes();
        if (binaryHeapUtils == null)
        {
            binaryHeapUtils = new BinaryHeapUtils(nodes.Count / 2);
        }
        else
        {
            binaryHeapUtils.Reset();
        }
    }
}