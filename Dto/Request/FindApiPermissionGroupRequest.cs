namespace Portal.Dto
{
    public class FindApiPermissionGroupRequest : PagerFindRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
