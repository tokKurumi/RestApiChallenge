namespace MultithredRest.Core.Endpoint
{
    public interface IProgressEndpoint
    {
        int CurrentProgress { get; }
    }
}