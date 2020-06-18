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