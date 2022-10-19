using System.Drawing;
using QRCoder;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class QRCodeObject : MonoBehaviour
{
    private string _qrCodeContent = "Red Herring";
    public string QRCodeContent { get { return _qrCodeContent; } set {
        if (_qrCodeContent != value)
        {
            _qrCodeContent = value;
            UpdateQRCode();
        }
    } }
    
    private Texture2D _qrCodeTexture;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateQRCode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateQRCode()
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(_qrCodeContent, QRCodeGenerator.ECCLevel.Q);
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.White, true);
        _qrCodeTexture = new Texture2D(qrCodeImage.Width, qrCodeImage.Height);
        
        for (int i = 0; i < qrCodeImage.Width; i++)
        {
            for (int j = 0; j < qrCodeImage.Height; j++)
            {
                Color color = new Color(qrCodeImage.GetPixel(i, j).R / 255.0f, qrCodeImage.GetPixel(i, j).G / 255.0f, qrCodeImage.GetPixel(i, j).B /255.0f);
                _qrCodeTexture.SetPixel(i, qrCodeImage.Height - j, color);
            }
        }
        _qrCodeTexture.Apply();
        GetComponent<Image>().sprite = Sprite.Create(_qrCodeTexture, new Rect(0, 0, qrCodeImage.Width, qrCodeImage.Height), new Vector2(0.5f, 0.5f));
    }
}
