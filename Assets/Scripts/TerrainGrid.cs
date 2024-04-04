using System.Collections.Generic;
using Drone2;
using UI.Settings;
using UnityEngine;

public class TerrainGrid : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _prefabs;

    [SerializeField]
    private Material[] _materials;

    [SerializeField]
    private int _gridSize;

    [SerializeField]
    private float _cellSize;

    [SerializeField]
    private float[] _lodDistance;

    [SerializeField]
    private GameObject _dummy;

    private int Lods => _lodDistance.Length;

    private List<Cell> _cells = new();
    private float[] _lodSqrMagnitudes;
    private Transform _drone;

    private void Awake()
    {
        for (var x = 0; x < _gridSize; x++)
        {
            for (var y = 0; y < _gridSize; y++)
            {
                var cellIndex = x * _gridSize + y;
                var cell = Instantiate(
                    _prefabs[cellIndex],
                    transform.position,
                    Quaternion.identity,
                    transform);
                cell.name = $"{cellIndex}_{x}:{y}";
                SetMaterials(cell.transform);
                //SetupCollider(cell.transform);
                var cellCenterPosition = new Vector2(transform.position.x, transform.position.z)
                                         + new Vector2(_cellSize * x, _cellSize * y)
                                         - Vector2.one * (2500 - _cellSize / 2);
                cellCenterPosition = -cellCenterPosition; // cuz of rotated terrain
                _cells.Add(new Cell(cell.transform, cellCenterPosition, -1, Lods));
            }
        }

        _lodSqrMagnitudes = new float[Lods];
        for (int i = 0; i < Lods; i++)
            _lodSqrMagnitudes[i] = Mathf.Pow(_cellSize * _lodDistance[i], 2);
    }

    private void Start()
    {
        var drone = FindObjectOfType<Drone>();
        if (drone != null) _drone = drone.transform;
        if (_drone == null)
        {
            var control = FindObjectOfType<DroneBridge>();
            if (control != null) _drone = control.transform;
        }
    }

    private void Update()
    {
        var dronePosition = new Vector2(_drone.position.x, _drone.position.z);
        foreach (var cell in _cells)
            cell.TrySetLod(GetLod(cell.Position, dronePosition));
    }

    private void SetMaterials(Transform cell)
    {
        for (int i = 0; i < cell.childCount; i++)
        {
            var meshRenderer = cell.GetChild(i).GetComponent<MeshRenderer>();
            if (!Application.isPlaying && Application.isEditor)
            {
                if (meshRenderer.sharedMaterials.Length > 1)
                    meshRenderer.sharedMaterials = _materials;
                else
                    meshRenderer.sharedMaterial = _materials[0];
            }
            else
            {
                if (meshRenderer.materials.Length > 1)
                    meshRenderer.materials = _materials;
                else
                    meshRenderer.material = _materials[0];
            }
        }
    }

    private void SetupCollider(Transform cell)
    {
        var lastLod = cell.GetChild(Lods - 1).transform;
        var go = new GameObject("collider", typeof(MeshCollider))
        {
            transform =
            {
                parent = cell,
                localPosition = lastLod.localPosition,
                localRotation = lastLod.localRotation,
                localScale = lastLod.localScale
            }
        };
        go.GetComponent<MeshCollider>().sharedMesh = lastLod.GetComponent<MeshFilter>().mesh;
    }

    private int GetLod(Vector2 cellPosition, Vector2 dronePosition)
    {
        var delta = cellPosition - dronePosition;
        var sqrMagnitude = delta.sqrMagnitude;

        if (GraphicsSettings.CameraDistanceSqr + 1 < sqrMagnitude) return -1;

        for (int i = 0; i < Lods; i++)
            if (sqrMagnitude < _lodSqrMagnitudes[i])
                return i;

        return -1;
    }

    public void GenerateCells()
    {
        for (var x = 0; x < _gridSize; x++)
        {
            for (var y = 0; y < _gridSize; y++)
            {
                var cellIndex = x * _gridSize + y;
                var cell = Instantiate(
                    _prefabs[cellIndex],
                    transform.position,
                    Quaternion.identity,
                    transform);
                cell.name = $"{cellIndex}_{x}:{y}";
                SetMaterials(cell.transform);
                //SetupCollider(cell.transform);
                var cellCenterPosition = new Vector2(transform.position.x, transform.position.z)
                                         + new Vector2(_cellSize * x, _cellSize * y)
                                         - Vector2.one * (2500 - _cellSize / 2);
                cellCenterPosition = -cellCenterPosition; // cuz of rotated terrain
                _cells.Add(new Cell(cell.transform, cellCenterPosition, 0, Lods));
            }
        }
    }

    public void RemoveCells()
    {
        foreach (var cell in _cells)
        {
            if (cell.Transform == null || cell.Transform.gameObject == null) continue;
            DestroyImmediate(cell.Transform.gameObject);
        }
        _cells.Clear();
    }
}