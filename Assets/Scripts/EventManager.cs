using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] Selection selection;
    // Start is called before the first frame update
    void Start()
    {
        selection.OnSelected += Selection_OnSelected;
        selection.OnDeselected += Selection_OnDeselected;
    }

    private void Selection_OnDeselected(object sender, Selection.OnDeselectedEventArgs e)
    {
        Debug.Log("Deselected Game object : "+ e.deselectedObject.ToString());
    }

    private void Selection_OnSelected(object sender, Selection.OnSelectedEventArgs e)
    {
        Debug.Log("Selected Game Object : "+ e.selectedObject.ToString());
    }

}
