namespace FunctionApp.DataAccess.Abstractions {
    public sealed class ConnectionSettings : IConnectionSettings {
        public string Uri { get; set; }
        public string Database { get; set; }
        public string PrimaryKey { get; set; }
    }
}