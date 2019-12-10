using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class GizmosUtils
{
    public static void DrawText(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSize = 0, float yOffset = 0)
    {
#if UNITY_EDITOR
        var prevSkin = GUI.skin;
        if (guiSkin == null)
            Debug.LogWarning("editor warning: guiSkin parameter is null");
        else
            GUI.skin = guiSkin;

        GUIContent textContent = new GUIContent(text);

        GUIStyle style = (guiSkin != null) ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
        if (color != null)
            style.normal.textColor = (Color)color;
        if (fontSize > 0)
            style.fontSize = fontSize;

        Vector2 textSize = style.CalcSize(textContent);
        Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

        if (screenPoint.z > 0) // checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
        {
            var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
            UnityEditor.Handles.Label(worldPosition, textContent, style);
        }
        GUI.skin = prevSkin;
#endif
    }
}

public class MapNode : MonoBehaviour
{
    public int id = 0;

    public int State = 0; //[ 0 - Not visited | 1 - To visit | 2 - Visited ]

    public MapNode Previous;

    private int AdjacentCount;

    public List<MapNode> AdjacentNodes; //DO NOT CHANGE ANYTHING ABOUT THIS VARIABLE

    public bool[] BlockedArc;

    // Start is called before the first frame update
    void Start()
    {
        this.AdjacentCount = this.AdjacentNodes.Count;

        this.BlockedArc = new bool[AdjacentCount];

        for (var i = 0; i < this.AdjacentCount; i++)
        {
            this.BlockedArc[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (State != 0)
        {
            if(this.GetComponentInParent<MapController>().SearchState)
            {
                State = 0;

                Previous = null;
            }
        }
    }

    // Sets an Arc between nodes to blocked or unblocked
    public bool BlockArc(MapNode target, bool value)
    {
        var ind = this.AdjacentNodes.FindIndex(x => x == target);

        if(ind != -1)
        {
            this.BlockedArc[ind] = value;

            return true;
        }

        return false;
    }

    // Returns wether or not an arc between nodes is blocked
    public bool ArcBlocked(MapNode target)
    {
        var ind = this.AdjacentNodes.FindIndex(x => x == target);

        if (ind != -1)
        {
            return this.BlockedArc[ind];
        }

        return true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
           
        Gizmos.DrawSphere(this.transform.position, 0.1f);

        var pos = this.transform.position;
        pos.z += 0.5f;

        //draw node names
        GizmosUtils.DrawText(GUI.skin, this.name, pos, Color.blue, 12, 0.5f);

        //draw lines
        var colors = new Color[4];
        colors[0] = Color.blue;
        colors[1] = Color.red;
        colors[2] = Color.green;
        colors[3] = Color.magenta;

        var off = 0.0f;

        for (var i = 0; i < this.AdjacentNodes.Count; i++){
            var child = this.AdjacentNodes[i];

            var p1 = this.transform.position;
            var p2 = child.transform.position;

            var linevec = p1 - p2;
            var offvec  = Quaternion.AngleAxis(-45, Vector3.up) * linevec;
            offvec = Vector3.ClampMagnitude(offvec, off);

            Gizmos.color = colors[ i%4 ];
            Gizmos.DrawLine(p1 + offvec, p2 + offvec);

            off += 0.05f;
            
        }
        
    }
}
