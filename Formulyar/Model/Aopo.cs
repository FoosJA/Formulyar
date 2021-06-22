using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.Model
{
    class Aopo
    {
        #region Members
        private string _dispatchCenter;
        private string _equipment;
        private int _equipmentID;
        private string _energyObject;
        private int _step;
        private string _elementName;
        private int _numberTIobject;
        private string _nameTIobject;
        private int _numberTIstandart;
        private string _nameTIstandart;
        private int _numberTI;
        private string _nameTI;
        private int _numberSitog;
        private string _nameSitog;
        private int _numberS;
        private string _nameS;
        #endregion
        public string DispatchCenter
        {
            get { return _dispatchCenter; }
            set { _dispatchCenter = value; }
        }
        public string Equipment
        {
            get { return _equipment; }
            set { _equipment = value; }
        }
        public int EquipmentID
        {
            get { return _equipmentID; }
            set { _equipmentID = value; }
        }
        public string EnergyObject
        {
            get { return _energyObject; }
            set { _energyObject = value; }
        }
        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }
        public string ElementName
        {
            get { return _elementName; }
            set { _elementName = value; }
        }
        public int NumberTI
        {
            get { return _numberTI; }
            set { _numberTI = value; }
        }
        public string NameTI
        {
            get { return _nameTI; }
            set { _nameTI = value; }
        }
        public int NumberTIobject
        {
            get { return _numberTIobject; }
            set { _numberTIobject = value; }
        }
        public string NameTIobject
        {
            get { return _nameTIobject; }
            set { _nameTIobject = value; }
        }
        public int NumberTIstandart
        {
            get { return _numberTIstandart; }
            set { _numberTIstandart = value; }
        }
        public string NameTIstandart
        {
            get { return _nameTIstandart; }
            set { _nameTIstandart = value; }
        }
        public int NumberSitog
        {
            get { return _numberSitog ; }
            set { _numberSitog = value; }
        }
        public string NameSitog
        {
            get { return _nameSitog; }
            set { _nameSitog = value; }
        }
        public int NumberS
        {
            get { return _numberS; }
            set { _numberS = value; }
        }
        public string NameS
        {
            get { return _nameS; }
            set { _nameS= value; }
        }
    }
}
