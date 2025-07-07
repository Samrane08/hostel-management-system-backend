namespace Service.Interface;

public interface IErrorLogger
{
    Task Log(string ExceptionAt, Exception ex);
}
