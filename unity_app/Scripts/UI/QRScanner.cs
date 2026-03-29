using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace ARNav
{
    public class QRScanner : MonoBehaviour
    {
        public UIController uiController;

        private WebCamTexture camTexture;
        private bool isScanning = true;
        private float scanInterval = 0.5f;
        private float timer = 0f;

        void Start()
        {
            camTexture = new WebCamTexture();
            camTexture.Play();
        }

        void Update()
        {
            if (!isScanning) return;
            timer += Time.deltaTime;
            if (timer < scanInterval) return;
            timer = 0f;
            ScanFrame();
        }

        void ScanFrame()
        {
            if (!camTexture.didUpdateThisFrame) return;
            try
            {
                Color32[] pixels = camTexture.GetPixels32();
                var reader = new QRCodeMultiReader();
                var hints = new System.Collections.Generic.Dictionary<DecodeHintType, object>();
                var luminance = new RGBLuminanceSource(pixels, camTexture.width, camTexture.height, RGBLuminanceSource.BitmapFormat.Unknown);
                var result = reader.decode(new BinaryBitmap(new ZXing.Common.HybridBinarizer(luminance)));

                if (result != null)
                {
                    Debug.Log("QR scanned: " + result.Text);
                    OnQRScanned(result.Text);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Scan error: " + e.Message);
            }
        }

        void OnQRScanned(string nodeId)
        {
            isScanning = false;
            uiController.ShowDestinationMenu(nodeId);
        }

        public void RestartScanning()
        {
            isScanning = true;
            timer = 0f;
        }

        void OnDestroy()
        {
            if (camTexture != null) camTexture.Stop();
        }
    }
}
