using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionEffect
{
    int effectValue;
    int effectID;
}

public class TrapEffect
{
    int effectValue;
    int effectID;
}

public class InteractionManager : MonoBehaviour
{
    public Dictionary<POTIONTYPE, PotionEffect> potionEffect;
    public Dictionary<TRAPTYPE, TrapEffect> trapEffect;
}
