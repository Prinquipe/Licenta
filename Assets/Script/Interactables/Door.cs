using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public DoorState state;
    public string DoorID;

    public void Open()
    {

    }

    public void Interact()
    {
        Open();
    }

    public bool MatchID(string ID)
    {
        return DoorID.Equals(ID);
    }

    public void ChangeState(DoorState _state)
    {
        state = _state;
    }
}
