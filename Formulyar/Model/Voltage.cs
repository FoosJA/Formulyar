using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class Voltage
    {
        #region Members
        private string _dispatchCenter;
        private string _energyObject;
        private int _voltageLevel;
        private int _idVoltage;
        private int _numberOIitog;
        private string _nameOIitog;
        private string _typeOIitog;
        private int _numberOI;
        private string _nameOI;
        private string _typeOI;
        private string _formula;
        private string _typeControl;
        private string _formulaCode;
        private string _formulaOI;
        private List<OperTechInform> _listFormula;
        #endregion
        #region Properties
        /// <summary>
        /// Диспетчерский контролирующий напряжение
        /// </summary>
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        public string TypeControl
        {
            get { return _typeControl; }
            set { _typeControl = value; }
        }
        /// <summary>
        /// Объект диспетчеризации
        /// </summary>
        public string EnergyObject
        {
            get { return _energyObject; }
            set { _energyObject = value; }
        }
        /// <summary>
        /// Номер ОИ итог
        /// </summary>
        public int NumberOIitog
        {
            get { return _numberOIitog; }
            set { _numberOIitog = value; }
        }
        public int IdVoltage
        {
            get { return _idVoltage; }
            set { _idVoltage= value; }
        }
        public string NameOIitog
        {
            get { return _nameOIitog; }
            set { _nameOIitog = value; }
        }
        /// <summary>
        /// Тип ОИ итог
        /// </summary>
        public string TypeOIitog
        {
            get { return _typeOIitog; }
            set { _typeOIitog = value; }
        }
        /// <summary>
        /// Номер ОИ
        /// </summary>
        public int NumberOI
        {
            get { return _numberOI; }
            set { _numberOI = value; }
        }
        public string NameOI
        {
            get { return _nameOI; }
            set { _nameOI = value; }
        }
        /// <summary>
        /// Тип ОИ
        /// </summary>
        public string TypeOI
        {
            get { return _typeOI; }
            set { _typeOI = value; }
        }
        public int VoltageLevel
        {
            get { return _voltageLevel; }
            set { _voltageLevel = value; }
        }
        /// <summary>
        /// Формула дорасчёта
        /// </summary>
        public string Formula
        {
            get { return _formula; }
            set
            {
                if (value != null)              
                    _formula = value;     
            }
        }
        public string FormulaOI
        {
            get { return _formulaOI; }
            set
            {
                if (value != null)
                    _formulaOI = value;
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
                if (_formulaCode != null)
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
                            if (list.Any(x=>x.NumberOI == oti.NumberOI && x.TypeOI==oti.TypeOI)==false)
                                list.Add(oti);
                        }
                        if ((s[0] == chS) && (s.Substring(s.Length - 1) == "V"))
                        {
                            int.TryParse(string.Join("", s.Where(c => char.IsDigit(c))), out result);
                            OperTechInform oti = new OperTechInform();
                            oti.TypeOI = "ТС";
                            oti.NumberOI = result;
                            if (list.Any(x => x.NumberOI == oti.NumberOI && x.TypeOI == oti.TypeOI) == false)
                                list.Add(oti);
                        }
                    }
                    ListFormula = list;
                }
            }
        }
        /// <summary>
        /// Код формулы дорасчёта
        /// </summary>
        public List<OperTechInform> ListFormula
        {
            get { return _listFormula; }
            set
            {
                _listFormula = value;
            }
        }
        #endregion

    }
}
