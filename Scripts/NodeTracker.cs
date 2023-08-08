using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "ScriptableObjects/NodeTracker", order = 1)]
public class NodeTracker : ScriptableObject
{
    public int nodeCount;
    public HashSet<DirectionalTreeNode> nodes;
}

// HashSet<Node> nodeSet = new HashSet<Node>();