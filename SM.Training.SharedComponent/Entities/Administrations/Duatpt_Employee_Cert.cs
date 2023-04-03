using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftMart.Core.Dao;
using SM.Training.SharedComponent.Constants;

namespace SM.Training.SharedComponent.Entities.Administrations
{
    public class Duatpt_Employee_Cert : BaseEntity
    {
        public const string C_CERT_ID = "CERT_ID"; // 
        private int? _CERT_ID;
        [PropertyEntity(C_CERT_ID, true)]
        public int? Cert_ID
        {
            get { return _CERT_ID; }
            set { _CERT_ID = value; NotifyPropertyChanged(C_CERT_ID); }
        }

        public const string C_EMPLOYEE_ID = "EMPLOYEE_ID"; // 
        private int? _EMPLOYEE_ID;
        [PropertyEntity(C_EMPLOYEE_ID, false)]
        public int? Employee_ID
        {
            get { return _EMPLOYEE_ID; }
            set { _EMPLOYEE_ID = value; NotifyPropertyChanged(C_EMPLOYEE_ID); }
        }

        public const string C_CERT_NAME = "CERT_NAME"; // 
        private string? _CERT_NAME;
        [PropertyEntity(C_CERT_NAME, false)]
        public string? Cert_Name
        {
            get { return _CERT_NAME; }
            set { _CERT_NAME = value; NotifyPropertyChanged(C_CERT_NAME); }
        }
        public const string C_CERT_CODE = "CERT_CODE"; // 
        private string? _CERT_CODE;
        [PropertyEntity(C_CERT_CODE, false)]
        public string? Cert_Code
        {
            get { return _CERT_CODE; }
            set { _CERT_CODE = value; NotifyPropertyChanged(C_CERT_CODE); }
        }
        public Duatpt_Employee_Cert() : base("DUATPT_EMPLOYEE_CERT", "CERT_ID", "", "") { }
    }
}
