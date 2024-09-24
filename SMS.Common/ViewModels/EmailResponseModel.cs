

namespace SMS.Common.ViewModels
{
    public class EmailResponseModel
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        // Replace unsupported types
        // public Encoding Encoding { get; set; } // This will cause issues
        public string EncodingName { get; set; } // Use string instead
    }

}
