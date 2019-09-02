namespace BitexTradingBot.Core.DataAccess.DataInvoke.ApiClient
{
    public class ApiClientOptions
    {
        public ApiClientRequestTypes RequestType { get; set; }
        public ApiClientRequestContentTypes ContentType { get; set; }
        public string Uri { get; set; }
        public string HttlClientName { get; set; }
        public dynamic UriParameters { get; set; }
        public object RequestContent { get; set; }
    }
}
