using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";

    private IRayProvider _rayProvider;
    private ISelector _selector;
    private ISelectionResponse _selectionResponse;

    private Transform _currentSelection;
    private Transform _selection;

    private void Awake()
    {
        _rayProvider = GetComponent<IRayProvider>();
        _selector = GetComponent<ISelector>();
        _selectionResponse = GetComponent<ISelectionResponse>();
    }

    private void Update()
    {
        //Deselection/Selectinon Response
        if (_currentSelection != null)
        {
            _selectionResponse.OnDeselect(_currentSelection);
        }

        //Selection Determination
        _selector.Check(_rayProvider.CreateRay());
        _currentSelection = _selector.GetSelection();

        //Deselection/Selectinon Response
        if (_currentSelection != null)
        {
            _selectionResponse.OnSelect(_currentSelection);
        }
    }

    public Transform getSelection()
    {
        return _currentSelection;
    }
}