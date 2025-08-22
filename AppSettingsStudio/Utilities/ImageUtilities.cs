namespace AppSettingsStudio.Utilities;

public static class ImageUtilities
{
    public static Bitmap Resize(this Image image, Size size) => image.Resize(size.Width, size.Height);
    public static Bitmap Resize(this Image image, int width, int height)
    {
        var rc = new Rectangle(0, 0, width, height);
        var bitmap = new Bitmap(width, height);
        bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using var graphics = Graphics.FromImage(bitmap);
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using var wrapMode = new ImageAttributes();
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, rc, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        return bitmap;
    }
}
