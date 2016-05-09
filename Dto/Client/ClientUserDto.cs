/*
 EasyDDD
ϵͳ���ƣ�  Portal�Ż�ϵͳ����ƽ̨
ģ�����ƣ�  ģ���
�������ڣ�  2015-11-16

����ժҪ��  �ⲿ��������Ϣ����Ϣ��
*/
using System;

namespace Portal.Dto
{
    [Serializable]
    public class ClientUserDto : DomainDto
    {
        #region 01.����
        /// <summary>
        /// �û���
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// ���ͣ�1��ʾ�ⲿ��2��ʾ�ڲ����ο�ö�٣�ClientUserType
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Desc { get; set; }
        #endregion

        #region 02.��ʼ��
        #endregion

        #region 03.����
        #endregion
    }

}