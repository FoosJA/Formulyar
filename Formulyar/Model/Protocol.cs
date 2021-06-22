using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class Protocol
    {
        #region Members
        private string _dispatchCenter;
        private string _kontrolObjectName;
        private string _typeKontrol;
        private string _info;
        private int _numberOI;
        private string _typeOI;
        private string _dcSource;
        private int _numberOIsource;        
        private string _dcReception;
        private int _numberOIreception;
        private string _category;
        private string _nameTI;


        #endregion
        #region Properties
        /// <summary>
        /// Система контроля
        /// </summary>
        public string TypeKontrol
        {
            get { return _typeKontrol; }
            set { _typeKontrol = value; }
        }
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public string NameTI
        {
            get { return _nameTI; }
            set { _nameTI = value; }
        }
        /// <summary>
        /// контролирующий ДЦ
        /// </summary>
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        /// <summary>
        /// ДЦ-источник
        /// </summary>
        public string DcSource
        {
            get
            {
                if (_dcSource == null)
                    return "-";
                else
                    return _dcSource;
            }
            set { _dcSource = value; }
        }
        /// <summary>
        /// ДЦ-получатель
        /// </summary>
        public string DCReception
        {
            get
            {
                if (_dcReception == null)
                    return "-";
                else
                    return _dcReception;
            }
            set { _dcReception = value; }
        }
        /// <summary>
        /// Номер ОТИ в ДЦ-источника
        /// </summary>
        public int NumberOIsource
        {
            get { return _numberOIsource; }
            set { _numberOIsource = value; }
        }
        /// <summary>
        /// Номер ОТИ в ДЦ-источнике строка
        /// </summary>
        public string NumberOIsourceStr
        {
            get
            {
                if (_numberOIsource == 0)
                    return "-";
                else
                    return _numberOIsource.ToString();
            }
            set {; }
        }
        /// <summary>
        /// Номер ОТИ в ДЦ-приемнике
        /// </summary>
        public int NumberOIreception
        {
            get { return _numberOIreception; }
            set { _numberOIreception = value; }
        }
        /// <summary>
        /// Номер ОТИ в ДЦ-приемнике строка
        /// </summary>
        public string NumberOIreceptionStr
        {
            get
            {
                if (_numberOIreception == 0)
                    return "-";
                else
                    return _numberOIreception.ToString();
            }
            set {; }
        }
        /// <summary>
        /// Номер ТИ\ТС в Объекте контроля
        /// </summary>
        public int NumberOI
        {
            get { return _numberOI; }
            set { _numberOI = value; }
        }
        /// <summary>
        /// Название Объекта контроля
        /// </summary>
        public string KontrolObjectName
        {
            get { return _kontrolObjectName; }
            set { _kontrolObjectName = value; }
        }        
        /// <summary>
        /// Информация о ошибке
        /// </summary>
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
        public string TypeOI
        {
            get { return _typeOI; }
            set { _typeOI = value; }
        }
        #endregion
        public bool IsSendCDU;
    }
    class ExchangeProtocol
    {
        private string _kontrolObjectName;
        public string KontrolObjectName
        {
            get { return _kontrolObjectName; }
            set { _kontrolObjectName = value; }
        }
        private string _dcSource;
        public string DcSource
        {
            get { return _dcSource; }
            set { _dcSource = value; }
        }
        private string _nameDCSource;
        public string NameDCSource
        {
            get { return _nameDCSource; }
            set { _nameDCSource = value; }
        }
        private string _categoryDCSource;
        public string CategoryDCSource
        {
            get { return _categoryDCSource; }
            set { _categoryDCSource = value; }
        }
        private int _numberOIsource;
        public int NumberOIsource
        {
            get { return _numberOIsource; }
            set { _numberOIsource = value; }
        }
        private string _uidDCSource;
        public string UidDCSource
        {
            get { return _uidDCSource; }
            set { _uidDCSource = value; }
        }
        private string _poDCSource;
        public string PoDCSource
        {
            get { return _poDCSource; }
            set { _poDCSource = value; }
        }
        private string _dcRec;
        public string dcRec
        {
            get { return _dcRec; }
            set { _dcRec = value; }
        }
        private string _namedcRec;
        public string NamedcRec
        {
            get { return _namedcRec; }
            set { _namedcRec = value; }
        }
        private string _categorydcRec;
        public string CategorydcRec
        {
            get { return _categorydcRec; }
            set { _categorydcRec = value; }
        }
        private int _numberOIrec;
        public int NumberOIrec
        {
            get { return _numberOIrec; }
            set { _numberOIrec = value; }
        }
        private string _uiddcRec;
        public string UiddcRec
        {
            get { return _uiddcRec; }
            set { _uiddcRec = value; }
        }
        private string _podcRec;
        public string PodcRec
        {
            get { return _podcRec; }
            set { _podcRec = value; }
        }
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
        public bool IsSendCDU;
    }
}
