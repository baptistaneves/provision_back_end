namespace ProvisionPadel.Api.Shared.Hikvision;

public class HikvisionHttpClient
{
    public HttpClient Client { get; }
    public string Rtsp { get; }

    public HikvisionHttpClient(IConfiguration configuration)
    {
        Client = new HttpClient();
        Rtsp = configuration["HikvisionNVR:Rtsp"]!;

        var handler = new HttpClientHandler
        {
            Credentials = new System.Net.NetworkCredential(configuration["HikvisionNVR:Username"], configuration["HikvisionNVR:Password"])
        };

        Client = new HttpClient(handler)
        {
            BaseAddress = new Uri(configuration["HikvisionNVR:BaseUrl"]!)
        };
    }
}