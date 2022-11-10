using Microsoft.AspNetCore.Mvc;

namespace QianShiChat.WebApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// user id
        /// </summary>
        protected int UserId
        {
            get
            {
                var val = User.Claims?.FirstOrDefault(x => x.Type == AppConsts.ClaimUserId)?.Value;
                if (string.IsNullOrWhiteSpace(val))
                {
                    return 0;
                }
                return int.Parse(val);
            }
        }

        /// <summary>
        /// is login
        /// </summary>
        protected bool IsLogin => UserId == 0;
    }
}
