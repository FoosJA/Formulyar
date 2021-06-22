using Formulyar.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.Model
{
    class ExchangeOTI
    {
        private string _dCsource;
        private string _dCreceiver;
        private string _address;
        private int _numberOIsour;
        private string _typeOIsour;
        private int _numberOIrec;
        private string _typeOIrec;
        public string DCsource
        {
            get { return _dCsource; }
            set { _dCsource = value; }
        }
        public string DCreceiver
        {
            get { return _dCreceiver; }
            set { _dCreceiver = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public int NumberOIsour
        {
            get { return _numberOIsour; }
            set { _numberOIsour = value; }
        }
        public string TypeOIsour
        {
            get { return _typeOIsour; }
            set { _typeOIsour = value; }
        }
        public int NumberOIrec
        {
            get { return _numberOIrec; }
            set { _numberOIrec = value; }
        }
        public string TypeOIrec
        {
            get { return _typeOIrec; }
            set { _typeOIrec = value; }
        }
        //public static ObservableCollection<ExchangeOTI> GetCollection(OperTechInform oti)
        //{

        //}
    }
}
