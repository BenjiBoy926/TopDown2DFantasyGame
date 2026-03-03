using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battlefield))]
public class Battle : MonoBehaviour
{
    private Battlefield _field;
    private List<Faction> _factions = new(2);
    private int _currentFaction = 0;

    private void Awake()
    {
        _field = GetComponent<Battlefield>();
    }
}
