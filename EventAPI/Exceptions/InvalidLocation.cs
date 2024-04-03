namespace EventAPI.Exceptions
{
    public class InvalidLocation : Exception
    {
        public InvalidLocation(string? message) : base(message) { }
    }
}
