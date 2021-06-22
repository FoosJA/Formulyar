using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class Secheniya
    {
        #region Members
        private int _idSech;
        private string _dispatchCenter;
        private string _nameSech;
        private string _nameOI;
        private int _numberOI;
        private string _typeOI;
        private string _formula;
        private string _energyObject;
        #endregion
        #region Properties
        /// <summary>
        /// Диспетчерский центр
        /// </summary>
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        public int IdSech
        {
            get { return _idSech; }
            set { _idSech = value; }
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
        /// Номер ТИ\ТС в КС
        /// </summary>
        public int NumberOI
        {
            get { return _numberOI; }
            set { _numberOI = value; }
        }
        /// <summary>
        /// Тип ОИ
        /// </summary>
        public string TypeOI
        {
            get { return _typeOI; }
            set { _typeOI = value; }
        }
        /// <summary>
        /// Название КС
        /// </summary>
        public string NameSech
        {
            get { return _nameSech; }
            set { _nameSech = value; }
        }
        
        /// <summary>
        /// Оборудование в составе КС
        /// </summary>
        public string NameOI
        {
            get { return _nameOI; }
            set { _nameOI = value; }
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
                    value.Replace('/', '!');
                    string[] words = value.Split('\r');
                    _formula = words[0];
                }

            }
        }
        #endregion
    }
}
