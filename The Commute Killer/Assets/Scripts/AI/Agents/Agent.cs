using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;

public class Agent : MonoBehaviour
{
    /* Items */
    public Item OnHand;

    public int InventorySize;

    private int FirstFree = -1;

    public List<Item> Inventory;

    public Vector3 HandPosition;

    /* Attributes */
    public enum Attribute
    {
        MaxHP,
        HP,
        Speed
    }

    public Dictionary<Attribute, float> Attributes;

    /* Actions */
    public List<Action> AvailableActions;

    public List<Action> PerformedActions;

    /* Movement */
    private DynamicCharacter DynamicC;

    public Vector3 GoalPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.Attributes[Attribute.MaxHP] = 10;
        this.Attributes[Attribute.HP]    = 10;
        this.Attributes[Attribute.Speed] = 10;

        if(this.Inventory == null)
        {
            this.Inventory = new List<Item>(this.InventorySize);

            this.FirstFree = 0;
        }
        else
        {
            this.FirstFree = this.Inventory.FindIndex(x => x == null);
        }

        this.AvailableActions = new List<Action>();
        this.PerformedActions = new List<Action>();

        this.DynamicC = new DynamicCharacter(this.gameObject)
        {
            MaxSpeed = 10f
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region === Pick Up Methods ===
    public bool PickUp(Item item)
    {
        if(this.OnHand == null)
        {
            this.OnHand = item;

            item.PickUp(this);

            return true;
        }

        if (this.Inventory.Count < this.Inventory.Capacity)
        {
            this.Inventory.Insert(this.FirstFree, item);

            this.FirstFree = this.Inventory.FindIndex(x => x == null);

            item.PickUp(this);

            return true;
        }

        return false;
    }


    public void Drop(int itemIndex)
    {
        if(itemIndex != -1)
        {
            var item = this.Inventory[itemIndex];

            item.Drop();

            this.Inventory.RemoveAt(itemIndex);

            if(this.FirstFree == -1 || itemIndex < this.FirstFree)
            {
                this.FirstFree = itemIndex;
            }
        }
    }

    #endregion

    #region === Equipping Methods ===
    public void Equip(int itemIndex)
    {
        if (this.OnHand == null)
        {
            if (itemIndex != -1)
            {
                this.OnHand = this.Inventory[itemIndex];

                this.Inventory.RemoveAt(itemIndex);
            }
        }
        else
        {
            var current = this.OnHand;

            this.OnHand.Unequip();

            this.OnHand = this.Inventory[itemIndex];

            this.Inventory[itemIndex] = current;
        }

        this.OnHand.Equip();
    }

    public void Unequip()
    {
        if(this.OnHand != null)
        {
            this.OnHand.Unequip();

            if (this.Inventory.Count < this.Inventory.Capacity)
            {
                this.Inventory.Insert(this.FirstFree, this.OnHand);

                this.FirstFree = this.Inventory.FindIndex(x => x == null);
            }
            else
            {
                var prev = this.OnHand;

                this.OnHand = this.Inventory[this.Inventory.Capacity - 1];

                this.OnHand.Equip();

                for (var i = 0; i < this.Inventory.Count; i++)
                {
                    var nPrev = this.Inventory[i];

                    this.Inventory[i] = prev;

                    prev = nPrev;
                }
            }
        }
    }
    #endregion

    public bool ExecuteAction(Action action)
    {
        if(action.CanExecute())
        {
            action.Execute();
        }

        return false;
    }
}
