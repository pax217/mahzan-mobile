using System;
using System.IO;

namespace Mahzan.Mobile.Utils.Images
{
    public static class ImagesUtil
    {
        public static string ConvertImageBase64(string rutaImagen)
        {
            string resultado = string.Empty;

            byte[] ImageData = File.ReadAllBytes(rutaImagen);

            resultado = Convert.ToBase64String(ImageData);

            return resultado;
        }

        public static string GetMIMEType(string extension)
        {
            string resultado = string.Empty;

            switch (extension)
            {
                case ".jpg":
                    resultado = "image/jpeg";
                    break;
                default:
                    break;
            }

            return resultado;
        }
    }
    

}