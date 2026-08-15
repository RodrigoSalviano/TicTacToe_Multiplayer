using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    
    //Singleton
    public static GameManager Instance{get; private set;}

    //Events
    public event EventHandler<OnClikedOnGridPositionEventArgs> OnClikedOnGridPosition;
    public event EventHandler OnGameStarted;
    public event EventHandler OnCurrentPlayerTypeChanged;

    private PlayerType _localPlayerType;
    private NetworkVariable<PlayerType> _currentPlayerType = new NetworkVariable<PlayerType>(
        PlayerType.None, 
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    #region Properties
    public PlayerType LocalPlayerType => _localPlayerType;
    public PlayerType CurrentPlayerType => _currentPlayerType.Value;
    #endregion

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

    public override void OnNetworkSpawn()
    {
        ulong localID = NetworkManager.Singleton.LocalClientId;
        Debug.Log("Local Cliente ID: " + localID);

        if(localID == 0)
        {
            _localPlayerType = PlayerType.Cross;
        }
        else
        {
            _localPlayerType = PlayerType.Circle;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallBack;
        }
        _currentPlayerType.OnValueChanged += OnNetworkCurrentPlayerTypeChanged;
    }

    private void OnNetworkCurrentPlayerTypeChanged(PlayerType oldValue, PlayerType newValue)
    {
        OnCurrentPlayerTypeChanged?.Invoke(this, EventArgs.Empty);
        
    }

    private void NetworkManager_OnClientConnectedCallBack(ulong obj)
    {
        if(NetworkManager.Singleton.ConnectedClientsList.Count == 1)
        {
            _currentPlayerType.Value = PlayerType.Cross;
            TriggerGameStartRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerGameStartRpc()
    {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    [Rpc(SendTo.Server)]
    public void ClickedOnGridPositionRpc(int x, int y, PlayerType _type)
    {
        //Debug.Log($"click! IN POSITION: {x}, {y}");

        if(_type != _currentPlayerType.Value)
        {
            return;
        }

        OnClikedOnGridPosition?.Invoke(this, new OnClikedOnGridPositionEventArgs{x = x, y = y, type = _type});

        ChangeCurrentPlayer(_type);
    }

    private void ChangeCurrentPlayer(PlayerType type)
    {
        if(type == PlayerType.Cross)
        {
            _currentPlayerType.Value = PlayerType.Circle;
        }
        else if(type == PlayerType.Circle)
        {
            _currentPlayerType.Value = PlayerType.Cross;
        }

    }
}

public enum PlayerType
    {
        None,
        Cross,
        Circle
    }

public class OnClikedOnGridPositionEventArgs : EventArgs
{
    public int x;
    public int y;
    public PlayerType type;
}