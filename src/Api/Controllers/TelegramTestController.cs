using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TelegramTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult LoginPage()
        {
            string htmlContent = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Telegram Login</title>
</head>
<body>
<script async src=""https://telegram.org/js/telegram-widget.js?22"" data-telegram-login=""neurosprints_test_bot"" data-size=""large"" data-onauth=""onTelegramAuth(user)""></script>
<script type=""text/javascript"">
  async function onTelegramAuth(user) {
    try {
      const userData = {
        id: user.id.toString(),
        firstName: user.first_name || """",
        lastName: user.last_name || """",
        userName: user.username || """",
        photoUrl: user.photo_url || """",
        authDate: user.auth_date.toString(),
        hash: user.hash
      };

      const response = await fetch('/User/ExternalTelegramLogin', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
      });

      if (response.ok) {
        const result = await response.json();
        if (result.isSucceed) {
          alert('Authentication successful!');
        } else {
          const errorMessages = Object.entries(result.messages)
            .map(([key, value]) => `${key}: ${value}`)
            .join('\n');
          alert('Authentication failed:\n' + errorMessages);
        }
      } else {
        alert('Server error: ' + response.status);
      }
    } catch (error) {
      console.error('Error during Telegram authentication:', error);
      alert('An error occurred during authentication.');
    }
  }
</script>
</body>
</html>";

            return Content(htmlContent, "text/html");
        }
    }
}
