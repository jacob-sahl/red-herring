using System.Drawing;
using System.Text;
using System.Threading;
using QRCoder;
using Unity.Collections;
using UnityEngine;
using Utils;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class QRCodeObject : MonoBehaviour
{
    private string _lastQrCodeContent = "";
    [SerializeField] public string QRCodeContent = "Red Herring";
    
    private Image _qrCodeImage;
    
    // Start is called before the first frame update
    void Start()
    {
        var dispatcher = Dispatcher.Instance; // Need to initialize the dispatcher
        _qrCodeImage = GetComponent<Image>();
        UpdateQRCode();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQRCode();
    }

    private void UpdateQRCode()
    {
        if (QRCodeContent == _lastQrCodeContent)
        {
            return;
        }

        Thread thread = new Thread(_generateQRCode);
        thread.Start();
        _lastQrCodeContent = QRCodeContent;
    }

    private void _generateQRCode()
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRCodeContent, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeBitmap = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.White, true);
        Dispatcher.Instance.RunInMainThread(() =>
        {
            Debug.Log("Updating QR Code");
            Texture2D qrCodeTexture = new Texture2D(qrCodeBitmap.Width, qrCodeBitmap.Height);
        
            for (int i = 0; i < qrCodeBitmap.Width; i++)
            {
                for (int j = 0; j < qrCodeBitmap.Height; j++)
                {
                    Color color = new Color(qrCodeBitmap.GetPixel(i, j).R / 255.0f, qrCodeBitmap.GetPixel(i, j).G / 255.0f, qrCodeBitmap.GetPixel(i, j).B /255.0f);
                    qrCodeTexture.SetPixel(i, qrCodeBitmap.Height - j, color);
                }
            }
            qrCodeTexture.Apply();
            _qrCodeImage.sprite = Sprite.Create(qrCodeTexture,
                new Rect(0, 0, qrCodeTexture.width, qrCodeTexture.height), new Vector2(0.5f, 0.5f));
        });
    }
}
