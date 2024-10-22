using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//MonoBehaviour es la super clase para usar todo lo de unity
public class ServiciosWeb : MonoBehaviour
{
    // creamos valiable para que nos guarde la solicitud
    public DatosServicioWeb registroUsuario;
    public RespuestaRegistroUsuario respuestaRegistroUsuario;

    // Start is called before the first frame update
    void Start()
    //un metodo no retorna valores
    {
        DatosRegistroUsuario registro = new DatosRegistroUsuario();
        registro.cedula="1000593543";
        registro.email="valen@gmail.com";
        registro.nombre="pedro";
        StartCoroutine(RegistrarUsuario(registro));
    }
    //IEnumerator: funciones asincronas en unity
    //registrar le entra un variable de tipo registro Usuario
    
    public IEnumerator RegistrarUsuario(DatosRegistroUsuario registro)
    {
        var respuestaJSON = JsonUtility.ToJson(registro);

        var solicitud = new UnityWebRequest();
        solicitud.url = registroUsuario.url;
        
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(respuestaJSON);
        solicitud.uploadHandler = new UploadHandlerRaw(bodyRaw);
        solicitud.downloadHandler = new DownloadHandlerBuffer();
        solicitud.method = UnityWebRequest.kHttpVerbPOST;
        solicitud.SetRequestHeader("Content-Type", "application/json");
        
        solicitud.timeout = 10;

        yield return solicitud.SendWebRequest();

        if (solicitud.result == UnityWebRequest.Result.ConnectionError)
        {
            registroUsuario.mensaje = "Conexion fallida";
        }
        else
        {           
            respuestaRegistroUsuario = (RespuestaRegistroUsuario)JsonUtility.FromJson(solicitud.downloadHandler.text, typeof(RespuestaRegistroUsuario));
            registroUsuario.mensaje = respuestaRegistroUsuario.mensaje;
        }
        solicitud.Dispose();
        registroUsuario.evento.Invoke();
    }
}