using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Assets.Scripts.IAJ.Unity.Utils;
using System.Collections.Generic;

public class NavigationMenu : ScriptableObject
{
    [MenuItem("Tools/AI/Navigation/Generate Graph")]
    private static void GenerateGraph()
    {
        // Get NavMesh Triangles
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        var indexes   = triangulation.indices;   // Indices of each triangle's vertices
        var positions = triangulation.vertices;  // Vertex's positions

        NavGraph graph = CreateInstance<NavGraph>(); // NabGraph
        graph.Initialize();

        var nodeCounter = 0; // To attribute each Node its Id
        Dictionary<int, NavNode> indToNode = new Dictionary<int, NavNode>(); // Map NavNodes to original position array
        
        // Create NavNodes and populate NavGraph
        for(var i = 0; i < positions.Length; i++)
        {
            Vector3 pos = positions[i];

            var node = graph.QuantizeToNode(pos, 0.3f);

            if(node == null)
            {
                node = InstantiateNode(nodeCounter, pos);

                nodeCounter++;

                graph.AddNode(node);
            }

            indToNode.Add(i, node);
        }

        var triangle = new NavNode[3]; // Temporary array to hold each triangle's NavNodes

        // Foreach Triangle in the NavMesh
        for (var i = 0; i < indexes.Length; i += 3)
        {
            // Foreach Vertex of the Triangle
            for(var j = i; j < i + 3; j++)
            {
                var tInd = j - i; // Index in the triangle

                var node = indToNode[indexes[j]];  // Get NavNode

                triangle[tInd] = node;
            }

            MakeTriangleEdges(triangle); // Add Edges between the NavNodes
        }

        int scene = SceneManager.GetActiveScene().buildIndex;

        graph.SaveToAssetDatabase(scene);
    }

    private static NavNode InstantiateNode(int id, Vector3 position)
    {
        var node = CreateInstance<NavNode>();

        node.Initialize(id, position);

        return node;
    }

    private static void MakeTriangleEdges(NavNode[] nodes)
    {
        for (var k = 0; k < 3; k++)
        {
            var node = nodes[k];

            var o1 = nodes[(k + 1) % 3];
            var o2 = nodes[(k + 2) % 3];

            node.AddAdjacent(o1);
            node.AddAdjacent(o2);
        }
    }

    [MenuItem("Tools/AI/Navigation/Generate Cluster Graph")]
    private static void GenerateClusterGraph()
    {
        NavCluster cluster;
        NavGateway gateway;

        // Get NavCluster gameobjects
        var clusters = GameObject.FindGameObjectsWithTag("Cluster");

        // Get NavGateway gameobjects
        var gateways = GameObject.FindGameObjectsWithTag("Gateway");

        int scene = SceneManager.GetActiveScene().buildIndex;

        // Get the NavGraph
        NavGraph navGraph = AssetDatabase.LoadAssetAtPath("Assets/Navigation/NavGraph_" + scene.ToString(), typeof(NavGraph)) as NavGraph;

        NavClusterGraph clusterGraph = ScriptableObject.CreateInstance<NavClusterGraph>();

        //create gateway instances for each gateway game object
        for (int i = 0; i < gateways.Length; i++)
        {
            var gatewayGO = gateways[i];
            gateway = ScriptableObject.CreateInstance<NavGateway>();
            gateway.Initialize(i, gatewayGO);
            clusterGraph.Gateways.Add(gateway);
        }

        //create cluster instances for each cluster game object and check for connections through gateways
        foreach (var clusterGO in clusters)
        {

            cluster = ScriptableObject.CreateInstance<NavCluster>();
            cluster.Initialize(clusterGO);
            clusterGraph.Clusters.Add(cluster);

            //determine intersection between cluster and gateways and add connections when they intersect
            foreach (var gate in clusterGraph.Gateways)
            {
                if (MathHelper.BoundingBoxIntersection(cluster.Min, cluster.Max, gate.Min, gate.Max))
                {
                    cluster.Gateways.Add(gate);
                    gate.Clusters.Add(cluster);
                }
            }
        }

        // Second stage of the algorithm, calculation of the Gateway table

        GlobalPath solution = null;

        float cost;
        NavGateway startGate;
        NavGateway endGate;

        var pathfindingManager = new PathfindingManager();

        GatewayDistanceTableRow[] gateDistTable = new GatewayDistanceTableRow[gateways.Length];

        for (var i = 0; i < gateways.Length; i++)
        {
            var row = CreateInstance<GatewayDistanceTableRow>();
            row.entries = new GatewayDistanceTableEntry[gateways.Length];

            gateDistTable[i] = row;
        }

        // For each gateway
        for (var i = 0; i < gateways.Length; i++)
        {
            startGate = clusterGraph.Gateways[i]; // Get start gateway

            // For each gateway
            for (var j = i; j < gateways.Length; j++)
            {
                endGate = clusterGraph.Gateways[j]; // Get end gateway
                cost = Mathf.Infinity;

                // If it's going somewhere else
                if (i != j)
                {
                    // Init the pathfinding
                    pathfindingManager.PathFinding.InitializeSearch(startGate.Center, endGate.Center);

                    if (pathfindingManager.PathFinding.InProgress)
                    {
                        var finished = pathfindingManager.PathFinding.Search(out solution);

                        while (!finished)
                        {
                            finished = pathfindingManager.PathFinding.Search(out solution);
                        }

                        if (solution != null)
                        {
                            cost = solution.Length;
                        }
                    }

                    var entry = CreateInstance<GatewayDistanceTableEntry>();
                    entry.Init(startGate.Center, endGate.Center, cost);

                    gateDistTable[i].entries[j] = entry;

                    entry = CreateInstance<GatewayDistanceTableEntry>();
                    entry.Init(endGate.Center, startGate.Center, cost);

                    gateDistTable[j].entries[i] = entry;
                }

                // If it's going to itself
                else
                {
                    cost = 0;

                    var entry = CreateInstance<GatewayDistanceTableEntry>();
                    entry.Init(startGate.Center, endGate.Center, cost);

                    gateDistTable[i].entries[j] = entry;
                }
            }
        }

        clusterGraph.GatewayDistanceTable = gateDistTable;

        //create a new asset that will contain the ClusterGraph and save it to disk (DO NOT REMOVE THIS LINE)
        clusterGraph.SaveToAssetDatabase();
    }
}