using UnityEngine;

public class GatewayDistanceTableEntry : ScriptableObject
{
    public Vector3 StartGatewayPosition;

    public Vector3 EndGatewayPosition;

    public float   ShortestDistance;

    public void Init(Vector3 start, Vector3 end, float dist)
    {
        this.StartGatewayPosition = start;
        this.EndGatewayPosition   = end;
        this.ShortestDistance     = dist;
    }

    public GatewayDistanceTableEntry Clone()
    {
        var entry = CreateInstance("GatewayDistanceTableEntry") as GatewayDistanceTableEntry;

        entry.Init(this.StartGatewayPosition, this.EndGatewayPosition, this.ShortestDistance);

        return entry;
    }
}
