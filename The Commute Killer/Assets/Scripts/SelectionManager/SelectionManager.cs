using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string Tag = "Selectable";

    private IRayProvider       RayProvider;
    private ISelector          Selector;
    private ISelectionResponse SelectionResponse;

    private Transform CurrentSelection;

    public Transform Selection { get; private set; }

    private void Start()
    {
        this.RayProvider       = this.GetComponent<IRayProvider>();
        this.Selector          = this.GetComponent<ISelector>();
        this.SelectionResponse = this.GetComponent<ISelectionResponse>();
    }

    private void Update()
    {
        //Deselection/Selectinon Response
        if (this.CurrentSelection != null)
        {
            this.SelectionResponse.OnDeselect(this.CurrentSelection);
        }

        //Selection Determination
        this.Selector.Check(this.RayProvider.CreateRay());
        this.CurrentSelection = this.Selector.GetSelection();

        //Deselection/Selectinon Response
        if (this.CurrentSelection != null)
        {
            this.SelectionResponse.OnSelect(this.CurrentSelection);
        }
    }
}