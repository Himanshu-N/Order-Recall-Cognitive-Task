using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    [SerializeField] Material selectionMaterial;

    public event EventHandler<OnHighlightEventArgs> OnHighlight;
    public class OnHighlightEventArgs : EventArgs
    {
        public Transform highlightObject;
    }
    public event EventHandler<OnSelectedEventArgs> OnSelected;
    public class OnSelectedEventArgs : EventArgs
    {
        public Transform selectedObject;
    }
    public event EventHandler<OnDeselectedEventArgs> OnDeselected;
    public class OnDeselectedEventArgs : EventArgs
    {
        public Transform deselectedObject;
    }

    private Material originalMaterialHighlight;
    private Material originalMaterialSelection;
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.GetComponent<MeshRenderer>().sharedMaterial = originalMaterialHighlight;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;

            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.GetComponent<MeshRenderer>().material != highlightMaterial)
                {
                    originalMaterialHighlight = highlight.GetComponent<MeshRenderer>().material;
                    highlight.GetComponent<MeshRenderer>().material = highlightMaterial;
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (highlight)
            {
                if (selection != null)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                }
                selection = raycastHit.transform;
                OnSelected?.Invoke(this, new OnSelectedEventArgs { selectedObject = selection });

                if (selection.GetComponent<MeshRenderer>().material != selectionMaterial)
                {
                    originalMaterialSelection = originalMaterialHighlight;
                    selection.GetComponent<MeshRenderer>().material = selectionMaterial;
                }
                highlight = null;
            }
            else
            {
                if (selection)
                {
                    selection.GetComponent<MeshRenderer>().material = originalMaterialSelection;
                    OnDeselected?.Invoke(this, new OnDeselectedEventArgs { deselectedObject = selection });
                    selection = null;
                }
            }
        }

    }
}
