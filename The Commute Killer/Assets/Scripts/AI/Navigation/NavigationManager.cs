using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public NavGraph Graph;

    public NavClusterGraph ClusterGraph;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if(this.Graph != null)
        {
            foreach(NavNode n in this.Graph.Nodes)
            {
                Gizmos.DrawSphere(n.Position, 0.1f);

                foreach(NavNode a in n.Adjacents)
                {
                    if(a.Id > n.Id)
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
    }
}
