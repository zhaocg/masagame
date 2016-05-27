using UnityEngine;
using System.Collections;



/// <summary>
/// 将该脚本绑定在发射点上,在PlayerSpells脚本中，拥有对本脚本的引用
/// </summary>
public class SpellsCaster : MonoBehaviour {

    public Spells spell { get; private set; }

    private bool hasSpell = false;

    /// <summary>
    /// 设置发射点的法术
    /// </summary>
    /// <param name="sp"></param>
    public void SetSpell(Spells sp)
    {
        this.spell = sp;
        hasSpell = true;
    }

    /// <summary>
    /// 清除发射点的法术
    /// </summary>
    public void ClearSpell()
    {
        this.spell = null;
        hasSpell = false;
    }

    /// <summary>
    /// 释放法术
    /// </summary>
    public void Cast()
    {
        if(hasSpell)
            this.spell.Cast(this.transform);
    }
}
