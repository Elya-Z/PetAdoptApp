using SkiaSharp;

namespace PetAdoptApp;

public static class ImageLoading
{
    public static async Task<SKImage> ResizeImage(Stream imageStream, int maxWidth, int maxHeight)
    {
        using var original = SKBitmap.Decode(imageStream);

        // Вычисляем новые размеры с сохранением пропорций
        float ratio = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
        int newWidth = (int)(original.Width * ratio);
        int newHeight = (int)(original.Height * ratio);

        // Создаем уменьшенное изображение
        var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium);
        return SKImage.FromBitmap(resized);
    }
}
