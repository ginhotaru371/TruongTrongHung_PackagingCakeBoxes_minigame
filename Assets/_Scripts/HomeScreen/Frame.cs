using UnityEngine;

public class Frame : MonoBehaviour
{
    public Vector2 Pos => gameObject.GetComponent<RectTransform>().anchoredPosition;

    public Block occupiedBlock;
}
