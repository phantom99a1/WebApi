using SoftMart.Core.Dao;
using System;

namespace SM.Training.SharedComponent.Entities.Administrations
{
    public class adm_Employee : BaseEntity
    {
        #region Primitive members

        public const string C_EMPLOYEE_ID = "EMPLOYEE_ID"; // 
        private int? _EMPLOYEE_ID;
        [PropertyEntity(C_EMPLOYEE_ID, true)]
        public int? Employee_ID
        {
            get { return _EMPLOYEE_ID; }
            set { _EMPLOYEE_ID = value; NotifyPropertyChanged(C_EMPLOYEE_ID); }
        }

        public const string C_USER_NAME = "USER_NAME"; // 
        private string _USER_NAME;
        [PropertyEntity(C_USER_NAME, false)]
        public string User_Name
        {
            get { return _USER_NAME; }
            set { _USER_NAME = value; NotifyPropertyChanged(C_USER_NAME); }
        }

        public const string C_NAME = "NAME"; // 
        private string _NAME;
        [PropertyEntity(C_NAME, false)]
        public string Name
        {
            get { return _NAME; }
            set { _NAME = value; NotifyPropertyChanged(C_NAME); }
        }

        public const string C_ADDRESS = "ADDRESS"; // 
        private string _ADDRESS;
        [PropertyEntity(C_ADDRESS, false)]
        public string Address
        {
            get { return _ADDRESS; }
            set { _ADDRESS = value; NotifyPropertyChanged(C_ADDRESS); }
        }

        public const string C_PHONE_NUMBER = "PHONE_NUMBER"; // 
        private string _PHONE_NUMBER;
        [PropertyEntity(C_PHONE_NUMBER, false)]
        public string Phone_Number
        {
            get { return _PHONE_NUMBER; }
            set { _PHONE_NUMBER = value; NotifyPropertyChanged(C_PHONE_NUMBER); }
        }

        public const string C_DOB = "DOB"; // 
        private DateTime? _DOB;
        [PropertyEntity(C_DOB, false)]
        public DateTime? DOB
        {
            get { return _DOB; }
            set { _DOB = value; NotifyPropertyChanged(C_DOB); }
        }

        public const string C_GENDER = "GENDER"; // 
        private int? _GENDER;
        [PropertyEntity(C_GENDER, false)]
        public int? Gender
        {
            get { return _GENDER; }
            set { _GENDER = value; NotifyPropertyChanged(C_GENDER); }
        }

        public const string C_DELETED = "DELETED"; // 
        private bool? _DELETED;
        [PropertyEntity(C_DELETED, false)]
        public bool? Deleted
        {
            get { return _DELETED; }
            set { _DELETED = value; NotifyPropertyChanged(C_DELETED); }
        }

        public const string C_VERSION = "VERSION"; // 
        private int? _VERSION;
        [PropertyEntity(C_VERSION, false)]
        public int? Version
        {
            get { return _VERSION; }
            set { _VERSION = value; NotifyPropertyChanged(C_VERSION); }
        }

        public const string C_CREATED_BY = "CREATED_BY"; // 
        private string _CREATED_BY;
        [PropertyEntity(C_CREATED_BY, false)]
        public string Created_By
        {
            get { return _CREATED_BY; }
            set { _CREATED_BY = value; NotifyPropertyChanged(C_CREATED_BY); }
        }

        public const string C_CREATED_DTG = "CREATED_DTG"; // 
        private DateTime? _CREATED_DTG;
        [PropertyEntity(C_CREATED_DTG, false)]
        public DateTime? Created_Dtg
        {
            get { return _CREATED_DTG; }
            set { _CREATED_DTG = value; NotifyPropertyChanged(C_CREATED_DTG); }
        }

        public const string C_UPDATED_BY = "UPDATED_BY"; // 
        private string _UPDATED_BY;
        [PropertyEntity(C_UPDATED_BY, false)]
        public string Updated_By
        {
            get { return _UPDATED_BY; }
            set { _UPDATED_BY = value; NotifyPropertyChanged(C_UPDATED_BY); }
        }

        public const string C_UPDATED_DTG = "UPDATED_DTG"; // 
        private DateTime? _UPDATED_DTG;
        [PropertyEntity(C_UPDATED_DTG, false)]
        public DateTime? Updated_Dtg
        {
            get { return _UPDATED_DTG; }
            set { _UPDATED_DTG = value; NotifyPropertyChanged(C_UPDATED_DTG); }
        }

        public adm_Employee() : base("ADM_EMPLOYEE", "EMPLOYEE_ID", C_DELETED, C_VERSION) { }

        #endregion

        #region Extend members

        #endregion

        #region Clone

        public adm_Employee CloneToInsert()
        {
            adm_Employee newItem = new adm_Employee();

            newItem.Employee_ID = this.Employee_ID;
            newItem.User_Name = this.User_Name;
            newItem.Name = this.Name;
            newItem.Address = this.Address;
            newItem.Phone_Number = this.Phone_Number;
            newItem.DOB = this.DOB;
            newItem.Gender = this.Gender;

            return newItem;
        }

        public adm_Employee CloneToUpdate()
        {
            adm_Employee newItem = new adm_Employee();

            newItem.Employee_ID = this.Employee_ID;
            newItem.User_Name = this.User_Name;
            newItem.Name = this.Name;
            newItem.Address = this.Address;
            newItem.Phone_Number = this.Phone_Number;
            newItem.DOB = this.DOB;
            newItem.Gender = this.Gender;
            newItem.Version = this.Version;

            return newItem;
        }

        #endregion
    }

}
