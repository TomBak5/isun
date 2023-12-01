namespace isun.Responses
{
    public class Response<TRespData> where TRespData : class
    {
        public bool IsOk { get; set; }
        public TRespData Object { get; set; }
        public ProblemDetails Error { get; set; }
    }
}
