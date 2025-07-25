using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchStart : MonoBehaviour, IPointerClickHandler
{

    public LoadMenuOptions loadMenuOptions;

    public void OnPointerClick(PointerEventData eventData)
    {
        // This method will be called when the UI button is clicked/touched
       
        loadMenuOptions.onPlayButton();

        // Perform your button click logic here
    }


}
