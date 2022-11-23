using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using QRCoder;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Color = System.Drawing.Color;
using SysImage = System.Drawing.Image;

public class QRCodeObject : MonoBehaviour
{
    [SerializeField] public string QRCodeContent = "Red Herring";
    private string _lastQrCodeContent = "";

    private Image _qrCodeImage;

    // Start is called before the first frame update
    private void Start()
    {
        var dispatcher = Dispatcher.Instance; // Need to initialize the dispatcher
        _qrCodeImage = GetComponent<Image>();
        UpdateQRCode();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateQRCode();
    }

    private void UpdateQRCode()
    {
        if (QRCodeContent == _lastQrCodeContent) return;

        var thread = new Thread(_generateQRCode);
        thread.Start();
        _lastQrCodeContent = QRCodeContent;
    }

    private void _generateQRCode()
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(QRCodeContent, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);
        var qrCodeBitmap = qrCode.GetGraphic(20, Color.Black, Color.White, true);

        Dispatcher.Instance.RunInMainThread(() =>
        {
            Debug.Log("Updating QR Code");
            var texture = new Texture2D(qrCodeBitmap.Width, qrCodeBitmap.Height);
            texture.LoadImage(ImageToByte2(qrCodeBitmap));
            _qrCodeImage.sprite = Sprite.Create(texture, new Rect(0, 0, qrCodeBitmap.Width, qrCodeBitmap.Height),
                new Vector2(0.5f, 0.5f));
        });
    }

    public byte[] ImageToByte2(SysImage img)
    {
        using (var stream = new MemoryStream())
        {
            img.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }
    }
}