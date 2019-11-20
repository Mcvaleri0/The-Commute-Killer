using UnityEngine;

public interface Interactable
{
    bool Interact(GameObject interactor);

    bool CanInteract(GameObject interactor);
}
