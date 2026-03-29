using UnityEngine;
using ZXing;
using ZXing.Common;

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
                int width = camTexture.width;
                int height = camTexture.height;

                // конвертируем Color32[] → byte[] вручную (RGB)
                byte[] bytes = new byte[pixels.Length * 3];
                for (int i = 0; i < pixels.Length; i++)
                {
                    bytes[i * 3] = pixels[i].r;
                    bytes[i * 3 + 1] = pixels[i].g;
                    bytes[i * 3 + 2] = pixels[i].b;
                }

                var source = new RGBLuminanceSource(bytes, width, height);
                var bitmap = new BinaryBitmap(new HybridBinarizer(source));
                var reader = new MultiFormatReader();
                var result = reader.decode(bitmap);

                if (result != null)
                {
                    Debug.Log("QR scanned: " + result.Text);
                    OnQRScanned(result.Text);
                }
            }
            catch (ReaderException) { /* QR не найден в кадре — норм */ }
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