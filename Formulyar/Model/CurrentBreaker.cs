using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class CurrentBreaker
    {
        #region Members
        private string _dispatchCenter;
        private string _nameBreaker;
        private int _idBreaker;
        private string _energyObject;
        private string _nameIfact;
        private int _numberIfact;
        private string _formula;
        private string _formulaCode;
        private string _nameIa;
        private int _numberIa;
        private string _nameIb;
        private int _numberIb;
        private string _nameIc;
        private int _numberIc;
        private string _nameItnv;
        private int _numberItnv;
        private string _nameS;
        private int _numberS;
        private List<OperTechInform> _listFormula;
        #endregion
        #region Properties
        /// <summary>
        /// Диспетчерский контролирующий ток
        /// </summary>
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        public int IdBreaker
        {
            get { return _idBreaker; }
            set { _idBreaker = value; }
        }
        /// <summary>
        /// Имя линии
        /// </summary>
        public string NameBreaker
        {
            get { return _nameBreaker; }
            set { _nameBreaker = value; }
        }
        /// <summary>
        /// Присоединение
        /// </summary>
        public string EnergyObject
        {
            get { return _energyObject; }
            set { _energyObject = value; }
        }
        /// <summary>
        /// Наименование Iфакт
        /// </summary>
        public string NameOIfact
        {
            get { return _nameIfact; }
            set { _nameIfact = value; }
        }
        /// <summary>
        /// Номер Iфакт
        /// </summary>
        public int NumberIfact
        {
            get { return _numberIfact; }
            set { _numberIfact = value; }
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
        /// Наименование Ia
        /// </summary>
        public string NameIa
        {
            get { return _nameIa; }
            set { _nameIa = value; }
        }
        /// <summary>
        /// Номер Ia
        /// </summary>
        public int NumberIa
        {
            get { return _numberIa; }
            set { _numberIa = value; }
        }
        /// <summary>
        /// Наименование Ib
        /// </summary>
        public string NameIb
        {
            get { return _nameIb; }
            set { _nameIb = value; }
        }
        /// <summary>
        /// Номер Ib
        /// </summary>
        public int NumberIb
        {
            get { return _numberIb; }
            set { _numberIb = value; }
        }
        /// <summary>
        /// Наименование Ic
        /// </summary>
        public string NameIc
        {
            get { return _nameIc; }
            set { _nameIc = value; }
        }
        /// <summary>
        /// Номер Ic
        /// </summary>
        public int NumberIc
        {
            get { return _numberIc; }
            set { _numberIc = value; }
        }
        /// <summary>
        /// Наименование ТНВ
        /// </summary>
        public string NameItnv
        {
            get { return _nameItnv; }
            set { _nameItnv = value; }
        }
        /// <summary>
        /// Номер ТНВ
        /// </summary>
        public int NumberItnv
        {
            get { return _numberItnv; }
            set { _numberItnv = value; }
        }
        /// <summary>
        /// Наименование ТС
        /// </summary>
        public string NameS
        {
            get { return _nameS; }
            set { _nameS = value; }
        }
        /// <summary>
        /// Номер ТС
        /// </summary>
        public int NumberS
        {
            get { return _numberS; }
            set { _numberS = value; }
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
