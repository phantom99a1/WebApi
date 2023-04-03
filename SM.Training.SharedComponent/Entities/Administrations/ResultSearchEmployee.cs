using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftMart.Core.Dao;

namespace SM.Training.SharedComponent.Entities.Administrations
{
    public class ResultSearchEmployee:Duatpt_Employee
    {
        public const string C_CERTIFICATE = "CERTIFICATE";
        private string? _CERTIFICATE;
        [PropertyEntity(C_CERTIFICATE, false)]
        public string? Certificate
        {
            get { return _CERTIFICATE; }
            set { _CERTIFICATE = value; NotifyPropertyChanged(C_CERTIFICATE); }
        }
    }
}
