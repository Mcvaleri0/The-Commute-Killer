using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NavGateway : ScriptableObject
{
    public int Id;

    public Vector3 Center;

    public Vector3 Min { get; set; }

    public Vector3 Max { get; set; }

    public List<NavCluster> Clusters { get; set; }

    public void Initialize()
    {
        this.Clusters = new List<NavCluster>();
    }

    public void Initialize(int Id, GameObject gatewayObject)
    {
        this.Clusters = new List<NavCluster>();

        this.Id = Id;

        this.Center = gatewayObject.transform.position;

        var halfLength = gatewayObject.transform.localScale.x * 1 / 2;
        var halfWidth  = gatewayObject.transform.localScale.z * 1 / 2;
        var halfHeight = gatewayObject.transform.localScale.y * 1 / 2;

        //clusters have a size of 10 multipled by the scale
        this.Min = new Vector3(this.Center.x - halfLength, this.Center.y - halfHeight, this.Center.z - halfWidth);
        this.Max = new Vector3(this.Center.x + halfLength, this.Center.y + halfHeight, this.Center.z + halfWidth);
    }

    public Vector3 Localize()
    {
        return this.Center;
    }
}