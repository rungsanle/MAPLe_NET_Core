using Maple2.AdminLTE.Bel;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Areas.Master.Models
{
    public class MachineLabelModel : M_Machine
    {

        public string QR_CODE { get; set; }

        public MachineLabelModel(M_Machine mc)
        {
            this.QR_CODE = BitmapText(mc.MachineCode);
            this.Id = mc.Id;
            this.MachineCode = mc.MachineCode;
            this.MachineName = mc.MachineName;
            this.MachineProdType = mc.MachineProdType;
            this.MachineProdTypeName = mc.MachineProdTypeName;
            this.MachineSize = mc.MachineSize;
            this.MachineRemark = mc.MachineRemark;
            this.CompanyCode = mc.CompanyCode;
            this.Is_Active = mc.Is_Active;
            this.Created_By = mc.Created_By;
            this.Created_Date = mc.Created_Date;
            this.Updated_By = mc.Updated_By;
            this.Updated_Date = mc.Updated_Date;
        }

        private string BitmapText(string byteData)
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
    }
}
