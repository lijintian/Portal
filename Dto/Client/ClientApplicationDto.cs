/*
 EasyDDD
ϵͳ���ƣ�  Portal�Ż�ϵͳ����ƽ̨
ģ�����ƣ�  ģ���
�������ڣ�  2015-11-16

����ժҪ��  �ⲿ������Ӧ����Ϣ����Ϣ��
*/
using System;

namespace Portal.Dto
{
    [Serializable]
    public class ClientApplicationDto : DomainDto
    {
        #region 01.����
        /// <summary>
        /// Ӧ������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ӧ�õ�ַ
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// �ص���ַ
        /// </summary>
        public string PostUrl { get; set; }
        /// <summary>
        /// Ӧ������,1��վӦ��,2����Ӧ��,3�ƶ�Ӧ�ã��ο�ö�٣�ClientApplicationType
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// ״̬��1�����У�2����У�3���ͨ�����ο�ö�٣�ClientApplicationState
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// Ӧ�ü��
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// ����Ȩ��
        /// </summary>
        public string Permissions { get; set; }
        /// <summary>
        /// ����Ȩ��
        /// </summary>
        public string PassPermissions { get; set; }
        /// <summary>
        /// ��˼�¼
        /// </summary>
        public string AuditRecord { get; set; }

        public string ClientId { get; set; }

        public string Secret { get; set; }
        #endregion

        #region 02.��ʼ��
        #endregion

        #region 03.����
        #endregion
    }

}