using Assets.Scripts.IAJ.Unity.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObstaclesDetector : MonoBehaviour
{
    public Dictionary<int, KinematicData> Characters { get; private set; }
    public Dictionary<string, StaticData> Obstacles { get; private set; }


    public static ObstaclesDetector CreateObstaclesDetector(Transform Parent, float Radius)
    {
        GameObject DetectorObj = new GameObject("Detector");

        DetectorObj.transform.parent = Parent;
        DetectorObj.transform.localPosition = Vector3.zero;

        SphereCollider Collider = DetectorObj.AddComponent<SphereCollider>();
        Collider.radius = Radius;
        Collider.isTrigger = true;

        ObstaclesDetector Detector = DetectorObj.AddComponent<ObstaclesDetector>();
        Detector.Characters = new Dictionary<int, KinematicData>();
        Detector.Obstacles = new Dictionary<string, StaticData>();

        return Detector;
    }


    #region === Unity Events ===

    private void OnTriggerEnter(Collider other)
    {


        if (!other.isTrigger)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                KinematicData data = other.gameObject.GetComponent<AutonomousAgent>().DCharacter.KinematicData;
                this.Characters.Add(other.gameObject.GetInstanceID(), data);

                Debug.Log("possible collision with " + other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.gameObject.CompareTag("NPC"))
            {
                this.Characters.Remove(other.gameObject.GetInstanceID());
            }
        }

        //Debug.Log("no possible collision with " + other.gameObject.name);

    }

    #endregion
}
