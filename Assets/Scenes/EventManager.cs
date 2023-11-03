using Assets.Scenes.Afx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A central hub for defining and interfacing with events
/// </summary>
public class EventManager : MonoBehaviour, IAfxInteractionManager
{
    static string GameName = "AfxSample";
    static string Version = "1.0";
    public static event Action DropBox;
    public static event Action<string> EmitEvent;

    private CancellationTokenSource CancellationToken = new CancellationTokenSource();

    public Afx AfxClient = null;


    protected static AfxEffect DropBoxEffect = new AfxEffect() { game_fx_id="drop_box", name="Drop Box", description="Drops a box in the game.", game_name = GameName, game_version = Version };
    protected static AfxEvent SwitchActivatedEvent = new AfxEvent() { game_event_id = "switch_activated", name = "Switch Activated", description = "Triggered when the switch is activated in the game.", game_name=GameName, game_version=Version };
    // Start is called before the first frame update

    EventManager()
    {


    }
    
    void Start()
    {
        if (AfxClient != null)
        {

            AfxClient.isActive = true;
            AfxClient.events.Add(SwitchActivatedEvent);
            AfxClient.effects.Add(DropBoxEffect);
            AfxClient.PlayFx += PlayFx;
            EventManager.EmitEvent += AfxClient.EmitEvent;
            AfxClient.SetupSocketIO();
            AfxClient.ConnectSocketIO();
            AfxClient.RegisterWithAfx();

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("button down");
            //if(ExampleEvent != null)
            //{
            //    ExampleEvent();
            //}
            PlayFx(DropBoxEffect.game_fx_id);
        }
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-7.0
    /// </summary>
    /// <returns></returns>
    private Task InvokeDropBoxAsync()
    {
       
        Task t1 = new Task(() => {
            DropBox?.Invoke(); 
        }, this.CancellationToken.Token);
        t1.Start();
        return t1;
    }

    public static void EmitSwitchActivated()
    {
        //EventManager can apply async requirements
        //this.AfxClient?.SentEventAsync(SwitchActivatedEvent);
        EmitEvent?.Invoke(EventManager.SwitchActivatedEvent.game_event_id);
    }

    public void PlayFx(string afxEffectId)
    {
        //determine the effect to play
        if(afxEffectId == DropBoxEffect.game_fx_id)
        {
            //should be quick returning to ensure good Twitch viewer experience.
            //approx 10 seconds or less for safety.
            //Otherwise a "paided for" trigger of this effect may look like it timed out.
            DropBox?.Invoke();

            //todo: will not work, will throw an error about needing to be on main thread for transforms.
            //todo: support early return to afx.
            //may need ensure afx client is running io on a different thread.
            //https://stackoverflow.com/questions/15522900/how-to-safely-call-an-async-method-in-c-sharp-without-await
            //InvokeDropBoxAsync().
            //ContinueWith(t => Console.WriteLine(t.Exception),
            //                 TaskContinuationOptions.OnlyOnFaulted);

            return;
        }

        //todo: make this a specific exception type
        throw new Exception($"unknown effect id. Id={afxEffectId}");
       
    }

}
