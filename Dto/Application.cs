namespace Portal.Dto
{
    /// <summary>
    /// Represents a dto object for application
    /// </summary>
    public class Application : DomainDto
    {
        public string CnName { get; set; }
        public string EnName { get; set; }
        public string Url { get; set; }
        public string Desc { get; set; }
        public string Remark { get; set; }

    }
}
