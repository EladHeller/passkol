using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PaskolProd.Models;
using Model.Users;
using Model.Models;
using Model.Filters;
using System.Net;
using System.Collections.Generic;
using System.Web.Caching;
using BotDetect.Web;
using PaskolProd.Areas.Admin.Controllers;
using System.Linq;
using PaskolProd.Models.Response;
using Services;
using Services.PDFService;
using Infrastructure.Configuration;
using Model.Confirm;
using Model.Logs;
using PaskolProd.Controllers.Helper;

namespace PaskolProd.Paskol.Controllers
{
    [Authorize]
    public class AccountController : UserBaseController
    {
        private FileSystemService FSservice;
        private PDFServiceSoapClient _pdfService;
        private ConfirmationService _conSrv;
        private UserActionService _userActionLogSrv;

        public AccountController()
        {
            FSservice = ServiceLocator.GetService<FileSystemService>();
            _pdfService = ServiceLocator.GetService<PDFServiceSoapClient>();
            _conSrv = ServiceLocator.GetService<ConfirmationService>();
            _userActionLogSrv = ServiceLocator.GetService<UserActionService>();
        }

        public Dictionary<string,string[]> DicModelState
        {
            get
            {
                return ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PaskolUser nameFrom = UserManager.FindByEmail(model.Email);
                    if (nameFrom != null)
                    {
                        var result = await SignInManager.PasswordSignInAsync(nameFrom.UserName, model.Password,
                            model.RememberMe, shouldLockout: true);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                var SingedUser = UserManager.FindByEmail(model.Email);
                                var newLogRes = _userActionLogSrv.Add(new UserActiveLog()
                                {
                                    ActionDate = DateTime.Now,
                                    UserId = SingedUser.Id
                                });
                                return Json(new { success = true });
                            case SignInStatus.LockedOut:

                            case SignInStatus.Failure:
                            default:
                                return Json(new { success = false });
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return Json(new { m = e.Message });
            }
            
            
        }
        
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterArtist(ArtistRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool ajaxValidationResult = CaptchaControl.AjaxValidate
                    (model.CaptchaId, model.CaptchaInput, model.InstanceId);

                if (ajaxValidationResult)
                {
                    try
                    {
                        Artist artist = new Artist()
                        {
                            ContactManName = model.ContactManName,
                            ContactManPhone = model.ContactManPhone,
                            ParticipateInAuction = true
                        };

                        var user = new PaskolUser
                        {
                            RegisteredDate = DateTime.Now,
                            UserName = model.Name,
                            Email = model.Email,
                            Artist = artist,
                            UserType = UserType.Artist,
                            Status = UserStatus.WaitingNewArtist
                        };

                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            // assign user artist to role
                            var roleResoult = await UserManager.AddToRoleAsync(user.Id,
                                UserType.Artist.ToString());

                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            // Add to confirmation waiting
                            _conSrv.Add(new Confirmation()
                            {
                                ConfirmType = ConfirmType.NewArtist,
                                DateUpdate = DateTime.Now,
                                EntityId = user.Id,
                                Name = user.UserName
                            });

                            // email to artist
                            EmailService.RegisterArtist(model.Email, model.Name, model.Password);
                            await _pdfService.ArtistPermissionAgreementAsync(DateTime.Now, user.UserName, user.Email, user.Id, WebConf.FSBaseRoute);
                            return Json(new { suceeded = true, UserName = model.Name });
                        }

                        AddErrors(result);
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                            "ארעה שגיאה אנא פנה לתמיכה");
                    }
                }
                else
                {
                    // handle not valid captcha
                    _errors.Add(new KeyValuePair<string, string>("Captcha", ""));
                }
            }

