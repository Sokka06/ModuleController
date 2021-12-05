using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Info))]
public class InfoEditor : Editor
{
    private Info _target;
    
    private void OnEnable()
    {
        _target = serializedObject.targetObject as Info;
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement customInspector = new VisualElement();

        var infoProperty = serializedObject.FindProperty("InfoText");
        var infoField = new TextField("", -1, true, false, '*');
        var infoBox = new HelpBox(_target.InfoText, HelpBoxMessageType.Info);
        
        infoField.BindProperty(infoProperty);
        infoField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                //_target.InfoText = e.newValue;
                infoBox.text = e.newValue;
                //EditorUtility.SetDirty(_target);
            }
        );
        
        customInspector.Add(infoField);
        customInspector.Add(infoBox);
        //customInspector.hierarchy.Insert(0, infoField);
        
        //customInspector.hierarchy.Insert(1, infoBox);
        
        return customInspector;
    }
}

/// <summary>
/// Info is a simple MonoBehavior that displays an info text box in editor inspector
/// </summary>
public class Info : MonoBehaviour
{
    public string InfoText;
}
