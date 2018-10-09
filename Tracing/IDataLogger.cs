namespace Tracing
{
    public interface IDataLogger<ReturnType, ArgType>
    {
        ReturnType LogData(ArgType customArg, System.Type dataType, string logMessage);
    }
}
