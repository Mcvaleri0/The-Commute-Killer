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
        Speed,
        Accelaration,
        Drag
    }

    public Dictionary<Attribute, float> Attributes;
    #endregion

    #region /* Actions */
    public List<Action.IDs> AvailableActions; // Actions the Agent can perform

    public List<Action> PerformedActions;
    #endregion

    #region /* Movement */
    private DynamicCharacter DynamicC;

    public Vector3 GoalPosition;

    public Vector3 GetPosition()
    {
        return this.DynamicC.GameObject.transform.position;
    }
    #endregion


    // Start is called before the first frame update
    protected void Start()
    {
        this.Attributes = new Dictionary<Attribute, float>
        {
            [Attribute.MaxHP] = 10,
            [Attribute.HP]    = 10,
            [Attribute.Speed] = 1,
            [Attribute.Accelaration] = 2,
            [Attribute.Drag]  = 0.1f
        };
        
        this.Inventory = new Item[this.InventorySize];

        this.FirstFree = 0;

        this.AvailableActions = new List<Action.IDs>() 
        { 
            Action.IDs.PickUp,
            Action.IDs.Drop,
            Action.IDs.Use
        };

        this.PerformedActions = new List<Action>();


        this.DynamicC = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = 5f
        };
    }

    protected void Update()
    {
        if(this.Attributes[Attribute.HP] <= 0)
        {
            Die();
            return;
        }
    }

    #region === Pick Up Methods ===
    public bool PickUp(Item item)
    {
        if(this.OnHand == null)
        {
            this.OnHand = item;

            item.PickUp(this);

            item.Equip();

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

                    UpdInventoryFirstFree();

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
            }
        }

        // If agent is holding something
        else
        {
            var current = this.OnHand;

            this.OnHand.Unequip();

            this.OnHand = this.Inventory[ind];

            this.Inventory[ind] = current;
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

    public bool ExecuteAction(Action action)
    {
        if(action != null)
        {
            action.Execute();

            this.PerformedActions.Add(action);
        }

        return false;
    }

    protected Action CreateAction(Action.IDs actionId, GameObject target = null)
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

            case Action.IDs.Insert:
                return new Insert(this, target);
        }

        return null;
    }
    #endregion

    public Agent GetInFront()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f);

        if(hit.transform != null)
        {
            return hit.transform.GetComponent<Agent>();
        }

        return null;
    }

    public void Die()
    {
       
        this.GetComponent<Animator>().SetBool("isDying", true);

        gameObject.AddComponent<Cadaver>();

        GameObject.Find("EventManager").GetComponent<EventManager>().TriggerEvent(Event.Killed);

        Destroy(this);
    }
}
