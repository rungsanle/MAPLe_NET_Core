using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Extensions
{
    public class GlobalFunction
    {
        public static string ConvertCsvFileToJsonObject(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {

                string content = new StreamReader(fs, Encoding.Default).ReadToEnd();

                string[] split = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length >= 2)
                {
                    var properties = SplitString(split[0]);
                    var listObjResult = new List<Dictionary<string, string>>();


                    for (int i = 1; i < split.Length; i++)
                    {
                        var objResult = new Dictionary<string, string>();
                        string[] fields = SplitString(split[i]);

                        for (int j = 0; j < properties.Length; j++)
                        {
                            objResult.Add(properties[j], fields[j]);
                        }

                        listObjResult.Add(objResult);
                    }

                    return JsonConvert.SerializeObject(listObjResult);

                }
            }

            return string.Empty;
        }

        private static string[] SplitString(string inputString)
        {
            System.Text.RegularExpressions.RegexOptions options = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline)
                        | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Regex reg = new Regex("(?:^|,)(\\\"(?:[^\\\"]+|\\\"\\\")*\\\"|[^,]*)", options);
            MatchCollection coll = reg.Matches(inputString);
            string[] items = new string[coll.Count];
            int i = 0;
            foreach (Match m in coll)
            {
                items[i++] = m.Groups[0].Value.Trim('"').Trim(',').Trim('"').Trim();
            }
            return items;
        }

        public static void SaveThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            try
            {
                int newWidth, newHeight;

                using (var image = Image.FromStream(sourcePath))
                {

                    if (image.Width > 100 || image.Height > 100)
                    {
                        newWidth = (int)(image.Width * scaleFactor);
                        newHeight = (int)(image.Height * scaleFactor);
                    }
                    else
                    {
                        newWidth = image.Width;
                        newHeight = image.Height;
                    }

                    var thumbnailImg = new Bitmap(newWidth, newHeight);
                    var thumbGraph = Graphics.FromImage(thumbnailImg);
                    thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                    thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                    thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                    thumbGraph.DrawImage(image, imageRectangle);
                    thumbnailImg.Save(targetPath, image.RawFormat);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
