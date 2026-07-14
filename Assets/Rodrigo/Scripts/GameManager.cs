using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance{get; private set;}

    //Events
    public event EventHandler<OnClikedOnGridPositionEventArgs> OnClikedOnGridPosition;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void ClickedOnGridPosition(int x, int y)
    {
        //Debug.Log($"click! IN POSITION: {x}, {y}");
        OnClikedOnGridPosition?.Invoke(this, new OnClikedOnGridPositionEventArgs{x = x, y = y});
    }
}

public class OnClikedOnGridPositionEventArgs : EventArgs
{
    public int x;
    public int y;
}