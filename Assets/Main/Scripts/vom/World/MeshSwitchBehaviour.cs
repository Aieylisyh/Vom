using UnityEngine;
using game;
using com;

public class MeshSwitchBehaviour : MonoBehaviour
{
    private MeshFilter _mf;

    void Start()
    {
        _mf = GetComponent<MeshFilter>();
        _mf.mesh = ConfigService.instance.mapConfig.tiles.GetRandomTileMesh();
    }
}
