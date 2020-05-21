namespace SWS.ApiServer.Models
{
    public class CardRequest
    {
        public string Id { get; set; }

        public string Token { get; set; }
    }

    public class CardRequestEncoded
    {
        public string Data { get; set; }
    }
}
