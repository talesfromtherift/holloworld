using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VR.WSA.Input;

public class HoloTap : MonoBehaviour {

    [Tooltip("The event fired on a Holo tap.")]
    public UnityEvent Tap;

    GestureRecognizer recognizer;

    void Awake() {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.StartCapturingGestures();
    }

    void OnEnable() {
        recognizer.TappedEvent += Recognizer_TappedEvent;
    }

    void OnDisable() {
        recognizer.TappedEvent -= Recognizer_TappedEvent;
    }

    void Update() {
#if UNITY_EDITOR
        // simulate tap with mouse button
        if (Input.GetMouseButtonDown(0)) {
            Recognizer_TappedEvent(
                InteractionSourceKind.Other,
                1,
                Camera.main.ScreenPointToRay(Input.mousePosition));
        }
#endif
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay) {
        Tap.Invoke();
    }
}