using Helper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

namespace Service.Implementation;

public static class ImageProcessor
{
    /// <summary>
    /// Processes the image.
    /// </summary>
    /// <param name="imageStream">The image stream.</param>
    /// <returns></returns>
    public static Stream ProcessImage(Stream imageStream)
    {
        try
        {
            using (var image = Image.Load(imageStream, out IImageFormat format))
            {
                var exifProfile = image.Metadata.ExifProfile;
                if (exifProfile != null)
                {
                    var orientationExif = image.Metadata.ExifProfile.GetValue(ExifTag.Orientation);
                    string strOrientationValue = string.Empty;
                    if (orientationExif == null)
                    {
                        strOrientationValue = "1";
                    }
                    else
                    {
                        strOrientationValue = orientationExif.Value.ToString();
                    }
                    int orientationValue = 1;
                    int.TryParse(strOrientationValue, out orientationValue);
                    switch (orientationValue)
                    {
                        case 1:
                            break;
                        case 2:
                            image.Mutate(x => x.Flip(FlipMode.Horizontal));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                        case 3:
                            image.Mutate(x => x.Rotate(RotateMode.Rotate180));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                        case 4:
                            image.Mutate(x => x.Flip(FlipMode.Vertical));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                        case 5:
                            image.Mutate(x => x.Flip(FlipMode.Horizontal).Rotate(RotateMode.Rotate270));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                        case 6:
                            image.Mutate(x => x.Rotate(RotateMode.Rotate90));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                        case 7:
                            image.Mutate(x => x.Flip(FlipMode.Horizontal).Rotate(RotateMode.Rotate90));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                        case 8:
                            image.Mutate(x => x.Rotate(RotateMode.Rotate270));
                            image.Metadata.ExifProfile.RemoveValue(ExifTag.Orientation);
                            break;
                    }
                }

                var processedImageString = new MemoryStream();
                image.Save(processedImageString, format);
                return processedImageString;
            }

        }
        catch (System.Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            throw ex;
        }
    }
}
