using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public int id = 0;

    public int State = 0; //[ 0 - Not visited | 1 - To visit | 2 - Visited ]

    public MapNode Previous;

    private int AdjacentCount;

    public List<MapNode> AdjacentNodes; //DO NOT CHANGE ANYTHING ABOUT THIS VARIABLE

    public bool[] BlockedArc;

    // Start is called before the first frame update
    void Start()
    {
        this.AdjacentCount = this.AdjacentNodes.Count;

        this.BlockedArc = new bool[AdjacentCount];

        for (var i = 0; i < this.AdjacentCount; i++)
        {
            this.BlockedArc[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (State != 0)
        {
            if(this.GetComponentInParent<MapController>().SearchState)
            {
                State = 0;

                Previous = null;
            }
        }
    }

    // Sets an Arc between nodes to blocked or unblocked
    public bool BlockArc(MapNode target, bool value)
    {
        var ind = this.AdjacentNodes.FindIndex(x => x == target);

        if(ind != -1)
        {
            this.BlockedArc[ind] = value;

            return true;
        }

        return false;
    }

    // Returns wether or not an arc between nodes is blocked
    public bool ArcBlocked(MapNode target)
    {
        var ind = this.AdjacentNodes.FindIndex(x => x == target);

        if (ind != -1)
        {
            return this.BlockedArc[ind];
        }

        return true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(this.transform.position, 0.1f);

        if (this.BlockedArc != null)
        {
            for(var i = 0; i < this.AdjacentCount; i++)
            {
                var child = this.AdjacentNodes[i];
                
                if (this.id < child.id && !this.BlockedArc[i])
                {
                    Gizmos.DrawLine(this.transform.position, child.transform.position);
                }
            }
        }
    }
}
