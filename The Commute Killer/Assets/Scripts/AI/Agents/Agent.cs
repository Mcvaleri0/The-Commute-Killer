using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class Agent : MonoBehaviour
{
    #region /* Items */
    public Item OnHand;

    public int InventorySize = 3;

    public int FirstFree { get; protected set; } = -1;

    public Item[] Inventory;

    public Vector3 HandPosition;
    #endregion

    #region /* Attributes */
    public enum Attribute
    {
        MaxHP,
        HP,
        Speed
    }

    public Dictionary<Attribute, float> Attributes;
    #endregion

    #region /* Actions */
    public List<Action.IDs> AvailableActions;

    public List<Action> PerformedActions;
    #endregion

    #region /* Movement */
    private DynamicCharacter DynamicC;

    public Vector3 GoalPosition;
    #endregion

    // Start is called before the first frame update
    protected void Start()
    {
        this.Attributes = new Dictionary<Attribute, float>
        {
            [Attribute.MaxHP] = 10,
            [Attribute.HP]    = 10,
            [Attribute.Speed] = 10
        };
        
        this.Inventory = new Item[this.InventorySize];

        this.FirstFree = 0;

        this.AvailableActions = new List<Action.IDs>() 
        { 
            Action.IDs.PickUp,
            Action.IDs.Use
        };

        this.PerformedActions = new List<Action>();

        this.DynamicC = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = 10f
        };
    }

    #region === Pick Up Methods ===
    public bool PickUp(Item item)
    {
        if(this.OnHand == null)
        {
            this.OnHand = item;

            item.PickUp(this);

            item.Equip();

            UpdateAvailableActions(item);

            return true;
        }

        if (this.FirstFree != -1)
        {
            this.Inventory[this.FirstFree] = item;

            UpdInventoryFirstFree();

            item.PickUp(this);

            return true;
        }

        return false; // Should never fail as can execute was run just before
    }

    public bool Drop(int itemIndex)
    {
        if(itemIndex != -1)
        {
            // Drop from Inventory
            if(itemIndex < this.InventorySize)
            {
                var item = this.Inventory[itemIndex];

                if(item != null)
                {
                    item.Drop();

                    RemoveFromInventory(itemIndex);

                    if (this.FirstFree == -1 || itemIndex < this.FirstFree)
                    {
                        this.FirstFree = itemIndex;
                    }

                    if(this.AvailableActions.FindIndex(x => x == Action.IDs.PickUp) == -1)
                    {
                        this.AvailableActions.Add(Action.IDs.PickUp);
                    }

                    return true;
                }
            }

            // Drop On Hand
            else if(itemIndex == this.InventorySize)
            {
                var item = this.OnHand;

                if(item != null)
                {
                    item.Drop();

                    UpdateAvailableActions(null, item);

                    this.OnHand = null;

                    return true;
                }
            }
        }

        return false;
    }
    #endregion

    #region === Inventory Methods ===
    public int FindInInventory(Item item)
    {
        for(var i = 0; i < this.InventorySize; i++)
        {
            if(this.Inventory[i] == item)
            {
                return i;
            }
        }

        return -1;
    }

    protected bool AddToInventory(Item item)
    {
        if(this.FirstFree != -1)
        {
            this.Inventory[this.FirstFree] = item;

            UpdInventoryFirstFree();

            return true;
        }

        return false;
    }

    protected bool RemoveFromInventory(int ind)
    {
        if(0 <= ind && ind < this.InventorySize)
        {
            var item = this.Inventory[ind];

            if(item != null)
            {
                this.Inventory[ind] = null;

                return true;
            }
        }

        return false;
    }

    private void UpdInventoryFirstFree()
    {
        for(var i = 0; i< this.InventorySize; i++)
        {
            if(this.Inventory[i] == null)
            {
                this.FirstFree = i;
                return;
            }
        }

        this.AvailableActions.Remove(Action.IDs.PickUp);

        this.FirstFree = -1;
    }

    public void Equip(int ind)
    {
        // If hand is free
        if (this.OnHand == null)
        {
            if (ind != -1)
            {
                this.OnHand = this.Inventory[ind];

                RemoveFromInventory(ind);

                UpdateAvailableActions(this.OnHand);
            }
        }

        // If agent is holding something
        else
        {
            var current = this.OnHand;

            this.OnHand.Unequip();

            this.OnHand = this.Inventory[ind];

            this.Inventory[ind] = current;

            UpdateAvailableActions(this.OnHand, current);
        }

        this.OnHand.Equip();
    }

    public void Unequip()
    {
        if(this.OnHand != null)
        {
            this.OnHand.Unequip();

            if (this.FirstFree != -1)
            {
                this.Inventory[this.FirstFree] = this.OnHand;

                UpdInventoryFirstFree();
            }
            else
            {
                var prev = this.OnHand;

                this.OnHand = this.Inventory[this.InventorySize - 1];

                this.OnHand.Equip();

                for (var i = 0; i < this.InventorySize; i++)
                {
                    var nPrev = this.Inventory[i];

                    this.Inventory[i] = prev;

                    prev = nPrev;
                }
            }
        }
    }
    #endregion

    #region === Action Methods ===
    protected void UpdateAvailableActions(Item equipped, Item unequipped = null)
    {
        if(unequipped != null)
        {
            foreach(Action.IDs a in unequipped.EnabledActions)
            {
                this.AvailableActions.Remove(a);
            }
        }

        if(equipped != null)
        {
            if(this.AvailableActions.FindIndex(x => x == Action.IDs.Drop) == -1)
            {
                this.AvailableActions.Add(Action.IDs.Drop);
            }

            foreach (Action.IDs a in equipped.EnabledActions)
            {
                this.AvailableActions.Add(a);
            }
        }
        else
        {
            this.AvailableActions.Remove(Action.IDs.Drop);
        }
    }

    protected List<Action.IDs> GetPossibleActions(GameObject target)
    {
        Interactable interactable = target.GetComponent<Interactable>();

        if(interactable != null)
        {
            return interactable.PossibleActions;
        }

        Item item = target.GetComponent<Item>();

        if(item != null)
        {
            return item.PossibleActions;
        }

        return null;
    }

    protected Action.IDs DetermineAction(List<Action.IDs> possibleActions)
    {
        if (possibleActions != null) 
        {
            foreach (Action.IDs aId in possibleActions)
            {
                if (this.AvailableActions.FindIndex(x => x == aId) != -1)
                {
                    return aId;
                }
            }
        }

        return Action.IDs.None;
    }

    public bool ExecuteAction(Action.IDs actionId, GameObject target = null)
    {
        var action = CreateAction(actionId, target);

        if(action != null)
        {
            if (action.CanExecute(this, target))
            {
                action.Execute();

                this.PerformedActions.Add(action);
            }
        }

        return false;
    }

    private Action CreateAction(Action.IDs actionId, GameObject target = null)
    {
        switch (actionId)
        {
            default:
                break;

            case Action.IDs.Sabotage:
                return new Sabotage(this, target);

            case Action.IDs.Stab:
                return new Stab(this, target);

            case Action.IDs.PickUp:
                return new PickUp(this, target);

            case Action.IDs.Drop:
                return new Drop(this, target);

            case Action.IDs.Use:
                return new Use(this, target);
        }

        return null;
    }
    #endregion
}
