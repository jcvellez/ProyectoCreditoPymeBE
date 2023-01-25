using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Prometheus;
using Serilog;
using System.Text.Json;

namespace bg.hd.banca.pyme.infrastructure.extentions
{
    public static class ApplicationExtentions
    {

        public static IApplicationBuilder ConfigureMetricServer(this IApplicationBuilder app)
        {
            app.UseMetricServer();
            app.UseHttpMetrics();

            return app;
        }

        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";



                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();



                    int _code = context.Response.StatusCode;
                    int _codeAPP = 0;
                    string _message = exceptionHandlerPathFeature.Error.Message;
                    string _stackTrace = null;
                    string? _source = null;
                    string? _helpLink = null;
                    try
                    {
                        _codeAPP = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Code;
                        _message = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Message;
                        _stackTrace = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).StackTrace;
                        _source = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Source;
                        _helpLink = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).HelpLink;


                        switch (_codeAPP)
                        {
                            case 500:
                                context.Response.StatusCode = _codeAPP;
                                _code = _codeAPP;
                                break;
                            case 401:

                                switch (_stackTrace)
                                {
                                    case "SecurityTokenExpiredException":
                                        context.Response.Headers.Add("Token-Expired", "true");
                                        break;
                                    case "ArgumentException":
                                    case "invalid_token":
                                        context.Response.Headers.Add("Token-Valid", "false");
                                        break;
                                }

                                context.Response.StatusCode = _codeAPP;
                                _code = _codeAPP;
                                break;
                            default:
                                context.Response.StatusCode = 400;
                                _code = 400;
                                break;
                        }

                    }
                    catch (InvalidCastException)
                    {

                    }
                    string sjson = string.Empty;
                    if (!string.IsNullOrEmpty(_source) && Int32.TryParse(_source, out int numeroIntentosRestantesBloqueo) && !string.IsNullOrEmpty(_helpLink) && Int32.TryParse(_helpLink, out int tiempoBloqueoRestante))
                    {
                        var error = new
                        {
                            code = _codeAPP == 0 ? _code : _codeAPP,
                            message = _stackTrace ??= _message,
                            numeroIntentosRestantesBloqueo = numeroIntentosRestantesBloqueo,
                            tiempoBloqueoRestante = tiempoBloqueoRestante
                        };
                        var _response = new
                        {
                            code = _code,
                            message = _message,
                            errors = new[] { error },
                            traceid = context?.TraceIdentifier == null ? "no-traceid" : context?.TraceIdentifier.Split(":")[0].ToLower()
                        };
                        sjson = JsonSerializer.Serialize(_response);
                    }
                    else if (!string.IsNullOrEmpty(_source) && Int32.TryParse(_source, out int _numeroIntentosRestantesBloqueo))
                    {
                        var error = new
                        {
                            code = _codeAPP == 0 ? _code : _codeAPP,
                            message = _stackTrace ??= _message,
                            numeroIntentosRestantesBloqueo = _numeroIntentosRestantesBloqueo
                        };
                        var _response = new
                        {
                            code = _code,
                            message = _message,
                            errors = new[] { error },
                            traceid = context?.TraceIdentifier == null ? "no-traceid" : context?.TraceIdentifier.Split(":")[0].ToLower()
                        };
                        sjson = JsonSerializer.Serialize(_response);
                    }
                    else
                    {
                        MsDtoResponseError _response = new MsDtoResponseError
                        {
                            code = _code,
                            message = _message,
                            errors = new List<MsDtoError> {
                                new MsDtoError
                                {
                                code = _codeAPP == 0 ? _code : _codeAPP,
                                message = _stackTrace ??= _message,
                                }
                            },
                            traceid = context?.TraceIdentifier == null ? "no-traceid" : context?.TraceIdentifier.Split(":")[0].ToLower()
                        };
                        sjson = JsonSerializer.Serialize(_response);
                    }

                    Log.Error("{Proceso} {errorCode} {errorMessage}", "ExceptionHandler", context.Response.StatusCode, _message);


                    await context.Response.WriteAsync(sjson);
                    await context.Response.Body.FlushAsync();
                });
            });

            return app;
        }
    }
}
