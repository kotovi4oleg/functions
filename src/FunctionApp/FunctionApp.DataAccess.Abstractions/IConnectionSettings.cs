namespace FunctionApp.DataAccess.Abstractions {
    public interface IConnectionSettings {
        public string Uri { get; }
        public string Database { get; }
        public string PrimaryKey { get; }
    }
}