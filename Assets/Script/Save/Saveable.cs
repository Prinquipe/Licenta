using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Saveable
{
     void SaveObject();

     void LoadObject();

     void OnRequestSaveEvent();
}
