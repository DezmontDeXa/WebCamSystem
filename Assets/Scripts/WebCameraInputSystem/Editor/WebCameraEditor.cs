using System.Linq;
using UnityEditor;
using UnityEngine;

namespace WebCameraInputSystem.Editor
{
    [CustomPropertyDrawer(typeof(WebCameraName))]
    public class WebCameraNamePropDrawer : PropertyDrawer
    {
        int _choiceIndex;
        string[] _choices = WebCamTexture.devices.Select(x => x.name).ToArray();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty userIndexProperty = property.FindPropertyRelative("SelectedIndex");

            EditorGUI.BeginChangeCheck();
            _choiceIndex = EditorGUI.Popup(position, userIndexProperty.intValue, _choices);
            if (EditorGUI.EndChangeCheck())
            {
                userIndexProperty.intValue = _choiceIndex;
            }
        }
    }
}
