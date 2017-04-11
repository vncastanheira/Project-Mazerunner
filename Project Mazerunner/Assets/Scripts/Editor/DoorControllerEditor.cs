using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(DoorController))]
public class DoorControllerEditor : Editor
{
    SerializedProperty openParameter;
    SerializedProperty closeParameter;
    
    SerializedProperty doorType;
    SerializedProperty totalCount;
    SerializedProperty timer;
    SerializedProperty onclose;
    SerializedProperty onopen;

    private void OnEnable()
    {
        openParameter = serializedObject.FindProperty("OpenParameter");
        closeParameter = serializedObject.FindProperty("CloseParameter");


        doorType = serializedObject.FindProperty("doorType");
        totalCount = serializedObject.FindProperty("TotalCount");
        timer = serializedObject.FindProperty("Timer");
        onclose = serializedObject.FindProperty("OnClose");
        onopen = serializedObject.FindProperty("OnOpen");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
        openParameter.stringValue = EditorGUILayout.TextField("Open Parameter", openParameter.stringValue);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Configurations", EditorStyles.boldLabel);
        doorType.enumValueIndex = EditorGUILayout.Popup("Type", doorType.enumValueIndex, doorType.enumDisplayNames);
        switch ((DoorType) doorType.enumValueIndex)
        {
            case DoorType.Simple:
                EditorGUILayout.HelpBox("Door will open as soon as the button is activated.", MessageType.Info);
                break;
            case DoorType.Numbered:
                totalCount.intValue = EditorGUILayout.IntField("Buttons", totalCount.intValue);
                EditorGUILayout.HelpBox("Number of buttons that must be activated so the door can open.", MessageType.Info);
                break;
            case DoorType.Timed:
                closeParameter.stringValue = EditorGUILayout.TextField("Close Parameter", closeParameter.stringValue);
                timer.floatValue = EditorGUILayout.FloatField("Timing", timer.floatValue);
                totalCount.intValue = EditorGUILayout.IntField("Buttons", totalCount.intValue);
                EditorGUILayout.HelpBox("A number of buttons need to be activated before time runs out.", MessageType.Info);

                EditorGUILayout.PropertyField(onclose);
                break;
            default:
                break;
        }
        EditorGUILayout.PropertyField(onopen);

        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
