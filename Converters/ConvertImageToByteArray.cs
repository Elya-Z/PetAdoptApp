namespace PetAdoptApp.Converters;
public class ImageHelper
{
    public async Task<byte[]> ConvertImageToByteArray(FileResult photo)
    {
        using var stream = await photo.OpenReadAsync();
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}