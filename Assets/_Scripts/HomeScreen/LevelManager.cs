using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private List<Level> levelList;

    public List<Level> LevelList => levelList;

    [SerializeField] private Transform contentContainer;
    [SerializeField] private Transform prefab;

    private void Awake()
    {
        if (instance == null) instance = this;
        LoadUserLevel();
    }

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Despawn();
        foreach (var level in levelList)
        {
            var newLevel = Instantiate(prefab, contentContainer).GetComponent<LevelUI>();

            newLevel.SetLevelDetail(level);

            newLevel.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public void Despawn()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateLevel(int index, bool stat, int star)
    {
        LevelList[index - 1].Update(stat, star);
    }

    public void SaveUserLevel()
    {
        var userLevels = LevelList.Where(x => x.stat == true).ToList();
        MySaveGame.instance.LevelSave(userLevels);
    }

    public void LoadUserLevel()
    {
        var userLevels = MySaveGame.instance.LevelLoad();
        foreach (var userLevel in userLevels)
        {
            foreach (var level in LevelList.Where(level => level.index == userLevel.index))
            {
                level.Update(userLevel.stat, userLevel.stars);
            }
        }
    }
}
