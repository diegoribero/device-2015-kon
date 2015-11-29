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
 * VRPNAnalogPlay.cs
 *
 * usage: Must be added once for each analog sensor that is desired to play.
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class VRPNAnalogPlay : MonoBehaviour
{
    //Public properties
    public string path;
    public bool isPlaying = false;

    //Private properties
    private VRPNAnalog.AnalogReports data;
    private bool firstReport = true;
    private float firstTime;
    private List<VRPNAnalog.AnalogReportNew>.Enumerator e;
    private VRPNAnalog.AnalogReportNew actualReport;

    //Public method that allows to start playing
    //It reads the data from the indicated path
    public void StartPlaying()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            data = (VRPNAnalog.AnalogReports)bf.Deserialize(file);

            file.Close();

            isPlaying = true;

            e = data.list.GetEnumerator();
        }
    }

    // Update is called once per frame
    void Update()
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
                actualReportTime = actualReport.msg_time.tv_sec + (actualReport.msg_time.tv_usec / 1000000f);
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