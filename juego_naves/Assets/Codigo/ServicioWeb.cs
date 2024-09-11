using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServicioWeb : MonoBehaviour
{
    public RespuestaRegistro RespuestaRegistro;
    // Start is called before the first frame update
    void Start()
    {
        //variable usuario y es local
        Usuario usuario = new Usuario();
        Usuario.cedula = "1000593544";
        Usuario.nombre = "valentina";
        Usuario.email = "valen.2802@gmail.com";
        //startCoroutine que se quede pensando hasta que me de una respuesta
        startCoroutine(RegistrarUsuario(usuario))
    }
    
    public IEnumerator RegistrarUsuario(Usuario datosResgistro)
    {
        // clase de unity para un objeto volverlo Json y viceversa
        var registroJSON = JsonUtility.ToJson(datosResgistro);
        //librerias para usar http
        var solicitud = new UnityWebRequest();
        solicitud.url="http://localhost:3000/api/jugador/registrar";
        //codifico en bytes y se decodifica en la web
        //maneje toda la informacion cruda para que suba a la web
        byte() bodyRaw = System.Text.Encoding.UTF8.GetBytes(registroJSON);
        solicitud.uploadHandler = new uploadHandlerRaw(bodyRaw);
        solicitud.downloadHandler = new downloadHandler();
        solicitud.method = UnityWebRequest.KHttpVerbPOST;
        solicitud.SetRequestHeader("Content-Type", "application/json");
        
        solicitud.timeout = 10; //10 seg de espera, para decir que esto no funciona

        yield return solicitud.SendWebRequest();

        if (solicitud.result == UnityWebRequest.Result.ConnectionError)
        {
            respuestaRegistro.mensaje = "Conexion fallida";
        }
        else
        {           
            respuestaRegistro = (RespuestaRegistro)JsonUtility.FromJson(solicitud.downloadHandler.text, typeof(RespuestaRegistro));
        }
        solicitud.Dispose();//desecha los archivos basura que se fueron creando
    }

    
}
