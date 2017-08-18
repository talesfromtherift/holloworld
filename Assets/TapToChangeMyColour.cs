using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TapToChangeMyColour : MonoBehaviour, IInputClickHandler, IFocusable {

   public void OnInputClicked(InputClickedEventData eventData) {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    public void OnFocusEnter() {
        transform.localScale *= 1.2f;
    }
    public void OnFocusExit() {
        transform.localScale /= 1.2f;
    }
}
