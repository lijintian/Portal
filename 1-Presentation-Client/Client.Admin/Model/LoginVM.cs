using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portal.Client.Model
{
    public class LoginVM
    {
        [Required(ErrorMessage = "请输入{0}")]
        [DisplayName("登录名")]
        public string LoginUserName { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DisplayName("密码")]
        public string LoginPassword { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DisplayName("验证码")]
        public string Code { get; set; }

        /// <summary>
        /// 验证码保存的Session对应的KEY值
        /// </summary>
        public string validateKey { get; set; }
    }
}
