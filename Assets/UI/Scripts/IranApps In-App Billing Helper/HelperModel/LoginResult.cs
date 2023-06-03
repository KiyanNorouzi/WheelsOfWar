namespace SimpleJSON
{
    public class LoginResult
    {
        private bool _success;
        private InAppError _error;

        public static LoginResult FromJson(string json)
        {
            var loginResult = new LoginResult();
            var o =JSON.Parse(json);
            loginResult._success = o["success"].AsBool;
            if (!loginResult._success)
            {
                loginResult._error = InAppError.Create(o["error"]);
            }
            return loginResult;
        }

        /// <summary>
        /// true if login is successful. false otherwise.
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return _success;
        }

        /// <summary>
        /// Returns the error when <see cref="IsSuccess"/> is false.
        /// </summary>
        /// <returns></returns>
        public InAppError GetError()
        {
            return _error;
        }
    }
}