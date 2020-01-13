using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    #region /* Collisions */

    private int PossibleCollisions { get; set; }

    #endregion

    #region /* Auxiliary */

    private bool Passed { get; set; }

    private bool RightLane { get; set; }

    private EventManager Manager {get; set; }

    #endregion

    public static CollisionDetector CreateCollisionDetector(Transform Parent, Vector3 Position, Vector3 Size, bool RightLane)
    {
        GameObject DetectorObj = new GameObject("Collision Detector");

        DetectorObj.transform.parent = Parent.transform;
        DetectorObj.transform.localPosition = Position;
        DetectorObj.transform.localScale    = Vector3.one;
        DetectorObj.transform.localRotation = Quaternion.identity;
        
        BoxCollider Collider = DetectorObj.AddComponent<BoxCollider>();
        Collider.size = Size;
        Collider.isTrigger = true;

        CollisionDetector Detector = DetectorObj.AddComponent<CollisionDetector>();
        Detector.PossibleCollisions = 0;
        Detector.Passed = false;

        Detector.RightLane = RightLane;

        Detector.Manager = GameObject.Find("EventManager").GetComponent<EventManager>();

        return Detector;
    }

    #region === Unity Events ===

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger &&
            other.gameObject.name != "Wall colliders")
        {
            this.PossibleCollisions++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            var Walls = other.gameObject.name == "Wall colliders";

            if (!Walls)
            {
                this.PossibleCollisions--;
            }
            else if (Walls && !this.Passed)
            {
                this.Passed = true;
                this.LaneFree();
            }
        }
    }

    #endregion

    #region === Possible Collisions ===

    public bool ImminentCollision()
    {
        return this.PossibleCollisions != 0;
    }

    #endregion

    #region === Auxliary ===
    
    private void LaneFree()
    {
        if (this.RightLane)
        {
            this.Manager.TriggerEvent(Event.RightLaneFree);
        }
        else
        {
            this.Manager.TriggerEvent(Event.LeftLaneFree);
        }
    }

    #endregion
}
