using UnityEngine;
using ZXing;
using ZXing.Common;

namespace ARNav
{
    public class QRScanner : MonoBehaviour
    {
        public NavigationManager navManager;
        public UIController uiController;

        private WebCamTexture camTexture;
        private IBarcodeReader reader;
        private bool isScanning = true;
        private float scanInterval = 0.5f;
        private float timer = 0f;

        void Start()
        {
            camTexture = new WebCamTexture();
            camTexture.Play();
            reader = new BarcodeReader();
            Debug.Log("QR Scanner started");
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
                int width = camTexture.width;
                int height = camTexture.height;

                // конвертируем Color32[] в LuminanceSource
                var source = new Color32LuminanceSource(pixels, width, height);
                var binaryBitmap = new BinaryBitmap(new HybridBinarizer(source));
                var result = new MultiFormatReader().decode(binaryBitmap);

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
            if (camTexture != null)
                camTexture.Stop();
        }
    }
}