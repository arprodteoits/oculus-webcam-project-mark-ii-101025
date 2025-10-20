using UnityEngine;
using UnityEngine.UI;

public class WebcamDisplay : MonoBehaviour
{
    public RawImage rawImage;
    private WebCamTexture webCamTexture;

    void Start()
    {
        webCamTexture = new WebCamTexture();
        rawImage.texture = webCamTexture;
        webCamTexture.Play();
    }

    void OnDestroy()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
            webCamTexture.Stop();
    }
}
