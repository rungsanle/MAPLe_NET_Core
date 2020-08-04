using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Areas.Administrator.Models
{
    public class ProductCardModel
    {
        public IEnumerable<ItemModel> Items { get; set; }

        public static ProductCardModel Example()
        {
            return new ProductCardModel()
            {
                Items = new List<ItemModel>()
                {
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000001",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000001")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000002",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000002")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000003",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000003")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000004",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000004")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000005",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000005")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000001",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000001")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000002",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000002")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000003",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000003")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000004",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000004")
                    },
                    new ItemModel()
                    {
                        JOB_NO = "A-12002000486",
                        SERIAL_NO = "HOZ20200224000005",
                        PRODUCT_NO = "PRODUCT A",
                        PRODUCT_NAME = "PRODUCT NAME A",
                        BOX_QTY = 100,
                        QR_CODE = BitmapText("HOZ20200224000005")
                    }
                    //,
                    //new ItemModel()
                    //{
                    //    JOB_NO = "A-12002000486",
                    //    SERIAL_NO = "HOZ20200224000001",
                    //    PRODUCT_NO = "PRODUCT A",
                    //    PRODUCT_NAME = "PRODUCT NAME A",
                    //    BOX_QTY = 100,
                    //    QR_CODE = BitmapText("HOZ20200224000001")
                    //},
                    //new ItemModel()
                    //{
                    //    JOB_NO = "A-12002000486",
                    //    SERIAL_NO = "HOZ20200224000002",
                    //    PRODUCT_NO = "PRODUCT A",
                    //    PRODUCT_NAME = "PRODUCT NAME A",
                    //    BOX_QTY = 100,
                    //    QR_CODE = BitmapText("HOZ20200224000002")
                    //},
                    //new ItemModel()
                    //{
                    //    JOB_NO = "A-12002000486",
                    //    SERIAL_NO = "HOZ20200224000003",
                    //    PRODUCT_NO = "PRODUCT A",
                    //    PRODUCT_NAME = "PRODUCT NAME A",
                    //    BOX_QTY = 100,
                    //    QR_CODE = BitmapText("HOZ20200224000003")
                    //},
                    //new ItemModel()
                    //{
                    //    JOB_NO = "A-12002000486",
                    //    SERIAL_NO = "HOZ20200224000004",
                    //    PRODUCT_NO = "PRODUCT A",
                    //    PRODUCT_NAME = "PRODUCT NAME A",
                    //    BOX_QTY = 100,
                    //    QR_CODE = BitmapText("HOZ20200224000004")
                    //},
                    //new ItemModel()
                    //{
                    //    JOB_NO = "A-12002000486",
                    //    SERIAL_NO = "HOZ20200224000005",
                    //    PRODUCT_NO = "PRODUCT A",
                    //    PRODUCT_NAME = "PRODUCT NAME A",
                    //    BOX_QTY = 100,
                    //    QR_CODE = BitmapText("HOZ20200224000005")
                    //}
                }
            };
        }

        private static string BitmapText(string byteData)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(byteData, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public class ItemModel
        {
            public string JOB_NO { get; set; }
            public string SERIAL_NO { get; set; }
            public string PRODUCT_NO { get; set; }
            public string PRODUCT_NAME { get; set; }
            public int BOX_QTY { get; set; }
            public string QR_CODE { get; set; }
        }
    }
}
