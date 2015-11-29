/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on: 
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/editor/building-custom-inspector?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/editor/drawdefaultinspector-function?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/editor/adding-buttons-to-inspector?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/beginner/live-training-archive/editor-basics/editor-scripting-intro?playlist=17117
 * http://unity3d.com/es/learn/tutorials/modules/intermediate/live-training-archive/property-drawers-custom-inspectors?playlist=17117
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNButtonSaveEditor.cs
 *
 * usage: Must be added in the Editor folder
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRPNButtonSave))]
public class VRPNButtonSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Properties
        VRPNButtonSave vrpnButtonSave = (VRPNButtonSave) target;
        bool ready = true;
        string errorText = "";

        //VRPNButton interaction
        if (vrpnButtonSave.gameObject.GetComponent<VRPNButton>() != null)
        {
            vrpnButtonSave.ButtonType = vrpnButtonSave.gameObject.GetComponent<VRPNButton>().ButtonType;
            vrpnButtonSave.ButtonName = vrpnButtonSave.gameObject.GetComponent<VRPNButton>().ButtonName;
        }

        //Validation
        if (vrpnButtonSave.path == null || vrpnButtonSave.path == "")
        {
            errorText = "A save path must be chosen";
            ready = false;
        }
        if (!Application.isPlaying)
        {
            errorText = "The editor must be running";
            ready = false;
        }
        if (vrpnButtonSave.gameObject.GetComponent<VRPNButton>() == null)
        {
            errorText = "This GameObject must contain a VRPNButton script to record";
            ready = false;
        }
        if (vrpnButtonSave.gameObject.GetComponents<VRPNButton>().Length > 1)
        {
            errorText = "This GameObject must contain ONLY ONE VRPNButton script to record";
            ready = false;
        }

        //Controls
        EditorGUILayout.LabelField("Button Type", vrpnButtonSave.ButtonType.ToString(), EditorStyles.textField);
        EditorGUILayout.LabelField("Button Name", vrpnButtonSave.ButtonName.ToString(), EditorStyles.textField);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Record", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(vrpnButtonSave.path, EditorStyles.textArea);
        if (GUILayout.Button("Record Path"))
        {
            vrpnButtonSave.path = EditorUtility.SaveFilePanel("Save VRPN Button File", "/Assets/VRPNFiles", vrpnButtonSave.ButtonType.ToString() + "-" + vrpnButtonSave.ButtonName.ToString(), "vrpnButtonFile");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        if (!ready || vrpnButtonSave.isRecording)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Start"))
        {
            vrpnButtonSave.StartRecording();
        }
        GUI.enabled = true;
        if (!ready || !vrpnButtonSave.isRecording)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Stop"))
        {
            vrpnButtonSave.StopRecording();
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        //User Feedback
        if (!ready)
        {
            EditorGUILayout.HelpBox(errorText, MessageType.Error);
        }

        if (vrpnButtonSave.isRecording)
        {
            EditorGUILayout.HelpBox("Recording", MessageType.Info);
        }
    }
} 