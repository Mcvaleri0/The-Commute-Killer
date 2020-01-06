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
}
