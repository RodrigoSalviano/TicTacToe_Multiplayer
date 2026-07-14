using System;
using UnityEngine;

public class EventSubscribeTest : MonoBehaviour
{
    private EventTestSender eventTestSender;

    private void Start()
    {
        eventTestSender = GetComponent<EventTestSender>();

        eventTestSender.OnEventHandlerTest += EventHandlerMethodTest;
        eventTestSender.OnTestDelegate += DelegateTestMethod;
    }

    private void EventHandlerMethodTest(object sender, OnEventHandlerTestEventArgs e)
    {
        Debug.Log(e.message);
    }

    private void DelegateTestMethod(float x)
    {
        Debug.Log(x);
    }

    public void UnityEventTestMethod(float x)
    {
        Debug.Log(x);
    }
}