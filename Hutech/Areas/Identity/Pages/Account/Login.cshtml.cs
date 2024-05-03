﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Hutech.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Hutech.Application;
using Hutech.Services;
using Hutech.Core.Constants;

namespace Hutech.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly LanguageService _languageService;
        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration,LanguageService languageService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
            _languageService = languageService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        public TranslationModel Translate { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        ///
        public class TranslationModel
        {
            public string SignInToYourAccount { get; set; }
            public string Login { get; set; }
            public string Forgotpassword { get; set; }
            public string UserNamePlaceHolder { get; set; }
            public string PasswordPlaceHolder { get; set; }
        }
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            //[EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            public string Login_Register { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            Translate = new TranslationModel();
            Translate.SignInToYourAccount = _languageService.Getkey("SignInToYourAccount");
            Translate.Login = _languageService.Getkey("Login");
            Translate.Forgotpassword = _languageService.Getkey("Forgotpassword");
            Translate.UserNamePlaceHolder = _languageService.Getkey("UserNamePlaceHolder");
            Translate.PasswordPlaceHolder=_languageService.Getkey("PasswordPlaceHolder");
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //_logger.LogInformation($"Logged out user{DateTime.Now}");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            string userId = string.Empty;
            returnUrl ??= Url.Content("~/");
            string BaseUrl = _configuration["Baseurl"];
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //using (var client = new HttpClient())
            //{

            //    client.BaseAddress = new Uri("http://localhost:5266/api/");
            //    client.DefaultRequestHeaders.Clear();
            //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    string append_url_for_Login = "Auth/Login";
            //    AuthServiceModel model = new AuthServiceModel();
            //    model.UserEmail = Input.Email;
            //    model.UserPassword = Input.Password;
            //    model.UserEmail = Input.Email;
            //    BaseUrl += append_url_for_Login;

            //    var json = JsonConvert.SerializeObject(model);
            //    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            //    HttpResponseMessage Res = await client.PostAsync(BaseUrl, stringContent);


            //    if (Res.IsSuccessStatusCode)
            //    {
            //        var content = await Res.Content.ReadAsStringAsync();
            //        var user = await _userManager.FindByNameAsync(Input.Email);
            //        var authClaims = new List<Claim>
            //        {
            //            new Claim(ClaimTypes.Name, user.UserName),
            //            new Claim(ClaimTypes.Email, user.Email),
            //            new Claim("User_Id", user.Id.ToString()),
            //        };
            //        JObject root = JObject.Parse(content);
            //        string retMessage = root["value"]["token"].ToString();
            //        //string accessToken = retMessage["token"].ToString();

            //        string token = retMessage.ToString();
            //        SetJWTCookie(retMessage);

            //        if (!string.IsNullOrEmpty(token))
            //        {
            //            var handler = new JwtSecurityTokenHandler();

            //            token = token.Replace("Bearer ", "");
            //            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            //            userId = Guid.Parse(tokenS.Claims.First(x => x.Type == "nameid").Value).ToString();
            //        }
            //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        return Redirect("Home/Index");
            //        //return Redirect("~/admin/LoanApplication/GetAllLoanRequest");
            //        //return RedirectToAction("GetAllLoanType", "LoanType",new { Areas="Admin"});
            //        //return RedirectToAction("~/Views/Admin/LoanType/GetAllLoanType");
            //        //return RedirectToAction("GetAllLoanType", "LoanType", new { area = "Admin" });
            //        //return RedirectToAction("/Admin/LoanType/GetAllLoanType");
            //        return RedirectToAction("GetAllLoanType", "LoanType", new { area = "admin" }); ;

            //    }


            //}
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByNameAsync(Input.Email);
                var userRoles = await _userManager.GetRolesAsync(user);
                string userrole = string.Empty;
                if (userRoles.Count > 0)
                {
                    foreach(var role in userRoles)
                    {
                        userrole= userrole+","+ role.ToString();
                    }
                }
                if(userrole.StartsWith(","))
                    userrole=userrole.TrimStart(',');
                if (userrole.EndsWith(","))
                    userrole = userrole.TrimEnd(',');
                HttpContext.Session.SetString("UserRole", userrole.ToString());
                HttpContext.Session.SetString("LoogedInUser", Input.Email.ToString());
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {


                    var authClaims = new List<Claim>();
                    authClaims.Add(new Claim(ClaimTypes.Name, string.Concat(user.UserName)));
                    authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
                    authClaims.Add(new Claim("User_Id", user.Id.ToString()));
                    var identity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal, new AuthenticationProperties() { });

                    // Set current principal
                    var accessToken = GetToken(authClaims);
                    SetJWTCookie(accessToken);
                    //call api to add entry in audit table and pass loggedinuser Id to api project to save in session
                    string apiUrl = _configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                        HttpResponseMessage response = await client.GetAsync(string.Format("Login/Login/{0}", user.Id));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                        }
                        List<ConfigurationViewModel> configure = new List<ConfigurationViewModel>();
                        HttpResponseMessage configurationresponse = await client.GetAsync(string.Format("Configuration/GetAllConfiguration"));
                        if (configurationresponse.IsSuccessStatusCode)
                        {
                            var content = await configurationresponse.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            configure = root["result"].ToObject<List<ConfigurationViewModel>>();
                            HttpContext.Session.SetString("FileType", configure.First().FileType);
                            HttpContext.Session.SetString("FileSize", configure.First().FileSize);
                        }
                    }



                    _logger.LogInformation($"User logged in {Input.Email} {DateTime.Now}");
                    //_logger.LogDebug(message: $"User logged in  {Input.Email} {DateTime.Now}");

                    //Get user permission menu according to role
                    string BaseURL = _configuration["Baseurl"];
                    var loggedinUserId = HttpContext.Session.GetString("UserId");
                    string URL = string.Format("Menu/GetLoggedInUserMenuPermission/{0}", loggedinUserId);
                    List<MenuViewModel> menu = new List<MenuViewModel>();
                    BaseURL += URL;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage responsedataForMenu = await client.GetAsync(BaseURL);
                        {
                            if (responsedataForMenu.IsSuccessStatusCode)
                            {
                                var contentdata = await responsedataForMenu.Content.ReadAsStringAsync();

                                JObject rootdata = JObject.Parse(contentdata);
                                menu = rootdata["result"].ToObject<List<MenuViewModel>>();
                            }
                        }
                    }
                    //HttpContext.Session.SetComplexData("leftsideMenu", menu);

                    //End get user permission menu according to role
                    HttpContext.Session.SetComplexData("leftsideMenu", menu);

                    var data = HttpContext.Session.GetComplexData<List<MenuViewModel>>("leftsideMenu");
                    List<LeftSideMenu> menus = new List<LeftSideMenu>();


                    var submenu = data.Where(x => x.ParentId > 0).Select(x => x.ParentId).ToList();
                    submenu = submenu.Distinct().ToList();
                    foreach (var item in submenu)
                    {
                        var submenus = data.Where(x => x.ParentId == item).ToList();
                        LeftSideMenu menudata = new LeftSideMenu();
                        menudata.ParentName = submenus[0].ParentName; ;
                        menudata.ParentId = submenus[0].ParentId;
                        foreach (var itemdata in submenus)
                        {
                            menudata.Sort = itemdata.sort;
                            menudata.ParentId = itemdata.ParentId;
                            SubMenu subMenu = new SubMenu();
                            subMenu.SubMenuName = itemdata.Name;
                            subMenu.URL = itemdata.URL;
                            subMenu.Sort = itemdata.sort;
                            menudata.SubMenus.Add(subMenu);

                        }

                        menus.Add(menudata);

                    }
                    var mainmenu = data.Where(x => x.ParentId == 0 && x.URL !=null).ToList();
                    foreach (var item in mainmenu)
                    {
                        LeftSideMenu menudata = new LeftSideMenu();
                        menudata.ParentName = item.Name;
                        menudata.URL = item.URL;
                        menudata.Sort = item.sort;
                        menus.Add(menudata);
                    }
                    //ViewBag.Data = menus.ToList();
                    menus = menus.OrderBy(x => x.Sort).ToList();
                    HttpContext.Session.SetComplexData("leftside", menus.ToList());
                    return Redirect("Home/Index");

                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        private string GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            int tokenExpireHours = System.Convert.ToInt32(_configuration["JWT:TokenExpireHours"]);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(tokenExpireHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private void SetJWTCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(3),
            };
            Response.Cookies.Append("jwtCookie", token, cookieOptions);
        }
    }

}
