namespace DDO_Service.API.Features.Token
{
  
    public interface IUserClaimsService
    {
        string GetUserId();
        string GetDDOId();
        string GetDDOCode();
        string GetdetailHead();
        string GetDeptId();
    }
}
