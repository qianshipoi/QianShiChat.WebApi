namespace QianShiChat.WebApi.Controllers;

public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// user id
    /// </summary>
    protected int CurrentUserId
    {
        get
        {
            var val = User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
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
    protected bool IsLogin => CurrentUserId == 0;
}