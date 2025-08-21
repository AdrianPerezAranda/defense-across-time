using UnityEngine;

public class TestArbolHabilidades : MonoBehaviour
{
    public ArbolDeHabilidades arbolDeHabilidades;
    public Inventario inventario;   

    public void GuardarProgreso()
    {
        if (arbolDeHabilidades != null)
        {
            arbolDeHabilidades.GuardarProgresoArbolHabilidades();
         
            Debug.Log("Progreso guardado");
        }
        else
        {
            Debug.LogError("No se ha encontrado el Arbol de Habilidades");
        }

        //inventario.GuardarInventario();

    }

    public void CargarProgreso()
    {
        if (arbolDeHabilidades != null)
        {
            arbolDeHabilidades.CargarProgresoArbolHabilidades();
            
            Debug.Log("Progreso cargado");
        }
        else
        {
            Debug.LogError("No se ha encontrado el Arbol de Habilidades");
        }

        //inventario.CargarInventario();
    }




}
