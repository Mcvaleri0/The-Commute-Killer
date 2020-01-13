using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Utils;

public class NavGateway : ScriptableObject
{
    public int Id;

    public Vector3 Center;

    public Vector3 Min;

    public Vector3 Max;

    public List<NavCluster> Clusters;

    public List<NavEdge> Edges;


    public void Initialize(int Id, GameObject gatewayObject)
    {
        this.Clusters = new List<NavCluster>();

        this.Edges = new List<NavEdge>();

        this.Id = Id;

        this.Center = gatewayObject.transform.position;

        var halfLength = gatewayObject.transform.localScale.x * 1 / 2;
        var halfWidth  = gatewayObject.transform.localScale.z * 1 / 2;
        var halfHeight = gatewayObject.transform.localScale.y * 1 / 2;

        //clusters have a size of 10 multipled by the scale
        this.Min = new Vector3(this.Center.x - halfLength, this.Center.y - halfHeight, this.Center.z - halfWidth);
        this.Max = new Vector3(this.Center.x + halfLength, this.Center.y + halfHeight, this.Center.z + halfWidth);
    }

    
    #region Methods
    public Vector3 Localize()
    {
        return this.Center;
    }
    public void Close()
    {
        foreach (var edge in this.Edges)
        {
            edge.Disconnect();
        }
    }

    public void Open()
    {
        foreach (var edge in this.Edges)
        {
            edge.Connect();
        }
    }
    #endregion
}