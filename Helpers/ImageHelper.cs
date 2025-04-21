namespace PetAdoptApp.Helpers;

public static class ImageHelper
{
    public static async Task<ImageSource?> DownloadImageAsync(this string imagePath)
    {
        string bucketId = imagePath.Substring(0, imagePath.IndexOf('/'));
        imagePath = imagePath.Substring(imagePath.IndexOf('/') + 1);
        var response = await SB.Storage.From(bucketId).Download(imagePath, onProgress: null);

        if (response == null || response.Length == 0)
            return null; 

        var stream = new MemoryStream(response);
        return ImageSource.FromStream(() => stream);
    }
}
