using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class CurrentEquipment
    {
        #region Members
        private string _dispatchCenter;
        private string _energyObject;
        private string _nameEquipment;
        private string _typeOTI;
        private int _numberOTI;
        private string _formula;
        private string _formulaCode;
        private string _nameOTI;
        private List<OperTechInform> _listFormula;
        #endregion
        #region Properties
        /// <summary>
        /// ДЦ
        /// </summary>
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        /// <summary>
        /// Оборудование
        /// </summary>
        public string NameEquipment
        {
            get { return _nameEquipment; }
            set { _nameEquipment = value; }
        }
        /// <summary>
        /// Объект
        /// </summary>
        public string EnergyObject
        {
            get { return _energyObject; }
            set { _energyObject = value; }
        }
        /// <summary>
        /// Тип ОИ
        /// </summary>
        public string TypeOTI
        {
            get { return _typeOTI; }
            set { _typeOTI = value; }
        }
        /// <summary>
        /// Номер ОИ
        /// </summary>
        public int NumberOTI
        {
            get { return _numberOTI; }
            set { _numberOTI = value; }
        }
        /// <summary>
        /// Имя ОИ
        /// </summary>
        public string NameOTI
        {
            get { return _nameOTI; }
            set { _nameOTI = value; }
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
                {
                    _formula = value;
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
