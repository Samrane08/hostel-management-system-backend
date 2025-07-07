namespace Service.Interface
{
    public interface IErrorLogger
    {
        Task Log(string ErrorAt, Exception ex);

        Task CustomLog(string ErrorAt, string ex);
    }
}
