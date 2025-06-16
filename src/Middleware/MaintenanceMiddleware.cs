

using Microsoft.AspNetCore.Http;
using Dinaup; 
 
public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;

    public MaintenanceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, DinaupClientC dinaupClient)
    {
        if (dinaupClient.IsConnected == false )
        {
            context.Response.StatusCode = 503; // Service Unavailable
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(@"<!DOCTYPE html>
<html>
  <head>
    <meta charset=""UTF-8"">
    <title>Fuera de servicio por mantenimiento....</title>
    <style>
      @import url('https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;600&display=swap');

      h1, h2 {
        font-weight: 200 !important;
      }
      body { 
        background-color: #111111; 
        text-align: center; 
        padding: 150px;
        font-family: 'Poppins', sans-serif;
        font-size: 20px;
        color: #fff;
      }
      h1 { 
        font-size: 30px;
        font-family: 'Roboto';
      }
      h2 { 
        font-size: 20px;
        font-weight: 600;
        font-family: 'Roboto';
        color: lime;
      }
      img, svg {
        border-radius: 100px;
        margin-left: 0px;
        animation: spin 4s linear infinite;
      }
      svg {
        fill: white !important;
      }
      @keyframes spin { 
        100% { transform: rotate(360deg); } 
      }
    </style>
  </head>
  <body>
    <img style=""width:100px;height:100px; fill: white !important; filter: invert(1)"" src=""data:image/svg+xml;base64,CjxzdmcgaGVpZ2h0PSI1MTIiIHZpZXdCb3g9IjAgMCAzMiAzMiIgd2lkdGg9IjUxMiIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48ZyBpZD0iTGF5ZXJfMiIgZGF0YS1uYW1lPSJMYXllciAyIj48cGF0aCBkPSJtMjkuMjEgMTEuODRhMy45MiAzLjkyIDAgMCAxIC0zLjA5LTUuMyAxLjg0IDEuODQgMCAwIDAgLS41NS0yLjA3IDE0Ljc1IDE0Ljc1IDAgMCAwIC00LjQtMi41NSAxLjg1IDEuODUgMCAwIDAgLTIuMDkuNTggMy45MSAzLjkxIDAgMCAxIC02LjE2IDAgMS44NSAxLjg1IDAgMCAwIC0yLjA5LS41OCAxNC44MiAxNC44MiAwIDAgMCAtNC4xIDIuMyAxLjg2IDEuODYgMCAwIDAgLS41OCAyLjEzIDMuOSAzLjkgMCAwIDEgLTMuMjUgNS4zNiAxLjg1IDEuODUgMCAwIDAgLTEuNjIgMS40OSAxNC4xNCAxNC4xNCAwIDAgMCAtLjI4IDIuOCAxNC4zMiAxNC4zMiAwIDAgMCAuMTkgMi4zNSAxLjg1IDEuODUgMCAwIDAgMS42MyAxLjU1IDMuOSAzLjkgMCAwIDEgMy4xOCA1LjUxIDEuODIgMS44MiAwIDAgMCAuNTEgMi4xOCAxNC44NiAxNC44NiAwIDAgMCA0LjM2IDIuNTEgMiAyIDAgMCAwIC42My4xMSAxLjg0IDEuODQgMCAwIDAgMS41LS43OCAzLjg3IDMuODcgMCAwIDEgMy4yLTEuNjggMy45MiAzLjkyIDAgMCAxIDMuMTQgMS41OCAxLjg0IDEuODQgMCAwIDAgMi4xNi42MSAxNSAxNSAwIDAgMCA0LTIuMzkgMS44NSAxLjg1IDAgMCAwIC41NC0yLjExIDMuOSAzLjkgMCAwIDEgMy4xMy01LjM5IDEuODUgMS44NSAwIDAgMCAxLjU3LTEuNTIgMTQuNSAxNC41IDAgMCAwIC4yNi0yLjUzIDE0LjM1IDE0LjM1IDAgMCAwIC0uMjUtMi42NyAxLjgzIDEuODMgMCAwIDAgLTEuNTQtMS40OXptLTguMjEgNC4xNmE1IDUgMCAxIDEgLTUtNSA1IDUgMCAwIDEgNSA1eiIvPjwvZz48L3N2Zz4="">
    <h1>Estamos en mantenimiento</h1>
    <h2>✅ Estamos trabajando para mejorar tu experiencia.</h2>
    <span><b>Vuelve pronto</b>, ¡Te esperamos!</span>
    
    <!-- Cuenta atrás -->
    <p id=""countdown"" style=""margin-top: 20px; font-size: 24px;""></p>
    <script>
      let seconds = 30;
      const countdownElement = document.getElementById('countdown');
      countdownElement.innerText = ""Recargando en "" + seconds + "" segundos..."";

      const countdownInterval = setInterval(function() {
        seconds--;
        countdownElement.innerText = ""Recargando en "" + seconds + "" segundos..."";
        if (seconds <= 0) {
          clearInterval(countdownInterval);
          location.reload();
        }
      }, 1000);
    </script>
  </body>
</html>
");
            return;
        }
        else
        {



            await _next(context);
        }
    }
}

