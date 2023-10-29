using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using Assets.Scenes.Afx;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.PackageManager.Requests;


class SequenceGameFxItem
{
    public string type;
    public string fxId;
    public string gameFxId;
}
class Sequence
{
    public string type;
    public SequenceGameFxItem[] items;
}

//[{\"version\":\"1.0\",\"player\":\"game\",\"sequence\":{\"type\":\"SEQUENCE\",\"items\":[{\"fxId\":\"8168c6cf-4b5e-4bb1-9823-1ec837d00b1d\",\"gameFxId\":\"drop_box\"}]}},\"19594700-a96f-436f-be71-e5dbfbe79aac\",0]"

class AfxScript
{
    public string version;
    public string? player;
    //assets?: Record<string, IAssetItem>;
    public Sequence sequence;
    //presentedBy?: IPresentedByData;
}

class AfxWebSocketRequest
{
    public static int AfxIndex = 0;
    public static int FxReqIdIndex = 1;
    public static int numIndex = 2;
    public System.Object[] data; 
}

internal class IdOnly
{
    public string id { get; set; }
}


/// <summary>
/// websocket communications
/// see https://github.com/bitgenetics/SocketIOUnity.git
/// </summary>
public class Afx : MonoBehaviour
{
    public string apiUrl = "http://localhost:3000";
    public string websocketUrl = "wss://localhost:3000";


    public IAfxInteractionManager interactionManager;
    public string ApiKey = "";
    public string GameFxChannelKey = "";
    protected SocketIOUnity socket;
    public event Action<string> PlayFx;

    public List<AfxEffect> effects = new List<AfxEffect>();
    public List<AfxEvent> events = new List<AfxEvent>();

    void Start()
    {
        var uri = new Uri(websocketUrl);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Auth = new Dictionary<string, string>
            {
                {"token", GameFxChannelKey }
            }
            ,Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
 
        });


        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };

        socket.OnUnityThread($"afx-{this.GameFxChannelKey}", request =>
        {

            //todo: support parameters
            var obj = JsonConvert.DeserializeObject<List<System.Object>>(request.ToString());
            //var obj = request.GetValue<AfxWebSocketRequest>();

            string fxRequestId = obj[AfxWebSocketRequest.FxReqIdIndex].ToString();
            AfxScript afxScript = JsonConvert.DeserializeObject<AfxScript>(obj[AfxWebSocketRequest.AfxIndex].ToString());
            string channelCB = $"afx-{this.GameFxChannelKey}-{fxRequestId}";
            System.Object[] data = { new AfxEffectResponse { id = fxRequestId, success = true, type= "PlayRequestResponse" }}; 

            if (obj != null) PlayFx?.Invoke(afxScript.sequence.items[0].gameFxId);
            //socket.EmitAsync(channelCB, data).Wait(10000);
            socket.Emit(channelCB, data);



            Debug.Log("Event" + request.ToString());
            //Debug.Log(request.GetValue<string>());
        });

       socket.Connect();

    }


    private static bool TrustCertificate(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
    {
        // all certificates are accepted
        return true;
    }
    void Update()
    {
     
    }

    void OnDestroy()
    {
        if (socket != null)
        {
            socket.Dispose();
        }
    }


    public void RegisterWithAfx()
    {
        foreach(var effect in effects)
        {
            RegisterEffect(effect);
        }

        foreach(var evt in events)
        {
            RegisterEvent(evt);
        }

        return;
    }

    //not yet supported through websocket
    //public Task SendEventAsync(string id)
    //{
    //    var obj = JsonConvert.SerializeObject(new IdOnly() { id = id });
    //    return socket.EmitAsync($"event-{this.GameFxChannelKey}", obj);
    //}

    public void EmitEvent(string afxEventId)
    {
        StartCoroutine(EmitGameEvent(afxEventId));
    }

    public void RegisterEvent(AfxEvent evt)
    {
        StartCoroutine(UploadEventRegistration(evt));
    }

    public void RegisterEffect(AfxEffect fx) {
        StartCoroutine(UploadEffectRegistration(fx));
    }

    IEnumerator UploadEventRegistration(AfxEvent afxEvent)
    {

        var obj = JsonConvert.SerializeObject(afxEvent);
        
        using (UnityWebRequest www = UnityWebRequest.Post(this.apiUrl+"/api/game/event", obj, "application/json"))
        {
            www.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            //www.certificateHandler = null;

            www.SetRequestHeader("x-api-key", this.ApiKey);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("AfxEvent upload complete!");
            }
        }
    }

    IEnumerator UploadEffectRegistration(AfxEffect afxEffect)
    {
        var obj = JsonConvert.SerializeObject(afxEffect);
        using (UnityWebRequest www = UnityWebRequest.Post(this.apiUrl+"/api/game/effect", obj, "application/json"))
        {
            www.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            //www.certificateHandler = null;

            www.SetRequestHeader("x-api-key", this.ApiKey);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("AfxEffect upload complete!");
            }
        }
    }

    IEnumerator EmitGameEvent(string afxEventId)
    {
      
        using (UnityWebRequest www = UnityWebRequest.Post(this.apiUrl + "/api/game/event/"+ afxEventId, "","application/json"))
        {
            www.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            //www.certificateHandler = null;

            www.SetRequestHeader("x-api-key", this.ApiKey);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("AfxEvent emitted");
            }
        }
    }

}