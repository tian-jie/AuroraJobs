namespace Aurora.Jobs.Items
{
    public class SendMessageToEmployeeData
    {
        public string LillyId { get; set; }
        public string AppId { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }

    public class SendMessageToEmployeeResult
    {
        public string Code { get; set; }
        public string Data { get; set; }
        public string Remark { get; set; }
    }
}
