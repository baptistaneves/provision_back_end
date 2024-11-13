namespace ProvisionPadel.Api.Shared.Hikvision;

public class HikvisionHttpClient
{
    public HttpClient Client { get; }
    public string Rtsp { get; }

    private readonly HikvisionNVR _hikvisionNVR;

    public HikvisionHttpClient(IOptions<HikvisionNVR> hikvisionNVR,
                               IConfiguration configuration)
    {
        Client = new HttpClient();
        _hikvisionNVR = hikvisionNVR.Value;

        Rtsp = _hikvisionNVR.Rtsp;

        var handler = new HttpClientHandler
        {
            Credentials = new System.Net.NetworkCredential(_hikvisionNVR.Username, _hikvisionNVR.Password)
        };

        Client = new HttpClient(handler)
        {
            BaseAddress = new Uri(_hikvisionNVR.BaseUrl)
        };
    }
}