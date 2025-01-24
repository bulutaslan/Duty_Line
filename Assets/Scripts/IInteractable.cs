using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // Bu aray�z, bir nesnenin etkile�ime a��k oldu�unu belirtir
    void Interact(Action onInteractionComplete);
}
