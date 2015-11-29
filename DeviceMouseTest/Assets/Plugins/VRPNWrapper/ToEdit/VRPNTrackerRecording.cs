/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Tracker Recording class to support VRPNEdit recordings playing
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNTrackerRecording.cs
 *
 * usage: 
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class VRPNTrackerRecording
{
    //Public properties
    public string name;
    public float reportTime;
    public Dictionary<int, int> sensors = new Dictionary<int, int>();
    public Dictionary<int, int> sensorsDisabled = new Dictionary<int, int>();
    public bool isPlaying = false;
    public float lastTime;

    //Private properties
    private VRPNTracker.TrackerReports data;
    private bool firstReport = true;
    private float firstTime;
    private List<VRPNTracker.TrackerReportNew>.Enumerator e;
    private VRPNTracker.TrackerReportNew actualReport;

    //VRPNTrackerRecording Constructor
    public VRPNTrackerRecording(string nName, float nTime, VRPNTracker.TrackerReports nData)
    {
        name = nName;
        reportTime = nTime;
        data = nData;

        e = data.list.GetEnumerator();

        while (e.MoveNext())
        {
            VRPNTracker.TrackerReportNew report = e.Current;
            int test;
            if (!sensors.TryGetValue(report.sensor, out test))
            {
                sensors.Add(report.sensor, report.sensor);
            }
            lastTime = report.msg_time.tv_sec + (report.msg_time.tv_usec / 1000000f);
        }

        e = data.list.GetEnumerator();
    }

    //Public method that allows to start playing
    public void StartPlaying()
    {
        isPlaying = true;
    }

    //Public method that allows to update the playing state
    public void Update()
    {
        if (isPlaying)
        {
            float actualTime;
            float actualReportTime = 0f;
            bool moreReports = true;
            bool alreadyAdvanced = false;
            Dictionary<int, VRPNTracker.TrackerReportNew> lastReports = new Dictionary<int, VRPNTracker.TrackerReportNew>();

            if (firstReport)
            {
                firstReport = false;
                firstTime = Time.time;
                if (e.MoveNext())
                {
                    actualReport = e.Current;
                }
                else
                {
                    isPlaying = false;
                    moreReports = false;
                    firstReport = true;
                }
            }

            actualTime = Time.time - firstTime;

            //It seeks the last appropiate report for the actual time
            while (moreReports)
            {
                actualReportTime = actualReport.msg_time.tv_sec + (actualReport.msg_time.tv_usec / 1000000f) + reportTime;
                if (actualReportTime <= actualTime)
                {
                    VRPNTracker.TrackerReportNew test;
                    if (lastReports.TryGetValue(actualReport.sensor, out test))
                    {
                        lastReports[actualReport.sensor] = actualReport;
                    }
                    else
                    {
                        lastReports.Add(actualReport.sensor, actualReport);
                    }
                    if (e.MoveNext())
                    {
                        actualReport = e.Current;
                        alreadyAdvanced = true;
                    }
                    else
                    {
                        sendingReports(lastReports);

                        moreReports = false;
                        isPlaying = false;
                        firstReport = true;
                    }
                }
                else if (alreadyAdvanced)
                {
                    sendingReports(lastReports);

                    moreReports = false;
                }
                else
                {
                    moreReports = false;
                }
            }
        }
    }

    //Auxiliar method that sends last frame report for each sensor
    private void sendingReports(Dictionary<int, VRPNTracker.TrackerReportNew> lastReports)
    {
        foreach (KeyValuePair<int, VRPNTracker.TrackerReportNew> pair in lastReports)
        {
            int test;
            if (!sensorsDisabled.TryGetValue(pair.Key, out test))
            {
                VRPNTracker.TrackerReport newReport = new VRPNTracker.TrackerReport();
                VRPNManager.TimeVal newMsgTime = new VRPNManager.TimeVal();
                newMsgTime.tv_sec = (UInt32)pair.Value.msg_time.tv_sec;
                newMsgTime.tv_usec = (UInt32)pair.Value.msg_time.tv_usec;
                newReport.msg_time = newMsgTime;
                newReport.pos = pair.Value.pos;
                newReport.quat = pair.Value.quat;
                newReport.sensor = pair.Value.sensor;
                VRPNEventManager.TriggerEventTracker(data.deviceType, data.deviceName, newReport);
            }
        }
    }

    //Public method that allows to stop playing
    public void StopPlaying()
    {
        isPlaying = false;
        firstReport = true;
        e = data.list.GetEnumerator();
    }
}

//Auxiliar class to store a list of recordings
[Serializable]
public class VRPNTrackerRecordings
{
    public List<VRPNTrackerRecording> recordings = new List<VRPNTrackerRecording>();
}
