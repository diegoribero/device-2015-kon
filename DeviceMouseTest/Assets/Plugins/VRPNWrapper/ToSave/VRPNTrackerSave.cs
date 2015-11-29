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
 * VRPNTrackerSave.cs
 *
 * usage: Must be added once for each tracker that is desired to record.
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

public class VRPNTrackerSave : MonoBehaviour {
    //Public Properties
    public string path;
    public VRPNManager.Tracker_Types TrackerType;
    public VRPNDeviceConfig.Device_Names TrackerName;
    public bool isRecording;

    //Private Properties
    private VRPNTracker.TrackerReports data = new VRPNTracker.TrackerReports();
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
        data.deviceType = TrackerType.ToString();
        data.deviceName = TrackerName.ToString();
        VRPNEventManager.StartListeningTracker(TrackerType, TrackerName, Record);
        isRecording = true;
    }

    //This is the listener that is called by the event manager
    //It transforms and adds the received report to the reports list
    void Record(string name, VRPNTracker.TrackerReport report)
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
        VRPNTracker.TrackerReportNew newReport = new VRPNTracker.TrackerReportNew();
        VRPNManager.TimeValNew newMsgTime = new VRPNManager.TimeValNew();
        newMsgTime.tv_sec = (int)report.msg_time.tv_sec;
        newMsgTime.tv_usec = (int)report.msg_time.tv_usec;
        newReport.msg_time = newMsgTime;
        newReport.pos = report.pos;
        newReport.quat = report.quat;
        newReport.sensor = report.sensor;
        data.list.Add(newReport);
    }

    //Public method that allows to stop recording
    //It saves the reports list in the indicated path
    public void StopRecording()
    {
        VRPNEventManager.StopListeningTracker(TrackerType, TrackerName, Record);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, data);
        file.Close();
        data = new VRPNTracker.TrackerReports();
        firstReport = true;
        isRecording = false;
    }
}