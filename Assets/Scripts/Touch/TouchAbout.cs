using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchAbout : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public LoadMenuOptions loadMenuOptions;

    public void OnPointerClick(PointerEventData eventData)
    {
        // This method will be called when the UI button is clicked/touched
 
        loadMenuOptions.onAboutButton();

        // Perform your button click logic here
    }
}
