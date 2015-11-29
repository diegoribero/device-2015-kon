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
 * PruebaTracker.cs
 *
 * usage: Se le debe agregar a representación de control - Se encuentra de forma predeterminada en el prefab RaizerHydra
 * 
 * inputs: Transformada de los "botones"
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;

public class PruebaTracker : MonoBehaviour {

    //Propiedades públicas (Para poder cambiar desde el editor)
    public bool estaMostrandoInfoBoton = false;
    public bool estaMostrandoInfoAnalogo = false;
    public bool estaMostrandoInfoTracker = false;
    public Transform[] capsulas;
    [Range(0, 1)]
    public int sensor;

    //Propiedad privada
    private Vector3[] capsulasEscalaOriginal;

    // Inicialización
    void Start()
    {
        //Se agrega un método por cada tipo de dispositivo para que esté pendiente de los mensajes que se envían desde este
        VRPNEventManager.StartListeningButton(VRPNManager.Button_Types.vrpn_Tracker_RazerHydra, VRPNDeviceConfig.Device_Names.Tracker0, CambioEnUnBoton);
        VRPNEventManager.StartListeningAnalog(VRPNManager.Analog_Types.vrpn_Tracker_RazerHydra, VRPNDeviceConfig.Device_Names.Tracker0, CambioEnUnAnalogo);
        VRPNEventManager.StartListeningTracker(VRPNManager.Tracker_Types.vrpn_Tracker_RazerHydra, VRPNDeviceConfig.Device_Names.Tracker0, CambioEnUnTracker);

        //Almacenando escala original de los "Botones"
        capsulasEscalaOriginal = new Vector3[capsulas.Length];
        for (int i = 0; i < capsulas.Length; i++)
        {
            capsulasEscalaOriginal[i] = capsulas[i].localScale;
        }
    }

