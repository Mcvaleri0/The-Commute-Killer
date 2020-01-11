using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NavigationManager : MonoBehaviour
{
    public NavGraph Graph;

    public NavClusterGraph ClusterGraph;

    public Dictionary<int, bool> GatewayOpen { get; set; }

    private GatewayDistanceTableRow[] OGatewayDistanceTable;

    public GatewayDistanceTableRow[] GatewayDistanceTable;


    // Use this for initialization
    void Start()
    {
        this.GatewayOpen = new Dictionary<int, bool>();

        foreach(NavGateway gate in ClusterGraph.Gateways)
        {
            this.GatewayOpen[gate.Id] = true;
        }

        this.OGatewayDistanceTable = this.ClusterGraph.GatewayDistanceTable;

        this.GatewayDistanceTable = new GatewayDistanceTableRow[this.OGatewayDistanceTable.Length];

        for(var i = 0; i < this.OGatewayDistanceTable.Length; i++)
        {
            this.GatewayDistanceTable[i] = this.OGatewayDistanceTable[i].Clone();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public bool PathBlocked(NavNode startNode, NavNode endNode)
    {
        if (startNode.Cluster == endNode.Cluster)
        {
            return false;
        }

        var gate = FindConnectingGateway(startNode, endNode);

        return !this.GatewayOpen[gate.Id];
    }


    private NavGateway FindConnectingGateway(NavNode startNode, NavNode endNode)
    {
        NavGateway gate = null;

        var centerOfEdge = Vector3.Lerp(startNode.Position, endNode.Position, 0.5f);

        var startCluster = startNode.Cluster;
        var endCluster   = endNode.Cluster;

        var possible = new List<NavGateway>();

        foreach(NavGateway gateway in startCluster.Gateways)
        {
            if(gateway.Clusters.Contains(endCluster))
            {
                possible.Add(gateway);
            }
        }

        if(possible.Count == 1)
        {
            gate = possible[0];
        }
        else if(possible.Count > 1)
        {
            var dist = Mathf.Infinity;

            foreach(var p in possible)
            {
                var ndist = Vector3.Distance(p.Center, centerOfEdge);

                if(ndist < dist)
                {
                    dist = ndist;

                    gate = p;
                }
            }
        }

        return gate;
    }


    public void CloseGateway(int id)
    {
        this.GatewayOpen[id] = false;

        var or_row = this.GatewayDistanceTable[id];

        foreach (GatewayDistanceTableEntry entry in or_row.entries)
        {
            entry.ShortestDistance = Mathf.Infinity;
        }

        foreach (GatewayDistanceTableRow row in this.GatewayDistanceTable)
        {
            row.entries[id].ShortestDistance = Mathf.Infinity;
        }
    }


    public void OpenGateway(int id)
    {
        this.GatewayOpen[id] = true;

        var or_row = this.OGatewayDistanceTable[id];
        var cl_row = this.GatewayDistanceTable[id];

        for(var i = 0; i < or_row.entries.Length; i++)
        {
            cl_row.entries[i].ShortestDistance = or_row.entries[i].ShortestDistance;
        }

        for(var i = 0; i < this.GatewayDistanceTable.Length; i++)
        {
            or_row = this.OGatewayDistanceTable[i];
            cl_row = this.GatewayDistanceTable[i];

            for(var j = 0; j < or_row.entries.Length; i++)
            {
                or_row.entries[j].ShortestDistance = cl_row.entries[j].ShortestDistance;
            }
        }
    }


    private void OnDrawGizmos()
    {
        /*
        if (this.Graph != null)
        {
            foreach (NavNode n in this.Graph.Nodes)
            {
                Gizmos.DrawSphere(n.Position, 0.1f);

                foreach (NavNode a in n.Adjacents)
                {
                    if (a.Id > n.Id)
                    {
                        Gizmos.color = Color.white;

                        //if(a.Adjacents.Contains(n))
                        //{
                        //    Gizmos.color = Color.black;
                        //}

                        Gizmos.DrawLine(n.Position, a.Position);
                    }
                }
            }
        }
        */

        if(this.ClusterGraph != null)
        {
            foreach(var cluster in this.ClusterGraph.Clusters)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(cluster.Center, 0.3f);

                Gizmos.color = Color.blue;
                foreach(var gate in cluster.Gateways)
                {
                    Gizmos.DrawLine(cluster.Center, gate.Center);
                }
            }

            Gizmos.color = Color.green;

            foreach(var gateway in this.ClusterGraph.Gateways)
            {
                if (this.GatewayOpen != null && !this.GatewayOpen[gateway.Id]) Gizmos.color = Color.yellow;

                Gizmos.DrawSphere(gateway.Center, 0.1f);
            }
        }
    }
}
