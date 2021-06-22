using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class OperTechInform
    {
        #region Members
        private int _numberOI;
        private string _nameOI;
        private string _typeOI;
        private string _typeTI;
        private string _energyObject;
        private string _dispatchCenter;
        private string _category;
        private string _dispatchCenterSour;
        private string _formula;
        private string _formulaCode;
        private string _currentTransform;
        private string _voltageTransform;
        private string _measureTransduse;
        private bool _triggerOIK;
        private bool _triggerAIP;
        private bool _triggerCSPA;
        private bool _triggerKOSMOS;
        private List<OperTechInform> _listFormula;
        #endregion

        #region Properties
        /// <summary>
        /// Номер ТИ\ТС
        /// </summary>
        public int NumberOI
        {
            get { return _numberOI; }
            set { _numberOI = value; }
        }
        /// <summary>
        /// Наименование ОИ
        /// </summary>
        public string NameOI
        {
            get { return _nameOI; }
            set { _nameOI = value; }
        }
        public string CurrentTransform
        {
            get { return _currentTransform; }
            set { _currentTransform = value; }
        }
        public string VoltagetTransform
        {
            get { return _voltageTransform; }
            set { _voltageTransform = value; }
        }
        public string MeasureTransduse
        {
            get { return _measureTransduse; }
            set { _measureTransduse = value; }
        }
        public string DispatchCenterSour
        {
            get { return _dispatchCenterSour; }
            set { _dispatchCenterSour = value; }
        }

        /// <summary>
        /// Тип ТИ/ТС
        /// </summary>
        public string TypeOI
        {
            get { return _typeOI; }
            set { _typeOI = value; }
        }


        /// <summary>
        /// Тип ТИ
        /// </summary>
        public string TypeTI
        {
            get { return _typeTI; }
            set {_typeTI = value; }
        }//TODO: может сделать два класса наследника ТИ и ТС

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>
        /// Объект диспетчеризации
        /// </summary>
        public string EnergyObject
        {
            get { return _energyObject; }
            set { _energyObject = value; }
        }
        ///// <summary>
        ///// Диспетчерское наиенование объекта диспетчеризации (не нашла в SQL)
        ///// </summary>
        //public string EnergyObjectName { get; set; }

        /// <summary>
        /// Диспетчерский центр
        /// </summary>
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        public bool TriggerOIK
        {
            get { return _triggerOIK; }
            set { _triggerOIK = value; }
        }
        public bool TriggerAIP
        {
            get { return _triggerAIP; }
            set { _triggerAIP = value; }
        }
        public bool TriggerCSPA
        {
            get { return _triggerCSPA; }
            set { _triggerCSPA = value; }
        }
        public bool TriggerKOSMOS
        {
            get { return _triggerKOSMOS; }
            set { _triggerKOSMOS = value; }
        }
        #endregion
        /// <summary>
        /// Формула дорасчёта
        /// </summary>
        public string Formula
        {
            get { return _formula; }
            set
            {
                if (value != null)
                {
                    _formula = value.Replace('\n', ' ');
                }
            }
        }
        /// <summary>
        /// Код формулы дорасчёта
        /// </summary>
        public string FormulaCode
        {
            get { return _formulaCode; }
            set
            {
                _formulaCode = value;
                if ((_formulaCode != null) && (_formulaCode != ""))
                {
                    List<OperTechInform> list = new List<OperTechInform>();
                    int result;
                    char[] delimiter = { ' ' };
                    char chI = 'I';
                    char chS = 'S';
                    string[] words = _formulaCode.Split(delimiter);
                    foreach (string s in words)
                    {
                        if ((s[0] == chI) && (s.Substring(s.Length - 1) == "V"))
                        {
                            int.TryParse(string.Join("", s.Where(c => char.IsDigit(c))), out result);
                            OperTechInform oti = new OperTechInform();
                            oti.TypeOI = "ТИ";
                            oti.NumberOI = result;
                            list.Add(oti);
                        }
                        if ((s[0] == chS) && (s.Substring(s.Length - 1) == "V"))
                        {
                            int.TryParse(string.Join("", s.Where(c => char.IsDigit(c))), out result);
                            OperTechInform oti = new OperTechInform();
                            oti.TypeOI = "ТС";
                            oti.NumberOI = result;
                            list.Add(oti);
                        }
                    }
                    ListFormula = list;
                }
            }
        }
        public List<OperTechInform> ListFormula
        {
            get { return _listFormula; }
            set
            {
                _listFormula = value;
            }
        }
    }
}
