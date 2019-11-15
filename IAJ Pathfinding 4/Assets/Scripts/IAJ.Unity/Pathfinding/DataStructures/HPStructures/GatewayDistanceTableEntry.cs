using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Pathfinding.DataStructures.HPStructures
{
    public class GatewayDistanceTableEntry : ScriptableObject
    {
        public Vector3 startGatewayPosition;
        public Vector3 endGatewayPosition;
        public float   shortestDistance;

        public void Init(Vector3 start, Vector3 end, float dist)
        {
            this.startGatewayPosition = start;
            this.endGatewayPosition   = end;
            this.shortestDistance     = dist;
        }
    }
}
