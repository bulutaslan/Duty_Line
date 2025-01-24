using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // Bu arayüz, bir nesnenin etkileþime açýk olduðunu belirtir
    void Interact(Action onInteractionComplete);
}
