namespace StringProcessingWebAPI.Handlers
{
    public class RequestTracker
    {
        private readonly int _maxRequests;
        private int _currentRequests;

        public RequestTracker(IConfiguration configuration)
        {
            _maxRequests = configuration.GetValue<int>("Settings:RequestLimit");
            _currentRequests = 0;
        }

        public bool TryStartRequest()
        {
            lock (this)
            {
                if (_currentRequests < _maxRequests)
                {
                    _currentRequests++;
                    return true;
                }
                return false;
            }
        }

        public void EndRequest()
        {
            lock (this)
            {
                _currentRequests--;
            }
        }
    }
}
