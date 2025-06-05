using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchManager : MonoBehaviour, IPointerClickHandler
{



    public void OnPointerClick(PointerEventData eventData)
{
    // This method will be called when the UI button is clicked/touched
    Debug.Log("Button Clicked!");

    // Perform your button click logic here
}


}
