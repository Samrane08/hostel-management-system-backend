namespace ApplicantService.Helper;

public class AadhaarImageFormFile : IFormFile
{
    private readonly byte[] _bytes;
    private readonly string _fileName;

    public AadhaarImageFormFile(byte[] bytes, string fileName)
    {
        _bytes = bytes;
        _fileName = fileName;
    }
    public string ContentType => "image/jpeg";

    public string ContentDisposition => throw new NotImplementedException();
    public IHeaderDictionary Headers => throw new NotImplementedException();
    public long Length => _bytes.Length;
    public string Name => _fileName;
    public string FileName => _fileName;
    public void CopyTo(Stream target)
    {
        using (var memoryStream = new MemoryStream(_bytes))
        {
            memoryStream.CopyTo(target);
        }
    }
    public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        using (var memoryStream = new MemoryStream(_bytes))
        {
            await memoryStream.CopyToAsync(target, cancellationToken);
        }
    }
    public Stream OpenReadStream()
    {
        return new MemoryStream(_bytes);
    }
}
