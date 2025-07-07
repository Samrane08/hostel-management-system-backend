using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model;
using Newtonsoft.Json.Linq;
using Repository.Entity;
using Repository.Interface;
using Service.Interface;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Web.Services.Description;
using UserService.Helper;
using UserService.Model.Request;
using UserService.Model.Response;
using UserService.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ExceptionLogging = Helper.ExceptionLogging;
using Utility = Helper.Utility;

namespace UserService.Controllers;
public class AccountController : APIBaseController
{
    private readonly APIUrl urloptions;
    private readonly ITokenHelper tokenHelper;
    private readonly IUserManagerService userManagerService;
    private readonly IHttpClientService clientService;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<ApplicationRole> roleManager;
  
    private readonly ICacheService cacheService;
    private readonly IErrorLogger errorLogger;
    private readonly AppleSarkarCred appCredOptions;
    private readonly ISessionService sessionService;
    private readonly IBruteForceService bruteForceService;


    public AccountController(UserManager<ApplicationUser> userManager,
                              ITokenHelper tokenHelper,
                              IUserManagerService userManagerService,
                              IOptions<APIUrl> urloptions,
                              IHttpClientService clientService,
                              RoleManager<ApplicationRole> roleManager,
                              IOptions<AppleSarkarCred> AppCredOptions, ICacheService cacheService,
                              IErrorLogger errorLogger, 
                              IHttpClientService httpClientService,
                              ISessionService sessionService,
                              IBruteForceService bruteForceService
                              )
    {
        this.urloptions = urloptions.Value;
        this.userManager = userManager;
        this.tokenHelper = tokenHelper;
   
        this.userManagerService = userManagerService;
        this.clientService = clientService;
        this.roleManager = roleManager;
        this.cacheService = cacheService;
        this.errorLogger = errorLogger;
        appCredOptions = AppCredOptions.Value;
        this.sessionService = sessionService;
        this.bruteForceService = bruteForceService;
    }

