// Initial Concept by http://www.reddit.com/user/zaikman
// Revised by http://www.reddit.com/user/quarkism
// Extended by William Pyle 2020 http://www.wpyle.com

using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// Add to methods to expose a button in the editor, that when pressed, executes the method. 
/// WARNING: Does not work on methods that require params
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
    private string buttonText = null;
    public string ButtonText => buttonText;

    private int spaceBefore;
    public int SpaceBefore => spaceBefore;

    private Color color = GUI.backgroundColor;
    public Color Color => color;

    /// <summary>
    ///  Button with custom text and set color.  NOTE: Only mark one color property as true. Later ones listed will overwrite previous ones.
    /// </summary>
    public EditorButtonAttribute(string buttonText, int spaceBefore = 10, bool white = false, bool cyan = false, bool blue = false,
        bool yellow = false, bool green = false, bool magenta = false, bool red = false, bool gray = false, bool black = false)
    {
        this.buttonText = buttonText;
        this.spaceBefore = spaceBefore;

        if (white) color = Color.white;
        if (cyan) color = Color.cyan;
        if (blue) color = Color.blue;
        if (yellow) color = Color.yellow;
        if (green) color = Color.green;
        if (magenta) color = Color.magenta;
        if (red) color = Color.red;
        if (gray) color = Color.gray;
        if (black) color = Color.black;
    }
    /// <summary>
    /// Button with text as method name and set color.
    /// </summary>
    public EditorButtonAttribute(int spaceBefore = 10, bool white = false, bool cyan = false, bool blue = false,
        bool yellow = false, bool green = false, bool magenta = false, bool red = false, bool gray = false, bool black = false)
    {
        this.spaceBefore = spaceBefore;

        if (white) color = Color.white;
        if (cyan) color = Color.cyan;
        if (blue) color = Color.blue;
        if (yellow) color = Color.yellow;
        if (green) color = Color.green;
        if (magenta) color = Color.magenta;
        if (red) color = Color.red;
        if (gray) color = Color.gray;
        if (black) color = Color.black;
    }
    /// <summary>
    /// Button with custom text and custom color. No alpha.
    /// </summary>
    public EditorButtonAttribute(string buttonText, float colorR, float colorG, float colorB, int spaceBefore = 10)
    {
        this.buttonText = buttonText;
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB);
    }
    /// <summary>
    /// Button with text as method name and custom color. No alpha.
    /// </summary>
    public EditorButtonAttribute(float colorR, float colorG, float colorB, int spaceBefore = 10)
    {
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB);
    }
    /// <summary>
    /// Button with custom text and custom color. With alpha.
    /// </summary>
    public EditorButtonAttribute(string buttonText, float colorR, float colorG, float colorB, float colorA, int spaceBefore = 10)
    {
        this.buttonText = buttonText;
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB, colorA);
    }
    /// <summary>
    /// Button with text as method name and custom color. With alpha.
    /// </summary>
    public EditorButtonAttribute(float colorR, float colorG, float colorB, float colorA, int spaceBefore = 10)
    {
        this.spaceBefore = spaceBefore;
        this.color = new Color(colorR, colorG, colorB, colorA);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mono = target as MonoBehaviour;

        var methods = mono.GetType()
            .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                        BindingFlags.NonPublic)
            .Where(o => Attribute.IsDefined(o, typeof(EditorButtonAttribute)));

        foreach (var memberInfo in methods)
        {
            var attr = memberInfo.GetCustomAttribute(typeof(EditorButtonAttribute)) as EditorButtonAttribute;

            string buttonText = attr.ButtonText != null
                ? buttonText = attr.ButtonText
                : buttonText = memberInfo.Name;

            GUILayout.Space(attr.SpaceBefore);

            Color defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = attr.Color;
            if (GUILayout.Button(buttonText))
            {
                var method = memberInfo as MethodInfo;

                ParameterInfo[] parameters = method.GetParameters();
                List<object> newCollection = new List<object>();
                foreach (var param in parameters)
                {
                    if (!param.HasDefaultValue)
                    {
                        Debug.LogError("EditorButtonAttribute only works on methods that contain exclusively parameters with default values." +
                            " Parameter '" + param.Name + "' does not have a default value.");
                        return;
                    }
                    var newParam = param.DefaultValue;
                    newCollection.Add(newParam);
                }

                object[] objArry = newCollection.ToArray<object>();

                method.Invoke(mono, objArry);
            }
            GUI.backgroundColor = defaultColor;
        }
    }
}
#endif