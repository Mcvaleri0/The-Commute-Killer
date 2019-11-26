using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    public int id = 0;

    public int State = 0; //[ 0 - Not visited | 1 - To visit | 2 - Visited ]

    public MapNode Previous;

    public List<MapNode> AdjacentNodes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(State != 0)
        {
            if(this.GetComponentInParent<MapController>().SearchState)
            {
                State = 0;

                Previous = null;
            }
        }
    }
}
