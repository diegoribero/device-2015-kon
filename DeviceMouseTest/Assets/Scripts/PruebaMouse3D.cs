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
 * PruebaMouse3D.cs
 *
 * usage: Se le debe agregar a representación de control - Se encuentra de forma predeterminada en el prefab 3DConnexionSpaceNavigator
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;

public class PruebaMouse3D : MonoBehaviour {

    //Propiedades públicas (Para poder cambiar desde el editor)
    public bool estaMostrandoInfoBoton = false;
    public bool estaMostrandoInfoAnalogo = false;
    public GameObject cubo;

    //Propiedades privadas
    private Color colorOriginal;

    // Inicialización
    void Start()
    {
        //Se agrega un método por cada tipo de dispositivo para que esté pendiente de los mensajes que se envían desde este
        VRPNEventManager.StartListeningButton(VRPNManager.Button_Types.vrpn_3DConnexion_Navigator, VRPNDeviceConfig.Device_Names.device0, CambioEnUnBoton);
        VRPNEventManager.StartListeningAnalog(VRPNManager.Analog_Types.vrpn_3DConnexion_Navigator, VRPNDeviceConfig.Device_Names.device0, CambioEnUnAnalogo);
        
        colorOriginal = cubo.GetComponent<MeshRenderer>().material.color;
    }

    //Método que se encarga de estar pendiente de los mensajes que se envían desde los botones del dispositivo
    void CambioEnUnBoton(string name, VRPNButton.ButtonReport report)
    {
        //Info para el Log
        if (estaMostrandoInfoBoton)
        {
            Debug.Log("Name: " + name + " Button: " + report.button + " State:" + report.state);
        }

        //Si el botón del que se recibe reporte es el izquierdo
        if(report.button == 0)
        {
            //Si el reporte es que se oprimió el botón
            if (report.state == 1)
            {
                cubo.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            //Si el reporte es que se soltó el botón
            else
            {
                cubo.GetComponent<MeshRenderer>().material.color = colorOriginal;
            }
        }
        //Si el botón del que se recibe reporte es el derecho
        else if (report.button == 1)
        {
            //Si el reporte es que se oprimió el botón
            if (report.state == 1)
            {
                cubo.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            //Si el reporte es que se soltó el botón
            else
            {
                cubo.GetComponent<MeshRenderer>().material.color = colorOriginal;
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

        //Posición: Notar que 'y' y 'z' se encuentran intercambiados e invertidos
        cubo.transform.position = new Vector3((float)report.channel[0] * 1f, (float)report.channel[2] * -1f, (float)report.channel[1] * -1f);
        //Rotación: Notar que 'y' y 'z' se encuentran intercambiados, y que 'x' se encuentra invertido
        //En este caso se pueden usar las entradas como ángulos de euler y convertirlos en cuaternión
        cubo.transform.localRotation = Quaternion.Euler((float)report.channel[3] * -45f, (float)report.channel[5] * 45f, (float)report.channel[4] * 45f);
    }
}
