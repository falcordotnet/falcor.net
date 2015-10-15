namespace Falcor.Server
{
    public interface IOption<out T>
    {

        bool IsEmpty { get; }


        bool IsDefined { get; }


        T GetOrDefault();


        T Get();
    }
}