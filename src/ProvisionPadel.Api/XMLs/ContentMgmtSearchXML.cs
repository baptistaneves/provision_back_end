namespace ProvisionPadel.Api.XMLs;

public static class ContentMgmtSearchXML
{
    public static StringContent GetXMLContent(string trackID, DateTime startTime, DateTime endTime)
    {
        string xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <CMSearchDescription>
                            <searchID>{Guid.NewGuid()}</searchID>
                            <trackList>
                                <trackID>{trackID}</trackID>
                            </trackList>
                            <timeSpanList>
                                <timeSpan>
                                    <startTime>{startTime:yyyy-MM-ddTHH:mm:ssZ}</startTime>
                                    <endTime>{endTime:yyyy-MM-ddTHH:mm:ssZ}</endTime>
                                </timeSpan>
                            </timeSpanList>
                            <maxResults>50</maxResults>
                            <searchResultPostion>0</searchResultPostion>
                            <metadataList>
                                <metadataDescriptor>//recordType.meta.std-cgi.com</metadataDescriptor>
                            </metadataList>
                        </CMSearchDescription>";

        return new StringContent(xml, Encoding.UTF8, "application/xml");
    }
}
