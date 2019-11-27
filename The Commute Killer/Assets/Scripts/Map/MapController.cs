using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public bool SearchState = false; //[ False - Not searching | True - Searching ]

    private Stack<MapNode> testPath;

    private List<MapNode> Nodes;

    private Vector3[] TestPath = null;

    // Start is called before the first frame update
    void Start()
    {
        this.Nodes = new List<MapNode>();

        var id = 0;

        foreach(Transform t in transform)
        {
            var node = t.gameObject;

            if (node.tag == "MapNode") 
            {
                this.Nodes.Add(node.GetComponent<MapNode>());

                node.GetComponent<MapNode>().id = id;

                id++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public MapNode[] GetPath(Vector3 start, Vector3 end)
    {
        var startNode = FindClosestNode(start);
        var goalNode  = FindClosestNode(end);

        var path = BFSPathSearch(startNode, goalNode);

        return path;
    }

    private MapNode FindClosestNode(Vector3 pos)
    {
        var closestNode = this.Nodes[0];
        var minDistance = Vector3.Distance(pos, closestNode.transform.position);

        for(var i = 1; i < this.Nodes.Count; i++)
        {
            var node = this.Nodes[i];

            var dist = Vector3.Distance(pos, node.transform.position);

            if(dist < minDistance)
            {
                minDistance = dist;

                closestNode = node;
            }
        }

        return closestNode;
    }

    private MapNode[] BFSPathSearch(MapNode start, MapNode goal)
    {
        this.SearchState = true;

        Stack<MapNode> path = null;

        if(start != null && goal != null)
        {
            path = new Stack<MapNode>();
            path.Push(goal);

            var toVisit = new Queue<MapNode>();
            toVisit.Enqueue(start);

            var goalFound = false;

            while(toVisit.Count != 0 && !goalFound) {
                var node = toVisit.Dequeue();
                
                var adjNodes = node.AdjacentNodes;

                var ind = 0;

                foreach (MapNode childNode in adjNodes)
                {
                    if (childNode.State == 0 && !node.BlockedArc[ind]) {
                        childNode.Previous = node;

                        if(childNode == goal)
                        {
                            goalFound = true;
                            break;
                        }

                        childNode.State = 1; // To be visited
                        toVisit.Enqueue(childNode);
                    }

                    ind++;
                }

                node.GetComponent<MapNode>().State = 2;
            }

            var prev = goal.GetComponent<MapNode>().Previous;

            while(prev != null)
            {
                path.Push(prev);

                prev = prev.Previous;
            }
        }

        return path.ToArray();
    }

    public bool BlockArc(int id_1, int id_2, bool value)
    {
        var start = this.Nodes[id_1];
        var end = this.Nodes[id_2];

        return start.BlockArc(end, value) && end.BlockArc(start, value);
    }

    public bool PathBlocked(int id_1, int id_2)
    {
        var startNode = this.Nodes[id_1];
        var endNode   = this.Nodes[id_2];

        return startNode.ArcBlocked(endNode);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (this.TestPath != null)
        {
            for(var i = 0; i < this.TestPath.Length - 1; i++)
            {
                Gizmos.DrawLine(this.TestPath[i], this.TestPath[i+1]);
            }
        }        
    }
}
