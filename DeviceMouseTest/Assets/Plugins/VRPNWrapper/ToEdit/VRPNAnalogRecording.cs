/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * Analog Recording class to support VRPNEdit recordings playing
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNAnalogRecording.cs
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
public class VRPNAnalogRecording
{
    //Public properties
    public string name;
    public float reportTime;
    public int channels;
    public bool isPlaying = false;
    public float lastTime;

    //Private properties
    private VRPNAnalog.AnalogReports data;
    private bool firstReport = true;
    private float firstTime;
    private List<VRPNAnalog.AnalogReportNew>.Enumerator e;
    private VRPNAnalog.AnalogReportNew actualReport;

    //VRPNAnalogRecording Constructor
    public VRPNAnalogRecording(string nName, float nTime, VRPNAnalog.AnalogReports nData)
    {
        name = nName;
        reportTime = nTime;
        data = nData;
        e = data.list.GetEnumerator();

        while (e.MoveNext())
        {
            VRPNAnalog.AnalogReportNew report = e.Current;
            channels = report.num_channel;
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
            VRPNAnalog.AnalogReportNew lastReport = new VRPNAnalog.AnalogReportNew();

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
                    lastReport = e.Current;
                    if (e.MoveNext())
                    {
                        actualReport = e.Current;
                        alreadyAdvanced = true;
                    }
                    else
                    {
                        sendingReport(lastReport);

                        moreReports = false;
                        isPlaying = false;
                        firstReport = true;
                    }
                }
                else if (alreadyAdvanced)
                {
                    sendingReport(lastReport);

                    moreReports = false;
                }
                else
                {
                    moreReports = false;
                }
            }
        }
    }

    //Auxiliar method that sends last frame report
    private void sendingReport(VRPNAnalog.AnalogReportNew lastReport)
    {
        VRPNAnalog.AnalogReport newReport = new VRPNAnalog.AnalogReport();
        VRPNManager.TimeVal newMsgTime = new VRPNManager.TimeVal();
        newMsgTime.tv_sec = (UInt32)lastReport.msg_time.tv_sec;
        newMsgTime.tv_usec = (UInt32)lastReport.msg_time.tv_usec;
        newReport.msg_time = newMsgTime;
        newReport.num_channel = lastReport.num_channel;
        newReport.channel = lastReport.channel;
        VRPNEventManager.TriggerEventAnalog(data.deviceType, data.deviceName, newReport);
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
public class VRPNAnalogRecordings
{
    public List<VRPNAnalogRecording> recordings = new List<VRPNAnalogRecording>();
}
