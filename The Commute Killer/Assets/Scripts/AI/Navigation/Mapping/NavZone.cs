using UnityEngine;

public class NavZone : MonoBehaviour
{
    public Vector3 StartPoint;

    public Vector3 EndPoint;

    public enum Awareness
    {
        None,
        Passive,
        Surveiled,
        Debug
    }

    public Awareness AwarenessLevel;


    public float In(Vector3 p)
    {
        if(Between(this.StartPoint.x, p.x, this.EndPoint.x))
        {
            if(Between(this.StartPoint.z, p.z, this.EndPoint.z))
            {
                return Mathf.Abs(p.y - this.StartPoint.y);
            }
        }

        return Mathf.Infinity;
    }


    private bool Between(float bound1, float val, float bound2) 
    {
        if(bound1 < bound2)
        {
            if(bound1 <= val && val <= bound2)
            {
                return true;
            }
        }
        else
        {
            if (bound2 <= val && val <= bound1)
            {
                return true;
            }
        }

        return false;
    }


    public Vector3 GetCenter()
    {    
        return Vector3.Lerp(this.StartPoint, this.EndPoint, 0.5f);
    }


    public void OnDrawGizmos()
    {
        var size = this.EndPoint - this.StartPoint;

        var center = this.StartPoint + size / 2;

        switch(this.AwarenessLevel)
        {
            case Awareness.None:
                Gizmos.color = Color.gray;
                break;

            case Awareness.Passive:
                Gizmos.color = Color.green;
                break;

            case Awareness.Surveiled:
                Gizmos.color = Color.red;
                break;

            case Awareness.Debug:
                Gizmos.color = Color.blue;
                break;
        }

        Gizmos.DrawWireCube(center, size);
    }
}