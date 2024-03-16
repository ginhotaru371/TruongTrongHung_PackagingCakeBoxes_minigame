using System;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Vector2 Pos => gameObject.GetComponent<RectTransform>().anchoredPosition;

    public BlockType Type;
    public Frame Frame;
    public Block MergingBlock;
    [SerializeField] private Sprite cake, box, candy;

    public void SetBlockDetail(BlockType type)
    {
        switch (type)
        {
            case BlockType.Cake:
                transform.GetChild(0).GetComponent<Image>().sprite = cake;
                Type = type;
                break;
            case BlockType.Box:
                transform.GetChild(0).GetComponent<Image>().sprite = box;
                Type = type;
                break;
            case BlockType.Candy:
                transform.GetChild(0).GetComponent<Image>().sprite = candy;
                Type = type;
                break;
            case BlockType.Coin:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void SetBlockSlot(Frame frame)
    {
        if (Frame != null) Frame.occupiedBlock = null;
        Frame = frame;
        Frame.occupiedBlock = this;
    }

    public void ClearFrame()
    {
        Frame.occupiedBlock = null;
    }
    
    public void MergeBlock(Block blockToMergeWith) {
        
        MergingBlock = blockToMergeWith;
        Frame.occupiedBlock = null;
    }
}
