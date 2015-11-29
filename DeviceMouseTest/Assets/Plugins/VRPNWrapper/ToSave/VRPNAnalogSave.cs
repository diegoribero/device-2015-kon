/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Based on http://unity3d.com/es/learn/tutorials/modules/beginner/live-training-archive/persistence-data-saving-loading
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNAnalogSave.cs
 *
 * usage: Must be added once for each analog sensor that is desired to record.
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class VRPNAnalogSave : MonoBehaviour
{
    //Public Properties
    public string path;
    public VRPNManager.Analog_Types AnalogType;
    public VRPNDeviceConfig.Device_Names AnalogName;
    public bool isRecording;

    //Private Properties
    private VRPNAnalog.AnalogReports data = new VRPNAnalog.AnalogReports();
    private bool firstReport = true;
    private UInt32 firstTime_sec;
    private UInt32 firstTime_usec;

    void Awake()
    {
        isRecording = false;
    }

    //Public method that allows to start recording in the indicated path
    //It registers the listener method in the event manager
    public void StartRecording()
    {
        data.deviceType = AnalogType.ToString();
        data.deviceName = AnalogName.ToString();
        VRPNEventManager.StartListeningAnalog(AnalogType, AnalogName, Record);
        isRecording = true;
    }

    //This is the listener that is called by the event manager
    //It transforms and adds the received report to the reports list
    void Record(string name, VRPNAnalog.AnalogReport report)
    {
        if (firstReport)
        {
            firstTime_sec = report.msg_time.tv_sec;
            firstTime_usec = report.msg_time.tv_usec;
            firstReport = false;
        }
        if (report.msg_time.tv_usec < firstTime_usec)
        {
            report.msg_time.tv_sec = report.msg_time.tv_sec - (firstTime_sec + 1);
            report.msg_time.tv_usec = (report.msg_time.tv_usec + 1000000) - firstTime_usec;
        }
        else
        {
            report.msg_time.tv_sec = report.msg_time.tv_sec - firstTime_sec;
            report.msg_time.tv_usec = report.msg_time.tv_usec - firstTime_usec;
        }
        VRPNAnalog.AnalogReportNew newReport = new VRPNAnalog.AnalogReportNew();
        VRPNManager.TimeValNew newMsgTime = new VRPNManager.TimeValNew();
        newMsgTime.tv_sec = (int)report.msg_time.tv_sec;
        newMsgTime.tv_usec = (int)report.msg_time.tv_usec;
        newReport.msg_time = newMsgTime;
        newReport.num_channel = report.num_channel;
        newReport.channel = report.channel;
        data.list.Add(newReport);
    }

    //Public method that allows to stop recording
    //It saves the reports list in the indicated path
    public void StopRecording()
    {
        VRPNEventManager.StopListeningAnalog(AnalogType, AnalogName, Record);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, data);
        file.Close();
        data = new VRPNAnalog.AnalogReports();
        firstReport = true;
        isRecording = false;
    }
}