using UnityEngine;

public enum TileType
{
	Blue,
	Red,
	Green,
	Yellow,
	Black,
}

public class TileItem : MonoBehaviour
{
	public TileType TileType;
	public Animator Animator;

	public TileSlot ParentSlot { get; private set; }

	public delegate void OnTilePopped(Vector2 selectedTileSlot);
	public static event OnTilePopped RegisterTileToPop;

	public void SetSlot(TileSlot slotToLink)
	{
		ParentSlot = slotToLink;
	}

	public void OnClickItem()
	{
		RegisterTileToPop?.Invoke(ParentSlot.Coordinates);
	}

	public void PopTile()
	{
		//Animator.Play("SmokePuff", 0);
	}
}