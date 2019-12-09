using UnityEngine;
using UnityEditor;

public class Zone : MonoBehaviour
{
    public Vector3 StartPoint { get; set; }

    public Vector3 EndPoint { get; set; }

    public enum Awareness
    {
        None,
        Passive,
        Active,
        Surveiled
    }

    public Awareness AwarenessLevel;
    
    public bool In(Vector3 p)
    {
        if(this.StartPoint.x < p.x && p.x < this.EndPoint.x)
        {
            if (this.StartPoint.z < p.z && p.z < this.EndPoint.z)
            {
                return true;
            }
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        var size = this.EndPoint - this.StartPoint;
        //size.x = Mathf.Abs(size.x);
        size.y = 1;
        //size.z = Mathf.Abs(size.z);

        var center = this.StartPoint + size / 2;

        switch(this.AwarenessLevel)
        {
            case Awareness.None:
                Gizmos.color = Color.gray;
                break;

            case Awareness.Passive:
                Gizmos.color = Color.green;
                break;

            case Awareness.Active:
                Gizmos.color = Color.yellow;
                break;

            case Awareness.Surveiled:
                Gizmos.color = Color.red;
                break;
        }

        Gizmos.DrawCube(center, size);
    }
}