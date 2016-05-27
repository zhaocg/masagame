using UnityEngine;
using System.Collections;

public enum PlayerState
{
    SELECT,
    SHOOT
}

public class Player : MonoBehaviour {



    public PlayerProperty property;

    [HideInInspector] public EnergySystem energySystem;
    [HideInInspector] public PlayerActions actions;
    [HideInInspector] public PlayerAnimations animations;
    [HideInInspector] public PlayerSpells spells;
    [HideInInspector] public SpellsSystem spellsSystem;

    public PlayerState gameState = PlayerState.SHOOT;

    public static Player instance;


    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        energySystem = this.GetComponent<EnergySystem>();
        actions = this.GetComponent<PlayerActions>();
        animations = this.GetComponent<PlayerAnimations>();
        spells = this.GetComponent<PlayerSpells>();
        spellsSystem = this.GetComponent<SpellsSystem>();
    }
}

