namespace Falcor.Examples.Netflix
{
    public abstract class ServiceResult
    {
        public ServiceResult(string error)
        {
            ErrorMessage = error;
        }

        public string ErrorMessage { get; }
        public bool Error => !Success;
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}