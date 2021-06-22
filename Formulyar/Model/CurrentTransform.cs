using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class CurrentTransform
    {
        #region Members
        private string _dispatchCenter;
        private string _energyObject;
        private string _nameTransform;
        private int _idTransform;
        private string _transformWinding;
        private int _idWinding;
        private string _nameIfact;
        private int _numberIfact;
        private string _formula;
        private string _formulaCode;
        private string _nameRpn;
        private int _numberRpn;
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
        /// <summary>
        /// Имя линии
        /// </summary>
        public string NameTransform
        {
            get { return _nameTransform; }
            set { _nameTransform = value; }
        }
       public int idTransform
        {
            get { return _idTransform; }
            set { _idTransform = value; }
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
        /// Обмотка тр-ра
        /// </summary>
        public string TransformWinding
        {
            get { return _transformWinding; }
            set { _transformWinding = value; }
        }
        public int idWinding
        {
            get { return _idWinding; }
            set { _idWinding = value; }
        }
        /// <summary>
        /// Наименование Iфакт
        /// </summary>
        public string NameIfact
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
            set {
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
        /// <summary>
        /// Наименование I рпн
        /// </summary>
        public string NameRpn
        {
            get { return _nameRpn; }
            set { _nameRpn = value; }
        }
        /// <summary>
        /// Номер I рпн
        /// </summary>
        public int NumberRpn
        {
            get { return _numberRpn; }
            set { _numberRpn = value; }
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
        #endregion
    }
}
