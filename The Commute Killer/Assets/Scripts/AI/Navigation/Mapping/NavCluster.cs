using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NavCluster : ScriptableObject
{
    public Vector3 Center { get; private set; }

    public Vector3 Min { get; private set; }

    public Vector3 Max { get; private set; }

    public List<NavGateway> Gateways { get; private set; }

    public void Initialize()
    {
        this.Gateways = new List<NavGateway>();
    }

    [System.Obsolete]
    public void Initialize(GameObject clusterObject)
    {
        this.Gateways = new List<NavGateway>();

        this.Center = clusterObject.transform.position;

        var halfLength = clusterObject.transform.localScale.x * 10 / 2;
        var halfWidth  = clusterObject.transform.localScale.z * 10 / 2;
        var halfHeight = clusterObject.transform.localScale.y * 10 / 2;

        //clusters have a size of 10 multipled by the scale
        this.Min = new Vector3(this.Center.x - halfLength, this.Center.y - halfHeight, this.Center.z - halfWidth);
        this.Max = new Vector3(this.Center.x + halfLength, this.Center.y + halfHeight, this.Center.z + halfWidth);
    }


    public void Initialize(NavZone zone)
    {
        this.Gateways = new List<NavGateway>();

        this.Center = zone.GetCenter();

        //clusters have a size of 10 multipled by the scale
        this.Min = Vector3.Min(zone.StartPoint, zone.EndPoint);
        this.Max = Vector3.Max(zone.StartPoint, zone.EndPoint);
    }


    public Vector3 Localize()
    {
        return this.Center;
    }


    public Vector3 GetSize()
    {
        return this.Max - this.Min;
    }

    public override bool Equals(object other)
    {
        var oCluster = other as NavCluster;

        if(oCluster != null)
        {
            return oCluster.Center == this.Center;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}