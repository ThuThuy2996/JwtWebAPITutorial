namespace JwtWebAPITutorial.Providers
{
    public class ActionResultService
    {
        public ActionResultService()
        {
            StatusCode = 200;
            Success = true;
        }

        public ActionResultService(bool pIsSuccess)
        {
            Success = pIsSuccess;
            if (pIsSuccess)
            {
                StatusCode = 200;
            }
            else
            {
                StatusCode = 500;
            }
        }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Data { get; set; }
        public object ObjData { get; set; }
        public string ErrMsg { get; set; }
    }
}

