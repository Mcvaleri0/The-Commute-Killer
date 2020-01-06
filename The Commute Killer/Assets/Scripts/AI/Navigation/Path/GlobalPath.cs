using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;
using System;



public class GlobalPath : Path
{
    public List<NavNode> PathNodes { get; protected set; }
    public List<Vector3> PathPositions { get; protected set; } 
    public List<LocalPath> LocalPaths { get; protected set; }
    public bool IsPartial { get; set; }
    public float Length { get; set; }
    public int NumberNodes { get; set; }


    public GlobalPath()
    {
        this.PathNodes = new List<NavNode>();
        this.PathPositions = new List<Vector3>();
        this.LocalPaths = new List<LocalPath>();
    }


    public override float GetParam(Vector3 position, float previousParam)
    {
        // The integer part will represent the local path inside the global path
        // The decimal part represents the percentage of the local path traversed so far
        int   localIndex     = Mathf.FloorToInt(previousParam);
        float percentageDone = previousParam % 1;

        // If the previous param is bigger then the number of local paths
        // then we already finished the global path
        if (localIndex >= LocalPaths.Count)
        {
            return LocalPaths.Count;
        }

        LocalPath path  = LocalPaths[localIndex];
        float localParm = path.GetParam(position, percentageDone);

        // If we already finished this segment then we are the begining of the next one
        if (path.PathEnd(localParm)) {
            localIndex++;

            // If the previous local path was the last one
            // then we already finished the global path
            if (localIndex == LocalPaths.Count)
            {
                return LocalPaths.Count;
            }

            Vector3 pathStart = path.GetPosition(0);
            Vector3 pathEnd   = path.GetPosition(1);

            LocalPath nextPath  = LocalPaths[localIndex];
            Vector3   nextStart = nextPath.GetPosition(0);
            Vector3   nextEnd   = nextPath.GetPosition(1);

            // closestPosition to our position in the next segment
            var closestPosition = MathHelper.ClosestPointInLineSegment2ToLineSegment1(pathStart, pathEnd, nextStart, nextEnd, position);

            // percentage of the next segment already done
            localParm = nextPath.GetParam(closestPosition, 0);
        }

        // If we are not done with the path
        return localIndex + localParm;
    }


    public override Vector3 GetPosition(float param)
    {
        // The integer part will represent the local path inside the global path
        // The decimal part represents the percentage of the local path traversed so far
        int   localIndex     = Mathf.FloorToInt(param);
        float percentageDone = param % 1;

        LocalPath path;

        // If the previous param is bigger then the number of local paths
        // then we already finished the global path
        if (localIndex >= LocalPaths.Count)
        {
            path = LocalPaths[LocalPaths.Count - 1];
            return path.GetPosition(1);
        }

        path = LocalPaths[localIndex];

        return path.GetPosition(percentageDone);
    }


    public override bool PathEnd(float param)
    {
        // The integer part will represent the local path inside the global path
        // The decimal part represents the percentage of the local path traversed so far
        int   localIndex     = Mathf.FloorToInt(param);
        float percentageDone = param % 1;
            
        if (localIndex >= LocalPaths.Count)
        {
            return true;
        }
            
        LocalPath path = LocalPaths[localIndex];

        return path.PathEnd(percentageDone);
    }
}
