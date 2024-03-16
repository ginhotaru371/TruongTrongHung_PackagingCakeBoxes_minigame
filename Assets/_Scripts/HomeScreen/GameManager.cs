using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Transform _gamePlaySize;
    [SerializeField] private Frame _framePrefab;

    [SerializeField] private Block _blockPrefab;
    [SerializeField] private float _speed = 0.4f;
    private Level _currentLevel;
    private Rect _rect;
    
    [SerializeField] private List<Frame> _frames = new List<Frame>();
    [SerializeField] private List<Block> _blocks = new List<Block>();
    private GameState _state;

    private void Awake()
    {
        if (instance == null) instance = this;
        _rect = _framePrefab.gameObject.GetComponent<RectTransform>().rect;
    }

    private void Update()
    {
        if (_state != GameState.WaitingInput) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) Move(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Move(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Move(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Move(Vector2.down);
    }

    public void Restart()
    {
        GenerateGrid(_currentLevel);
    }

    public void UnlockLevel()
    {
        var nextLevel = LevelManager.instance.LevelList[_currentLevel.index];
        if (nextLevel.stat) return;

        nextLevel.Update(true);
    }

    public void NextLevel()
    {
        var nextLevel = LevelManager.instance.LevelList[_currentLevel.index];
        GenerateGrid(nextLevel);
    }

    public void ChangeState(GameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case GameState.Generate:
                
                break;
            case GameState.SpawningCharacter:
                
                Spawning(_currentLevel);
                CountDownTimer.instance.StartCountDownTimer();
                break;
            case GameState.WaitingInput:
                
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                var stars = WinUI.instance.OnLevelCompleted(CountDownTimer.instance.CheckTimeLeft());
                _currentLevel.Update(true ,stars);
                UnlockLevel();
                LevelManager.instance.SaveUserLevel();
                break;
            case GameState.Lose:
                LoseUI.instance.Show();
                LevelManager.instance.SaveUserLevel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void GenerateGrid(Level currentLevel)
    {
        if (!currentLevel.stat) return;
        
        _currentLevel = currentLevel;

        Despawn();

        var width = currentLevel.w;
        var height = currentLevel.h;
        _gamePlaySize.GetComponent<RectTransform>().sizeDelta = new Vector2(width * _rect.width, height * _rect.height);
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var frame = Instantiate(_framePrefab, _gamePlaySize);
                frame.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * _rect.width, -(y * _rect.height));
                _frames.Add(frame);
            }
        }
        
        ChangeState(GameState.SpawningCharacter);
    }

    private void Spawning(Level currentLevel)
    {
        foreach (var cake in currentLevel.cakePos)
        {
            SpawnBlock(cake, BlockType.Cake);
        }
        
        foreach (var candy in currentLevel.blockPos)
        {
            SpawnBlock(candy, BlockType.Candy);
        }
        
        SpawnBlock(currentLevel.boxPos, BlockType.Box);
        
        ChangeState(GameState.WaitingInput);
    }

    private void SpawnBlock(CharacterPos pos, BlockType type)
    {
        var newBlock = Instantiate(_blockPrefab, _gamePlaySize);
        newBlock.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x * _rect.width, -(pos.y * _rect.height));
        newBlock.SetBlockDetail(type);
        foreach (var frame in _frames)
        {
            if (frame.Pos == newBlock.Pos)
            {
                newBlock.SetBlockSlot(frame);
            }
        }
        
        _blocks.Add(newBlock);
    }

    private void Move(Vector2 dir)
    {
        ChangeState(GameState.Moving);
        
        var orderedBlocks = _blocks.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();

        foreach (var block in orderedBlocks) {

            if (block.Type != BlockType.Candy)
            {
                var next = block.Frame;
                do {
                    block.SetBlockSlot(next);
                
                    var possibleFrame = GetFrameAtPosition(next.Pos + (dir * 150));
                    if (possibleFrame != null) {
                    
                        if (possibleFrame.occupiedBlock != null
                            && possibleFrame.occupiedBlock.Type == BlockType.Box && block.Type == BlockType.Cake && dir == Vector2.down)
                        {
                            possibleFrame.occupiedBlock.MergeBlock(block);
                            possibleFrame.occupiedBlock.SetBlockSlot(possibleFrame);
                        }
                    
                        if (possibleFrame.occupiedBlock != null
                            && possibleFrame.occupiedBlock.Type == BlockType.Cake && block.Type == BlockType.Box && dir == Vector2.up)
                        {
                            block.MergeBlock(possibleFrame.occupiedBlock);
                            block.SetBlockSlot(possibleFrame);
                        }
                    
                        if (possibleFrame.occupiedBlock == null)
                        {
                            next = possibleFrame;
                        }
                    }

                } while (next != block.Frame);
                
            }
            
        }
        
        var sequence = DOTween.Sequence();
        
        foreach (var block in orderedBlocks) {
        
            if (block.Type != BlockType.Candy)
            {
                var movePoint = block.MergingBlock != null ? block.MergingBlock.Frame.Pos : block.Frame.Pos;
        
                sequence.Insert(0, block.GetComponent<RectTransform>().DOAnchorPos(movePoint, _speed).SetEase(Ease.InQuad));
            }
        }
        
        sequence.OnComplete(() => {
            var mergeBlocks = orderedBlocks.Where(b => b.MergingBlock != null).ToList();
            foreach (var block in mergeBlocks) {
                RemoveBlock(block.MergingBlock);
            }
            CheckGameWin();
        });
    }

    void RemoveBlock(Block block) {
        _blocks.Remove(block);
        Destroy(block.gameObject);
    }

    private void CheckGameWin()
    {
        var anyCake = _blocks.Any(c => c.Type == BlockType.Cake);
        
        ChangeState(anyCake ? GameState.WaitingInput : GameState.Win);
    }
    
    private Frame GetFrameAtPosition(Vector2 pos) {
        return _frames.FirstOrDefault(f => f.Pos == pos);
    }

    public void Despawn()
    {
        if (_gamePlaySize.childCount == 0) return;
        foreach (Transform child in _gamePlaySize)
        {
            Destroy(child.gameObject);
        }
        _frames.Clear();
        _blocks.Clear();
    }
}
