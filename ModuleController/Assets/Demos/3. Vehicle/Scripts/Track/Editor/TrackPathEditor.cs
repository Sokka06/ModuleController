using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPath))]
public class TrackPathEditor : Editor
{
    private void OnSceneGUI()
    {
        var verts = new[]
        {
            new Vector3(-1, 0f, -1),
            new Vector3(-1f,0f,1f),
            new Vector3(1f,0f,1f),
            new Vector3(1f,0f,-1f),
        };
        Handles.DrawSolidRectangleWithOutline(verts, Color.blue, Color.cyan);
    }
}
