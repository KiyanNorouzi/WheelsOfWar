namespace SimpleJSON
{
    public class ConsumeResult
    {

        public const int StatusOk = 0;
        public const int StatusNotOwned = 1;
        public const int StatusFailed = 2;

        private int _status;
        private InAppError _error;
        private bool _success;

        private ConsumeResult()
        {
        }

        public static ConsumeResult FromJson(string json)
        {
            var o = JSON.Parse(json);
            var consumeResult = new ConsumeResult {_status = o["status"].AsInt};
            consumeResult._success = consumeResult._status==StatusOk;
            if (consumeResult._status==StatusFailed)
            {
                consumeResult._error = InAppError.Create(o["error"]);
            }
            return consumeResult;
        }


        /// <summary>
        /// true if item is consumed, otherwise is false.
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return _success;
        }

        /// <summary>
        /// Returns the status of the consume procedure.
        /// </summary>
        /// <returns><see cref="StatusOk"/> if consuming is successful. <see cref="StatusNotOwned"/> if Item is not owned to consume. <see cref="StatusFailed"/> if consuming is failed, you can call <see cref="GetError"/> to know what happened.</returns>
        public int GetStatus()
        {
            return _status;
        }

        public InAppError GetError()
        {
            return _error;
        }

    }
}

