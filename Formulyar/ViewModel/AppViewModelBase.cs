using Formulyar.Foundation;
using Formulyar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Formulyar.ViewModel
{
    class AppViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Members
        private ObservableCollection<ExchangeOTI> _sourceCollect = new ObservableCollection<ExchangeOTI>();
        private ObservableCollection<ExchangeOTI> _reseiverCollect = new ObservableCollection<ExchangeOTI>();
        private ObservableCollection<OperTechInform> _otiCollect = new ObservableCollection<OperTechInform>();
        private OperTechInform _selectedOti;
        private ObservableCollection<OperTechInform> _otiCollectReception = new ObservableCollection<OperTechInform>();
        private ObservableCollection<OperTechInform> _otiCollectSource = new ObservableCollection<OperTechInform>();
        private ObservableCollection<ExchangeOTI> _otiCollectExchange= new ObservableCollection<ExchangeOTI>();
        private ObservableCollection<Secheniya> _secheniyaCollect = new ObservableCollection<Secheniya>();
        private Secheniya _selectedSech;
        private ObservableCollection<Voltage> _voltageCollect = new ObservableCollection<Voltage>();
        private Voltage _selectedVoltage;
        private ObservableCollection<CurrentLine> _currentLineCollect = new ObservableCollection<CurrentLine>();
        private CurrentLine _selectedCurrentLine;
        private ObservableCollection<CurrentTransform> _currentTransformCollect = new ObservableCollection<CurrentTransform>();
        private CurrentTransform _selectedCurrentTransform;
        private ObservableCollection<CurrentBreaker> _currentBreakerCollect = new ObservableCollection<CurrentBreaker>();
        private CurrentBreaker _selectedCurrentBreaker;
        private ObservableCollection<CurrentEquipment> _currentEquipmentCollect = new ObservableCollection<CurrentEquipment>();
        private CurrentEquipment _selectedCurrentEquipment;
        private ObservableCollection<Aopo> _currentAopoCollect = new ObservableCollection<Aopo>();
        private Aopo _selectedCurrentAopo;
        private ObservableCollection<Protocol> _chekProtocol = new ObservableCollection<Protocol>();
        private Protocol _selectedItemProtocol;
        private bool _saveButtonIsEnebled = false;

        #endregion
        #region Properties
        public void RaisePropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public bool SaveButtonIsEnebled
        {
            get { return _saveButtonIsEnebled; }
            set { _saveButtonIsEnebled = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ  на прием
        /// </summary>
        public ObservableCollection<OperTechInform> OtiCollectReception
        {
            get { return _otiCollectReception; }
            set { _otiCollectReception = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ на передачу
        /// </summary>
        public ObservableCollection<OperTechInform> OtiCollectSource
        {
            get { return _otiCollectSource; }
            set { _otiCollectSource = value; RaisePropertyChanged(); }
        }
       public  ObservableCollection<ExchangeOTI> ExchangeCollect
        {
            get { return _otiCollectExchange; }
            set { _otiCollectExchange = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ всех ДЦ
        /// </summary>
        public ObservableCollection<OperTechInform> OtiCollect
        {
            get { return _otiCollect;  }
            set { _otiCollect = value; RaisePropertyChanged();  }
        }
        /// <summary>
        /// Список ОТИ контроля КПОС
        /// </summary>
        public ObservableCollection<Secheniya> SecheniyaCollect
        {
            get { return _secheniyaCollect; }
            set { _secheniyaCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ передаваемой
        /// </summary>
        public ObservableCollection<ExchangeOTI> SourceCollect
        {
            get { return _sourceCollect; }
            set { _sourceCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ принимаемой
        /// </summary>
        public ObservableCollection<ExchangeOTI> ReseiverCollect
        {
            get { return _reseiverCollect; }
            set { _reseiverCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ контроля МУН
        /// </summary>
        public ObservableCollection<Voltage> VoltageCollect
        {
            get { return _voltageCollect; }
            set { _voltageCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ контроля СМТН.ЛЭП
        /// </summary>
        public ObservableCollection<CurrentLine> CurrentLineCollect
        {
            get { return _currentLineCollect; }
            set { _currentLineCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ контроля СМТН.АОПО
        /// </summary>
        public ObservableCollection<Aopo> CurrentAopoCollect
        {
            get { return _currentAopoCollect; }
            set { _currentAopoCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ контроля СМТН.АТ(Т)
        /// </summary>
        public ObservableCollection<CurrentTransform> CurrentTransformCollect
        {
            get { return _currentTransformCollect; }
            set { _currentTransformCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ контроля СМТН.Выкл
        /// </summary>
        public ObservableCollection<CurrentBreaker> CurrentBreakerCollect
        {
            get { return _currentBreakerCollect; }
            set { _currentBreakerCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Список ОТИ котроля СМТН.Доп
        /// </summary>
        public ObservableCollection<CurrentEquipment> CurrentEquipmentCollect
        {
            get { return _currentEquipmentCollect; }
            set { _currentEquipmentCollect = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Протокол проверки
        /// </summary>
        public ObservableCollection<Protocol> ChekProtocol
        {
            get { return _chekProtocol; }
            set { _chekProtocol = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т КПОС
        /// </summary>
        public Secheniya SelectedSech
        {
            get { return _selectedSech; }
            set { _selectedSech = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т МУН
        /// </summary>
        public Voltage SelectedVoltage
        {
            get { return _selectedVoltage; }
            set { _selectedVoltage = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т СМТН.ЛЭП
        /// </summary>
        public CurrentLine SelectedCurrentLine
        {
            get { return _selectedCurrentLine; }
            set { _selectedCurrentLine = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т СМТН.АТ(Т)
        /// </summary>
        public CurrentTransform SelectedCurrentTransform
        {
            get { return _selectedCurrentTransform; }
            set { _selectedCurrentTransform = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т СМТН.Выкл
        /// </summary>
        public CurrentBreaker SelectedCurrentBreaker
        {
            get { return _selectedCurrentBreaker; }
            set { _selectedCurrentBreaker = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т СМТН.Доп
        /// </summary>
        public CurrentEquipment SelectedCurrentEquipment
        {
            get { return _selectedCurrentEquipment; }
            set { _selectedCurrentEquipment = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т СМТН.АОПО
        /// </summary>
        public Aopo SelectedCurrentAopo
        {
            get { return _selectedCurrentAopo; }
            set { _selectedCurrentAopo = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т Протокола проверки
        /// </summary>
        public Protocol SelectedItemProtocol
        {
            get { return _selectedItemProtocol; }
            set { _selectedItemProtocol = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранный эл-т ОТИ
        /// </summary>
        public OperTechInform SelectedOti
        {
            get { return _selectedOti; }
            set { _selectedOti = value; RaisePropertyChanged(); }
        }
        #endregion
    }
    public class MyObservableCollection<OperTechInform> : ObservableCollection<OperTechInform>
    {
        public void UpdateCollection()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Reset));
        }
    }
}
