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
        // Param are the index of the segment that the character is following

        if (previousParam >= LocalPaths.Count)
        {
            return LocalPaths.Count;
        }

        // in reality previousParam is just an int, so this floor is just to get the real part
        int localIndex = Mathf.FloorToInt(previousParam);

        LocalPath path = LocalPaths[localIndex];

        // second argument does not matter: local paths does not use it
        float localParm = path.GetParam(position, 0);

        var currentParam = previousParam;
        if (path.PathEnd(localParm))
        {
            currentParam++;
        }

        return currentParam;
    }


    public override Vector3 GetPosition(float param)
    {
        // The integer part will represent the local path inside the global path
        // The decimal part represents the percentage of the local path traversed so far
        int   localIndex  = Mathf.FloorToInt(param);

        LocalPath path;

        // If the previous param is bigger then the number of local paths
        // then we already finished the global path
        if (localIndex >= LocalPaths.Count)
        {
            path = LocalPaths[LocalPaths.Count - 1];
        }
        else
        {
            path = LocalPaths[localIndex];
        }

        return path.GetPosition(1);

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
