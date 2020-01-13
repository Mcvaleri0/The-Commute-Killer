using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Utils;

public class NavClusterGraph : ScriptableObject
{
    public List<NavCluster> Clusters;

    public List<NavGateway> Gateways;

    public GatewayDistanceTableRow[] GatewayDistanceTable;

    public Dictionary<int, int> Node2Cluster;


    public void Initialize()
    {
        this.Clusters = new List<NavCluster>();
        this.Gateways = new List<NavGateway>();

        this.Node2Cluster = new Dictionary<int, int>();
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

    //    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(NavClusterGraph).Name.ToString() + "_" + id.ToString() + ".asset");

    //    AssetDatabase.CreateAsset(this, assetPathAndName);
    //    EditorUtility.SetDirty(this);

    //    //save the Clusters
    //    foreach (var cluster in this.Clusters)
    //    {
    //        AssetDatabase.AddObjectToAsset(cluster, assetPathAndName);
    //    }

    //    //save the Gateways
    //    foreach (var gateway in this.Gateways)
    //    {
    //        AssetDatabase.AddObjectToAsset(gateway, assetPathAndName);

    //        foreach (var edge in gateway.Edges)
    //        {
    //            AssetDatabase.AddObjectToAsset(edge, assetPathAndName);
    //        }
    //    }

    //    //save the gatewayTableRows and tableEntries
    //    foreach (var tableRow in this.GatewayDistanceTable)
    //    {
    //        AssetDatabase.AddObjectToAsset(tableRow, assetPathAndName);

    //        foreach (var tableEntry in tableRow.entries)
    //        {
    //            AssetDatabase.AddObjectToAsset(tableEntry, assetPathAndName);
    //        }
    //    }

    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    EditorUtility.FocusProjectWindow();
    //    Selection.activeObject = this;
    //}
}