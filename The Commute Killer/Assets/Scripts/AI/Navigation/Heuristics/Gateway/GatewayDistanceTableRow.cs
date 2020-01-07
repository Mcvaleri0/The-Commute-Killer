using UnityEngine;

public class GatewayDistanceTableRow : ScriptableObject
{
    public GatewayDistanceTableEntry[] entries;

    public GatewayDistanceTableRow Clone()
    {
        var clone = CreateInstance("GatewayDistanceTableRow") as GatewayDistanceTableRow;

        clone.entries = new GatewayDistanceTableEntry[entries.Length];

        for(var i = 0; i < entries.Length; i++)
        {
            clone.entries[i] = this.entries[i].Clone();
        }

        return clone;
    }
}
