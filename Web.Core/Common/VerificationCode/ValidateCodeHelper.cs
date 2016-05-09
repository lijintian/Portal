using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Portal.Web.Core
{
    public class ValidateCodeHelper
    {
        #region 对象的属性和字段.
        /// <summary>
        /// 用于生成随机数的随机对象.
        /// </summary>
        private Random _Random = new Random(GetRandomSeed());

        /// <summary>
        /// 定义字体颜色的数组.
        /// </summary>
        private string[,] Colors = new string[8, 2] { { "FF0000", "FF34B3" }, { "FF4193", "FF7F24" }, { "FFA500", "D15FEE" }, { "B23AEE", "BC8F8F" }, { "B22222", "BA55D3" }, { "698B22", "7A67EE" }, { "3A5FCD", "43CD80" }, { "551A8B", "CD6600" } };//, { "B23AEE", "B3EE3A" }, { "CAFF70", "C67171" }, { "8DEEEE", "9370DB" }, { "7FFF00", "79CDCD" }

        /// <summary>
        /// 用于生成随机数的随机对象.
        /// </summary>
        public Random Random
        {
            get { return _Random; }
            set { _Random = value; }
        }
        #endregion

        #region 属性
        public int FontSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Length { get; set; }

        public int Rotate { get; set; }

        public int OffsetX { get; set; }

        public int OffsetY { get; set; }
        #endregion

        #region 初始化
        /// <summary>
        /// 默认构造函数.
        /// </summary>
        public ValidateCodeHelper()
        {
            FontSize = 9;
            Width = 60;
            Height = 20;
            Length = 4;
            Rotate = 10;
            OffsetX = 2;
            OffsetY = 8;
        }
        #endregion

        #region 主方法
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="allowRepeat">是否允许重复,如果不允许重复则长度不能超过26,否则抛出异常.</param>
        /// <returns></returns>
        public string CreateValidateCode(bool allowRepeat = true)
        {
            string result = string.Empty;
            try
            {
                string letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";//abcdefghijklmnopqrstuvwxyz0123456789
                if (letter.Length < Length && !allowRepeat)
                {
                    throw new Exception("传入的长度大于原始字符的长度且不允许重复,将导致无限循环.");
                }
                Random rnd = new Random();
                while (result.Length < Length)
                {
                    int x = rnd.Next(0, letter.Length - 1);
                    string s = letter.Substring(x, 1);
                    if (allowRepeat)
                    {
                        result += s;
                    }
                    else
                    {
                        if (result.IndexOf(s) == -1)
                        {
                            result += s;
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">验证码</param>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(image);
            try
            {
                graphics.Clear(Color.White);
                for (int x = 0; x < validateCode.Length; x++)
                {
                    PointF pf = GetLeftPointF(x);
                    Brush brush = this.GetBrush(pf);
                    graphics.Transform = this.GetMatrix(pf);
                    graphics.DrawString(validateCode.Substring(x, 1), new Font("Arial", FontSize, FontStyle.Bold), brush, pf);
                }
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                graphics.Dispose();
                image.Dispose();
            }
        }
        /// <summary>
        /// 检查验证码是否正确
        /// </summary>
        /// <param name="validateKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CheckCode(string validateKey, string code)
        {
            var type = Check(validateKey, code);
            return type == ValidateCodeError.Success ? string.Empty : type.GetDescription();
        }
        public static ValidateCodeError Check(string validateKey, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return ValidateCodeError.InputCode;
            }
            if (GetSession(validateKey) == null || !string.Equals(GetSession(validateKey), code, StringComparison.CurrentCultureIgnoreCase))
            {
                return ValidateCodeError.CodeError;
            }
            return ValidateCodeError.Success;
        }
        #endregion

        #region 02.验证码
        /// <summary>
        /// 验证码
        /// </summary>
        public static string GetSession(string key)
        {
            if (HasCurrent)
            {
                return (HttpContext.Current.Session[key] as string);
            }
            return null;

        }
        public static bool HasCurrent
        {
            get { return HttpContext.Current != null && HttpContext.Current.Session != null; }
        }
        #endregion

        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        #region GetMatrix:取得一个随机的旋转角度.
        /// <summary>
        /// 取得一个随机的旋转角度.
        /// </summary>
        /// <param name="pf">一个指定的相对坐标点.</param>
        /// <returns></returns>
        private Matrix GetMatrix(PointF pf)
        {
            Matrix matrix = new Matrix();
            matrix.RotateAt(Random.Next(-Rotate, Rotate), new PointF(pf.X + 5, pf.Y + 5));
            return matrix;
        }
        #endregion

        #region GetBrush:用随机设置取得一个笔刷.
        /// <summary>
        /// 用随机设置取得一个笔刷.
        /// </summary>
        /// <param name="pf">一个指定的相对坐标点.</param>
        /// <returns></returns>
        private Brush GetBrush(PointF pf)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.X = (int)pf.X;
            rectangle.Y = (int)pf.Y;
            rectangle.Width = rectangle.Height = 20;
            int ColorIndex = Random.Next(0, Colors.GetLength(0));
            int R, G, B;

            R = Convert.ToInt32(Colors[ColorIndex, 0].Substring(0, 2), 16);
            G = Convert.ToInt32(Colors[ColorIndex, 0].Substring(2, 2), 16);
            B = Convert.ToInt32(Colors[ColorIndex, 0].Substring(4, 2), 16);
            Color BeginColor = Color.FromArgb(255, R, G, B);

            R = Convert.ToInt32(Colors[ColorIndex, 1].Substring(0, 2), 16);
            G = Convert.ToInt32(Colors[ColorIndex, 1].Substring(2, 2), 16);
            B = Convert.ToInt32(Colors[ColorIndex, 1].Substring(4, 2), 16);
            Color EndColor = Color.FromArgb(255, R, G, B);

            return new System.Drawing.Drawing2D.LinearGradientBrush(rectangle, BeginColor, EndColor, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
        }
        #endregion

        #region GetLeftPointF:根据索引值取得一个随机的坐标点.
        /// <summary>
        /// 根据索引值取得一个随机的坐标点.
        /// </summary>
        /// <param name="index">索引值.</param>
        /// <returns></returns>
        private PointF GetLeftPointF(int index)
        {
            PointF result = new PointF();
            result.X = Random.Next(0, OffsetX);
            result.Y = Random.Next(0, OffsetY);
            result.X += (index * (Width / Length));
            return result;
        }
        #endregion

    }

    public enum ValidateCodeError
    {
        [Description("请输入验证码")]
        InputCode = 1,

        [Description("验证码输入错误")]
        CodeError = 2,

        [Description("登陆成功！")]
        Success = 10
    }
}
