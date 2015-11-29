/* ========================================================================
 * PROJECT: VRPN Tool
 * ========================================================================
 * 
 * 
 *
 * ========================================================================
 ** @author   Andrés Roberto Gómez (and-gome@uniandes.edu.co)
 *
 * ========================================================================
 *
 * VRPNEdit.cs
 *
 * usage: Must be added once in the scene to enable the VRPNEditor playback functionality.
 * It comes in VRPNEventManager prefab.
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;

public class VRPNEdit : MonoBehaviour {

    //Public properties
    public bool isPlaying = false;
    public float initTime;

    //VRPNEdit Singleton
    private static VRPNEdit vrpnEdit;

    //VRPNEdit Singleton getter
    public static VRPNEdit instance
    {
        get
        {
            if (!vrpnEdit)
            {
                vrpnEdit = FindObjectOfType(typeof(VRPNEdit)) as VRPNEdit;
                if (!vrpnEdit)
                {
                    Debug.LogError("There needs to be one active VRPNEdit script on a GameObject in your scene.");
                }
            }
            return vrpnEdit;
        }
    }

    //Initialize VRPNEdit in Awake
    void Awake()
    {
        //init VRPNEdit
        #pragma warning disable 0219
        VRPNEdit getInstance = VRPNEdit.instance;
        #pragma warning restore 0219
    }
	
	// Update is called once per frame
	void Update () {
        if (isPlaying)
        {
            if (!VRPNEditEditor.Instance.Update())
            {
                isPlaying = false;
            }
        }
    }

    //Public method that allows to start playing
    public void StartPlaying()
    {
        VRPNEditEditor.Instance.StartPlaying();
        isPlaying = true;
        initTime = Time.time;
    }

    //Public method that allows to stop playing
    public void StopPlaying()
    {
        VRPNEditEditor.Instance.StopPlaying();
        isPlaying = false;
    }
}
