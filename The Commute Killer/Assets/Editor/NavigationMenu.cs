//using UnityEngine;
//using UnityEditor;
//using UnityEngine.AI;
//using UnityEngine.SceneManagement;
//using Assets.Scripts.IAJ.Unity.Utils;
//using System.Collections.Generic;
//using UnityEngine.ProBuilder;

//public class NavigationMenu : ScriptableObject
//{
//    struct Triangle
//    {
//        public NavNode[] Nodes;

//        public Triangle[] Subdivided;

//        public Triangle(NavNode node0, NavNode node1, NavNode node2)
//        {
//            this.Nodes = new NavNode[3];

//            this.Nodes[0] = node0;
//            this.Nodes[1] = node1;
//            this.Nodes[2] = node2;

//            this.Subdivided = new Triangle[4];
//        }

//        public Triangle(NavNode[] nodes)
//        {
//            this.Nodes = nodes;

//            this.Subdivided = new Triangle[4];
//        }

//        public int Subdivide(NavGraph graph, int nodecounter)
//        {
//            var newNodes = new NavNode[3];

//            for(var i = 0; i < 3; i++)
//            {
//                var ind = (i + 1) % 3;

//                var pos = Vector3.Lerp(this.Nodes[i].Position, this.Nodes[ind].Position, 0.5f);

//                newNodes[i] = graph.QuantizeToNode(pos, 0.3f);

//                if(newNodes[i] == null)
//                {
//                    newNodes[i] = InstantiateNode(nodecounter, pos);
//                    graph.AddNode(newNodes[i]);
//                    nodecounter++;
//                }
//            }

//            this.Subdivided[0] = new Triangle(this.Nodes[0], newNodes[0], newNodes[2]);
//            this.Subdivided[0].ConnectVertices();

//            this.Subdivided[1] = new Triangle(this.Nodes[1], newNodes[1], newNodes[0]);
//            this.Subdivided[1].ConnectVertices();

//            this.Subdivided[2] = new Triangle(this.Nodes[2], newNodes[2], newNodes[1]);
//            this.Subdivided[2].ConnectVertices();

//            this.Subdivided[3] = new Triangle(  newNodes[0], newNodes[1], newNodes[2]);
//            this.Subdivided[3].ConnectVertices();

//            return nodecounter;
//        }

//        public void ConnectVertices()
//        {
//            for (var k = 0; k < 3; k++)
//            {
//                var node = this.Nodes[k];

//                var o1 = this.Nodes[(k + 1) % 3];
//                var o2 = this.Nodes[(k + 2) % 3];

//                node.AddAdjacent(o1);
//                node.AddAdjacent(o2);
//            }
//        }
//    }

//    struct Face
//    {
//        public NavNode[] Nodes;

//        public Face(NavNode[] nodes)
//        {
//            this.Nodes = nodes;
//        }

//        public void ConnectVertices()
//        {
//            var prev = this.Nodes[0];

//            for (var k = 1; k < this.Nodes.Length; k++)
//            {
//                var node = this.Nodes[k];

//                prev.AddAdjacent(node);
//                node.AddAdjacent(prev);
//            }
//        }
//    }

//    [MenuItem("Tools/AI/Navigation/Generate Graph")]
//    private static void GenerateGraph()
//    {
//        // Get NavMesh Triangles
//        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

//        var indexes   = triangulation.indices;   // Indices of each triangle's vertices
//        var positions = triangulation.vertices;  // Vertex's positions

//        NavGraph graph = CreateInstance<NavGraph>(); // NabGraph
//        graph.Initialize();

//        var nodeCounter = 0; // To attribute each Node its Id
//        Dictionary<int, NavNode> indToNode = new Dictionary<int, NavNode>(); // Map NavNodes to original position array
        
//        // Create NavNodes and populate NavGraph
//        for(var i = 0; i < positions.Length; i++)
//        {
//            Vector3 pos = positions[i];

//            var node = graph.QuantizeToNode(pos, 0.3f);
            
//            if(node == null)
//            {
//                node = InstantiateNode(nodeCounter++, pos);

//                graph.AddNode(node);
//            }

//            indToNode.Add(i, node);
//        }

//        var triangleVertices = new NavNode[3]; // Temporary array to hold each triangle's NavNodes

//        // Foreach Triangle in the NavMesh
//        for (var i = 0; i < indexes.Length; i += 3)
//        {
//            // Foreach Vertex of the Triangle
//            for(var j = 0; j < 3; j++)
//            {
//                var node = indToNode[indexes[i + j]];  // Get NavNode

//                triangleVertices[j] = node;
//            }

