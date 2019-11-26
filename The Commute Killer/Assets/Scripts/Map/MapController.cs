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
        if(Input.GetKeyDown(KeyCode.F12))
        {
            var start = new Vector3(19f, 1, -15);
            var end   = new Vector3(11.5f, 1, -44.5f);

            this.TestPath = GetPath(start, end);
        }
    }


    public Vector3[] GetPath(Vector3 start, Vector3 end)
    {
        var startNode = FindClosestNode(start);
        var goalNode  = FindClosestNode(end);

        var pathStack = BFSPathSearch(startNode, goalNode);

        pathStack.Push(start);

        var path = new Vector3[pathStack.Count + 1];

        pathStack.ToArray().CopyTo(path, 0);
        path[path.Length - 1] = end;

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

    private Stack<Vector3> BFSPathSearch(MapNode start, MapNode goal)
    {
        this.SearchState = true;

        Stack<Vector3> path = null;

        if(start != null && goal != null)
        {
            path = new Stack<Vector3>();
            path.Push(goal.transform.position);

            var toVisit = new Queue<MapNode>();
            toVisit.Enqueue(start);

            var goalFound = false;

            while(toVisit.Count != 0 && !goalFound) {
                var node = toVisit.Dequeue();
                
                var adjNodes = node.GetComponent<MapNode>().AdjacentNodes;

                foreach (MapNode childNode in adjNodes)
                {
                    if (childNode.State == 0) {
                        childNode.Previous = node;

                        if(childNode == goal)
                        {
                            goalFound = true;
                            break;
                        }

                        childNode.State = 1; // To be visited
                        toVisit.Enqueue(childNode);
                    }
                }

                node.GetComponent<MapNode>().State = 2;
            }

            var prev = goal.GetComponent<MapNode>().Previous;

            while(prev != null)
            {
                path.Push(prev.transform.position);

                prev = prev.Previous;
            }
        }

        return path;
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
        
        
        Gizmos.color = Color.blue;

        foreach(MapNode node in this.Nodes)
        {
            Gizmos.DrawSphere(node.transform.position, 0.1f);

            foreach(MapNode child in node.AdjacentNodes)
            {
                if(node.id < child.id)
                {
                    Gizmos.DrawLine(node.transform.position, child.transform.position);
                }
            }
        }
        
    }
}
