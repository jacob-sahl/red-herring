using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using QRCoder;
using Unity.Collections;
using UnityEngine;
using Utils;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

using SysImage = System.Drawing.Image;

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
            Texture2D texture = new Texture2D(qrCodeBitmap.Width, qrCodeBitmap.Height);
            texture.LoadImage(ImageToByte2(qrCodeBitmap));
            _qrCodeImage.sprite = Sprite.Create(texture, new Rect(0, 0, qrCodeBitmap.Width, qrCodeBitmap.Height), new Vector2(0.5f, 0.5f));
        });
    }

    public byte[] ImageToByte2(SysImage img)
    {
        using (var stream = new MemoryStream())
        {
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
