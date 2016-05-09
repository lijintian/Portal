namespace Portal.Dto
{
    public class AggregateDto
    {
        #region 属性
        public string Id { get; set; }
        #endregion

        #region 方法
        public bool IsNew()
        {
            return string.IsNullOrEmpty(this.Id);
        }
        #endregion
    }
}
