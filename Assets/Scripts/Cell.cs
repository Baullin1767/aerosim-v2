using UnityEngine;

public struct Cell
{
	public readonly Transform Transform;
	public readonly Vector2 Position;
	private int _lod;
	private readonly int _lods;

	public Cell(Transform transform, Vector2 position, int lod, int lods)
	{
		Transform = transform;
		Position = position;
		_lod = lod;
		_lods = lods;
		SetLod(lod);
	}
	
	public void TrySetLod(int lod)
	{
		if (_lod != lod)
			SetLod(lod);
	}
	
	private void SetLod(int lod)
	{
		for (int i = 0; i < _lods; i++)
		{
			Transform.GetChild(i).gameObject.SetActive(i == lod);
		}

		_lod = lod;
	}
}