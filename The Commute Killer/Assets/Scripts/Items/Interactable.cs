using UnityEngine;

public interface Interactable
{
    bool Interact(Agent interactor);

    bool CanInteract(Agent interactor);
}
