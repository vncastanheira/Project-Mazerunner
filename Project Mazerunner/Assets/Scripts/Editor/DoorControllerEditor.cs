using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(DoorController))]
public class DoorControllerEditor : Editor
{
    SerializedProperty doorType;
    SerializedProperty totalCount;
    SerializedProperty openParameter;

    private void OnEnable()
    {
        openParameter = serializedObject.FindProperty("OpenParameter");
        totalCount = serializedObject.FindProperty("TotalCount");
        doorType = serializedObject.FindProperty("doorType");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
        openParameter.stringValue = EditorGUILayout.TextField("Trigger Parameter", openParameter.stringValue);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Configurations", EditorStyles.boldLabel);
        doorType.enumValueIndex = EditorGUILayout.Popup("Type", doorType.enumValueIndex, doorType.enumDisplayNames);
        switch ((DoorType) doorType.enumValueIndex)
        {
            case DoorType.Simple:
                EditorGUILayout.HelpBox("Door will open as soon as the button is activated.", MessageType.Info);
                break;
            case DoorType.Numbered:
                totalCount.intValue = EditorGUILayout.IntField("Total Count", totalCount.intValue);
                EditorGUILayout.HelpBox("Number of buttons that must be activated so the door can open.", MessageType.Info);
                break;
            default:
                break;
        }


        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
