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
 * PruebaMouse.cs
 *
 * usage: Se le debe agregar a representación de control - Se encuentra de forma predeterminada en el prefab Mouse
 * 
 * inputs:
 *
 * Notes:
 *
 * ========================================================================*/

using UnityEngine;

public class PruebaMouse : MonoBehaviour {

    // Inicialización
    void Start()
    {
        //Se agrega un método por cada tipo de dispositivo para que esté pendiente de los mensajes que se envían desde este
        VRPNEventManager.StartListeningAnalog(VRPNManager.Analog_Types.vrpn_Mouse, VRPNDeviceConfig.Device_Names.Mouse0, moverConRaton);
    }

    //Método que se encarga de estar pendiente de los mensajes que se envían desde los sensores análogos del dispositivo
    void moverConRaton(string name, VRPNAnalog.AnalogReport report)
    {
        //Se debe notar que los valores que se reportan son de 0 a 1, siendo (0, 0) la esquina superior izquierda de la pantalla y (1, 1) la esquina inferior derecha de la pantalla
        this.transform.position = new Vector3((float)report.channel[0]*10 - 5, (1-(float)report.channel[1])*10 - 5, this.transform.position.z);
    }
}
