using System;
using System.Collections.Generic;

[Serializable]

public class Level
{
    public int index;
    public bool stat;
    public int stars;
    public int w;
    public int h;
    public List<CharacterPos> cakePos;
    public CharacterPos boxPos;
    public List<CharacterPos> blockPos;
    public List<CharacterPos> coinPos;
    

    public Level(int index, bool stat, int star, int w, int h, List<CharacterPos> cake, CharacterPos box,
        List<CharacterPos> block, List<CharacterPos> coin)
    {
        this.index = index;
        this.stat = stat;
        stars = star;
        this.w = w;
        this.h = h;
        cakePos = cake;
        boxPos = box;
        blockPos = block;
        coinPos = coin;
    }
    
    public void Update(bool stat = false, int star = 0)
    {
        this.stat = stat;
        stars = star;
    }
    
}