            return Json(new { suceeded = false, errors = _errors });
        }

        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterCustomer(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool ajaxValidationResult = CaptchaControl.AjaxValidate
                    (model.CaptchaId, model.CaptchaInput, model.InstanceId);

                if (ajaxValidationResult)
                {
                    try
                    {
                        // Set customer details
                        Customer customer = new Customer() { CompanyName = model.CompanyName };
                        var user = new PaskolUser { RegisteredDate = DateTime.Now, UserName = model.Name, 
                            Email = model.Email, Customer = customer, UserType = UserType.Customer, Status = UserStatus.Active };

                        // Create user  
                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            // assign user to role
                            var roleResoult = await UserManager.AddToRoleAsync(user.Id, 
                                UserType.Customer.ToString());
                            
                            // Sign in
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            // email to customer
                            EmailService.RegisterCustomer(model.Email, model.Name, model.Password);

                            return Json(new { suceeded = true, UserName = model.Name });
                        }

                        AddErrors(result);
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, 
                            "ארעה שגיאה אנא פנה לתמיכה");
                    }
                }
                else
                {
                    // handle not valid captcha
                    _errors.Add(new KeyValuePair<string, string>("Captcha", ""));
                }
            }

            return Json(new { suceeded = false, errors = _errors });
        }

        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterArtistAgent(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool ajaxValidationResult = CaptchaControl.AjaxValidate
                   (model.CaptchaId, model.CaptchaInput, model.InstanceId);

                if (ajaxValidationResult)
                {
                    try
                    {
                        // Set customer details
                        ArtistAgent artistAgent = new ArtistAgent() { CompanyName = model.CompanyName };
                        var user = new PaskolUser
                        {
                            RegisteredDate = DateTime.Now,
                            UserName = model.Name,
                            Email = model.Email,
                            ArtistAgent = artistAgent,
                            UserType = UserType.ArtistAgent,
                            Status = UserStatus.Active
                        };

                        // Create user  
                        var result = await UserManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            // assign user to role
                            var roleResoult = await UserManager.AddToRoleAsync(UserManager.FindByName(model.Name).Id,
                                UserType.ArtistAgent.ToString());

                            // Sign in
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                            // email to artist agent
                            EmailService.RegisterArtistAgnt(model.Email, model.Name, model.Password);

                            return Json(new { suceeded = true, UserName = model.Name });
                        }

                        AddErrors(result);
                    }
                    catch (Exception ex)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                            "ארעה שגיאה אנא פנה לתמיכה");
                    }
                }
                else
                {
                    // handle not valid captcha
                    _errors.Add(new KeyValuePair<string, string>("Captcha", ""));
                }
            }

            return Json(new { suceeded = false, errors = _errors });
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Search");
        }

        [HttpPost]
        [AllowAnonymous]
        [PreventRepeatedRequet]
        [AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || (await UserManager.IsLockedOutAsync(user.Id)) || user.RecoverPasswordLockOut)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Json(new { suceeded = false, Message = "אין לך אפשרות לשחזר סיסמה" });
                }
                
                Random generator = new Random();
                string code = generator.Next(0, 1000000).ToString("D6");
               
                //Check wather code is already sent to suer
                if (HttpContext.Cache["RP" + user.Id] != null)
                {
                    HttpContext.Cache.Remove("RP" + user.Id);
                }
                
                HttpContext.Cache.Add("RP" + user.Id, code, null, DateTime.Now.AddMinutes(5),Cache.NoSlidingExpiration,
                    CacheItemPriority.Default,null);
                // =  UserManager.GeneratePasswordResetToken(user.Id);
                EmailService.ResetPassword(code, model.Email, user.UserName);
                return Json(new { suceeded = true, Message = "קוד לאיפוס סיסמה נשלח לך למייל" });
            }

            // If we got this far, something failed, redisplay form
            return Json(new { suceeded = false, Message = "מייל לא תקין" });
        }

        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public async Task<JsonResult> updateCusomerProfile(CustomerEditView EditedCustomer, string property)
        {
            bool Succeeded = false;
            PaskolUser user = UserManager.FindById(User.Identity.GetUserId());

            if (user != null)
            {
                // Save artist details
                if (property == "Name")
                {
                    if (Succeeded = ModelState.IsValidField("Name"))
                    {
                        user.UserName = EditedCustomer.Name;
                    }
                }
                else if (property == "Email")
                {
                    if (Succeeded = ModelState.IsValidField("Email"))
                    {
                        user.Email = EditedCustomer.Email;
                    }
                }
                else if (property == "Phone")
                {
                    if(Succeeded = ModelState.IsValidField("Phone"))
                    {
                        user.PhoneNumber = EditedCustomer.Phone;
                    }
                }
                else if (property == "CompanyName")
                {
                    if (Succeeded = ModelState.IsValidField("CompanyName"))
                    {
                        user.Customer.CompanyName = EditedCustomer.CompanyName;
                    }
                }
                else if (property == "Password")
                {
                    Succeeded = ModelState.IsValidField("Password");
                }

                if ((Succeeded) && await UpdateUserAsync(user, EditedCustomer.Password))
                {
                    Succeeded = true;
                }
                else
                {
                    Succeeded = false;
                }
            }

            return Json(new ActionResponse() { Succeeded = Succeeded , FiledErrors = DicModelState });
        }
        
        [HttpPost]
        [AllowAnonymous]
        //[AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> updateArtistProfile(ArtistEditView EditedArtist, 
            string property, HttpPostedFileBase upload)
        {
            bool Succeeded = false;
            PaskolUser user = UserManager.FindById(User.Identity.GetUserId());

            if (user != null)
            {
                // Save artist details
                if (property == "Photo")
                {
                    var res = FSservice.StorePhoto(upload, user, user.Artist.PhotoID);
                    if (Succeeded = res.Success)
                    {
                        user.Artist.PhotoID = res.PhotoId;
                    }
                    else
                    {
                        ModelState.Add(new KeyValuePair<string, System.Web.Mvc.ModelState>("Photo", new ModelState()));
                        ModelState.AddModelError("Photo", res.Message);
                    }
                }
                if (property == "Name")
                {
                    if (Succeeded = ModelState.IsValidField("Name"))
                    {
                        if (user.Artist.TempArtist == null)
                        {
                            user.Artist.TempArtist = new TempArtist();
                        }

                        user.Artist.TempArtist.UserName = EditedArtist.Name;
                    }
                }
                else if (property == "PublicPage")
                {
                    if (Succeeded = ModelState.IsValidField("PublicPage"))
                    {
                        user.Artist.PageUrl = EditedArtist.PublicPage;
                    }
                }
                else if (property == "ContactManName")
                {
                    if (Succeeded = ModelState.IsValidField("ContactManName"))
                    {
                        user.Artist.ContactManName = EditedArtist.ContactManName;
                    }
                }
                else if (property == "ContactManPhone")
                {
                    if (Succeeded = ModelState.IsValidField("ContactManPhone"))
                    {
                        user.Artist.ContactManPhone = EditedArtist.ContactManPhone;
                    }
                }
                else if (property == "Biography")
                {
                    if (Succeeded = ModelState.IsValidField("Biography"))
                    {
                        user.Artist.Biography = EditedArtist.Biography;
                    }
                }
                else if (property == "Email")
                {
                    if (Succeeded = ModelState.IsValidField("Email"))
                    {
                        //user.Email = EditedArtist.Email;
                        if(user.Artist.TempArtist == null)
                        {
                            user.Artist.TempArtist = new TempArtist();
                        }

                        user.Artist.TempArtist.Email = EditedArtist.Email;
                    }
                }
                else if (property == "Password")
                {
                    Succeeded = ModelState.IsValidField("Password");
                }

                if ((Succeeded) && await UpdateUserAsync(user, EditedArtist.Password))
                {
                    Succeeded = true;

                    // Save new waiting for confirm if name or email has been changed
                    if (property == "Name" || property == "Email")
                    {
                        // Build the Confirmation object
                        var confEntity = _conSrv.GetByEntityId(user.Id);
                        if (confEntity.Success && confEntity.Entity != null)
                        {
                            confEntity.Entity.DateUpdate = DateTime.Now;
                            confEntity.Entity.Name = user.UserName;
                        }
                        else
                        {
                            _conSrv.Add(new Confirmation()
                            {
                                ConfirmType = ConfirmType.UpdateArtist,
                                DateUpdate = DateTime.Now,
                                EntityId = user.Id,
                                Name = user.UserName
                            });
                        }
                    }
                }
                else
                {
                    Succeeded = false;
                }
            }

            if (property == "Photo")
            {
                return RedirectToAction("MyProfile", "Artist");
            }

            return Json(new ActionResponse() { Succeeded = Succeeded, FiledErrors = DicModelState });
        }

        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                // If user can recover his password
                if (!user.RecoverPasswordLockOut)
                {
                    if (user != null)
                    {
                        // if code is correct
                        if ((string)HttpContext.Cache.Get("RP" + user.Id) == model.Code)
                        {
                            // reset recover password attempt
                            user.RecoverPasswordAttempt = 0;

                            UserManager.RemovePassword(user.Id);
                            UserManager.AddPassword(user.Id, model.Password);

                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return Json(new { suceeded = true, Message = "הסיסמה עודכנה בהצלחה" });
                        }
                        else // If code is wrong
                        {
                            user.RecoverPasswordAttempt += 1;
                            string message = "קוד לא תקין";
                            bool recoverFailed = false;

                            if (user.RecoverPasswordAttempt == 5)
                            {
                                user.RecoverPasswordReleased = DateTime.Now.AddMinutes(30);
                                user.RecoverPasswordAttempt = 0;
                                message = "תהליך איפוס הסיסמא נכשל";
                                recoverFailed = true;
                            }

                            // save changes
                            UserManager.Update(user);
                            return Json(new { suceeded = false, Message = message, RecoverFailed = recoverFailed });
                        }
                    }
                }
                else
                {
                    return Json(new { suceeded = false,  RecoverFailed = true });
                }
            }

            return null;
        }

        #region Helpers

        private UploadedFile RetrieveFileFromRequest()
        {
            string filename = null;
            string fileType = null;
            byte[] fileContents = null;

            if (Request.Files.Count > 0)
            { //they're uploading the old way
                var file = Request.Files[0];
                fileContents = new byte[file.ContentLength];
                fileType = file.ContentType;
                filename = file.FileName;
            }
            else if (Request.ContentLength > 0)
            {
                fileContents = new byte[Request.ContentLength];
                Request.InputStream.Read(fileContents, 0, Request.ContentLength);
                filename = Request.Headers["X-File-Name"];
                fileType = Request.Headers["X-File-Type"];
            }

            return new UploadedFile()
            {
                Filename = filename,
                ContentType = fileType,
                FileSize = fileContents != null ? fileContents.Length : 0,
                Contents = fileContents
            };
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private List<KeyValuePair<string, string>> _errors = new
            List<KeyValuePair<string, string>>();
        
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Contains("Name"))
                {
                    _errors.Add(new KeyValuePair<string, string>("Name", "שם זה כבר תפוס במערכת"));
                }
                else if(error.Contains("Email"))
                {
                    _errors.Add(new KeyValuePair<string, string>("Email", "איימיל זה כבר תפוס במערכת"));
                }
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}