using DMnDBCS.UI.Services.Jwt;
using DMnDBCS.UI.Services.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace DMnDBCS.UI.Views.Shared.Components.Notifications
{
    public class NotificationsViewComponent(INotificationsService notificationsService, IJwtService jwtService) : ViewComponent
    {
        private readonly INotificationsService _notificationsService = notificationsService;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _notificationsService.GetAllForUserAsync(int.Parse(_jwtService.GetUserId()!));
            if (!response.IsSuccessful)
            {
                return View();
            }

            response.Data = [.. response.Data!.OrderByDescending(n => n.Time)];

            return View(response.Data);
        }
    }
}