    //Método que se encarga de estar pendiente de los mensajes que se envían desde los botones del dispositivo
    void CambioEnUnBoton(string name, VRPNButton.ButtonReport report)
    {
        //Info para el Log
        if (estaMostrandoInfoBoton)
        {
            Debug.Log("Name: " + name + " Button: " + report.button + " State:" + report.state);
        }

        //Si el control que se está revisando es el izquierdo
        if (sensor == 0)
        {
            switch (report.button)
            {
                //Boton del medio
                case 0:
                    if (report.state == 0)
                    {
                        capsulas[0].localScale = capsulasEscalaOriginal[0];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[0].localScale = new Vector3(capsulasEscalaOriginal[0].x, capsulasEscalaOriginal[0].y * 2, capsulasEscalaOriginal[0].z);
                    }
                    break;
                //Boton 1
                case 1:
                    if (report.state == 0)
                    {
                        capsulas[1].localScale = capsulasEscalaOriginal[1];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[1].localScale = new Vector3(capsulasEscalaOriginal[1].x, capsulasEscalaOriginal[1].y * 2, capsulasEscalaOriginal[1].z);
                    }
                    break;
                //Boton 2
                case 2:
                    if (report.state == 0)
                    {
                        capsulas[2].localScale = capsulasEscalaOriginal[2];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[2].localScale = new Vector3(capsulasEscalaOriginal[2].x, capsulasEscalaOriginal[2].y * 2, capsulasEscalaOriginal[2].z);
                    }
                    break;
                //Boton 3
                case 3:
                    if (report.state == 0)
                    {
                        capsulas[3].localScale = capsulasEscalaOriginal[3];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[3].localScale = new Vector3(capsulasEscalaOriginal[3].x, capsulasEscalaOriginal[3].y * 2, capsulasEscalaOriginal[3].z);
                    }
                    break;
                //Boton 4
                case 4:
                    if (report.state == 0)
                    {
                        capsulas[4].localScale = capsulasEscalaOriginal[4];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[4].localScale = new Vector3(capsulasEscalaOriginal[4].x, capsulasEscalaOriginal[4].y * 2, capsulasEscalaOriginal[4].z);
                    }
                    break;
                //Bumper
                case 5:
                    if (report.state == 0)
                    {
                        capsulas[5].localScale = capsulasEscalaOriginal[5];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[5].localScale = new Vector3(capsulasEscalaOriginal[5].x, capsulasEscalaOriginal[5].y * 2, capsulasEscalaOriginal[5].z);
                    }
                    break;
                //Joystick
                case 6:
                    if (report.state == 0)
                    {
                        capsulas[6].localScale = capsulasEscalaOriginal[6];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[6].localScale = new Vector3(capsulasEscalaOriginal[6].x, capsulasEscalaOriginal[6].y * 2, capsulasEscalaOriginal[6].z);
                    }
                    break;
                default:
                    break;
            }
        }
        //Si el control que se está revisando es el derecho
        else if (sensor == 1)
        {
            switch (report.button)
            {
                //Boton del medio
                case 7:
                    if (report.state == 0)
                    {
                        capsulas[0].localScale = capsulasEscalaOriginal[0];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[0].localScale = new Vector3(capsulasEscalaOriginal[0].x, capsulasEscalaOriginal[0].y * 2, capsulasEscalaOriginal[0].z);
                    }
                    break;
                //Boton 1
                case 8:
                    if (report.state == 0)
                    {
                        capsulas[1].localScale = capsulasEscalaOriginal[1];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[1].localScale = new Vector3(capsulasEscalaOriginal[1].x, capsulasEscalaOriginal[1].y * 2, capsulasEscalaOriginal[1].z);
                    }
                    break;
                //Boton 2
                case 9:
                    if (report.state == 0)
                    {
                        capsulas[2].localScale = capsulasEscalaOriginal[2];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[2].localScale = new Vector3(capsulasEscalaOriginal[2].x, capsulasEscalaOriginal[2].y * 2, capsulasEscalaOriginal[2].z);
                    }
                    break;
                //Boton 3
                case 10:
                    if (report.state == 0)
                    {
                        capsulas[3].localScale = capsulasEscalaOriginal[3];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[3].localScale = new Vector3(capsulasEscalaOriginal[3].x, capsulasEscalaOriginal[3].y * 2, capsulasEscalaOriginal[3].z);
                    }
                    break;
                //Boton 4
                case 11:
                    if (report.state == 0)
                    {
                        capsulas[4].localScale = capsulasEscalaOriginal[4];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[4].localScale = new Vector3(capsulasEscalaOriginal[4].x, capsulasEscalaOriginal[4].y * 2, capsulasEscalaOriginal[4].z);
                    }
                    break;
                //Bumper
                case 12:
                    if (report.state == 0)
                    {
                        capsulas[5].localScale = capsulasEscalaOriginal[5];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[5].localScale = new Vector3(capsulasEscalaOriginal[5].x, capsulasEscalaOriginal[5].y * 2, capsulasEscalaOriginal[5].z);
                    }
                    break;
                //Joystick
                case 13:
                    if (report.state == 0)
                    {
                        capsulas[6].localScale = capsulasEscalaOriginal[6];
                    }
                    else if (report.state == 1)
                    {
                        capsulas[6].localScale = new Vector3(capsulasEscalaOriginal[6].x, capsulasEscalaOriginal[6].y * 2, capsulasEscalaOriginal[6].z);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    //Método que se encarga de estar pendiente de los mensajes que se envían desde los sensores análogos del dispositivo
    void CambioEnUnAnalogo(string name, VRPNAnalog.AnalogReport report)
    {
        //Info para el Log
        if (estaMostrandoInfoAnalogo)
        {
            string text;
            text = "Name: " + name;
            for (int i = 0; i < report.num_channel; i++)
            {
                text = text + " Channel " + i + ": " + report.channel[i];
            }
            Debug.Log(text);
        }

        //Si el control que se está revisando es el izquierdo
        if (sensor == 0)
        {
            //Joystick
            capsulas[5].localScale = new Vector3(capsulasEscalaOriginal[5].x * (1 + (float)report.channel[0]), capsulasEscalaOriginal[5].y, capsulasEscalaOriginal[5].z * (1 + (float)report.channel[1]));
            //Trigger
            capsulas[7].localScale = new Vector3(capsulasEscalaOriginal[7].x, capsulasEscalaOriginal[7].y * (1 + (float)report.channel[2]), capsulasEscalaOriginal[7].z);
        }
        //Si el control que se está revisando es el derecho
        else if (sensor == 1)
        {
            //Joystick
            capsulas[5].localScale = new Vector3(capsulasEscalaOriginal[5].x * (1 + (float)report.channel[3]), capsulasEscalaOriginal[5].y, capsulasEscalaOriginal[5].z * (1 + (float)report.channel[4]));
            //Trigger
            capsulas[7].localScale = new Vector3(capsulasEscalaOriginal[7].x, capsulasEscalaOriginal[7].y * (1 + (float)report.channel[5]), capsulasEscalaOriginal[7].z);
        }
    }

    //Método que se encarga de estar pendiente de los mensajes (posición y orientación) que se envían desde el tracker del dispositivo
    void CambioEnUnTracker(string name, VRPNTracker.TrackerReport report)
    {
        //Info para el Log
        if (estaMostrandoInfoTracker)
        {
            string text;
            text = "Name: " + name + " Sensor: " + report.sensor;
            for (int i = 0; i < report.pos.Length; i++)
            {
                text = text + " Pos " + i + ": " + report.pos[i];
            }
            for (int i = 0; i < report.quat.Length; i++)
            {
                text = text + " Quat " + i + ": " + report.quat[i];
            }
            Debug.Log(text);
        }

        //Si el control que se está revisando es el izquierdo
        if (sensor == 0 && report.sensor == 0)
        {
            //Posición: Notar que 'y' y 'z' se encuentran intercambiados
            this.transform.position = new Vector3((float)report.pos[0] * 10, (float)report.pos[2] * 10, (float)report.pos[1] * 10);
            //Rotación: Notar que es un cuaternión y no ángulos de euler, y que nuevamente 'y' y 'z' se encuentran intercambiados, además que 'x', 'y', y 'z' se encuentran invertidos
            this.transform.localRotation = new Quaternion(-1 * (float)report.quat[0], -1 * (float)report.quat[2], -1 * (float)report.quat[1], (float)report.quat[3]);
        }
        //Si el control que se está revisando es el derecho
        else if (sensor == 1 && report.sensor == 1)
        {
            //Posición: Notar que 'y' y 'z' se encuentran intercambiados
            this.transform.position = new Vector3((float)report.pos[0] * 10, (float)report.pos[2] * 10, (float)report.pos[1] * 10);
            //Rotación: Notar que es un cuaternión y no ángulos de euler, y que nuevamente 'y' y 'z' se encuentran intercambiados, además que 'x', 'y', y 'z' se encuentran invertidos
            this.transform.localRotation = new Quaternion(-1 * (float)report.quat[0], -1 * (float)report.quat[2], -1 * (float)report.quat[1], (float)report.quat[3]);
        }
    }
}
