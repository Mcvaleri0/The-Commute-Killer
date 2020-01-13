﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    #region /* Collisions */
    private int PossibleCollisions { get; set; }

    private bool Passed { get; set; }

    #endregion

    public static CollisionDetector CreateCollisionDetector(Transform Parent, Vector3 Position, Vector3 Size)
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

        return Detector;
    }

    #region === Unity Events ===

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger &&
            other.gameObject.name != "Wall colliders" && 
            this.Passed)
        {
            this.PossibleCollisions++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            var Walls = other.gameObject.name == "Wall colliders";

            if (!Walls && this.Passed)
            {
                this.PossibleCollisions--;
            }
            else if (Walls && !this.Passed)
            {
                this.Passed = true;
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
}
