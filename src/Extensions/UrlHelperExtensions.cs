//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                return urlHelper.Page("/Index");
            }

            return localUrl;
        }

        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { userId, code },
                protocol: scheme);
        }


        //public static string GetImageDataURI(this IUrlHelper urlHelper, string filePath)
        //{
        //    // Return if the file doesn't exist
        //    if (!File.Exists(filePath))
        //    {
        //        return null;
        //    }

        //    // Get the file extension to use from the path 
        //    var extension = Path.GetExtension(filePath);

        //    // Replace the period since it should not be used
        //    if (extension != null)
        //    {
        //        extension = extension.Replace(".", "");
        //    }

        //    // Special handling of SVG
        //    if (extension == "svg")
        //    {
        //        extension = "svg+xml";
        //    }

        //    // Get file bytes, convert to base64 and format the data URI
        //    var bytes = File.ReadAllBytes(filePath);
        //    var base64 = Convert.ToBase64String(bytes);
        //    var imgSrc = string.Format("data:image/{0};base64,{1}", extension, base64);

        //    return imgSrc;
        //}
    }
}
