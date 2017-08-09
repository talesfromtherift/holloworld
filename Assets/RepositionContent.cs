using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionContent : MonoBehaviour {

    public Transform Anchor;

    void OnApplicationFocus(bool focus) {
        if (focus) {
            StartCoroutine(RepositionContentCoroutine());
        }
    }

    IEnumerator RepositionContentCoroutine() {
        yield return new WaitForEndOfFrame();
        transform.position = Anchor.position;
    }
}
