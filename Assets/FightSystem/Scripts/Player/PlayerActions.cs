using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
public class PlayerActions : MonoBehaviour {

    public Player player;

    [SerializeField] private VRInput m_vrInput;

    void OnEnable()
    {
        m_vrInput.OnSwipe += Cast;
    }

    void OnDisable()
    {
        m_vrInput.OnSwipe -= Cast;
    }

    void Cast(VRInput.SwipeDirection dir)
    {
        if(player.gameState == PlayerState.SHOOT)
        {
            if (dir == VRInput.SwipeDirection.LEFT)
            {
                //CastRightSpells();
                StartCoroutine(CastLeftCoroutine());
            }
            if (dir == VRInput.SwipeDirection.RIGHT)
            {
                //CastLeftSpells();
                StartCoroutine(CastRightCoroutine());
            }
        }
    }

    /// <summary>
    /// 释放左手法术
    /// </summary>
    public void CastLeftSpells()
    {
        player.animations.HandAttack(0.5f);
    }


    /// <summary>
    /// 释放右手法术
    /// </summary>
    public void CastRightSpells()
    {
        player.animations.HandAttack(-0.5f);
    }

    public IEnumerator CastLeftCoroutine()
    {
        yield return player.animations.HandAttackCoroutine(0.5f);
        player.spells.leftCaster.Cast();
        yield return player.animations.HandAttackCoroutine(0f);
    }

    public IEnumerator CastRightCoroutine()
    {
        yield return player.animations.HandAttackCoroutine(-0.5f);
        player.spells.rightCaster.Cast();
        yield return player.animations.HandAttackCoroutine(0f);
    }

    public IEnumerator LeftHandGetSpells()
    {
        yield return player.animations.HandAttackCoroutine(0.5f);
        yield return player.animations.HandAttackCoroutine(0f);
    }

    public IEnumerator RightHandGetSpells()
    {
        yield return player.animations.HandAttackCoroutine(-0.5f);
        yield return player.animations.HandAttackCoroutine(0f);
    }
}
