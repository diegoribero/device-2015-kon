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
 * VRPNAnalogSaveEditor.cs
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

[CustomEditor(typeof(VRPNAnalogSave))]
public class VRPNAnalogSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Properties
        VRPNAnalogSave vrpnAnalogSave = (VRPNAnalogSave) target;
        bool ready = true;
        string errorText = "";

        //VRPNAnalog interaction
        if (vrpnAnalogSave.gameObject.GetComponent<VRPNAnalog>() != null)
        {
            vrpnAnalogSave.AnalogType = vrpnAnalogSave.gameObject.GetComponent<VRPNAnalog>().AnalogType;
            vrpnAnalogSave.AnalogName = vrpnAnalogSave.gameObject.GetComponent<VRPNAnalog>().AnalogName;
        }

        //Validation
        if (vrpnAnalogSave.path == null || vrpnAnalogSave.path == "")
        {
            errorText = "A save path must be chosen";
            ready = false;
        }
        if (!Application.isPlaying)
        {
            errorText = "The editor must be running";
            ready = false;
        }
        if (vrpnAnalogSave.gameObject.GetComponent<VRPNAnalog>() == null)
        {
            errorText = "This GameObject must contain a VRPNAnalog script to record";
            ready = false;
        }
        if (vrpnAnalogSave.gameObject.GetComponents<VRPNAnalog>().Length > 1)
        {
            errorText = "This GameObject must contain ONLY ONE VRPNAnalog script to record";
            ready = false;
        }

        //Controls
        EditorGUILayout.LabelField("Analog Type", vrpnAnalogSave.AnalogType.ToString(), EditorStyles.textField);
        EditorGUILayout.LabelField("Analog Name", vrpnAnalogSave.AnalogName.ToString(), EditorStyles.textField);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Record", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(vrpnAnalogSave.path, EditorStyles.textArea);
        if (GUILayout.Button("Record Path"))
        {
            vrpnAnalogSave.path = EditorUtility.SaveFilePanel("Save VRPN Analog File", "/Assets/VRPNFiles", vrpnAnalogSave.AnalogType.ToString() + "-" + vrpnAnalogSave.AnalogName.ToString(), "vrpnAnalogFile");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        if (!ready || vrpnAnalogSave.isRecording)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Start"))
        {
            vrpnAnalogSave.StartRecording();
        }
        GUI.enabled = true;
        if (!ready || !vrpnAnalogSave.isRecording)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Stop"))
        {
            vrpnAnalogSave.StopRecording();
        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        //User Feedback
        if (!ready)
        {
            EditorGUILayout.HelpBox(errorText, MessageType.Error);
        }

        if (vrpnAnalogSave.isRecording)
        {
            EditorGUILayout.HelpBox("Recording", MessageType.Info);
        }
    }
} 