using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GluEditor : Editor
{ 
    //To Organise a group we will use a style help box with indent level
    public static bool BeginFold(string foldName, bool foldState)
    {
        EditorGUILayout.BeginVertical(boxStyle);
        GUILayout.Space(3);

        EditorGUI.indentLevel++;
        foldState = EditorGUI.Foldout(EditorGUILayout.GetControlRect(),
            foldState, foldName, true, foldoutStyle);
        EditorGUI.indentLevel--;

        if (foldState) GUILayout.Space(3);
       
        return foldState;
    }
   
    //Ending Fold
    public static void EndFold()
    {
        GUILayout.Space(3);
        EditorGUILayout.EndVertical();
        GUILayout.Space(0);
    }




    //Help Box Style
    static GUIStyle _boxStyle;
    static GUIStyle boxStyle
    {
        get
        {
            if (_boxStyle == null)
                _boxStyle = new GUIStyle(EditorStyles.helpBox);
            return _boxStyle;
        }
    }
   //Folding Style
    static GUIStyle _foldoutStyle;
    static GUIStyle _boldStyle;
    public static GUIStyle BoldStyle
    {
        get
        {
            if (_boldStyle == null)
            {
                _boldStyle = new GUIStyle();
                _boldStyle.font = EditorStyles.boldFont;
            }
            return _foldoutStyle;
        }
    }
    static GUIStyle foldoutStyle
    {
        get
        {
            if (_foldoutStyle == null)
            {
                _foldoutStyle = new GUIStyle(EditorStyles.foldout);
                _foldoutStyle.font = EditorStyles.boldFont;
            }
            return _foldoutStyle;
        }
    }

}
