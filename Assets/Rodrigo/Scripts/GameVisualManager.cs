using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : NetworkBehaviour
{   
    private const float GRID_SIZE = 3.1f;

    [SerializeField] private Transform crossPrefab;
    [SerializeField] private Transform circlePrefab;

    private Transform _localPrefab;

    private void Start()
    {
        GameManager.Instance.OnClikedOnGridPosition += GameManager_OnClickedOnGridPosition;
    }

    private void GameManager_OnClickedOnGridPosition(object sender, OnClikedOnGridPositionEventArgs e)
    {
        Debug.Log($"Visuals were called by position {e.x}, {e.y}");

        SpawnObjectRpc(e.x, e.y, e.type);
    }

    [Rpc(SendTo.Server)]
    private void SpawnObjectRpc(int _x, int _y, PlayerType _type)
    {   
        Debug.Log("Spwan Object");

        Vector2 pos = GetGridWorldPosition(_x, _y);

        if (!TryDecideLocalPrefab(_type))
        {
            Debug.LogError("Prefab could not be resolved.");
            return;
        }

        Transform instantietedPrefab = Instantiate(_localPrefab, pos, Quaternion.identity);
        instantietedPrefab.GetComponent<NetworkObject>().Spawn();
    }

    private Vector2 GetGridWorldPosition(int _x, int _y)
    {
        Vector2 pos = new Vector2(-GRID_SIZE + _x * GRID_SIZE, -GRID_SIZE + _y * GRID_SIZE);
        return pos;
    }

    private bool TryDecideLocalPrefab(PlayerType _type)
    {
        switch (_type)
        {
            case PlayerType.Cross:
                _localPrefab = crossPrefab;
                return true;
            case PlayerType.Circle:
                _localPrefab = circlePrefab;
                return true;
            case PlayerType.None:
                return false;
            default:
                return false;
        }
    }
}
