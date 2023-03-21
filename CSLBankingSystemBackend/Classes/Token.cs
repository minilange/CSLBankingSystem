namespace CSLBankingSystemBackend.Classes
{
    public class Token
    {
        public string value { get; private set; }
        public int customerId { get; private set; }
        public DateTime timestamp { get; private set; }

        public Token(string value, int customerId, string timestamp)
        {
            this.value = value;
            this.customerId = customerId;
            this.timestamp = DateTime.Parse(timestamp);
        }

    }
}