    [HttpPost("auth")]
    [AllowAnonymous]
    public async Task<IActionResult> Auth([FromBody] string str)
    {
        string serviceResponse = string.Empty;
        string decrptedServiceResponse = string.Empty;
        try
        {
            string ClientCode = appCredOptions.ClientCode;
            string CheckSumKey = appCredOptions.CheckSumKey;
            string EncryptKey = appCredOptions.EncryptKey;
            string EncryptIV = appCredOptions.EncryptIV;
            string RequestDecryStr = Utility.SimpleTripleDesDecrypt(str, EncryptKey, EncryptIV);
            string[] param = RequestDecryStr.Split('|');
            if (param != null && param.Length > 0)
            {
                string _UsrId = param[0];
                string _UsrTimeStamp = param[1];
                string _UsrSession = param[2];
                string _ClientCheckSumValue = param[3];
                string _strServiceCookie = param[4];
                string _ChkValueRawData = string.Format("{0}|{1}|{2}|{3}|{4}", _UsrId, _UsrTimeStamp, _UsrSession, CheckSumKey, _strServiceCookie);
                try
                {
                    DataSet ds = new DataSet();

                    if (appCredOptions.Environment == "UAT")
                    {
                        try
                        {
                            DeptAuthUAT.Dept_AuthenticationSoap deptAuthUAT = new DeptAuthUAT.Dept_AuthenticationSoapClient(DeptAuthUAT.Dept_AuthenticationSoapClient.EndpointConfiguration.Dept_AuthenticationSoap);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                            var result = deptAuthUAT.GetParameterNew(new DeptAuthUAT.GetParameterNewRequest(new DeptAuthUAT.GetParameterNewRequestBody(str, ClientCode)));
                            string xmlResponse = result.Body.GetParameterNewResult;
                            serviceResponse = xmlResponse;
                            string ResponseXML =Utility.SimpleTripleDesDecrypt(xmlResponse, EncryptKey, EncryptIV);
                            decrptedServiceResponse = ResponseXML;
                            ds.ReadXml(new StringReader(ResponseXML));
                        }
                        catch (Exception ex)
                        {
                            // ExceptionLogging.LogException(Convert.ToString(ex));
                            await errorLogger.Log("Dept_AuthenticationSoap UAT", ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            DepartmentAuthenticationSOAPService.Dept_AuthenticationSoap deptAuthProd = new DepartmentAuthenticationSOAPService.Dept_AuthenticationSoapClient(DepartmentAuthenticationSOAPService.Dept_AuthenticationSoapClient.EndpointConfiguration.Dept_AuthenticationSoap);

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                            var result = deptAuthProd.GetParameterNew(new DepartmentAuthenticationSOAPService.GetParameterNewRequest(new DepartmentAuthenticationSOAPService.GetParameterNewRequestBody(str, ClientCode)));
                            string xmlResponse = result.Body.GetParameterNewResult;
                            serviceResponse = xmlResponse;
                            string ResponseXML = Utility.SimpleTripleDesDecrypt(xmlResponse, EncryptKey, EncryptIV);
                            decrptedServiceResponse = ResponseXML;
                            ds.ReadXml(new StringReader(ResponseXML));
                        }
                        catch (Exception ex)
                        {
                            // ExceptionLogging.LogException(Convert.ToString(ex));
                            await errorLogger.Log("Dept_AuthenticationSoap PROD", ex);
                        }
                    }
                    var model = new WebServiceUserModel
                    {
                        UserName = Convert.ToString(ds.Tables[0].Rows[0]["Username"]),
                        Password = Convert.ToString(ds.Tables[0].Rows[0]["Password"]),
                        EmailID = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]),
                        MobileNo = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]),
                        FullName = Convert.ToString(ds.Tables[0].Rows[0]["FullName"]),
                        FullNameInMarathi = Convert.ToString(ds.Tables[0].Rows[0]["FullName_mr"]),
                        Gender = Convert.ToString(ds.Tables[0].Rows[0]["Gender"]),
                        DateofBirth = Convert.ToString(ds.Tables[0].Rows[0]["DOB"]),
                        UIDNO = Convert.ToString(ds.Tables[0].Rows[0]["UIDNO"]),
                        PANNO = Convert.ToString(ds.Tables[0].Rows[0]["PANNo"]),
                        TrackID = Convert.ToString(ds.Tables[0].Rows[0]["TrackId"]),
                        UserID = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]),
                        Domain = "Apple Sarkar"
                    };
                    var user = await userManagerService.ApplicantAuthentication(model);
                    if (user != null)
                    {
                        model.NumericID = user.NumericId;
                        model.UserIdentity = user.Id;
                      
                     
                        var logindetails = new logindetails
                        {
                            UserIdentity = model.UserIdentity,
                            Password = model.Password,
                            EmailId = model.EmailID,
                            MobileNo = model.MobileNo,
                            UserName = model.UserName,
                            Domain = "Apple Sarkar",
                            CreatedOn = DateTime.Now,
                            UserId=model.UserID,
                            FullName = model.FullName,
                            FullName_mr=model.FullNameInMarathi,
                            Age=0,
                            DOB= model.DateofBirth,
                            IsMobileVerified= model.IsMobileVerified,
                            IsEmailVerified= model.IsEmailVerified,
                            Gender =model.Gender,
                            CreatedBy = "admin"
                        };
                        var UserRegister = await userManagerService.SaveloginDetails(logindetails);//await clientService.RequestSend<WebServiceUserModel>(HttpMethod.Post, $"{urloptions.ApplicantService}/account/save-login-details", model);


                        if (UserRegister>0)
                        {

                            var sessionid = Guid.NewGuid().ToString();
                            var userToken = tokenHelper.GenerateAccessToken(user.Id, UserRegister.ToString(), sessionid, "", "", "", "0");
                            await userManagerService.UserLoginSessionStore(user.Id, sessionid);
                            return Ok(new
                            {
                                HttpStatusCode = HttpStatusCode.OK,
                                Data = new
                                {
                                    Token = userToken,
                                    user.UserName,
                                    user.Name,
                                    LoginAt = DateTime.Now,
                                }
                            });
                        }
                        else
                        {
                            return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Data = new { Status = false, Message = "User Authentication Failed." } });
                        }
                    }
                    else
                    {
                        return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Data = new { Status = false, Message = "User Authentication Failed." } });
                    }
                }
                catch (Exception ex)
                {
                    // ExceptionLogging.LogException(Convert.ToString(ex));
                    await errorLogger.Log("CSC Error", ex);
                    return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Data = new { Status = false, Message = $"{ex.Message} ServiceResponse:{serviceResponse} DecryptedResponse:{decrptedServiceResponse}" } });
                }
            }
            else
            {
                return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Data = new { Status = false, Message = "Authentication failed." } });
            }
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("CSC Error", ex);
            return BadRequest(new { Status = false, Message = $"{ex.Message} ServiceResponse:{serviceResponse} DecryptedResponse:{decrptedServiceResponse}" });
        }
    }

    [HttpPost("auth-2")]
    [AllowAnonymous]
    public async Task<IActionResult> Auth2([FromBody] string param)
    {
        var UserId = AesAlgorithm.DecryptString(param);
        var UserRegister = new WebServiceUserModel();
        try
        {
            UserRegister = await clientService.RequestSend<WebServiceUserModel>(HttpMethod.Get, $"{urloptions.ApplicantService}/account/get-login-details?UserId={UserId}", null);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Data = new { Status = false, Message = "Some Error Occurred. Please Try Again" } });
            
        }
        if (UserRegister != null && !string.IsNullOrWhiteSpace(UserRegister.UserIdentity))
        {
            var sessionid = Guid.NewGuid().ToString();
            var userToken = tokenHelper.GenerateAccessToken(UserRegister.UserIdentity, UserRegister.ID.ToString(), sessionid, "", "", "","0");
            await userManagerService.UserLoginSessionStore(UserRegister.UserIdentity, sessionid);
            return Ok(new { HttpStatusCode = HttpStatusCode.OK, Data = new 
            {
                Token = userToken,
                UserName = UserRegister.UserName,
                Name = UserRegister.FullName,
                LoginAt = DateTime.Now,
            } });
        }
        else
        {
            return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Data = new { Status = false, Message = "User Authentication Failed." } });
        }
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        //List<string> allowedUser = new List<string>().ToList();
        //var allowedUserList = urloptions.AllowedTestuser.Split(",");
        //foreach(var item in allowedUserList)
        //{
        //    allowedUser.Add(item.ToLower());
        //}
        //if(allowedUser.Contains(model.UserName.ToLower()))
        //{
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isBruteForce = await bruteForceService.IsBruteForce(model.UserName, "Username");
            if (isBruteForce)
            {
                bruteForceService.RegisterFailedAttempt(model.UserName, "Username");
                return Ok(new { Status = false, Message = "Too many requests. You can raise next login request after 5 minutes." });
            }
            else
            {
                bruteForceService.RegisterFailedAttempt(model.UserName, "Username");
            }

            try
            {
                var IsKeyExist = await cacheService.IsKeyExists(model.UserName + model.Password);
                if (IsKeyExist)
                {
                    return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Status = false, Message = "Username or Password is incorrect" });
                }
                await cacheService.GetOrCreateAsync(model.UserName + model.Password, async () => { return await CreatePwdCacheKey(); }, TimeSpan.FromHours(12));
                model.Password = AesAlgorithm.DecryptString(model.Password);
                var password = model.Password.Substring(5, model.Password.Length - 10);
                var mds = CreateMD5(password);

                var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == model.UserName);

                if (user != null && mds.Trim().ToUpper() == user.Password)
                {
                    // var param = new WebServiceUserModel { UserIdentity = user.Id };
                    //  var UserRegister = await clientService.RequestSend<WebServiceUserModel>(HttpMethod.Post, $"{urloptions.ApplicantService}/account/save-login-details", param);
                    //var logindetails = new logindetails
                    //{
                    //    UserIdentity = user.Id,
                    //    Password = model.Password,
                    //    UserName = model.UserName,
                    //    Domain = "HMS",

                    //};
                    long numericID = await userManagerService.GetUserNumericId(user.Id);
                    if (numericID > 0)
                    {
                        var sessionid = Guid.NewGuid().ToString();
                        var userToken = tokenHelper.GenerateAccessToken(user.Id, numericID.ToString(), sessionid, "", "", "", "0");
                        await userManagerService.UserLoginSessionStore(user.Id, sessionid);
                        //await sessionService.StoreSessionInRedis(user.Id, sessionid);
                        await sessionService.StoreSessionInMemoryCache(user.Id, sessionid);
                        await cacheService.Clear(model.UserName);
                        return Ok(new
                        {
                            HttpStatusCode = HttpStatusCode.OK,
                            Token = userToken,
                            user.UserName,
                            user.Name,
                            LoginAt = DateTime.Now
                        });
                    }
                    else
                    {
                        return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Status = false, Message = "Username or Password is incorrect." });
                    }
                }
                else
                {
                    return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Status = false, Message = "Username or Password is incorrect." });
                }
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Status = false, Message = "Some Error Occurred. Please try again" });
            }
        //}
        //else 
        //{
        //    return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Status = false, Message = "User Now Allowed" });
        //}

       
    }
    [HttpGet("check-user-session")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckUserSession(string userId)
    {
        // string response = await sessionService.GetSessionFromRedis(userId);
        string response = await sessionService.GetSessionFromMemoryCache(userId);
        if (response != "")
        {
            return Ok(new
            {
                HttpStatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                Message = "Session Valid"

            });
        }
        else
        {
            return BadRequest(new
            {
                HttpStatusCode = HttpStatusCode.OK,
                IsSuccessStatusCode = false,
                Message = "Session Invalid"

            });
        }
        
    }
    [HttpPost("applicant-forgot-password-OTP-send")]
    [AllowAnonymous]
    public async Task<IActionResult> ApplicantForgotPasswordOTPSend([FromBody] ForgotPasswordModel model)
    {
        try
        {
            model.UserName = AesAlgorithm.DecryptString(model.UserName);
            model.IP = HttpContext.Connection.RemoteIpAddress?.ToString();


            var isBruteForce = await bruteForceService.IsBruteForce(model.UserName, "ApplicantForgotPassword");
            if (isBruteForce)
            {
                bruteForceService.RegisterFailedAttempt(model.UserName, "ApplicantForgotPassword");
                return Ok(new { Status = false, Message = "Too many requests. You can raise next otp request after 5 minutes." });
            }
            else
            {
                bruteForceService.RegisterFailedAttempt(model.UserName, "ApplicantForgotPassword");
            }

            var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    await clientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.NotificationService}/OTP/SMS/Send", user.PhoneNumber);
                    return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." });
                }
                else
                    return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." }); // Mobile number not registered. VAPT - Abbas
            }
            else
                return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." }); //User Not found. VAPT - Abbas

        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = "Something went wrong.", Error = ex.Message });
        }
    }
    [HttpPost("applicant-forgot-password-OTP-verify")]
    [AllowAnonymous]
    public async Task<IActionResult> ApplicantForgotPasswordOTPVerify([FromBody] ForgotPassword model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }
            model.UserName = AesAlgorithm.DecryptString(model.UserName);
            model.OTP = AesAlgorithm.DecryptString(model.OTP);
            model.IP = HttpContext.Connection.RemoteIpAddress?.ToString();


            var isBruteForce = await bruteForceService.IsBruteForce(model.IP, "ApplicantForgotPasswordOTPVerify");
            if (isBruteForce)
            {
                return Ok(new { Status = false, Message = "Too many requests. You can raise next verify request after 5 minutes." });
            }
            else
            {
                bruteForceService.RegisterFailedAttempt(model.IP, "ApplicantForgotPasswordOTPVerify");
            }

            var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    HttpClient httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{urloptions.NotificationService}/OTP/SMS/Verify");
                    string content = JsonSerializer.Serialize(new { Mobile = user.PhoneNumber, OTP = model.OTP });
                    request.Content = new StringContent(content, null, "application/json");
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var data = JsonSerializer.Deserialize<OTPVerifyResponse?>(result);
                        if (data != null && data.Status)
                        {
                            user.Password = model.NewPassword;
                            var result1 = await userManager.UpdateAsync(user);
                            return Ok(new { Status = true, Message = "Password Change Successfully." });
                        }
                        else
                        {
                            return Ok(new { Status = false, Message = data != null ? data.Message : "OTP Verify failed." });
                        }
                    }
                    else
                    {
                        return Ok(new { Status = false, Message = "OTP Verify failed." });
                    }
                }
                else
                    return Ok(new { Status = false, Message = "OTP Verify failed." }); //Mobile number not registered. VAPT - Abbas
            }
            else
                return Ok(new { Status = false, Message = "OTP Verify failed." }); // User not found VAPT - Abbas

        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = "Something went wrong.", Error = ex.Message });
        }
    }
    [HttpPost("admin-login")]
    [AllowAnonymous]
    public async Task<IActionResult> AdminLogin([FromBody] LoginRequest model)
    {

        //List<string> allowedUser = new List<string>().ToList();
        //var allowedUserList = urloptions.AllowedTestuser.Split(",");
        //foreach (var item in allowedUserList)
        //{
        //    allowedUser.Add(item.ToLower());
        //}
        //if (allowedUser.Contains(model.UserName.ToLower()))
        //{
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isBruteForce = await bruteForceService.IsBruteForce(model.UserName, "AdminUsername");
                if (isBruteForce)
                {
                    bruteForceService.RegisterFailedAttempt(model.UserName, "AdminUsername");
                    return Ok(new { Status = false, Message = "Too many requests. You can raise next login request after 5 minutes." });
                }
                else
                {
                    bruteForceService.RegisterFailedAttempt(model.UserName, "AdminUsername");
                }

                var IsKeyExist = await cacheService.IsKeyExists(model.UserName + model.Password);
                if (IsKeyExist)
                {
                    return Ok(new { Status = false, Message = "Username or Password is incorrect" });
                }
                await cacheService.GetOrCreateAsync(model.UserName + model.Password, async () => { return await CreatePwdCacheKey(); }, TimeSpan.FromHours(12));
               var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == model.UserName);
                if (user != null)
                {
                    await userManagerService.UserLoginSessionStore(user.Id, "Expired");
                    var pass = AesAlgorithm.DecryptString(model.Password);
                    var password = pass.Substring(5, pass.Length - 10);
                    var result = userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    if (result == PasswordVerificationResult.Success)
                    {
                        string hostilId = "";
                        string districtId = "";
                        string WorkFlowId = "";
                        string UserName = model.UserName;
                        bool IsFirstLogin = false;
                        WrapperDDOModel ddoDetails = null;
                        var roles = await userManager.GetRolesAsync(user);
                        int deptId = await userManagerService.GetDepartmentIdByRoleName(roles.FirstOrDefault());
                        var roleMappingId = await userManagerService.GetEntityRoleMappingId(user.Id, deptId);

                        if (roles.Contains("Applicant"))
                        {
                            return Ok(new { Status = false, Message = "Username or Password is incorrect" });
                        }

                        if (roles.Contains("Warden"))
                        {
                            try
                            {
                                var UserRegister = await clientService.RequestSend<HostelServiceUserModel>(HttpMethod.Get, $"{urloptions.HostelService}/account/{user.Id}/{deptId}", null);
                                if (UserRegister != null)
                                {
                                    hostilId = UserRegister.HostelID.ToString();
                                    IsFirstLogin = UserRegister.IsFirstLogin;
                                    UserName = UserRegister.Hostel;
                                }
                            }
                            catch (Exception ex)
                            {
                                return Ok(new { Status = false, Message = $"No hostel has been assigned to the user {model.UserName}. Please contact the administrator for assistance." });

                            }
                        }
                        else if (roles.Contains("DDO") || roles.Contains("VJNT_DDO"))
                        {
                            try
                            {
                                var m = new GenericReqModel();
                                m.UserId = user.Id;

                                ddoDetails = await clientService.RequestSend<WrapperDDOModel>(HttpMethod.Post, $"{urloptions.DDOService}ddo/ddo-by-id", m);
                            }
                            catch (Exception ex)
                            {
                                return Ok(new { Status = false, Message = $"No department has been assigned to the user {model.UserName}. Please contact the administrator for assistance." });
                            }
                        }
                        else
                        {
                            try
                            {
                                var DeptData = await clientService.RequestSend<DepartmentProfileModel>(HttpMethod.Get, $"{urloptions.HostelService}/account/Department/{user.Id}/{roleMappingId?.FirstOrDefault()}/{deptId}", null);
                                if (DeptData != null)
                                {
                                    districtId = DeptData.DistrictId != null ? DeptData.DistrictId.Value.ToString() : "";
                                    WorkFlowId = DeptData.WorkFlowId != null ? DeptData.WorkFlowId.Value.ToString() : "";
                                    IsFirstLogin = DeptData.IsFirstLogin != null ? DeptData.IsFirstLogin.Value : false;
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionLogging.LogException(Convert.ToString(ex));
                                districtId = string.Empty;

                            }
                        }
                        var sessionid = Guid.NewGuid().ToString();
                        var userToken = "";
                        if (roles.Contains("DDO") || roles.Contains("VJNT_DDO"))
                    {


                            if (!string.IsNullOrEmpty(ddoDetails.data.ddoid) && !string.IsNullOrEmpty(ddoDetails.data.ddO_Code) && !string.IsNullOrEmpty(ddoDetails.data.detailHead) && deptId > 0)
                            {
                                userToken = tokenHelper.GenerateAccessToken(user.Id, ddoDetails.data.ddoid, sessionid, roleMappingId?.FirstOrDefault().ToString(), ddoDetails.data.ddO_Code, ddoDetails.data.detailHead, Convert.ToString(deptId));
                            }
                            else
                          
                        {
                                return Ok(new { Status = false, Message = "Username or Password is incorrect" });

                            }
                        }
                        else
                        {
                            userToken = tokenHelper.GenerateAccessToken(user.Id, hostilId, sessionid, roleMappingId?.FirstOrDefault().ToString(), districtId, WorkFlowId, Convert.ToString(deptId));
                        }
                        await userManagerService.UserLoginSessionStore(user.Id, sessionid);
                        await sessionService.StoreSessionInMemoryCache(user.Id, sessionid);
                        await cacheService.Clear(model.UserName);
                        await bruteForceService.Clear(model.UserName, "AdminUsername");
                        return Ok(new
                        {
                            Token = userToken,
                            user.UserName,
                            Name = UserName,
                            LoginAt = DateTime.Now,
                            IsFirstLogin,
                            UserRole = roleMappingId?.FirstOrDefault(),
                            DeptId = deptId
                        });
                    }
                    else
                    {
                        return Ok(new { Status = false, Message = "Username or Password is Incorrect" });
                    }
                }
                else
                {
                    return Ok(new { Status = false, Message = "Username or Password is Incorrect" });
                }
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, Message = "Username or Password is Incorrect" });
            }

        //}
        //else
        //{
        //    return Ok(new { HttpStatusCode = HttpStatusCode.BadRequest, Status = false, Message = "User Now Allowed" });

        //}
            
    }
    [HttpPost("forgot-password-OTP-send")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordOTPSend([FromBody] ForgotPasswordModel model)
    {
        try
        {

            var isBruteForce = await bruteForceService.IsBruteForce(model.IP, "ForgotPassword");
            if (isBruteForce)
            {
                bruteForceService.RegisterFailedAttempt(model.IP, "ForgotPassword");
                return Ok(new { Status = false, Message = "Too many requests. You can raise next otp request after 5 minutes." });
            }
            else
            {
                bruteForceService.RegisterFailedAttempt(model.IP, "ForgotPassword");
            }

            try
            {
                model.UserName = AesAlgorithm.DecryptString(model.UserName);
            }
            catch (Exception ex)
            {
                model.UserName = "Text";
            }
           

            var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
               var roles = await userManager.GetRolesAsync(user);
              
               
                if (roles.Contains("Warden"))
                {
                    string? Mobile =string.Empty;
                    try
                    {
                        var UserRegister = await clientService.RequestSend<HostelServiceUserModel>(HttpMethod.Get, $"{urloptions.HostelService}/account/{user.Id}", null);
                        if (UserRegister != null)
                        {
                            Mobile = UserRegister?.Mobile;
                        }
                    }
                    catch (Exception ex)
                    {
                        // ExceptionLogging.LogException(Convert.ToString(ex));
                        Mobile = string.Empty;
                    }

                    if (!string.IsNullOrEmpty(Mobile))
                    {
                        await clientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.NotificationService}/OTP/SMS/Send", Mobile);
                        return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." });
                    }
                    else
                        return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." }); //Mobile number not registered. VAPT - Abbas
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        await clientService.RequestSend<object>(HttpMethod.Post, $"{urloptions.NotificationService}/OTP/SMS/Send", user.PhoneNumber);
                        return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." });
                    }
                    else
                        return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." }); //Mobile number not registered. VAPT - Abbas
                }
            }
            else
                return Ok(new { Status = true, Message = "OTP will send to your registered mobile number." }); // User not found VAPT - Abbas

        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = "Something went wrong.", Error = ex.Message });
        }
    }
    [HttpPost("forgot-password-OTP-verify")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordOTPVerify([FromBody] ForgotPassword model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }


            var isBruteForce = await bruteForceService.IsBruteForce(model.IP, "ForgotPasswordOTPVerify");
            if (isBruteForce)
            {
                return Ok(new { Status = false, Message = "Too many requests. You can raise next verify request after 5 minutes." });
            }
            else
            {
                bruteForceService.RegisterFailedAttempt(model.IP, "ForgotPasswordOTPVerify");
            }


            model.UserName = AesAlgorithm.DecryptString(model.UserName);
            
            model.OTP = AesAlgorithm.DecryptString(model.OTP);

            model.NewPassword = AesAlgorithm.DecryptString(model.NewPassword);
             
           var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                string? mobile = string.Empty;
                if (roles.Contains("Warden"))
                {
                    try
                    {
                        var UserRegister = await clientService.RequestSend<HostelServiceUserModel>(HttpMethod.Get, $"{urloptions.HostelService}/account/{user.Id}", null);

                        if (UserRegister != null)
                        {
                            mobile = UserRegister?.Mobile;
                        }
                    }
                    catch (Exception ex)
                    {
                        // ExceptionLogging.LogException(Convert.ToString(ex));
                    }
                }
                else
                    mobile = user.PhoneNumber;

                if (!string.IsNullOrEmpty(mobile))
                {
                    HttpClient httpClient = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{urloptions.NotificationService}/OTP/SMS/Verify");
                    string content = JsonSerializer.Serialize(new { Mobile = mobile, OTP = model.OTP });
                    request.Content = new StringContent(content, null, "application/json");
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var data = JsonSerializer.Deserialize<OTPVerifyResponse?>(result);
                        if (data != null && data.Status)
                        {
                            string resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                            IdentityResult passwordChangeResult = await userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
                            if (passwordChangeResult.Succeeded)
                            {
                                return Ok(new { Status = true, Message = "Password Change Successfully." });
                            }
                            else
                            {
                                return Ok(new { Status = false, Message = "OTP Verify failed." });
                            }
                        }
                        else
                        {
                            return Ok(new { Status = false, Message = data != null ? data.Message : "OTP Verify failed." });
                        }
                    }
                    else
                    {
                        return Ok(new { Status = false, Message = "OTP Verify failed." });
                    }
                }
                else
                    return Ok(new { Status = false, Message = "OTP Verify failed." });
            }
            else
                return Ok(new { Status = false, Message = "OTP Verify failed." }); // User Not found.VAPT Abbas

        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = "Something went wrong.", Error = ex.Message });
        }
    }
    [HttpPost("registration")]
    [AllowAnonymous]
    public async Task<IActionResult> Registration([FromBody] RegistrationModel model)
    {
        try
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName.Trim());
            if (user != null)
            {
                return Ok(new { Status = false, Message = "User Name Already Exists." });
            }

            var userEmail = await userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email.Trim());
            if (userEmail != null)
            {
                return Ok(new { Status = false, Message = "Email exists.." });
            }

            var userMobile = await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.Mobile.Trim());
            if (userMobile != null)
            {
                return Ok(new { Status = false, Message = "Mobile exists." });
            }

            var IsEmailVerify = await CheckVerify($"{urloptions.NotificationService}/OTP/is-email-verified", model.Email);
            if (!IsEmailVerify)
            {
                return Ok(new { Status = false, Message = "Email not verified." });

            }

            var IsMobileVerify = await CheckVerify($"{urloptions.NotificationService}/OTP/is-mobile-verified", model.Mobile);
            if (!IsMobileVerify)
            {
                return Ok(new { Status = false, Message = "Mobile Number not verified." });
            }

            var applicantRole = new ApplicationRole("Applicant");

            if (roleManager.Roles.All(r => r.Name != applicantRole.Name))
            {
                await roleManager.CreateAsync(applicantRole);
            }

            var applicant = new ApplicationUser
            {
                UserName = model.UserName.Trim(),
                Password = model.Password.Trim().ToUpper(),
                Email = model.Email.Trim(),
                PhoneNumber = model.Mobile.Trim(),
                Status = Repository.Enums.Status.Active
            };

            var result = await userManager.CreateAsync(applicant, "Password@123");
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(applicant, new[] { applicantRole.Name });
                var numericid = await userManagerService.CreateNumericId(applicant.Id);
                var logindetails = new logindetails
                {
                    UserIdentity = applicant.Id,
                    Password = model.Password,
                     EmailId = model.Email,
                    MobileNo = model.Mobile,
                    UserName = model.UserName,
                    Domain = "HMS",
                    IsEmailVerified = true,
                    IsMobileVerified = true,
                    CreatedOn= DateTime.Now,
                    CreatedBy="admin"
                };
               long numericID= await userManagerService.SaveloginDetails(logindetails);
               // var UserRegister = await clientService.RequestSend<WebServiceUserModel>(HttpMethod.Post, $"{urloptions.ApplicantService}/account/save-login-details", param);
               if(numericID>0)
                return Ok(new { Status = true, Message = "Registration Success." });
               else
                    return Ok(new { Status = false, Message = "Registration Failed ,Please try again." });
            }
            else
            {
                return Ok(new { Status = false, Message = result.Errors.Select(x => x.Description).FirstOrDefault() });
            }
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new { Status = false, Message = ex.Message });
        }
    }
    [HttpPost("check-user-exists")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckUserExists([FromBody] string Username)
    {
        if (userManager.Users.All(u => u.UserName != Username))
            return Ok(false);
        else
            return Ok(true);
    }
    private static string GenerateCheckSumValue(string reqStr)
    {
        try
        {
            System.Text.ASCIIEncoding AsciiEncoding = new System.Text.ASCIIEncoding();
            //UInt32 checksumvalue =  CRC32.Compute(AsciiEncoding.GetBytes(reqStr)); // String to bytes
            //return checksumvalue.ToString();
            return "";
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            //PAY.Utility.ErrorLog.writeLog(this, ee);
            return string.Empty;
        }
        finally
        {

        }
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await userManager.GetUserAsync(User);
        await userManagerService.UserLoginSessionStore(user.Id, "Expired");
        //await sessionService.RemoveKey(user.Id);
        await sessionService.RemoveKeyMemoryCache(user.Id);
        return NoContent();
    }
    //private async Task<List<UserBruteforceModel>?> CheckBruteforce(string UserName)
    //{
    //    var UserAttempt = await cacheService.GetOrCreateAsync(UserName, async () => { return await CreateAttempt(UserName, 1); }, TimeSpan.FromMinutes(5));
    //    if (UserAttempt != null && UserAttempt.Count > 0)
    //    {
    //        await cacheService.Clear(UserName);
    //        return await cacheService.GetOrCreateAsync(UserName, async () => { return await CreateAttempt(UserName, UserAttempt.Count + 1); }, TimeSpan.FromMinutes(5));
    //    }
    //    else
    //        return UserAttempt;
    //}
    //private async Task<List<UserBruteforceModel>> CreateAttempt(string User, int Attempt)
    //{
    //    var list = new List<UserBruteforceModel>();
    //    for (int i = 0; i < Attempt; i++)
    //    {
    //        list.Add(new UserBruteforceModel { User = User, Attempt = i + 1, BlockTime = DateTime.Now.AddMinutes(5) });
    //    }
    //    return list;
    //}
    private async Task<bool> CheckVerify(string URL, string param)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, URL);
            string content = JsonSerializer.Serialize(param);
            request.Content = new StringContent(content, null, "application/json");
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<bool>(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return false;
        }
    }
    public static string CreateMD5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
    private async Task<bool> CreatePwdCacheKey()
    {

        return true;
    }

    [HttpGet("Update-AadharStatus")]
   [AllowAnonymous]
    public async Task<IActionResult> UpdateAadharStatus(bool IsAadharVerified , long UserId)
    {
        try
        {
            var aadhardata = await userManagerService.UpdateAadharStatus(IsAadharVerified, UserId);
            if(aadhardata)
               return Ok(new { Status = true, Message = "Aadhar verified successfully" });
            else
                return Ok(new { Status = false, Message = "Something went wrong." });
        }
        catch (Exception ex)
        {
            
            return Ok(new { Status = false, Message = "Something went wrong."});
        }
    }
    [HttpGet("check-verified-status")]
    public async Task<IActionResult> VerifiedStatus()
    {
        try
        {
           

            var result = await userManagerService.Getlogindetails();

            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new VerifiedStatusModel());
        }
    }
    [HttpGet("logindetails-ByUserId")]
    [AllowAnonymous]
    public async Task<IActionResult> GetlogindetailsByAadharRefNo(long userId)
    {
        try
        {


            var result = await userManagerService.GetlogindetailsByUserId(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new Applicantdetails());
        }
    }


    [HttpGet("check-applicant-data-availble")]
    
    public async Task<IActionResult> CheckApplicantData(string academicYear)
    {
        var CheckApplicationData = new DbtIntgrationModel();
        CheckApplicationData = userManagerService.CheckApplicantDataAvailbility(academicYear);
        if (CheckApplicationData != null)
        {
            if (CheckApplicationData.Result == 0)
            {
                //var applicantPrefilledData = httpClientService.RequestSend<object>(HttpMethod.Get, $"{urloptions.DbtIntegration}/HmsValidateApplicant/hms-validate-applicant-data?AadhaarReferenceNumber={1120283057393356800}&AcademicYear={academicYear}", null);


                var httpClientService = new HttpClient();

                //string aadhaarNumberRefNo = "1170995935935287296"; //Yash Bhai
                string aadhaarNumberRefNo = CheckApplicationData.AadharRefNo;
                string academicYear1 = academicYear;
                string requestUrl = $"{urloptions.DbtIntegration}/HmsValidateApplicant/hms-validate-applicant-data?AadhaarReferenceNumber={aadhaarNumberRefNo}&AcademicYear={academicYear1}";

                // Send GET request
                HttpResponseMessage response = await httpClientService.GetAsync(requestUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Raw JSON Response:");
                Console.WriteLine(jsonResponse);

                if (response.IsSuccessStatusCode)
                {
                    // Parse first-level JSON
                    JObject responseObject = JObject.Parse(jsonResponse);

                    // Extract "result" (which is a stringified JSON)
                    string resultJson = responseObject["result"]?.ToString();
                    Console.WriteLine($"Extracted Result JSON String: {resultJson}");

                    if (!string.IsNullOrEmpty(resultJson))
                    {
                        // Parse the JSON string inside "result"
                        JObject resultData = JObject.Parse(resultJson);
                        Console.WriteLine("Deserialized Result Object:");
                        Console.WriteLine(resultData.ToString());
                        var result = userManagerService.InsertApplicantPrefilledData(resultData.ToString(), academicYear); // give try catch for this insertion
                        if (result != null && result != -1)
                        {
                            if (result == 1)
                            {
                                return Ok(new { Status = true, Message = "Applicant details inserted succesfully", Data = result });
                            }
                            else if (result == 0)
                            {
                                return Ok(new { Status = false, Message = "Applicant data already availble in SJSA", Data = result });
                            }
                        }
                        else if (result == -1)
                        {
                            return Ok(new { Status = false, Message = "Something went wrong as Insertion failed", Data = result });
                        }

                    }
                    else
                    {
                        return Ok(new { Status = false, Message = "Applicant Data not availble in DBT" });
                    }
                }

            }
            return Ok(new { Status = true, Message = "Data  Available in Autofil Table.", Data = CheckApplicationData });
        }
        else
        {
            return Ok(new { Status = false, Message = "Some Error Occured Please try after some time" });
        }
    }
}
