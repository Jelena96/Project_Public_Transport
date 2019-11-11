using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;
using WebApp.Providers;
using WebApp.Results;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        public IUnitOfWork db { get; set; }

        //promena 8.3.2019
        public AccountController(IUnitOfWork db)
        {
            this.db = db;
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

       
        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [ResponseType(typeof(ApplicationUser))]
        [Route("UserInfo")]
        public IHttpActionResult GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                                    .GetUserManager<ApplicationUserManager>()
                                    .FindById(User.Identity.GetUserId());

            return Ok(user);
        }

        [Authorize(Roles = "AppUser")]
        [Route("UpdateUser")]
        public IHttpActionResult PutUser([FromBody]RegisterBindingModel user)
        {
            string rez = "";
            ApplicationUser app = UserManager.FindByEmail(user.Email);
            if (app != null)
            {
                app.FirstName = user.FirstName;
                app.LastName = user.LastName;
                app.Email = user.Email;
                app.Address = user.Address;
                app.UserName = user.Email;

                if (!user.ImageUrl.Equals(""))
                {
                   app.VerificationStatus = "In Progress";
                   string imgUrl = MakePath(user);
                   app.ImageUrl = imgUrl;
                }
                else
                {
                    if (app.ImageUrl != "")
                    {
                    }
                    else
                    {
                        app.VerificationStatus = "Invalid";
                    }
                }
            }
        
            IdentityResult res = UserManager.Update(app);
            SendEmail(user.Email, $"Your varification status of profile is : " + app.VerificationStatus + ".");
            
            if (!res.Succeeded)
            {
                foreach (var r in res.Errors)
                {
                    rez += r;
                }
                return BadRequest(rez);
            }
            else
            {
                return Ok("ok");
            }
        }

        private void SendEmail(string recipient, string body)
        {
            {
                if (String.IsNullOrEmpty(recipient))
                    return;
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(recipient);
                    mail.From = new MailAddress("bajicbjelena@gmail.com");
                    mail.Subject = "Status verifikacije";
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential
                         ("youremail", "yourpassword"); // ***use valid credentials***
                    smtp.Port = 587;

                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception in sendEmail:" + ex.Message);
                }
            }
        }
        //Save user photo or document, convert to byte array
        public string MakePath(RegisterBindingModel user)
        {
            string imgUrl = "";
            if (user.ImageUrl != "" && user.ImageUrl != null)
            {
                byte[] imageBytes = Convert.FromBase64String(user.ImageUrl);
                // Convert byte[] to Image
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    Image image = Image.FromStream(ms, true);
                    // image = resizeImage(image, new Size(500, 500));
                    try
                    {
                        image.Save(@"C:\Users\Jelena\Desktop\slike 18.07.2019" + user.Email + ".jpg");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    imgUrl = @"C:\Users\Jelena\Desktop\slike 18.07.2019" + user.Email + ".jpg";
                }
            }
            return imgUrl;
        }

        //Get specific user for changing profile
        [Authorize(Roles = "AppUser")]
        [Route("UserInformation")]
        public RegisterBindingModel GetUser(string email)
        {
            var email1 = Request.GetOwinContext().Authentication.User.Identity.Name;

            ApplicationUser app = UserManager.FindByEmail(email);
            string base64String = "";
            Passenger passenger = db.Passengers.Find(o => o.UserName == email).FirstOrDefault();
            RegisterBindingModel rbm = new RegisterBindingModel();
            PassengerType passengerType = db.PassengerTypes.Find(o => o.Name == passenger.TypeOfPassenger).FirstOrDefault();

            if (app.ImageUrl != "" && app.ImageUrl != null)
            {
                Image image = Image.FromFile(app.ImageUrl);
                using (MemoryStream ms = new MemoryStream())
                {
                  //Convert Image to byte[]
                  image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                  byte[] imageBytes = ms.ToArray();

                  //Convert byte[] to base 64 string
                  base64String = Convert.ToBase64String(imageBytes);
                }
            }
            rbm.VerificationStatus = "InProgress";
            if (app != null)
            {
                rbm.FirstName = app.FirstName;
                rbm.LastName = app.LastName;
                rbm.Address = app.Address;
                rbm.IDtypeOfUser = passengerType.Id;
                rbm.BirthDate = app.BirthDate;
                rbm.Email = app.Email;
                rbm.VerificationStatus = app.VerificationStatus;
                rbm.ImageUrl = base64String;
                return rbm;
          }
          return rbm;
        }

        //Controler validated profile and send email about this information
        [Authorize(Roles = "Controller")]
        [Route("Validate")]
        public IHttpActionResult PutValidate([FromBody]RegisterBindingModel user)
        {
            ApplicationUser app = UserManager.FindByEmail(user.Email);
            app.VerificationStatus = user.VerificationStatus;
            IdentityResult res = UserManager.Update(app);

            if (!res.Succeeded)
                return BadRequest();
            else
            {
                SendEmail(app.Email, $"Controller has checked your profile! {Environment.NewLine} Your varification status of profile is : " + app.VerificationStatus + ".");
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        //Which profiles can be validate
        [Authorize(Roles = "Controller")]
        [Route("GetUsers")]
        public List<RegisterBindingModel> Get()
        {
            List<RegisterBindingModel> users = new List<RegisterBindingModel>();

            users = UserManager.Users.Where(v => v.ImageUrl != "" && (v.VerificationStatus == "Invalid" || v.VerificationStatus == "In Progress")).Select(u => new RegisterBindingModel() { FirstName = u.FirstName, LastName = u.LastName, ImageUrl = u.ImageUrl, Email = u.Email, VerificationStatus = u.VerificationStatus }).ToList();

            foreach (var u in users)
            {
                u.ImageUrl = MakeImg(u.ImageUrl);
            }
            return users;
        }

        //Transform bytes into tobase64String format for view
        public string MakeImg(string imgUrl)
        {
            string base64String = "";

            if (imgUrl != "" && imgUrl != null)
            {
                Image image = Image.FromFile(imgUrl);
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to base 64 string
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            return base64String;
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
