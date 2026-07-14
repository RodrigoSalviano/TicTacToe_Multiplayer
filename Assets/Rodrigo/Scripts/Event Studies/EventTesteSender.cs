using System;
using UnityEngine;
using UnityEngine.Events;

public class EventTestSender : MonoBehaviour
{
    public EventHandler<OnEventHandlerTestEventArgs> OnEventHandlerTest;

    public delegate void TestDelegate(float x);

    public event TestDelegate OnTestDelegate;

    public FloatEvent OnUnityEventTest;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnEventHandlerTest?.Invoke(this, new OnEventHandlerTestEventArgs{});
            
            OnTestDelegate?.Invoke(5.5f);

            OnUnityEventTest?.Invoke(3.5f);
        }
    }

}

[Serializable]
public class FloatEvent : UnityEvent<float>{}


public class OnEventHandlerTestEventArgs : EventArgs
{
    public string message = "From the sender";
}