//            var triangle = new Triangle(triangleVertices);
//            triangle.ConnectVertices();
//            //nodeCounter = triangle.Subdivide(graph, nodeCounter); // Subdivide Triangle and update Node Count
//        }

//        int scene = SceneManager.GetActiveScene().buildIndex;

//        graph.SaveToAssetDatabase(scene);
//    }

//    [MenuItem("Tools/AI/Navigation/Generate Graph from Custom")]
//    private static void GenerateGraphCustom()
//    {
//        // Get NavMesh Triangles
//        Mesh mesh = GameObject.Find("CustomNavMesh").GetComponent<MeshFilter>().sharedMesh;

//        var indexes   = mesh.GetIndices(0); // Indices of each triangle's vertices
//        var positions = mesh.vertices;      // Vertex's positions

//        NavGraph graph = CreateInstance<NavGraph>(); // NabGraph
//        graph.Initialize();

//        var nodeCounter = 0; // To attribute each Node its Id
//        Dictionary<int, NavNode> indToNode = new Dictionary<int, NavNode>(); // Map NavNodes to original position array

//        // Create NavNodes and populate NavGraph
//        for (var i = 0; i < positions.Length; i++)
//        {
//            Vector3 pos = positions[i];

//            var node = graph.QuantizeToNode(pos, 0.3f);

//            if (node == null)
//            {
//                node = InstantiateNode(nodeCounter++, pos);

//                graph.AddNode(node);
//            }

//            indToNode.Add(i, node);
//        }

//        var triangleVertices = new NavNode[3]; // Temporary array to hold each triangle's NavNodes

//        // Foreach Triangle in the NavMesh
//        for (var i = 0; i < indexes.Length; i += 3)
//        {
//            // Foreach Vertex of the Triangle
//            for (var j = 0; j < 3; j++)
//            {
//                var node = indToNode[indexes[i + j]];  // Get NavNode

//                triangleVertices[j] = node;
//            }

//            var triangle = new Triangle(triangleVertices);
//            triangle.ConnectVertices();
//            //nodeCounter = triangle.Subdivide(graph, nodeCounter); // Subdivide Triangle and update Node Count
//        }

//        int scene = SceneManager.GetActiveScene().buildIndex;

//        graph.SaveToAssetDatabase(scene);
//    }

//    private static NavNode InstantiateNode(int id, Vector3 position)
//    {
//        var node = CreateInstance<NavNode>();

//        node.Initialize(id, position);

//        return node;
//    }

//    [MenuItem("Tools/AI/Navigation/Generate Cluster Graph")]
//    private static void GenerateClusterGraph()
//    {
//        NavCluster cluster;
//        NavGateway gateway;

//        // Get NavZone gameobjects
//        var zones = GameObject.FindGameObjectsWithTag("Zone");

//        // Get NavGateway gameobjects
//        var gateways = GameObject.FindGameObjectsWithTag("Gateway");

//        int scene = SceneManager.GetActiveScene().buildIndex;

//        // Get the NavGraph
//        NavGraph navGraph = AssetDatabase.LoadAssetAtPath("Assets/Navigation/NavGraph_" + scene.ToString() + ".asset", typeof(NavGraph)) as NavGraph;

//        NavClusterGraph clusterGraph = CreateInstance<NavClusterGraph>();
//        clusterGraph.Initialize();

//        // Create NavGateway instances for each Gateway GameObject
//        for (int i = 0; i < gateways.Length; i++)
//        {
//            var gatewayGO = gateways[i];

//            gateway = CreateInstance<NavGateway>();
//            gateway.Initialize(i, gatewayGO);

//            clusterGraph.Gateways.Add(gateway);

//            LinkNodes2Gate(gateway, navGraph);
//        }

//        // Create NavCluster instances for each NavZone GameObject and check for connections through gateways
//        var ind = 0;

//        foreach (var zone in zones)
//        {
//            cluster = CreateInstance<NavCluster>();
//            cluster.Initialize(ind++, zone.GetComponent<NavZone>());
//            clusterGraph.Clusters.Add(cluster);

//            // Determine intersection between cluster and gateways and add connections when they intersect
//            foreach (var gate in clusterGraph.Gateways)
//            {
//                if (BoxIntersection(cluster.Min, cluster.Max, gate.Min, gate.Max))
//                {
//                    cluster.Gateways.Add(gate);
//                    gate.Clusters.Add(cluster);
//                }
//            }
//        }


//        foreach (var node in navGraph.Nodes)
//        {
//            var assigned = false;

//            foreach (var c in clusterGraph.Clusters)
//            {
//                if (c.Inside(node.Position))
//                {
//                    node.Cluster = c;

