namespace BookApi.Helpers
{
    public class LoggingHelper
    {
        private readonly ILogger<LoggingHelper> _logger;

        public LoggingHelper(ILogger<LoggingHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }
    }
}
