using UnityEngine;
using System.Collections;

public class MenuPrincipal : MonoBehaviour {

    // Use this for initialization
    public void Empezar () {
        Application.LoadLevel(1);
           }

    // Update is called once per frame
    public void NuevoJuego () {
	
	}
    public void Opciones()
    {

    }
    public void Salir()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
