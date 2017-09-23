using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;
using System.Linq;

public class LocatableCameraCapture : MonoBehaviour {

    PhotoCapture photoCaptureObject = null;

    Renderer previewRenderer;
    void TakePhotoToPreview(Renderer preview) {
        previewRenderer = preview;
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject) {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result) {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result) {
        if (result.success) {
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        } else {
            Debug.LogError("Unable to start photo mode!");
        }
    }
    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame) {
        if (result.success) {
            // Create our Texture2D for use and set the correct resolution
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            // Copy the raw image data into our target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            // Do as we wish with the texture such as apply it to a material, etc.

            previewRenderer.material.mainTexture = targetTexture;
            // update the aspect ratio to match webcam
            float aspectRatio = (float)targetTexture.width / (float)targetTexture.height;
            Vector3 scale = previewRenderer.transform.localScale;
            scale.x = scale.y * aspectRatio;
            previewRenderer.transform.localScale = scale;

            // location
            Matrix4x4 cameraToWorldMatrix;
            photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
            Matrix4x4 worldToCameraMatrix = cameraToWorldMatrix.inverse;


            Matrix4x4 projectionMatrix;
            photoCaptureFrame.TryGetProjectionMatrix(out projectionMatrix);

            // Position the canvas object slightly in front
            // of the real world web camera.
            //Vector3 position = cameraToWorldMatrix.GetColumn(3) - cameraToWorldMatrix.GetColumn(2);

            // Rotate the canvas object so that it faces the user.
            // Quaternion rotation = Quaternion.LookRotation(-cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));
            Vector3 position = cameraToWorldMatrix.MultiplyPoint(Vector3.zero);
   //         Quaternion rotation = Quaternion.LookRotation(-cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));



            previewRenderer.transform.position = position;
//            previewRenderer.transform.rotation = rotation;
        }
        // Clean up
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    public static Vector3 UnProjectVector(Matrix4x4 proj, Vector3 to) {
        Vector3 from = new Vector3(0, 0, 0);
        var axsX = proj.GetRow(0);
        var axsY = proj.GetRow(1);
        var axsZ = proj.GetRow(2);
        from.z = to.z / axsZ.z;
        from.y = (to.y - (from.z * axsY.z)) / axsY.y;
        from.x = (to.x - (from.z * axsX.z)) / axsX.x;
        return from;
    }

    public void InstantiatePhoto(GameObject prefab) {
        Debug.Log("InstantiatePhoto");
        GameObject go = GameObject.Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward * 0.5f, Camera.main.transform.rotation);
        TakePhotoToPreview(go.transform.GetChild(0).GetComponent<Renderer>());
    }
}
