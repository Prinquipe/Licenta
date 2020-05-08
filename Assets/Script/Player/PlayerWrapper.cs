using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWrapper
{
    public PlayerState playerState;
    public InventoryState inState;

    public PlayerWrapper(PlayerState _player, InventoryState _inv)
    {
        playerState = _player;
        inState = _inv;
    }
}