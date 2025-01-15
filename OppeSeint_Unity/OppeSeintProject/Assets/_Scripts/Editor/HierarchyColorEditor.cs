using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierarchyColorEditor
{
    private static readonly Color enemyColor = new Color(0.5f, 0, 0, 1);
    private static readonly Color playerColor = new Color(0, 0.5f, 0, 1);
    private static readonly Color canvasColor = new Color(0.7f, 0.3f, 0.5f, 1);
    private static readonly Color levelColor = new Color(0, 0.2f, 0.5f, 1);
    private static readonly Color activatorColor = new Color(0.1f, 0.2f, 0.4f, 1);

    //static constructor to add the event handler
    static HierarchyColorEditor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
    }

    //draw colors in the hierarchy
    private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (obj != null)
        {
            //color based on tag
            Color? colorToUse = GetColorByTag(obj.tag);

            if (colorToUse.HasValue)
            {
                //apply color
                EditorGUI.DrawRect(selectionRect, colorToUse.Value);
                //ensure name is readable in new color
                EditorGUI.LabelField(selectionRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = Color.white }
                });   
            }
        }
    }

    //return a color based on the tag assigned
    private static Color? GetColorByTag(string tag)
    {
        switch (tag)
        {
            case "Enemy":
                return enemyColor;
            case "Boss":
                return enemyColor;
            case "Player":
                return playerColor;
            case "Canvas":
                return canvasColor;
            case "LevelLoader":
                return levelColor;
            case "Activator":
                return activatorColor;
            case "Coin":
                return Color.yellow;
            default: 
                return null;
        }
    }
}
