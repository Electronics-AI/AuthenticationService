using System;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Infrastructure.Loggers.Serilog
{
    public class GenericSerilogLogger<T> : Core.Interfaces.Infrastructure.Loggers.ILogger<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public GenericSerilogLogger(ILogger<T> logger)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
        }

        public void Log(Core.Interfaces.Infrastructure.Loggers.LogLevelTypes logLevel, string message, Exception exception = null, params object[] args)
        {
            switch (logLevel)
            {
                case (Core.Interfaces.Infrastructure.Loggers.LogLevelTypes.Trace):
                    _logger.LogTrace(message, exception, args);
                    break;
                case (Core.Interfaces.Infrastructure.Loggers.LogLevelTypes.Debug):
                    _logger.LogDebug(message, exception, args);
                    break;
                case (Core.Interfaces.Infrastructure.Loggers.LogLevelTypes.Information):
                    _logger.LogInformation(message, exception, args);
                    break;
                case (Core.Interfaces.Infrastructure.Loggers.LogLevelTypes.Warning):
                    _logger.LogWarning(message, exception, args);
                    break;
                case (Core.Interfaces.Infrastructure.Loggers.LogLevelTypes.Critical):
                    _logger.LogCritical(message, exception, args);
                    break;
                case (Core.Interfaces.Infrastructure.Loggers.LogLevelTypes.Error):
                    _logger.LogError(message, exception, args);
                    break;
            }
        }
    }
}