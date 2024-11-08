namespace ProvisionPadel.Api.Services;

public class HikvisionService(HikvisionHttpClient hikvisionHttpClient) : IHikvisionService
{
    private readonly HikvisionHttpClient _hikvisionHttpClient = hikvisionHttpClient;

    public async Task<(string startTime, string name)> ExtractNameAndStartTimeFromXml(string trackID, DateTime startTime, DateTime endTime)
    {
        var playbackUri = await ExtractRtspFromXml(trackID, startTime, endTime);

        playbackUri = playbackUri.Replace("&amp;", "&");

        var match = Regex.Match(playbackUri, @"[?&]starttime=([^&]*)&.*[?&]name=([^&]+)");

        if (match.Success)
        {
            return (startTime: match.Groups[1].Value, name: match.Groups[2].Value);
        }

        return (startTime: null, name: null);
    }

    public async Task<(string endTime, string size)> ExtractSizeAndEndTimeFromXml(string trackID, DateTime startTime, DateTime endTime)
    {
        var playbackUri = await ExtractRtspFromXml(trackID, startTime, endTime);

        playbackUri = playbackUri.Replace("&amp;", "&");

        var match = Regex.Match(playbackUri, @"[?&]endtime=([^&]*)&.*[?&]size=([^&]+)");

        if (match.Success)
        {
            return (endtime: match.Groups[1].Value, size: match.Groups[2].Value);
        }

        return (endtime: null, size: null);
    }

    public async Task<string> ExtractRtspFromXml(string trackID, DateTime startTime, DateTime endTime)
    {
        var xmlContent = await ContentMgmtSearch(trackID, startTime, endTime);

        XDocument doc = XDocument.Parse(xmlContent);

        XNamespace ns = "http://www.hikvision.com/ver20/XMLSchema";

        var playbackUriElement = doc.Descendants(ns + "playbackURI").FirstOrDefault();

        string playbackUri = playbackUriElement?.Value;

        return playbackUri;
    }

    private async Task<string> ContentMgmtSearch(string trackID, DateTime startTime, DateTime endTime)
    {
        var url = $"/ISAPI/ContentMgmt/search";

        var response = await _hikvisionHttpClient.Client.PostAsync(url, ContentMgmtSearchXML.GetXMLContent(trackID, startTime, endTime));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}

