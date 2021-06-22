using Formulyar.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.Model
{
    class CheckNew
    {
        #region Members
        private string _dcMain;
        private string _dcSecond;
        private Secheniya _SechDcMain;
        private Secheniya _SechDcSecondary;
        private Voltage _VoltageDCMain;
        private Voltage _VoltageDCSecondary;
        
        
        #endregion
        #region Properties
        /// <summary>
        /// Диспетчерский центр
        /// </summary>
        public string DcMain
        {
            get { return _dcMain; }
            set { _dcMain = value; }
        }
        /// <summary>
        /// Диспетчерский центр второй
        /// </summary>
        public string DcSecond
        {
            get { return _dcSecond; }
            set { _dcSecond = value; }
        }
        /// <summary>
        /// Сечение Главного ДЦ
        /// </summary>
        public Secheniya SechDcMain
        {
            get { return _SechDcMain; }
            set { _SechDcMain = value; }
        }
        //public ObservableCollection<Secheniya> SechDcMainCollect
        //{
        //    get { return _sechDcMainCollect; }
        //    set { _sechDcMainCollect = value; }
        //}
        //public ObservableCollection<Secheniya> SechDcSecondaryCollect
        //{
        //    get { return _sechDcSecondaryCollect; }
        //    set { _sechDcSecondaryCollect = value; }
        //}
        /// <summary>
        /// Сечение вторичного ДЦ
        /// </summary>
        public Secheniya SechDcSecondary
        {
            get { return _SechDcSecondary; }
            set { _SechDcSecondary = value; }
        }

        /// <summary>
        /// Информация о ошибке
        /// </summary>
        //public string Info
        //{
        //    get { return _info; }
        //    set { _info = value; }
        //}
 
        public Voltage VoltageDCMain
        {
            get { return _VoltageDCMain; }
            set { _VoltageDCMain = value; }
        }
        public Voltage VoltageDCSecondary
        {
            get { return _VoltageDCSecondary; }
            set { _VoltageDCSecondary = value; }
        }
        #endregion
    }
    public class CommonSech
    {
        public string DC1;
        public int IDsech1;
        public string DC2;
        public int IDsech2;
    }
    public class CommonVoltage
    {
        public string DC1;
        public int IDvoltage1;
        public string DC2;
        public int IDvoltage2;
    }
    public class CommonLine
    {
        public string DC1;
        public int IdLine1;
        public int idEO1;
        public string DC2;
        public int IdLine2;
        public int idEO2;
    }
    public class CommonTransform
    {
        public string DC1;
        public int idTrans1;
        public int idWinding1;
        public string DC2;
        public int idTrans2;
        public int idWinding2;
    }
    public class CommonBreaker
    {
        public string DC1;
        public int idbreaker1;
        public string DC2;
        public int idBreaker2;
    }
}
