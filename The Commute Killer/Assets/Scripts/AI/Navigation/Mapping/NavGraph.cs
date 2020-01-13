using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NavGraph : ScriptableObject
{
    public NavNode[] Nodes;

    public void Initialize()
    {
        this.Nodes = new NavNode[0];
    }

    public bool AddNode(NavNode node)
    {
        if(this.Nodes.Length > node.Id)
        {
            if(this.Nodes[node.Id] == null)
            {
                this.Nodes[node.Id] = node;

                return true;
            }

            return false;
        }
        else
        {
            var arr = this.Nodes;

            Array.Resize<NavNode>(ref arr, node.Id + 1);

            this.Nodes = arr;

            this.Nodes[node.Id] = node;

            return true;
        }
    }

    public NavNode FindNode(int id)
    {
        if (this.Nodes.Length > id && this.Nodes[id] != null)
        {
            return this.Nodes[id];
        }

        return null;
    }

    public NavNode FindNode(NavNode node)
    {
        if (this.Nodes.Length > node.Id && this.Nodes[node.Id] != null)
        {
            return this.Nodes[node.Id];
        }

        return null;
    }

    public NavNode Localize(Vector3 pos)
    {
        NavNode temp = InstantiateNode(-1, pos);

        foreach(NavNode n in this.Nodes)
        {
            if(n == temp)
            {
                return n;
            }
        }

        return null;
    }

    public NavNode FindClosestNode(Vector3 pos)
    {
        NavNode node = null;
        var dist = Mathf.Infinity;

        foreach (NavNode n in this.Nodes)
        {
            var nDist = (pos - n.Position).magnitude;

            if (dist > nDist)
            {
                dist = nDist;
                node = n;
            }
        }

        return node;
    }

    public NavNode QuantizeToNode(Vector3 pos, float margin)
    {
        NavNode node = null;
        var dist = Mathf.Infinity;

        foreach (NavNode n in this.Nodes)
        {
            var nDist = (pos - n.Position).magnitude;

            if (dist > nDist)
            {
                dist = nDist;
                node = n;
            }

            if(dist <= margin)
            {
                return node;
            }
        }

        return null;
    }

    private static NavNode InstantiateNode(int id, Vector3 position)
    {
        var node = ScriptableObject.CreateInstance<NavNode>();

        node.Initialize(id, position);

        return node;
    }

    //public void SaveToAssetDatabase(int id = 0)
    //{
    //    string path = AssetDatabase.GetAssetPath(Selection.activeObject);

    //    if (path == "")
    //    {
    //        path = "Assets/Navigation";
    //    }
    //    else if (System.IO.Path.GetExtension(path) != "")
    //    {
    //        path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
    //    }

    //    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(NavGraph).Name.ToString() + "_" + id.ToString() + ".asset");

    //    AssetDatabase.CreateAsset(this, assetPathAndName);
    //    EditorUtility.SetDirty(this);

    //    // Save the Nodes
    //    foreach (var node in this.Nodes)
    //    {
    //        if (node == null) continue;

    //        AssetDatabase.AddObjectToAsset(node, assetPathAndName);
    //    }

    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    EditorUtility.FocusProjectWindow();
    //    Selection.activeObject = this;
    //}
}