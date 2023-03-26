using UnityEngine;

public class TileSlot : MonoBehaviour
{
	[Tooltip("For debug purposes in the inspector. Dictionary key drives coordinate truth.")]
	public Vector2 Coordinates;
	public TileItem TileItem;
}