using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WorldOfWords.API.Models;
using WorldOfWords.API.Models.Models;
using WorldOfWords.Domain.Services.IServices;
using WorldOfWords.Validation;
using WorldOfWords.Domain.Services.MessagesAndConsts;

namespace WorldofWords.Controllers
{
    [RoutePrefix("api/register")]
    public class RegisterController : BaseController
    {
        private readonly IIncomingUserMapper _incomingUserMapper;
        private readonly IUserService _service;
        private readonly ITokenValidation _token;
        private readonly TokenModel _tokenModel;
        private readonly IIdentityMessageService _emailService;

        public RegisterController(IUserService userService,
            ITokenValidation token, IIncomingUserMapper incomingUserMapper,
            IIdentityMessageService emailService)
        {
            _service = userService;
            _token = token;
            _incomingUserMapper = incomingUserMapper;
            _emailService = emailService;
            _tokenModel = new TokenModel();
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> Post(RegisterUserModel args)
        {
            var newUser = _incomingUserMapper.ToIncomingUser(args);
            if (!_service.Exists(newUser))
            {
                _service.Add(newUser);
                Guid randomPart = Guid.NewGuid();
                var tokenToHash = randomPart.ToString() + newUser.Id.ToString();
                _tokenModel.EmailConfirmationToken = _token.GetHashSha256(tokenToHash);

                var code = _tokenModel.EmailConfirmationToken;
                newUser.Token = code;
                _service.AddToken(newUser);

                var callbackUrl = new Uri(Url.Link("AngularRoute", new { }));

                await (_emailService).SendAsync(new IdentityMessage
                {
                    Body = String.Format(MessagesContainer.ConfiramtionMessage + callbackUrl + "#/EmailConfirmed?id={0}&code={1}", newUser.Id, code),
                    Destination = newUser.Email,
                    Subject = "Registration confirmation at WoW"
                });

                return Ok(_tokenModel);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public IHttpActionResult ConfirmEmail(int userId = 0, string code = "")
        {
            bool isEmptyUserId = (userId == 0);
            if (isEmptyUserId || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }
            if (_service.ConfirmUserRegistration(userId, code))
            {
                return Ok();
            }
            return BadRequest("Invalid data");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordUserModel model)
        {
            if (_service.CheckUserEmail(model))
            {
                var tokenToHash = model.Id.ToString();
                _tokenModel.ForgotPasswordToken = _token.GetHashSha256(tokenToHash);

                var code = _tokenModel.ForgotPasswordToken;

                var callbackUrl = new Uri(Url.Link("AngularRoute", new { }));

                await (_emailService).SendAsync(new IdentityMessage
                {
                    Body = String.Format(MessagesContainer.ForgotPasswordMessage + callbackUrl + "#/ChangePassword?id={0}", code),
                    Destination = model.Email,
                    Subject = "Reset password at at WoW"
                });

                return Ok(_tokenModel);
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(ForgotPasswordUserModel model)
        {
            if (_service.CheckUserId(model))
            {
                _service.ChangePassword(model);
                return Ok(_tokenModel);
            }
            return BadRequest();
        }
    }
}