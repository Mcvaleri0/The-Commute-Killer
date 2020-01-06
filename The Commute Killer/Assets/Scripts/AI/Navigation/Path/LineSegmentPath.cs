using Assets.Scripts.IAJ.Unity.Utils;
using UnityEngine;
using System;



public class LineSegmentPath : LocalPath
{
    /// <summary>
    ///     Path to follow
    /// </summary>
    protected Vector3 LineVector;
    public LineSegmentPath(Vector3 start, Vector3 end)
    {
        StartPosition = start;
        EndPosition   = end;
        LineVector    = end - start;
    }


    public override Vector3 GetPosition(float param)
    {
        if (param == 1)
        {
            return EndPosition;
        }
        return StartPosition + LineVector.normalized * param;
    }


    public override bool PathEnd(float param)
    {
        // 1 represents that 100% of the path is done
        return param == 1;
    }


    public override float GetParam(Vector3 position, float lastParam)
    {
        var res = MathHelper.closestParamInLineSegmentToPoint(StartPosition, EndPosition, position);
        return res;
    }
}