//                    assigned = true;

//                    break;
//                }
//            }

//            if(assigned)
//            {
//                continue;
//            }
//            else
//            {
//                Debug.Log("No Cluster found for a Node");
//            }
//        }

//        // Second stage of the algorithm, calculation of the Gateway table
//        GlobalPath solution;

//        float cost;

//        NavGateway startGate;
//        NavGateway endGate;

//        var pathfindingManager = GameObject.Find("NavigationHelper").GetComponent<PathfindingManager>();
//        pathfindingManager.Start();

//        GatewayDistanceTableRow[] gateDistTable = new GatewayDistanceTableRow[gateways.Length];

//        for (var i = 0; i < gateways.Length; i++)
//        {
//            var row = CreateInstance<GatewayDistanceTableRow>();
//            row.entries = new GatewayDistanceTableEntry[gateways.Length];

//            gateDistTable[i] = row;
//        }

//        // For each gateway
//        for (var i = 0; i < gateways.Length; i++)
//        {
//            startGate = clusterGraph.Gateways[i]; // Get start gateway

//            // For each gateway
//            for (var j = i; j < gateways.Length; j++)
//            {
//                endGate = clusterGraph.Gateways[j]; // Get end gateway
//                cost = Mathf.Infinity;

//                // If it's going somewhere else
//                if (i != j)
//                {
//                    // Init the pathfinding
//                    pathfindingManager.PathFinding.InitializeSearch(startGate.Center, endGate.Center);

//                    if (pathfindingManager.PathFinding.InProgress)
//                    {
//                        var finished = pathfindingManager.PathFinding.Search(out solution);

//                        while (!finished)
//                        {
//                            finished = pathfindingManager.PathFinding.Search(out solution);
//                        }

//                        if (solution != null)
//                        {
//                            cost = solution.Length;
//                        }
//                    }

//                    var entry = CreateInstance<GatewayDistanceTableEntry>();
//                    entry.Init(startGate.Center, endGate.Center, cost);

//                    gateDistTable[i].entries[j] = entry;

//                    entry = CreateInstance<GatewayDistanceTableEntry>();
//                    entry.Init(endGate.Center, startGate.Center, cost);

//                    gateDistTable[j].entries[i] = entry;
//                }

//                // If it's going to itself
//                else
//                {
//                    cost = 0;

//                    var entry = CreateInstance<GatewayDistanceTableEntry>();
//                    entry.Init(startGate.Center, endGate.Center, cost);

//                    gateDistTable[i].entries[j] = entry;
//                }
//            }
//        }

//        EditorUtility.SetDirty(navGraph);

//        clusterGraph.GatewayDistanceTable = gateDistTable;

//        //create a new asset that will contain the ClusterGraph and save it to disk (DO NOT REMOVE THIS LINE)
//        clusterGraph.SaveToAssetDatabase(scene);
//    }

//    private static bool BoxIntersection(Vector3 b1Min, Vector3 b1Max, Vector3 b2Min, Vector3 b2Max)
//    {
//        if (!Overlap1D(b1Min.x, b1Max.x, b2Min.x, b2Max.x)) return false;
//        if (!Overlap1D(b1Min.z, b1Max.z, b2Min.z, b2Max.z)) return false;
//        if (!Overlap1D(b1Min.y, b1Max.y, b2Min.y, b2Max.y)) return false;

//        return true;
//    }

//    private static bool Overlap1D(float min1, float max1, float min2, float max2)
//    {
//        return max1 >= min2 && max2 >= min1;
//    }

//    private static void LinkNodes2Gate(NavGateway gate, NavGraph graph)
//    {
//        foreach(var node in graph.Nodes)
//        {
//            foreach(var adjacent in node.Adjacents)
//            {
//                var gateCoords = CrossesGateway(node.Position, adjacent.Position);

//                if (gateCoords == gate.Center)
//                {
//                    if (gate.Edges == null) gate.Edges = new List<NavEdge>();

//                    var edge = CreateInstance<NavEdge>();
//                    edge.Initialize(node, adjacent);

//                    if(!gate.Edges.Contains(edge))
//                    {
//                        gate.Edges.Add(edge);
//                    }
//                }
//            }
//        }
//    }

//    private static Vector3 CrossesGateway(Vector3 p1, Vector3 p2)
//    {
//        Vector3 direction = p2 - p1;

//        Physics.Raycast(p1, direction, out RaycastHit hit, direction.magnitude);

//        if(hit.transform != null && hit.transform.CompareTag("Gateway"))
//        {
//            return hit.transform.position;
//        }

//        return Vector3.positiveInfinity;
//    }
//}