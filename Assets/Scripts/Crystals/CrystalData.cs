using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Types
{
    Red,
    Blue,
    Green,
    White
}

[CreateAssetMenu(fileName = "Crystal Data", menuName = "Match 3/Crystal Data")]
public class CrystalData : ScriptableObject
{
    public GameObject Prefab;
    public Types Type;
}
