using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;
using Formulyar.Foundation;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using Formulyar.Model;
using System.IO;

namespace Formulyar.ViewModel
{
    class AppViewModel : AppViewModelBase
    {
        public AppViewModel() { }

        #region Members
        private ObservableCollection<Protocol> _chekProtocolAll = new ObservableCollection<Protocol>();
        private OperTechInform _selectedOti;
        private StoreDB _database = new StoreDB();
        private string _dispatchCenter = "ОДУ Средней Волги";
        private bool displayXamlTab;
        //private bool _saveButtonIsEnebled=false;
        private int _index;
        private DataGridCellInfo _cellInfo;
        #endregion

        #region Triggers    

        private bool _triggerSMTNline;
        public bool TriggerSMTNline
        {
            get { return _triggerSMTNline; }
            set { _triggerSMTNline = value; RaisePropertyChanged(); }
        }
        private bool _triggerSMTNtransformer;
        public bool TriggerSMTNtransformer
        {
            get { return _triggerSMTNtransformer; }
            set { _triggerSMTNtransformer = value; RaisePropertyChanged(); }
        }
        private bool _triggerSMTNbreaker;
        public bool TriggerSMTNbreaker
        {
            get { return _triggerSMTNbreaker; }
            set { _triggerSMTNbreaker = value; RaisePropertyChanged(); }
        }
        private bool _triggerSMTNequipment;
        public bool TriggerSMTNbequipment
        {
            get { return _triggerSMTNequipment; }
            set { _triggerSMTNequipment = value; RaisePropertyChanged(); }
        }
        private bool _mun;
        public bool TriggerMUN
        {
            get { return _mun; }
            set { _mun = value; RaisePropertyChanged(); }
        }
        private bool _triggerKPOS;
        public bool TriggerKPOS
        {
            get { return _triggerKPOS; }
            set { _triggerKPOS = value; RaisePropertyChanged(); }
        }

        private bool _triggerAOPO;
        public bool TriggerAOPO
        {
            get { return _triggerAOPO; }
            set { _triggerAOPO = value; RaisePropertyChanged(); }
        }
        private bool _triggerOIK;
        public bool TriggerOIK
        {
            get { return _triggerOIK; }
            set { _triggerOIK = value; RaisePropertyChanged(); }
        }
        public bool triggerCheckKPOS = false;
        public bool triggerCheckMUN = false;
        public bool triggerCheckSMTNline = false;
        public bool triggerCheckSMTNtransformer = false;
        public bool triggerCheckSMTNbreaker = false;
        public bool triggerCheckSMTNequipment = false;
        public bool triggerCheckSMTNAOPO = false;
        public bool triggerCheckAIP = false;
        public bool triggerCheckExchange = false;
        public bool triggerCheckCIM = true;
        #endregion

        #region Properties
        /// <summary>
        /// Переход на вкладку
        /// </summary>
        public bool DisplayXamlTab
        {
            get { return this.displayXamlTab; }
            set { displayXamlTab = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// ДЦ для фильтра проверок
        /// </summary>
        public string DispatchCenterFilter
        {
            get { return _dispatchCenter; }
            set
            {
                _dispatchCenter = value;
                if (value != "Все ДЦ")
                    ChekProtocol = new ObservableCollection<Protocol>(_chekProtocolAll.Where(x => x.DispatchCenter.Equals(_dispatchCenter)));
                else
                    ChekProtocol = ChekProtocolAll;
            }
        }
        /// <summary>
        /// Коллекция проверок по всем ДЦ
        /// </summary>
        public ObservableCollection<Protocol> ChekProtocolAll
        {
            get { return _chekProtocolAll; }
            set { _chekProtocolAll = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Выбранное ОТИ
        /// </summary>
        public new OperTechInform SelectedOti
        {
            get { return _selectedOti; }
            set
            {
                TriggerKPOS = false;
                TriggerMUN = false;
                TriggerSMTNbequipment = false;
                TriggerSMTNbreaker = false;
                TriggerSMTNline = false;
                TriggerSMTNtransformer = false;
                TriggerAOPO = false;
                OtiCollectReception.Clear();
                OtiCollectSource.Clear();

                if (value != null)
                {
                    _selectedOti = value;                   
                    //OtiCollectReception = _database.GetReception(value); //Для проверки правильности запроса
                    //OtiCollectSource = _database.GetSource(value);
                    var collectReception = ExchangeCollect.Where(x => x.NumberOIrec == value.NumberOI &&
                    x.DCreceiver == value.DispatchCenter && x.TypeOIrec == value.TypeOI);
                    foreach (var ex in collectReception)
                    {
                        var otiSour = OtiCollect.First(x => x.NumberOI == ex.NumberOIsour &&
                             x.DispatchCenter == ex.DCsource &&
                             x.TypeOI == ex.TypeOIsour);
                        OtiCollectSource.Add(otiSour);
                    }                    
                    var collectSource = ExchangeCollect.Where(x => x.NumberOIsour == value.NumberOI &&
                    x.DCsource == value.DispatchCenter && x.TypeOIsour == value.TypeOI);                    
                    foreach (var ex in collectSource)
                    {
                        var otiRec = OtiCollect.First(x => x.NumberOI == ex.NumberOIrec &&
                           x.DispatchCenter == ex.DCreceiver &&
                           x.TypeOI == ex.TypeOIrec);
                        OtiCollectReception.Add(otiRec);
                    }
                   
                    #region Тригер КПОС
                    if (SecheniyaCollect.Any(x => x.NumberOI == value.NumberOI &&
                    x.DispatchCenter == value.DispatchCenter && value.TypeOI == x.TypeOI))
                    {
                        TriggerKPOS = true;
                    }
                    #endregion
                    #region Тригер МУН
                    if (VoltageCollect.Any(x => x.NumberOI == value.NumberOI &&
                    x.DispatchCenter == value.DispatchCenter && value.TypeOI == x.TypeOI))
                    {
                        TriggerMUN = true;
                    }
                    if (VoltageCollect.Any(x => x.NumberOIitog == value.NumberOI &&
                    x.DispatchCenter == value.DispatchCenter && value.TypeOI == x.TypeOIitog))
                    {
                        TriggerMUN = true;
                    }
                    //if (VoltageCollect.Any(x => x.ListFormula != null && x.ListFormula.Any(y => y.NumberOI == value.NumberOI &&
                    //y.TypeOI == value.TypeOI) == true && x.DispatchCenter == value.DispatchCenter))
                    //{
                    //    TriggerMUN = true;
                    //}
                    #endregion
                    #region Тригер СМТН ЛЭП
                    if (CurrentLineCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIa == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNline = true;
                    }
                    if (CurrentLineCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIc == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNline = true;
                    }
                    if (CurrentLineCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIb == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNline = true;
                    }
                    if (CurrentLineCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIfact == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNline = true;
                    }
                    if (CurrentLineCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberItnv == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNline = true;
                    }
                    if (CurrentLineCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberS == value.NumberOI && value.TypeOI == "ТС"))
                    {
                        TriggerSMTNline = true;
                    }
                    if (CurrentLineCollect.Any(x => x.ListFormula != null && x.ListFormula.Any(y => y.NumberOI == value.NumberOI &&
                    y.TypeOI == value.TypeOI) == true && x.DispatchCenter == value.DispatchCenter))
                    {
                        TriggerSMTNline = true;
                    }
                    #endregion
                    #region Тригер СМТН Выкл
                    if (CurrentBreakerCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIa == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    if (CurrentBreakerCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIc == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    if (CurrentBreakerCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIb == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    if (CurrentBreakerCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIfact == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    if (CurrentBreakerCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberItnv == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    if (CurrentBreakerCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberS == value.NumberOI && value.TypeOI == "ТС"))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    if (CurrentBreakerCollect.Any(x => x.ListFormula != null && x.ListFormula.Any(y => y.NumberOI == value.NumberOI &&
                    y.TypeOI == value.TypeOI) == true && x.DispatchCenter == value.DispatchCenter))
                    {
                        TriggerSMTNbreaker = true;
                    }
                    #endregion
                    #region Тригер СМТН АТ(Т)               
                    if (CurrentTransformCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberIfact == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNtransformer = true;
                    }
                    if (CurrentTransformCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberItnv == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNtransformer = true;
                    }
                    if (CurrentTransformCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberRpn == value.NumberOI && value.TypeOI == "ТИ"))
                    {
                        TriggerSMTNtransformer = true;
                    }
                    if (CurrentTransformCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberS == value.NumberOI && value.TypeOI == "ТС"))
                    {
                        TriggerSMTNtransformer = true;
                    }
                    if (CurrentTransformCollect.Any(x => x.ListFormula != null && x.ListFormula.Any(y => y.NumberOI == value.NumberOI &&
                    y.TypeOI == value.TypeOI) == true && x.DispatchCenter == value.DispatchCenter))
                    {
                        TriggerSMTNtransformer = true;
                    }
                    #endregion
                    #region АОПО
                    if (CurrentAopoCollect.Any(x => x.NumberTI == value.NumberOI &&
                    x.DispatchCenter == value.DispatchCenter && value.TypeOI == "ТИ"))
                    {
                        TriggerAOPO = true;
                    }
                    if (CurrentAopoCollect.Any(x => x.NumberSitog == value.NumberOI &&
                    x.DispatchCenter == value.DispatchCenter && value.TypeOI == "ТС"))
                    {
                        TriggerAOPO = true;
                    }
                    if (CurrentAopoCollect.Any(x => x.NumberS == value.NumberOI &&
                    x.DispatchCenter == value.DispatchCenter && value.TypeOI == "ТС"))
                    {
                        TriggerAOPO = true;
                    }
                    #endregion
                    #region Тригер СМТН Доп               
                    if (CurrentEquipmentCollect.Any(x => x.DispatchCenter == value.DispatchCenter && x.NumberOTI == value.NumberOI && value.TypeOI == x.TypeOTI))
                    {
                        TriggerSMTNbequipment = true;
                    }
                    if (CurrentEquipmentCollect.Any(x => x.ListFormula != null && x.ListFormula.Any(y => y.NumberOI == value.NumberOI &&
                    y.TypeOI == value.TypeOI) == true && x.DispatchCenter == value.DispatchCenter))
                    {
                        TriggerSMTNbequipment = true;
                    }
                    #endregion
                }
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Текущее значение ячейки
        /// </summary>
        public DataGridCellInfo CellInfo
        {
            get { return _cellInfo; }
            set
            {
                _cellInfo = value;
                if (_cellInfo.Column != null)
                    _index = _cellInfo.Column.DisplayIndex;
            }
        }
        /// <summary>
        /// Видимость кнопки сохранить в БД
        /// </summary>
        //public bool SaveButtonIsEnebled
        //{
        //    get { return _saveButtonIsEnebled; }
        //    set { _saveButtonIsEnebled = value; RaisePropertyChanged(); }
        //}
        #endregion

        #region Command

        /// <summary>
        /// Команда запуска проверок
        /// </summary>
        public ICommand StartCheck { get { return new RelayCommand(StartCheckExecute, CanStartCheck); } }
        bool CanStartCheck() { return true; }
        void StartCheckExecute()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            WaitWindow waitWindow = new WaitWindow();
            waitWindow.Show();
            if (ChekProtocolAll != null)
                ChekProtocolAll.Clear();
            if (triggerCheckAIP)
            {
                ObservableCollection<OperTechInform> otiAIPCollect = _database.GetOtiAIP();
                for (int i = 0; i < otiAIPCollect.Count(); i++)
                {
                    ChekProtocolAll.Add(new Protocol()
                    {
                        KontrolObjectName = otiAIPCollect[i].EnergyObject,
                        TypeOI = otiAIPCollect[i].TypeOI,
                        NumberOI = otiAIPCollect[i].NumberOI,
                        DcSource = otiAIPCollect[i].DispatchCenter,
                        NumberOIsource = otiAIPCollect[i].NumberOI,
                        DispatchCenter = otiAIPCollect[i].DispatchCenter,
                        TypeKontrol = "АИП",
                        Category = otiAIPCollect[i].Category,
                        NameTI = otiAIPCollect[i].NameOI,
                        Info = otiAIPCollect[i].TypeOI + otiAIPCollect[i].NumberOI + " не заведено в АИП"
                    });
                }
            }

            //Формируем инф об приеме-передачи всей ОТИ
            /*var result = from exRec in ReseiverCollect
                         join exSour in SourceCollect on new { exRec.Address, exRec.DCreceiver, exRec.DCsource, Type=exRec.TypeOIrec}
                         equals new { exSour.Address, exSour.DCreceiver, exSour.DCsource, Type=exSour.TypeOIsour}
                         select (new ExchangeOTI()
                         {
                             NumberOIrec = exRec.NumberOIrec,
                             DCreceiver = exRec.DCreceiver,
                             TypeOIrec = exRec.TypeOIrec,
                             NumberOIsour = exSour.NumberOIsour,
                             DCsource = exSour.DCsource,
                             TypeOIsour = exSour.TypeOIsour,
                             Address = exSour.Address
                         });
           
                ExchangeCollect = new ObservableCollection<ExchangeOTI>(result);*/
            ObservableCollection<ExchangeProtocol> exProtocolList = new ObservableCollection<ExchangeProtocol>();
            string[] DC = new string[6] { "ОДУ Средней Волги", "Нижегородское РДУ",  "Пензенское РДУ", "Самарское РДУ", "Саратовское РДУ", "РДУ Татарстана" };            
            if (triggerCheckCIM)
            {
                ObservableCollection<CIMObject> otiCIMCollect = _database.GetOtiCIM();                
                int dc = 0;
                bool isSendCDU = false;
                ObservableCollection<CIMObject> otiCIM_DC = new ObservableCollection<CIMObject>(otiCIMCollect.Where(x => x.DC == DC[dc]));//вся ОИ в ИМ в одного ОДУ
                foreach (CIMObject oi_CIM in otiCIM_DC)
                {
                    try
                    {
                        OperTechInform otiODU = OtiCollect.Single(x => x.DispatchCenter == DC[dc] && x.TypeOI== oi_CIM.mvCat && x.NumberOI == oi_CIM.mvExternalId);//ОИ в СК2007    
                        ObservableCollection<ExchangeOTI> exchangeRec = new ObservableCollection<ExchangeOTI>(ExchangeCollect.Where(x => (x.NumberOIrec == otiODU.NumberOI && x.TypeOIrec == otiODU.TypeOI && x.DCreceiver == otiODU.DispatchCenter)));
                        ObservableCollection<ExchangeOTI> exchangeSour = new ObservableCollection<ExchangeOTI>(ExchangeCollect.Where(x => (x.NumberOIsour == otiODU.NumberOI && x.TypeOIsour == otiODU.TypeOI && x.DCsource == otiODU.DispatchCenter)));
                        if (otiODU.TypeOI != "ТИ" && otiODU.TypeOI != "ТС" && otiODU.NumberOI==2762)
                        {
                            dc = 0;
                        }
                        foreach (ExchangeOTI obmenRec in exchangeSour)
                        {
                            try
                            {
                                CIMObject otiODUrecCIM = otiCIMCollect.First(x => x.DC == obmenRec.DCreceiver &&
                                x.mvCat == obmenRec.TypeOIrec &&
                                x.mvExternalId== obmenRec.NumberOIrec);
                                isSendCDU = SourceCollect.Any(x => x.NumberOIsour == otiODU.NumberOI && x.TypeOIsour == otiODU.TypeOI && x.DCsource == otiODU.DispatchCenter && x.DCreceiver == "ЦДУ ИА");
                                if (isSendCDU == false)
                                    isSendCDU = SourceCollect.Any(x => x.NumberOIsour == otiODU.NumberOI && x.TypeOIsour == otiODU.TypeOI && x.DCsource == otiODU.DispatchCenter && x.DCreceiver == "РДЦ ЦДУ ИА");
                                if (otiODUrecCIM.poUID != oi_CIM.poUID)
                                {
                                    exProtocolList.Add(new ExchangeProtocol()
                                    {
                                        KontrolObjectName= otiODU.EnergyObject,
                                        DcSource= obmenRec.DCsource,
                                        NameDCSource= otiODU.NameOI,
                                        CategoryDCSource = obmenRec.TypeOIsour,
                                        NumberOIsource= obmenRec.NumberOIsour,
                                        UidDCSource= oi_CIM.mvUID,
                                        PoDCSource= oi_CIM.poUID,

                                        dcRec = obmenRec.DCreceiver,
                                        NamedcRec = otiODUrecCIM.mvName,
                                        CategorydcRec = obmenRec.TypeOIrec,
                                        NumberOIrec = obmenRec.NumberOIrec,
                                        UiddcRec = otiODUrecCIM.mvUID,
                                        PodcRec = otiODUrecCIM.poUID,
                                        IsSendCDU = isSendCDU,
                                        Info = "Несоответсвие привязки в ИМ"
                                    });
                                    /*ChekProtocolAll.Add(new Protocol()
                                    {
                                        IsSendCDU = isSendCDU,
                                        KontrolObjectName = otiODU.NameOI,
                                        TypeOI = otiODU.TypeOI,
                                        NumberOI = otiODU.NumberOI,
                                        DcSource = obmenRec.DCsource,
                                        NumberOIsource = obmenRec.NumberOIsour,
                                        DCReception = obmenRec.DCreceiver,
                                        NumberOIreception = obmenRec.NumberOIrec,
                                        DispatchCenter = DC[dc],
                                        TypeKontrol = otiODU.EnergyObject,
                                        Category = oi_CIM.UIDparentObj,
                                        NameTI = otiODUrecCIM.UIDparentObj,                                        
                                        Info = "Несоответсвие привязки в ИМ" /*+ otiODU.DispatchCenter + ": " + otiCIM_DC[i].UIDparentObj +
                                        "\n" + obmenRec.DCreceiver + ": " + otiODUrecCIM.UIDparentObj
                                    });*/

                                }                                    
                                /*else if (otiODUrecCIM.MeasValueType != oi_CIM.MeasValueType)
                                    exProtocolList.Add(new ExchangeProtocol()
                                    {
                                        KontrolObjectName = otiODU.EnergyObject,
                                        DcSource = obmenRec.DCsource,
                                        NameDCSource = otiODU.NameOI,
                                        CategoryDCSource = obmenRec.TypeOIsour,
                                        NumberOIsource = obmenRec.NumberOIsour,
                                        UidDCSource = oi_CIM.mvUID,
                                        PoDCSource = oi_CIM.poUID,

                                        dcRec = obmenRec.DCreceiver,
                                        NamedcRec = otiODUrecCIM.mvName,
                                        CategorydcRec = obmenRec.TypeOIrec,
                                        NumberOIrec = obmenRec.NumberOIrec,
                                        UiddcRec = otiODUrecCIM.mvUID,
                                        PodcRec = otiODUrecCIM.poUID,
                                        IsSendCDU = isSendCDU,
                                        Info = "Несоответсвие типа измерения: " + otiODUrecCIM.MeasValueType + " и " + oi_CIM.MeasValueType
                                    });*/
                               /* ChekProtocolAll.Add(new Protocol()
                                    {

                                        IsSendCDU=isSendCDU,
                                        KontrolObjectName = otiODU.NameOI,
                                        TypeOI = otiODU.TypeOI,
                                        NumberOI = otiODU.NumberOI,
                                        DcSource = obmenRec.DCsource,
                                        NumberOIsource = obmenRec.NumberOIsour,
                                        DCReception = obmenRec.DCreceiver,
                                        NumberOIreception = obmenRec.NumberOIrec,
                                        DispatchCenter = DC[dc],
                                        TypeKontrol = otiODU.EnergyObject,
                                        Category = oi_CIM.UIDparentObj,
                                        NameTI = otiODUrecCIM.UIDparentObj,
                                        Info = "Несоответсвие типа измерения: "+otiODUrecCIM.MeasValueType + " и " + oi_CIM.MeasValueType
                                    });*/
                            }
                            catch (Exception e)
                            {
                                exProtocolList.Add(new ExchangeProtocol()
                                {
                                    KontrolObjectName = otiODU.EnergyObject,
                                    DcSource = obmenRec.DCsource,
                                    NameDCSource = otiODU.NameOI,
                                    CategoryDCSource = obmenRec.TypeOIsour,
                                    NumberOIsource = obmenRec.NumberOIsour,
                                    UidDCSource = oi_CIM.mvUID,
                                    PoDCSource = oi_CIM.poUID,

                                    dcRec = obmenRec.DCreceiver,
                                    //NamedcRec = otiODUrecCIM.mvName,
                                    CategorydcRec = obmenRec.TypeOIrec,
                                    NumberOIrec = obmenRec.NumberOIrec,
                                    //UiddcRec = otiODUrecCIM.mvUID,
                                    //PodcRec = otiODUrecCIM.poUID,
                                    IsSendCDU = isSendCDU,
                                    Info = "В ИМ " + obmenRec.DCreceiver + " нет получаемой ОИ " + obmenRec.TypeOIrec + obmenRec.NumberOIrec
                                });
                               // string inform = "В ИМ " + obmenRec.DCreceiver + " нет получаемой ОИ " + obmenRec.TypeOIrec + obmenRec.NumberOIrec + e.Message;
                            }
                        }
                        foreach (ExchangeOTI obmenSour in exchangeRec)
                        {
                            try
                            {
                                CIMObject otiODUsourCIM = otiCIMCollect.First(x => x.DC == obmenSour.DCsource &&
                                x.mvCat == obmenSour.TypeOIsour &&
                                x.mvExternalId == obmenSour.NumberOIsour);
                                isSendCDU = SourceCollect.Any(x => x.NumberOIsour == otiODU.NumberOI && x.TypeOIsour == otiODU.TypeOI && x.DCsource == otiODU.DispatchCenter && x.DCreceiver == "ЦДУ ИА");
                                if (isSendCDU==false)
                                    isSendCDU = SourceCollect.Any(x => x.NumberOIsour == otiODU.NumberOI && x.TypeOIsour == otiODU.TypeOI && x.DCsource == otiODU.DispatchCenter &&  x.DCreceiver == "РДЦ ЦДУ ИА");

                                if (otiODUsourCIM.poUID != oi_CIM.poUID)
                                    exProtocolList.Add(new ExchangeProtocol()
                                    {
                                        KontrolObjectName = otiODU.EnergyObject,
                                        DcSource = obmenSour.DCsource,
                                        NameDCSource = otiODUsourCIM.mvName,
                                        CategoryDCSource = obmenSour.TypeOIsour,
                                        NumberOIsource = obmenSour.NumberOIsour,
                                        UidDCSource = otiODUsourCIM.mvUID,
                                        PoDCSource = otiODUsourCIM.poUID,

                                        dcRec = obmenSour.DCreceiver,
                                        NamedcRec = otiODU.NameOI,
                                        CategorydcRec = obmenSour.TypeOIrec,
                                        NumberOIrec = obmenSour.NumberOIrec,
                                        UiddcRec = oi_CIM.mvUID,
                                        PodcRec = oi_CIM.poUID,
                                        IsSendCDU = isSendCDU,
                                        Info = "Несоответсвие привязки в ИМ"
                                    });
                                /*ChekProtocolAll.Add(new Protocol()
                                    {
                                        IsSendCDU = isSendCDU,
                                        KontrolObjectName = otiODU.NameOI,
                                        TypeOI = otiODU.TypeOI,
                                        NumberOI = otiODU.NumberOI,
                                        DcSource = obmenSour.DCsource,
                                        NumberOIsource = obmenSour.NumberOIsour,
                                        DCReception = obmenSour.DCreceiver,
                                        NumberOIreception = obmenSour.NumberOIrec,
                                        DispatchCenter = DC[dc],
                                        TypeKontrol = otiODU.EnergyObject,
                                        Category = otiODUsourCIM.UIDparentObj,
                                        NameTI = oi_CIM.UIDparentObj,
                                        Info = "Несоответсвие привязки в ИМ" /*+ otiODU.DispatchCenter + ": " + otiCIM_DC[i].UIDparentObj +
                                        "\n" + obmenRec.DCreceiver + ": " + otiODUrecCIM.UIDparentObj
                                    });*/
                                /*else if (otiODUsourCIM.MeasValueType != oi_CIM.MeasValueType)
                                    exProtocolList.Add(new ExchangeProtocol()
                                    {
                                        KontrolObjectName = otiODU.EnergyObject,
                                        DcSource = obmenSour.DCsource,
                                        NameDCSource = otiODUsourCIM.mvName,
                                        CategoryDCSource = obmenSour.TypeOIsour,
                                        NumberOIsource = obmenSour.NumberOIsour,
                                        UidDCSource = otiODUsourCIM.mvUID,
                                        PoDCSource = otiODUsourCIM.poUID,

                                        dcRec = obmenSour.DCreceiver,
                                        NamedcRec = otiODU.NameOI,
                                        CategorydcRec = obmenSour.TypeOIrec,
                                        NumberOIrec = obmenSour.NumberOIrec,
                                        UiddcRec = oi_CIM.mvUID,
                                        PodcRec = oi_CIM.poUID,
                                        IsSendCDU = isSendCDU,
                                        Info = "Несоответсвие типа измерения: " + otiODUsourCIM.MeasValueType + " и " + oi_CIM.MeasValueType
                                    });*/
                                /*ChekProtocolAll.Add(new Protocol()
                                    {
                                        IsSendCDU = isSendCDU,
                                        KontrolObjectName = otiODU.NameOI,
                                        TypeOI = otiODU.TypeOI,
                                        NumberOI = otiODU.NumberOI,
                                        DcSource = obmenSour.DCsource,
                                        NumberOIsource = obmenSour.NumberOIsour,
                                        DCReception = obmenSour.DCreceiver,
                                        NumberOIreception = obmenSour.NumberOIrec,
                                        DispatchCenter = DC[dc],
                                        TypeKontrol = otiODU.EnergyObject,
                                        Category = otiODUsourCIM.UIDparentObj,
                                        NameTI = oi_CIM.UIDparentObj,
                                        Info = "Несоответсвие типа измерения: " + otiODUsourCIM.MeasValueType + " и " + oi_CIM.MeasValueType
                                    });*/
                            }
                            catch (Exception e)
                            {
                                exProtocolList.Add(new ExchangeProtocol()
                                {
                                    KontrolObjectName = otiODU.EnergyObject,
                                    DcSource = obmenSour.DCsource,
                                    //NameDCSource = otiODUsourCIM.mvName,
                                    CategoryDCSource = obmenSour.TypeOIsour,
                                    NumberOIsource = obmenSour.NumberOIsour,
                                    //UidDCSource = otiODUsourCIM.mvUID,
                                    //PoDCSource = otiODUsourCIM.poUID,

                                    dcRec = obmenSour.DCreceiver,
                                    NamedcRec = otiODU.NameOI,
                                    CategorydcRec = obmenSour.TypeOIrec,
                                    NumberOIrec = obmenSour.NumberOIrec,
                                    UiddcRec = oi_CIM.mvUID,
                                    PodcRec = oi_CIM.poUID,
                                    IsSendCDU = isSendCDU,
                                    Info = "В ИМ " + obmenSour.DCsource + " нет получаемой ОИ " + obmenSour.TypeOIsour + obmenSour.NumberOIsour
                                });
                                //string inform = "В ИМ " + obmenRec.DCreceiver + " нет получаемой ОИ " + obmenRec.TypeOIrec + obmenRec.NumberOIrec + e.Message;
                            }
                        }
                    }
                    catch
                    {
                        exProtocolList.Add(new ExchangeProtocol()
                        {
                            //KontrolObjectName = otiODU.EnergyObject,
                            DcSource = oi_CIM.DC,
                            //NameDCSource = otiODUsourCIM.mvName,
                            //CategoryDCSource = obmenSour.TypeOIsour,
                            //NumberOIsource = obmenSour.NumberOIsour,
                            //UidDCSource = otiODUsourCIM.mvUID,
                            //PoDCSource = otiODUsourCIM.poUID,

                            //dcRec = obmenSour.DCreceiver,
                            //NamedcRec = otiODU.NameOI,
                            CategorydcRec = oi_CIM.mvCat,
                            NumberOIrec = oi_CIM.mvExternalId,
                            UiddcRec = oi_CIM.mvUID,
                            PodcRec = oi_CIM.poUID,
                            IsSendCDU = isSendCDU,
                            Info = " нет в БД СК-2007"
                    });
                        //string inform = oi_CIM.SourceID + "("+oi_CIM.UIDvalue +")"+ " нет в БД СК-2007";
                    };
                }                
            }
            if (triggerCheckExchange)
            {
                for (int dc = 0; dc < DC.Length; dc++)
                {
                    ObservableCollection<OperTechInform> otiODU = new ObservableCollection<OperTechInform>(OtiCollect.Where(x => x.DispatchCenter == DC[dc]));
                    for (int i = 0; i < otiODU.Count(); i++)
                    {
                        ObservableCollection<ExchangeOTI> exchangeRec = new ObservableCollection<ExchangeOTI>(ExchangeCollect.Where(x => (x.NumberOIrec == otiODU[i].NumberOI && x.TypeOIrec == otiODU[i].TypeOI && x.DCreceiver == otiODU[i].DispatchCenter)));
                        ObservableCollection<ExchangeOTI> exchangeSour = new ObservableCollection<ExchangeOTI>(ExchangeCollect.Where(x => (x.NumberOIsour == otiODU[i].NumberOI && x.TypeOIsour == otiODU[i].TypeOI && x.DCsource == otiODU[i].DispatchCenter)));
                        for (int j = 0; j < exchangeRec.Count(); j++)
                        {
                            try
                            {
                                exchangeSour.First(x => x.DCreceiver == exchangeRec[j].DCsource);
                                ChekProtocolAll.Add(new Protocol()
                                {
                                    KontrolObjectName = otiODU[i].EnergyObject,
                                    TypeOI = otiODU[i].TypeOI,
                                    NumberOI = otiODU[i].NumberOI,
                                    DcSource = exchangeRec[j].DCsource,
                                    NumberOIsource = exchangeRec[j].NumberOIsour,
                                    DCReception = exchangeRec[j].DCreceiver,
                                    NumberOIreception = exchangeRec[j].NumberOIrec,
                                    DispatchCenter = otiODU[i].DispatchCenter,
                                    TypeKontrol = "Прием/передача",
                                    Info = otiODU[i].TypeOI + otiODU[i].NumberOI + " принимается и передачется между ДЦ"
                                });
                            }
                            catch { }
                        }
                    }
                }
            }
            if (triggerCheckKPOS)
            {
                var commonSech = _database.GetCommonSech();                
                for (int dc = 0; dc < DC.Length; dc++)
                {
                    string DispatchCenter = DC[dc];
                    foreach (CommonSech s in commonSech)
                    {
                        ObservableCollection<Secheniya> sechMainCollect = new ObservableCollection<Secheniya>();
                        ObservableCollection<Secheniya> sechSecondaryCollect = new ObservableCollection<Secheniya>();
                        if (s.DC1 == DispatchCenter)
                        {
                            sechMainCollect = new ObservableCollection<Secheniya>
                                (SecheniyaCollect.Where(x => x.DispatchCenter == s.DC1 && x.IdSech == s.IDsech1));
                            sechSecondaryCollect = new ObservableCollection<Secheniya>
                                (SecheniyaCollect.Where(x => x.DispatchCenter == s.DC2 && x.IdSech == s.IDsech2));
                        }
                        else if (s.DC2 == DispatchCenter)
                        {
                            sechMainCollect = new ObservableCollection<Secheniya>
                                (SecheniyaCollect.Where(x => x.DispatchCenter == s.DC2 && x.IdSech == s.IDsech2));
                            sechSecondaryCollect = new ObservableCollection<Secheniya>
                                (SecheniyaCollect.Where(x => x.DispatchCenter == s.DC1 && x.IdSech == s.IDsech1));
                        }
                        foreach (Secheniya sechMain in sechMainCollect)
                        {
                            ExchangeOTI exchangeOTI;
                            //Находим инфу о передачи-приеме ОТИ
                            try
                            {
                                if (s.DC1 == DispatchCenter)
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == s.DC1 && x.DCreceiver == s.DC2 &&
                                    x.NumberOIsour == sechMain.NumberOI && x.TypeOIsour == sechMain.TypeOI) ||
                                    (x.DCsource == s.DC2 && x.DCreceiver == s.DC1 &&
                                    x.NumberOIrec == sechMain.NumberOI && x.TypeOIrec == sechMain.TypeOI)));
                                }
                                else
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == s.DC2 && x.DCreceiver == s.DC1 &&
                                    x.NumberOIsour == sechMain.NumberOI && x.TypeOIsour == sechMain.TypeOI) ||
                                    (x.DCsource == s.DC1 && x.DCreceiver == s.DC2 &&
                                    x.NumberOIrec == sechMain.NumberOI && x.TypeOIrec == sechMain.TypeOI)));
                                }
                            }
                            catch
                            {
                                exchangeOTI = null;
                            }
                            string info = "";
                            if (exchangeOTI == null)
                            {
                                var operTechInform = OtiCollect.Single(x => x.DispatchCenter == sechMain.DispatchCenter
                                && x.NumberOI == sechMain.NumberOI && x.TypeOI == sechMain.TypeOI);
                                if (operTechInform.Formula != "")
                                {
                                    info = operTechInform.NumberOI + operTechInform.TypeOI + "является дорасчётным. ";
                                }
                                ChekProtocolAll.Add(new Protocol()
                                {
                                    KontrolObjectName = sechMain.NameSech,
                                    TypeOI = sechMain.TypeOI,
                                    NumberOI = sechMain.NumberOI,
                                    DcSource = sechMain.DispatchCenter,
                                    NumberOIsource = sechMain.NumberOI,
                                    DispatchCenter = DispatchCenter,
                                    TypeKontrol = "КПОС",
                                    Info = sechMain.TypeOI + sechMain.NumberOI + " в обмене между " + s.DC1 + " и " + s.DC2 + " не участвует.\n" + info
                                });
                            }
                            else
                            {
                                bool flag = false;
                                if (sechMain.DispatchCenter == exchangeOTI.DCsource)
                                {
                                    foreach (Secheniya sechSecond in sechSecondaryCollect)
                                    {
                                        if (sechSecond.NumberOI == exchangeOTI.NumberOIrec)
                                        { flag = true; break; }
                                    }
                                }
                                else
                                {
                                    foreach (Secheniya sechSecond in sechSecondaryCollect)
                                    {
                                        if (sechSecond.NumberOI == exchangeOTI.NumberOIsour)
                                        { flag = true; break; }
                                    }
                                }
                                if (flag == false)
                                {
                                    if (sechMain.DispatchCenter == exchangeOTI.DCreceiver)
                                    {
                                        info = sechMain.TypeOI + exchangeOTI.NumberOIrec + " для контроля применяется только в " + exchangeOTI.DCreceiver
                                            + ".\n" + sechMain.TypeOI + exchangeOTI.NumberOIsour + " для контроля не применяется в " + exchangeOTI.DCsource;
                                    }
                                    else
                                    {
                                        info = sechMain.TypeOI + exchangeOTI.NumberOIsour + " для контроля применяется только в " + exchangeOTI.DCsource
                                            + ".\n" + sechMain.TypeOI + exchangeOTI.NumberOIrec + " для контроля не применяется в " + exchangeOTI.DCreceiver;
                                    }
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = sechMain.NameSech,
                                        TypeOI = sechMain.TypeOI,
                                        NumberOI = sechMain.NumberOI,
                                        DCReception = exchangeOTI.DCreceiver,
                                        DcSource = exchangeOTI.DCsource,
                                        DispatchCenter = DispatchCenter,
                                        NumberOIreception = exchangeOTI.NumberOIrec,
                                        NumberOIsource = exchangeOTI.NumberOIsour,
                                        TypeKontrol = "КПОС",
                                        Info = info
                                    });
                                }
                            }
                        }
                    }
                }
            }
            if (triggerCheckMUN)
            {
                var commonVoltage = _database.GetCommonVoltage();
                for (int dc = 0; dc < DC.Length; dc++)
                {
                    string DispatchCenter = DC[dc];
                    foreach (CommonVoltage v in commonVoltage)
                    {
                        ObservableCollection<Voltage> voltageCollectMain = new ObservableCollection<Voltage>();
                        ObservableCollection<Voltage> voltageCollectSecond = new ObservableCollection<Voltage>();
                        if (v.DC1 == DispatchCenter)
                        {
                            voltageCollectMain = new ObservableCollection<Voltage>(VoltageCollect.Where(x => x.DispatchCenter == v.DC1 && x.IdVoltage == v.IDvoltage1));
                            voltageCollectSecond = new ObservableCollection<Voltage>(VoltageCollect.Where(x => x.DispatchCenter == v.DC2 && x.IdVoltage == v.IDvoltage2));
                        }
                        else if (v.DC2 == DispatchCenter)
                        {
                            voltageCollectMain = new ObservableCollection<Voltage>(VoltageCollect.Where(x => x.DispatchCenter == v.DC2 && x.IdVoltage == v.IDvoltage2));
                            voltageCollectSecond = new ObservableCollection<Voltage>(VoltageCollect.Where(x => x.DispatchCenter == v.DC1 && x.IdVoltage == v.IDvoltage1));
                        }
                        //else { break; }

                        foreach (Voltage voltageMain in voltageCollectMain)
                        {
                            ExchangeOTI exchangeOTI;
                            //Находим инфу о передачи-приеме ОТИ
                            try
                            {
                                if (v.DC1 == DispatchCenter)
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == v.DC1 && x.DCreceiver == v.DC2 &&
                                    x.NumberOIsour == voltageMain.NumberOI && x.TypeOIsour == voltageMain.TypeOI) ||
                                    (x.DCsource == v.DC2 && x.DCreceiver == v.DC1 &&
                                    x.NumberOIrec == voltageMain.NumberOI && x.TypeOIrec == voltageMain.TypeOI)));
                                }
                                else
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == v.DC2 && x.DCreceiver == v.DC1 &&
                                    x.NumberOIsour == voltageMain.NumberOI && x.TypeOIsour == voltageMain.TypeOI) ||
                                    (x.DCsource == v.DC1 && x.DCreceiver == v.DC2 &&
                                    x.NumberOIrec == voltageMain.NumberOI && x.TypeOIrec == voltageMain.TypeOI)));
                                }
                            }
                            catch
                            {
                                exchangeOTI = null;
                            }

                            string info = "";
                            if (exchangeOTI == null)
                            {
                                var operTechInform = OtiCollect.Single(x => x.DispatchCenter == voltageMain.DispatchCenter
                                && x.NumberOI == voltageMain.NumberOI && x.TypeOI == voltageMain.TypeOI);
                                if (operTechInform.Formula != "")
                                {
                                    info = operTechInform.TypeOI + operTechInform.NumberOI + " является дорасчётным. ";
                                }
                                ChekProtocolAll.Add(new Protocol()
                                {
                                    KontrolObjectName = voltageMain.EnergyObject,
                                    TypeOI = voltageMain.TypeOI,
                                    NumberOI = voltageMain.NumberOI,
                                    DcSource = DispatchCenter,
                                    NumberOIsource = voltageMain.NumberOI,
                                    DispatchCenter = DispatchCenter,
                                    TypeKontrol = "МУН",
                                    Info = voltageMain.TypeOI + voltageMain.NumberOI + " в обмене между " + v.DC1 + " и " + v.DC2 + " не участвует. \n" + info
                                });
                            }
                            else
                            {
                                bool flag = false;
                                if (v.DC1 == exchangeOTI.DCsource)
                                {
                                    foreach (Voltage voltageSecond in voltageCollectSecond)
                                    {
                                        if (voltageSecond.NumberOI == exchangeOTI.NumberOIrec)
                                        { flag = true; break; }
                                        else
                                        {
                                            foreach (OperTechInform operand in voltageSecond.ListFormula)
                                            {
                                                if (operand.NumberOI == exchangeOTI.NumberOIrec && operand.TypeOI == exchangeOTI.TypeOIrec)
                                                { flag = true; break; }
                                            }
                                        }
                                        if (flag == true) break;
                                    }
                                }
                                else
                                {
                                    foreach (Voltage voltageSecond in voltageCollectSecond)
                                    {
                                        if (voltageSecond.NumberOI == exchangeOTI.NumberOIsour)
                                        { flag = true; break; }
                                        else
                                        {
                                            if (voltageSecond.ListFormula != null)
                                            {
                                                foreach (OperTechInform operand in voltageSecond.ListFormula)
                                                {
                                                    if (operand.NumberOI == exchangeOTI.NumberOIsour && operand.TypeOI == exchangeOTI.TypeOIsour)
                                                    { flag = true; break; }
                                                }
                                            }
                                            if (flag == true) break;
                                        }
                                    }
                                }
                                if (flag == false)
                                {
                                    var flagSecond = false;
                                    if (DispatchCenter == exchangeOTI.DCreceiver)
                                    {
                                        Voltage voltageSecond = new Voltage();
                                        var voltageDcList = new ObservableCollection<Voltage>(VoltageCollect.Where(x => x.DispatchCenter == exchangeOTI.DCsource));
                                        foreach (Voltage voltageDC in voltageDcList)
                                        {
                                            if (voltageDC.ListFormula != null)
                                            {
                                                foreach (OperTechInform oti in voltageDC.ListFormula)
                                                {
                                                    if (oti.NumberOI == exchangeOTI.NumberOIsour && oti.TypeOI == exchangeOTI.TypeOIsour)
                                                    { voltageSecond = voltageDC; flagSecond = true; break; }
                                                }
                                            }
                                            if (voltageDC.NumberOIitog == exchangeOTI.NumberOIsour && voltageDC.TypeOIitog == exchangeOTI.TypeOIsour)
                                            { voltageSecond = voltageDC; flagSecond = true; }
                                            if (voltageDC.NumberOI == exchangeOTI.NumberOIsour && voltageDC.TypeOI == exchangeOTI.TypeOIsour)
                                            { voltageSecond = voltageDC; flagSecond = true; }
                                            if (flagSecond == true)
                                                break;
                                        }
                                        if (flagSecond == true)
                                        {
                                            info = "Несоответсвие объекта контроля:\n"
                                                + voltageMain.TypeOI + exchangeOTI.NumberOIrec + " объект: " + voltageMain.EnergyObject + " (" + exchangeOTI.DCreceiver + ")"
                                                + ".\n" + voltageMain.TypeOI + exchangeOTI.NumberOIsour + " объект: " + voltageSecond.EnergyObject + " (" + exchangeOTI.DCsource + ")";
                                        }
                                        else
                                        {
                                            info = voltageMain.TypeOI + exchangeOTI.NumberOIrec + " для контроля применяется только в " + exchangeOTI.DCreceiver
                                            + ".\n" + voltageMain.TypeOI + exchangeOTI.NumberOIsour + " для контроля не применяется в " + exchangeOTI.DCsource;
                                        }
                                    }
                                    else
                                    {
                                        Voltage voltageSecond = new Voltage();
                                        var voltageDcList = new ObservableCollection<Voltage>(VoltageCollect.Where(x => x.DispatchCenter == exchangeOTI.DCreceiver));
                                        foreach (Voltage voltageDC in voltageDcList)
                                        {
                                            if (voltageDC.ListFormula != null)
                                            {
                                                foreach (OperTechInform oti in voltageDC.ListFormula)
                                                {
                                                    if (oti.NumberOI == exchangeOTI.NumberOIrec && oti.TypeOI == exchangeOTI.TypeOIrec)
                                                    { voltageSecond = voltageDC; flagSecond = true; }
                                                }
                                            }
                                            if (voltageDC.NumberOIitog == exchangeOTI.NumberOIrec && voltageDC.TypeOIitog == exchangeOTI.TypeOIrec)
                                            { voltageSecond = voltageDC; flagSecond = true; }
                                            if (voltageDC.NumberOI == exchangeOTI.NumberOIrec && voltageDC.TypeOI == exchangeOTI.TypeOIrec)
                                            { voltageSecond = voltageDC; flagSecond = true; }
                                            if (flagSecond == true)
                                                break;
                                        }
                                        if (flagSecond == true)
                                        {
                                            info = "Несоответсвие объекта контроля:\n"
                                                + voltageMain.TypeOI + exchangeOTI.NumberOIsour + " объект: " + voltageMain.EnergyObject + " (" + exchangeOTI.DCsource + ")"
                                                + ".\n" + voltageMain.TypeOI + exchangeOTI.NumberOIrec + " объект: " + voltageSecond.EnergyObject + " (" + exchangeOTI.DCreceiver + ")";
                                        }
                                        else
                                        {
                                            info = voltageMain.TypeOI + exchangeOTI.NumberOIsour + " для контроля применяется только в " + exchangeOTI.DCsource
                                            + ".\n" + voltageMain.TypeOI + exchangeOTI.NumberOIrec + " для контроля не применяется в " + exchangeOTI.DCreceiver;
                                        }
                                    }
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = voltageMain.EnergyObject,
                                        TypeOI = voltageMain.TypeOI,
                                        NumberOI = voltageMain.NumberOI,
                                        DCReception = exchangeOTI.DCreceiver,
                                        DcSource = exchangeOTI.DCsource,
                                        DispatchCenter = DispatchCenter,
                                        NumberOIreception = exchangeOTI.NumberOIrec,
                                        NumberOIsource = exchangeOTI.NumberOIsour,
                                        TypeKontrol = "МУН",
                                        Info = info
                                    });
                                }
                            }
                        }

                    }
                }
            }
            if (triggerCheckSMTNline)
            {
                var commonLine = _database.GetCommonLine();
                for (int dc = 0; dc < DC.Length; dc++)
                {
                    string DispatchCenter = DC[dc];
                    foreach (CommonLine l in commonLine)
                    {
                        if (l.IdLine1 == 3170 || l.IdLine2 == 3170)
                        { }
                        CurrentLine lineMain = new CurrentLine();
                        CurrentLine lineSecond = new CurrentLine();
                        if (l.DC1 == DispatchCenter)
                        {
                            lineMain = CurrentLineCollect.First(x => x.DispatchCenter == l.DC1 && x.IdLine == l.IdLine1 && x.IdEnergyObject == l.idEO1);
                            lineSecond = CurrentLineCollect.First(x => x.DispatchCenter == l.DC2 && x.IdLine == l.IdLine2 && x.IdEnergyObject == l.idEO2);
                        }
                        else if (l.DC2 == DispatchCenter)
                        {
                            lineMain = CurrentLineCollect.First(x => x.DispatchCenter == l.DC2 && x.IdLine == l.IdLine2 && x.IdEnergyObject == l.idEO2);
                            lineSecond = CurrentLineCollect.First(x => x.DispatchCenter == l.DC1 && x.IdLine == l.IdLine1 && x.IdEnergyObject == l.idEO1);
                        }
                        if (l.DC1==DispatchCenter || l.DC2==DispatchCenter)
                        {
                            ExchangeOTI exchangeOTI;
                            //Находим инфу о передачи-приеме Iфакт      
                            string info = "";
                            try
                            {
                                if (l.DC1 == DispatchCenter)
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                    x.NumberOIsour == lineMain.NumberIfact && x.TypeOIsour == "ТИ") ||
                                    (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                    x.NumberOIrec == lineMain.NumberIfact && x.TypeOIrec == "ТИ")));
                                }
                                else
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                    x.NumberOIsour == lineMain.NumberIfact && x.TypeOIsour == "ТИ") ||
                                    (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                    x.NumberOIrec == lineMain.NumberIfact && x.TypeOIrec == "ТИ")));
                                }
                                if (exchangeOTI.NumberOIrec == lineSecond.NumberIfact || exchangeOTI.NumberOIsour == lineSecond.NumberIfact)
                                { }
                                else
                                {
                                    info = GetInfoChekLine(exchangeOTI, lineMain);
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = lineMain.NameLine + ":\n " + lineMain.EnergyObject,
                                        TypeOI = "ТИ",
                                        NumberOI = lineMain.NumberIfact,
                                        DCReception = exchangeOTI.DCreceiver,
                                        DcSource = exchangeOTI.DCsource,
                                        DispatchCenter = lineMain.DispatchCenter,
                                        NumberOIreception = exchangeOTI.NumberOIrec,
                                        NumberOIsource = exchangeOTI.NumberOIsour,
                                        TypeKontrol = "СМТН.ЛЭП",
                                        Info = info
                                    });
                                }
                            }
                            catch
                            {
                                exchangeOTI = null;
                                if (lineMain.Formula != "" && lineSecond.Formula != "")
                                {
                                    info = GetInfoChekLine(lineMain, lineSecond, ExchangeCollect);
                                }
                                else
                                {
                                    info = "Несоответсвие формирования Iфакт:\n" +
                                         "ТИ" + lineMain.NumberIfact + " в обмене между " + l.DC1 + " и " + l.DC2 + " не участвует.";
                                }
                                if (info != "")
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                        TypeOI = "ТИ",
                                        NumberOI = lineMain.NumberIfact,
                                        DcSource = DispatchCenter,
                                        NumberOIsource = lineMain.NumberIfact,
                                        DispatchCenter = DispatchCenter,
                                        TypeKontrol = "СМТН.ЛЭП",
                                        Info = info
                                    });
                            }
                            if (lineMain.NumberIa != 0)
                            {
                                info = "";
                                try
                                {
                                    if (l.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                        x.NumberOIsour == lineMain.NumberIa && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == lineSecond.NumberIa && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                        x.NumberOIrec == lineMain.NumberIa && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == lineSecond.NumberIa && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                        x.NumberOIsour == lineMain.NumberIa && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == lineSecond.NumberIa && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                        x.NumberOIrec == lineMain.NumberIa && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == lineSecond.NumberIa && x.TypeOIsour == "ТИ")));
                                    }

                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == lineMain.DispatchCenter && x.NumberOI == lineMain.NumberIa
                                      && x.TypeOI == "ТИ"));

                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + lineMain.DispatchCenter + " и " + lineSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                    {
                                        try
                                        {
                                            if (l.DC1 == DispatchCenter)
                                            {
                                                exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                                x.NumberOIsour == lineMain.NumberIa && x.TypeOIsour == "ТИ") ||
                                                (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                                x.NumberOIrec == lineMain.NumberIa && x.TypeOIrec == "ТИ")));
                                                info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + exchangeOTI.NumberOIsour + " для контроля применяется только в  " + exchangeOTI.DCsource + "\n" +
                                            "ТИ" + exchangeOTI.NumberOIrec + " для контроля в  " + exchangeOTI.DCreceiver + " не применяется";

                                            }
                                            else
                                            {
                                                exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                                x.NumberOIsour == lineMain.NumberIa && x.TypeOIsour == "ТИ") ||
                                                (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                                x.NumberOIrec == lineMain.NumberIa && x.TypeOIrec == "ТИ")));
                                                info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + exchangeOTI.NumberOIrec + " для контроля применяется только в  " + exchangeOTI.DCreceiver + "\n" +
                                            "ТИ" + exchangeOTI.NumberOIsour + " для контроля в  " + exchangeOTI.DCsource + " не применяется";
                                            }
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                                TypeOI = "ТИ",
                                                NumberOI = lineMain.NumberIa,
                                                DcSource = exchangeOTI.DCsource,
                                                DCReception = exchangeOTI.DCreceiver,
                                                NumberOIsource = exchangeOTI.NumberOIsour,
                                                NumberOIreception = exchangeOTI.NumberOIrec,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.ЛЭП",
                                                Info = info
                                            });

                                        }
                                        catch
                                        {
                                            info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + oti.NumberOI + " для контроля применяется только в  " + lineMain.DispatchCenter + "\nв обмене с " + lineSecond.DispatchCenter +
                                                                    " не участвует";
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                                TypeOI = "ТИ",
                                                NumberOI = lineMain.NumberIa,
                                                DcSource = DispatchCenter,
                                                NumberOIsource = lineMain.NumberIa,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.ЛЭП",
                                                Info = info
                                            });
                                        }
                                    }

                                }
                            }
                            if (lineMain.NumberIb != 0)
                            {
                                info = "";
                                try
                                {
                                    if (l.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                        x.NumberOIsour == lineMain.NumberIb && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == lineSecond.NumberIb && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                        x.NumberOIrec == lineMain.NumberIb && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == lineSecond.NumberIb && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                        x.NumberOIsour == lineMain.NumberIb && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == lineSecond.NumberIb && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                        x.NumberOIrec == lineMain.NumberIb && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == lineSecond.NumberIb && x.TypeOIsour == "ТИ")));
                                    }
                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == lineMain.DispatchCenter && x.NumberOI == lineMain.NumberIb
                                      && x.TypeOI == "ТИ"));
                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования Ib:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + lineMain.DispatchCenter + " и " + lineSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                    {
                                        try
                                        {
                                            if (l.DC1 == DispatchCenter)
                                            {
                                                exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                                x.NumberOIsour == lineMain.NumberIb && x.TypeOIsour == "ТИ") ||
                                                (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                                x.NumberOIrec == lineMain.NumberIb && x.TypeOIrec == "ТИ")));
                                                info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + exchangeOTI.NumberOIsour + " для контроля применяется только в  " + exchangeOTI.DCsource + "\n" +
                                            "ТИ" + exchangeOTI.NumberOIrec + " для контроля в  " + exchangeOTI.DCreceiver + " не применяется";
                                            }
                                            else
                                            {
                                                exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                                x.NumberOIsour == lineMain.NumberIb && x.TypeOIsour == "ТИ") ||
                                                (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                                x.NumberOIrec == lineMain.NumberIb && x.TypeOIrec == "ТИ")));
                                                info = "Несоответсвие формирования Ib:\n" +
                                            "ТИ" + exchangeOTI.NumberOIrec + " для контроля применяется только в  " + exchangeOTI.DCreceiver + "\n" +
                                            "ТИ" + exchangeOTI.NumberOIsour + " для контроля в  " + exchangeOTI.DCsource + " не применяется";
                                            }
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                                TypeOI = "ТИ",
                                                NumberOI = lineMain.NumberIb,
                                                DcSource = exchangeOTI.DCsource,
                                                DCReception = exchangeOTI.DCreceiver,
                                                NumberOIsource = exchangeOTI.NumberOIsour,
                                                NumberOIreception = exchangeOTI.NumberOIrec,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.ЛЭП",
                                                Info = info
                                            });
                                        }
                                        catch
                                        {
                                            info = "Несоответсвие формирования Ib:\n" +
                                            "ТИ" + oti.NumberOI + " для контроля применяется только в  " + lineMain.DispatchCenter + "\nв обмене с " + lineSecond.DispatchCenter +
                                                                    " не участвует";
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                                TypeOI = "ТИ",
                                                NumberOI = lineMain.NumberIb,
                                                DcSource = DispatchCenter,
                                                NumberOIsource = lineMain.NumberIb,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.ЛЭП",
                                                Info = info
                                            });
                                        }
                                    }
                                }
                            }
                            if (lineMain.NumberIc != 0)
                            {
                                info = "";
                                try
                                {
                                    if (l.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                        x.NumberOIsour == lineMain.NumberIc && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == lineSecond.NumberIc && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                        x.NumberOIrec == lineMain.NumberIc && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == lineSecond.NumberIc && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                        x.NumberOIsour == lineMain.NumberIc && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == lineSecond.NumberIc && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                        x.NumberOIrec == lineMain.NumberIc && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == lineSecond.NumberIc && x.TypeOIsour == "ТИ")));
                                    }

                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == lineMain.DispatchCenter && x.NumberOI == lineMain.NumberIc
                                      && x.TypeOI == "ТИ"));
                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования Ic:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + lineMain.DispatchCenter + " и " + lineSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                    {
                                        try
                                        {
                                            if (l.DC1 == DispatchCenter)
                                            {
                                                exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                                x.NumberOIsour == lineMain.NumberIc && x.TypeOIsour == "ТИ") ||
                                                (x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                                x.NumberOIrec == lineMain.NumberIc && x.TypeOIrec == "ТИ")));
                                                info = "Несоответсвие формирования Ic:\n" +
                                            "ТИ" + exchangeOTI.NumberOIsour + " для контроля применяется только в  " + exchangeOTI.DCsource + "\n" +
                                            "ТИ" + exchangeOTI.NumberOIrec + " для контроля в  " + exchangeOTI.DCreceiver + " не применяется";
                                            }
                                            else
                                            {
                                                exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == l.DC2 && x.DCreceiver == l.DC1 &&
                                                x.NumberOIsour == lineMain.NumberIc && x.TypeOIsour == "ТИ") ||
                                                (x.DCsource == l.DC1 && x.DCreceiver == l.DC2 &&
                                                x.NumberOIrec == lineMain.NumberIc && x.TypeOIrec == "ТИ")));
                                                info = "Несоответсвие формирования Ic:\n" +
                                            "ТИ" + exchangeOTI.NumberOIrec + " для контроля применяется только в  " + exchangeOTI.DCreceiver + "\n" +
                                            "ТИ" + exchangeOTI.NumberOIsour + " для контроля в  " + exchangeOTI.DCsource + " не применяется";
                                            }
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                                TypeOI = "ТИ",
                                                NumberOI = lineMain.NumberIc,
                                                DcSource = exchangeOTI.DCsource,
                                                DCReception = exchangeOTI.DCreceiver,
                                                NumberOIsource = exchangeOTI.NumberOIsour,
                                                NumberOIreception = exchangeOTI.NumberOIrec,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.ЛЭП",
                                                Info = info
                                            });
                                        }
                                        catch
                                        {
                                            info = "Несоответсвие формирования Ic:\n" +
                                            "ТИ" + oti.NumberOI + " для контроля применяется только в  " + lineMain.DispatchCenter + "\nв обмене с " + lineSecond.DispatchCenter +
                                                                    " не участвует";
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = lineMain.NameLine + ":\n" + lineMain.EnergyObject,
                                                TypeOI = "ТИ",
                                                NumberOI = lineMain.NumberIc,
                                                DcSource = DispatchCenter,
                                                NumberOIsource = lineMain.NumberIc,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.ЛЭП",
                                                Info = info
                                            });
                                        }
                                    }

                                }
                            }
                        }
                        
                    }
                }
            }
            if (triggerCheckSMTNtransformer)
            {
                var commonTrans = _database.GetCommonTransform();
                for (int dc = 0; dc < DC.Length; dc++)
                {
                    string DispatchCenter = DC[dc];
                    foreach (CommonTransform t in commonTrans)
                    {
                        CurrentTransform transMain = new CurrentTransform();
                        CurrentTransform transSecond = new CurrentTransform();
                        if (t.DC1 == DispatchCenter)
                        {
                            transMain = CurrentTransformCollect.First(x => x.DispatchCenter == t.DC1 && x.idTransform == t.idTrans1 && x.idWinding == t.idWinding1);
                            transSecond = CurrentTransformCollect.First(x => x.DispatchCenter == t.DC2 && x.idTransform == t.idTrans2 && x.idWinding == t.idWinding2);
                        }
                        else if (t.DC2 == DispatchCenter)
                        {
                            transMain = CurrentTransformCollect.First(x => x.DispatchCenter == t.DC2 && x.idTransform == t.idTrans2 && x.idWinding == t.idWinding2);
                            transSecond = CurrentTransformCollect.First(x => x.DispatchCenter == t.DC1 && x.idTransform == t.idTrans1 && x.idWinding == t.idWinding1);
                        }
                        if (t.DC1 == DispatchCenter || t.DC2 == DispatchCenter)
                        {
                            ExchangeOTI exchangeOTI;
                            //Находим инфу о передачи-приеме Iфакт      
                            string info = "";
                            try
                            {
                                if (t.DC1 == DispatchCenter)
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == t.DC1 && x.DCreceiver == t.DC2 &&
                                    x.NumberOIsour == transMain.NumberIfact && x.TypeOIsour == "ТИ") ||
                                    (x.DCsource == t.DC2 && x.DCreceiver == t.DC1 &&
                                    x.NumberOIrec == transMain.NumberIfact && x.TypeOIrec == "ТИ")));
                                }
                                else
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == t.DC2 && x.DCreceiver == t.DC1 &&
                                    x.NumberOIsour == transMain.NumberIfact && x.TypeOIsour == "ТИ") ||
                                    (x.DCsource == t.DC1 && x.DCreceiver == t.DC2 &&
                                    x.NumberOIrec == transMain.NumberIfact && x.TypeOIrec == "ТИ")));
                                }
                                if (exchangeOTI.NumberOIrec == transSecond.NumberIfact || exchangeOTI.NumberOIsour == transSecond.NumberIfact)
                                { }
                                else
                                {
                                    info = GetInfoChekTrans(exchangeOTI, transMain);
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = transMain.NameTransform + ":\n " + transMain.TransformWinding,
                                        TypeOI = "ТИ",
                                        NumberOI = transMain.NumberIfact,
                                        DCReception = exchangeOTI.DCreceiver,
                                        DcSource = exchangeOTI.DCsource,
                                        DispatchCenter = transMain.DispatchCenter,
                                        NumberOIreception = exchangeOTI.NumberOIrec,
                                        NumberOIsource = exchangeOTI.NumberOIsour,
                                        TypeKontrol = "СМТН.АТ(Т)",
                                        Info = info
                                    });
                                }

                            }
                            catch
                            {
                                exchangeOTI = null;
                                if (transMain.Formula != "" && transSecond.Formula != "")
                                {
                                    info = GetInfoChekTrans(transMain, transSecond, ExchangeCollect);
                                }
                                else
                                {
                                    info = "Несоответсвие формирования Iфакт:\n" +
                                         "ТИ" + transMain.NumberIfact + " в обмене между " + t.DC1 + " и " + t.DC2 + " не участвует.";
                                }
                                if (info != "")
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = transMain.NameTransform + ":\n" + transMain.TransformWinding,
                                        TypeOI = "ТИ",
                                        NumberOI = transMain.NumberIfact,
                                        DcSource = DispatchCenter,
                                        NumberOIsource = transMain.NumberIfact,
                                        DispatchCenter = DispatchCenter,
                                        TypeKontrol = "СМТН.АТ(Т)",
                                        Info = info
                                    });
                            }
                            if (transMain.NumberRpn != 0)
                            {
                                try
                                {
                                    if (t.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == t.DC1 && x.DCreceiver == t.DC2 &&
                                        x.NumberOIsour == transMain.NumberRpn && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == transSecond.NumberRpn && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == t.DC2 && x.DCreceiver == t.DC1 &&
                                        x.NumberOIrec == transMain.NumberRpn && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == transSecond.NumberRpn && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == t.DC2 && x.DCreceiver == t.DC1 &&
                                        x.NumberOIsour == transMain.NumberRpn && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == transSecond.NumberRpn && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == t.DC1 && x.DCreceiver == t.DC2 &&
                                        x.NumberOIrec == transMain.NumberRpn && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == transSecond.NumberRpn && x.TypeOIsour == "ТИ")));
                                    }

                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == transMain.DispatchCenter && x.NumberOI == transMain.NumberRpn
                                      && x.TypeOI == "ТИ"));
                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования РПН:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + transMain.DispatchCenter + " и " + transSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                    {
                                        try
                                        {
                                            exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == oti.DispatchCenter && x.DCreceiver == transSecond.DispatchCenter &&
                                            x.NumberOIsour == oti.NumberOI && x.TypeOIsour == "ТИ") ||
                                            (x.DCreceiver == oti.DispatchCenter && x.DCsource == transSecond.DispatchCenter &&
                                            x.NumberOIrec == oti.NumberOI && x.TypeOIrec == "ТИ")));
                                            int otiSecond = 0;
                                            if (exchangeOTI.DCsource == transSecond.DispatchCenter)
                                            {
                                                otiSecond = exchangeOTI.NumberOIsour;
                                            }
                                            else
                                            {
                                                otiSecond = exchangeOTI.NumberOIrec;
                                            }
                                            try
                                            {
                                                var transSecondNew = CurrentTransformCollect.First(x => x.DispatchCenter == transSecond.DispatchCenter &&
                                                  x.NumberRpn == otiSecond);
                                                info = "Несоответсвие применения РПН:\n" +
                                                "ТИ" + transMain.NumberRpn + " объект: " + transMain.NameTransform + " обмотка: " + transMain.TransformWinding + "(" + transMain.DispatchCenter + ")"
                                                + ".\n" + "ТИ" + transSecondNew.NumberRpn + " объект: " + transSecondNew.NameTransform + " обмотка: " + transSecondNew.TransformWinding + "(" + transSecondNew.DispatchCenter + ")";

                                            }
                                            catch
                                            {
                                                info = "Несоответсвие формирования РПН:\n" +
                                            "ТИ" + oti.NumberOI + " для контроля применяется только в " + oti.DispatchCenter
                                            + ".\n" + "ТИ" + otiSecond + " для контроля не применяется в " + transSecond.DispatchCenter;

                                            }
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = transMain.NameTransform + ":\n" + transMain.TransformWinding,
                                                TypeOI = "ТИ",
                                                NumberOI = transMain.NumberRpn,
                                                DcSource = exchangeOTI.DCsource,
                                                DCReception = exchangeOTI.DCreceiver,
                                                NumberOIsource = exchangeOTI.NumberOIsour,
                                                NumberOIreception = exchangeOTI.NumberOIrec,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.АТ(Т)",
                                                Info = info
                                            });
                                        }
                                        catch
                                        {
                                            info = "Несоответсвие формирования РПН:\n" +
                                                "ТИ" + oti.NumberOI + " в обмене между " + transMain.DispatchCenter + " и " + transSecond.DispatchCenter +
                                                                                                    " не участвует";
                                            ChekProtocolAll.Add(new Protocol()
                                            {
                                                KontrolObjectName = transMain.NameTransform + ":\n" + transMain.TransformWinding,
                                                TypeOI = "ТИ",
                                                NumberOI = transMain.NumberRpn,
                                                DcSource = DispatchCenter,
                                                NumberOIsource = transMain.NumberRpn,
                                                DispatchCenter = DispatchCenter,
                                                TypeKontrol = "СМТН.АТ(Т)",
                                                Info = info
                                            });
                                        }
                                    }



                                }
                            }
                        }
                          
                    }
                }
            }
            if (triggerCheckSMTNbreaker)
            {
                var commonBreaker = _database.GetCommonBreaker();
                for (int dc = 0; dc < DC.Length; dc++)
                {
                    string DispatchCenter = DC[dc];
                    foreach (CommonBreaker b in commonBreaker)
                    {
                        CurrentBreaker breakerMain = new CurrentBreaker();
                        CurrentBreaker breakerSecond = new CurrentBreaker();
                        if (b.DC1 == DispatchCenter)
                        {
                            breakerMain = CurrentBreakerCollect.First(x => x.DispatchCenter == b.DC1 && x.IdBreaker == b.idbreaker1);
                            breakerSecond = CurrentBreakerCollect.First(x => x.DispatchCenter == b.DC2 && x.IdBreaker == b.idBreaker2);
                        }
                        else if (b.DC2 == DispatchCenter)
                        {
                            breakerMain = CurrentBreakerCollect.First(x => x.DispatchCenter == b.DC2 && x.IdBreaker == b.idBreaker2);
                            breakerSecond = CurrentBreakerCollect.First(x => x.DispatchCenter == b.DC1 && x.IdBreaker == b.idbreaker1);
                        }
                        if (b.DC2==DispatchCenter || b.DC1==DispatchCenter)
                        {
                            ExchangeOTI exchangeOTI;
                            //Находим инфу о передачи-приеме Iфакт      
                            string info = "";
                            try
                            {
                                if (b.DC1 == DispatchCenter)
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                    x.NumberOIsour == breakerMain.NumberIfact && x.TypeOIsour == "ТИ") ||
                                    (x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                    x.NumberOIrec == breakerMain.NumberIfact && x.TypeOIrec == "ТИ")));
                                }
                                else
                                {
                                    exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                    x.NumberOIsour == breakerMain.NumberIfact && x.TypeOIsour == "ТИ") ||
                                    (x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                    x.NumberOIrec == breakerMain.NumberIfact && x.TypeOIrec == "ТИ")));
                                }
                                if (exchangeOTI.NumberOIrec == breakerSecond.NumberIfact || exchangeOTI.NumberOIsour == breakerSecond.NumberIfact)
                                { }
                                else
                                {
                                    info = GetInfoChekBreaker(exchangeOTI, breakerMain);
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = breakerMain.EnergyObject + ":\n " + breakerMain.NameBreaker,
                                        TypeOI = "ТИ",
                                        NumberOI = breakerMain.NumberIfact,
                                        DCReception = exchangeOTI.DCreceiver,
                                        DcSource = exchangeOTI.DCsource,
                                        DispatchCenter = breakerMain.DispatchCenter,
                                        NumberOIreception = exchangeOTI.NumberOIrec,
                                        NumberOIsource = exchangeOTI.NumberOIsour,
                                        TypeKontrol = "СМТН.Выкл",
                                        Info = info
                                    });
                                }
                            }
                            catch
                            {
                                exchangeOTI = null;
                                if (breakerMain.Formula != "" && breakerSecond.Formula != "")
                                {
                                    info = GetInfoChekBreaker(breakerMain, breakerSecond, ExchangeCollect);
                                }
                                else
                                {
                                    info = "Несоответсвие формирования Iфакт:\n" +
                                         "ТИ" + breakerMain.NumberIfact + " в обмене между " + b.DC1 + " и " + b.DC2 + " не участвует.";
                                }
                                if (info != "")
                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = breakerMain.EnergyObject + ":\n " + breakerMain.NameBreaker,
                                        TypeOI = "ТИ",
                                        NumberOI = breakerMain.NumberIfact,
                                        DcSource = DispatchCenter,
                                        NumberOIsource = breakerMain.NumberIfact,
                                        DispatchCenter = DispatchCenter,
                                        TypeKontrol = "СМТН.Выкл",
                                        Info = info
                                    });
                            }
                            if (breakerMain.NumberIa != 0)
                            {
                                try
                                {
                                    if (b.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                        x.NumberOIsour == breakerMain.NumberIa && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == breakerSecond.NumberIa && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                        x.NumberOIrec == breakerMain.NumberIa && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == breakerSecond.NumberIa && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                        x.NumberOIsour == breakerMain.NumberIa && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == breakerSecond.NumberIa && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                        x.NumberOIrec == breakerMain.NumberIa && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == breakerSecond.NumberIa && x.TypeOIsour == "ТИ")));
                                    }

                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == breakerMain.DispatchCenter && x.NumberOI == breakerMain.NumberIa
                                      && x.TypeOI == "ТИ"));
                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                        info = "Несоответсвие формирования Ia:\n" +
                                            "ТИ" + oti.NumberOI + " в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter +
                                                                    " не участвует";

                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = breakerMain.EnergyObject + ":\n " + breakerMain.NameBreaker,
                                        TypeOI = "ТИ",
                                        NumberOI = breakerMain.NumberIa,
                                        DcSource = DispatchCenter,
                                        NumberOIsource = breakerMain.NumberIa,
                                        DispatchCenter = DispatchCenter,
                                        TypeKontrol = "СМТН.Выкл",
                                        Info = info
                                    });
                                }
                            }
                            if (breakerMain.NumberIb != 0)
                            {
                                try
                                {
                                    if (b.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                        x.NumberOIsour == breakerMain.NumberIb && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == breakerSecond.NumberIb && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                        x.NumberOIrec == breakerMain.NumberIb && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == breakerSecond.NumberIb && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                        x.NumberOIsour == breakerMain.NumberIb && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == breakerSecond.NumberIb && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                        x.NumberOIrec == breakerMain.NumberIb && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == breakerSecond.NumberIb && x.TypeOIsour == "ТИ")));
                                    }
                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == breakerMain.DispatchCenter && x.NumberOI == breakerMain.NumberIa
                                      && x.TypeOI == "ТИ"));
                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования Ib:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                        info = "Несоответсвие формирования Ib:\n" +
                                            "ТИ" + oti.NumberOI + " в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter +
                                                                    " не участвует";

                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = breakerMain.EnergyObject + ":\n " + breakerMain.NameBreaker,
                                        TypeOI = "ТИ",
                                        NumberOI = breakerMain.NumberIb,
                                        DcSource = DispatchCenter,
                                        NumberOIsource = breakerMain.NumberIb,
                                        DispatchCenter = DispatchCenter,
                                        TypeKontrol = "СМТН.Выкл",
                                        Info = info
                                    });
                                }
                            }
                            if (breakerMain.NumberIc != 0)
                            {
                                try
                                {
                                    if (b.DC1 == DispatchCenter)
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                        x.NumberOIsour == breakerMain.NumberIc && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == breakerSecond.NumberIc && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                        x.NumberOIrec == breakerMain.NumberIc && x.TypeOIrec == "ТИ" &&
                                        x.NumberOIsour == breakerSecond.NumberIc && x.TypeOIsour == "ТИ")));
                                    }
                                    else
                                    {
                                        exchangeOTI = ExchangeCollect.First(x => ((x.DCsource == b.DC2 && x.DCreceiver == b.DC1 &&
                                        x.NumberOIsour == breakerMain.NumberIc && x.TypeOIsour == "ТИ" &&
                                        x.NumberOIrec == breakerSecond.NumberIc && x.TypeOIrec == "ТИ") ||
                                        (x.DCsource == b.DC1 && x.DCreceiver == b.DC2 &&
                                        x.NumberOIrec == breakerMain.NumberIc && x.TypeOIrec == "ТИ" &&
                                       x.NumberOIsour == breakerSecond.NumberIc && x.TypeOIsour == "ТИ")));
                                    }

                                }
                                catch
                                {
                                    var oti = OtiCollect.First(x => (x.DispatchCenter == breakerMain.DispatchCenter && x.NumberOI == breakerMain.NumberIa
                                      && x.TypeOI == "ТИ"));
                                    if (oti.Formula != "")
                                        info = "Несоответсвие формирования Ic:\n" +
                                            "ТИ" + oti.NumberOI + "дорасчётное и в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter +
                                            " не участвует";
                                    else
                                        info = "Несоответсвие формирования Ic:\n" +
                                            "ТИ" + oti.NumberOI + " в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter +
                                                                    " не участвует";

                                    ChekProtocolAll.Add(new Protocol()
                                    {
                                        KontrolObjectName = breakerMain.EnergyObject + ":\n " + breakerMain.NameBreaker,
                                        TypeOI = "ТИ",
                                        NumberOI = breakerMain.NumberIc,
                                        DcSource = DispatchCenter,
                                        NumberOIsource = breakerMain.NumberIc,
                                        DispatchCenter = DispatchCenter,
                                        TypeKontrol = "СМТН.Выкл",
                                        Info = info
                                    });
                                }
                            }
                        }
                        
                    }
                }
            }    
            ChekProtocol = new ObservableCollection<Protocol>(ChekProtocolAll.Where(x => x.DispatchCenter.Equals(DispatchCenterFilter)));
            //SaveProtocol(ChekProtocolAll);
            SaveExchangeProtocol(exProtocolList);
            waitWindow.Close();
            Mouse.OverrideCursor = null;
        }
        private void SaveProtocol(ObservableCollection<Protocol> infoList)
        {
            try
            {
                string fileResult = Directory.GetCurrentDirectory() + "\\info.csv";
                var d = ";";
                if (infoList.Count() > 0)
                    using (StreamWriter sw = new StreamWriter(fileResult, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine("Объект"+ d + "Название ОИ" + d 
                            + "ДЦ источник" + d + "№ОИ" + d + "UID" + d 
                            + "ДЦ получатель" + d + "№ОИ" + d + "UID" + d 
                            + "Комментарии"+d+"передача в ИА");
                        foreach (var infoStr in infoList)
                        {
                            sw.WriteLine(infoStr.TypeKontrol + d + infoStr.KontrolObjectName + d +
                                       infoStr.DcSource + d + infoStr.NumberOIsource + d + infoStr.Category + d +
                                       infoStr.DCReception + d + infoStr.NumberOIreception + d + infoStr.NameTI + d +
                                       infoStr.Info + d + infoStr.IsSendCDU);
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(" ПРОВЕРИТЬ" + e.Message);
            }

        }
        private void SaveExchangeProtocol(ObservableCollection<ExchangeProtocol> infoList)
        {
            try
            {
                string fileResult = Directory.GetCurrentDirectory() + "\\info.csv";
                var d = ";";
                if (infoList.Count() > 0)
                    using (StreamWriter sw = new StreamWriter(fileResult, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine("Объект" + d
                            + "ДЦ источник" + d + "Название ОИ" + d + "Категория" + d + "№ОИ" + d + "UID OI" + d + "UID PO" + d
                             + "ДЦ получатель" + d + "Название ОИ" + d + "Категория" + d + "№ОИ" + d + "UID OI" + d + "UID PO" + d
                            + "Комментарии" + d + "передача в ИА");
                        foreach (var infoStr in infoList)
                        {
                            sw.WriteLine(infoStr.KontrolObjectName + d +
                                       infoStr.DcSource + d + infoStr.NameDCSource + d + infoStr.CategoryDCSource + d + infoStr.NumberOIsource + d + infoStr.UidDCSource + d + infoStr.PoDCSource + d +
                                      infoStr.dcRec + d + infoStr.NamedcRec + d + infoStr.CategorydcRec + d + infoStr.NumberOIrec + d + infoStr.UiddcRec + d + infoStr.PodcRec + d +
                                      infoStr.Info + d + infoStr.IsSendCDU);
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(" ПРОВЕРИТЬ" + e.Message);
            }

        }
        private string GetInfoChekLine(ExchangeOTI exchangeOTI, CurrentLine lineMain)
        {
            OperTechInform otiMain;
            OperTechInform otiSecond;
            ObservableCollection<CurrentLine> lineCollectSecondAll = new ObservableCollection<CurrentLine>();
            if (lineMain.DispatchCenter == exchangeOTI.DCsource)
            {
                otiMain = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIsour && x.DispatchCenter == exchangeOTI.DCsource && x.TypeOI == "ТИ");
                otiSecond = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIrec && x.DispatchCenter == exchangeOTI.DCreceiver && x.TypeOI == "ТИ");
                lineCollectSecondAll = new ObservableCollection<CurrentLine>(CurrentLineCollect.Where(x => x.DispatchCenter == exchangeOTI.DCreceiver));
            }
            else
            {
                otiMain = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIrec && x.DispatchCenter == exchangeOTI.DCreceiver && x.TypeOI == "ТИ");
                otiSecond = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIsour && x.DispatchCenter == exchangeOTI.DCsource && x.TypeOI == "ТИ");
                lineCollectSecondAll = new ObservableCollection<CurrentLine>(CurrentLineCollect.Where(x => x.DispatchCenter == exchangeOTI.DCsource));
            }
            string info = "";
            try
            {
                CurrentLine lineSecond = lineCollectSecondAll.First(x => x.NumberIfact == otiSecond.NumberOI && x.DispatchCenter == otiSecond.DispatchCenter);
                info = "Несоответсвие объекта контроля:\n"
                    + "ТИ" + otiMain.NumberOI + " ВЛ: " + lineMain.NameLine + "\nобъект: " + lineMain.EnergyObject + " (" + lineMain.DispatchCenter + ")"
                    + ".\n" + "ТИ" + otiSecond.NumberOI + " ВЛ: " + lineMain.NameLine + "\nобъект: " + lineSecond.EnergyObject + " (" + lineSecond.DispatchCenter + ")";

            }
            catch
            {
                info = "Несоответсвие формирования Iфакт контроля:\n" +
                    "ТИ" + otiMain.NumberOI + " для контроля применяется только в " + otiMain.DispatchCenter
                + ".\n" + "ТИ" + otiSecond.NumberOI + " для контроля не применяется в " + otiSecond.DispatchCenter;
            }
            return info;
        }
        private string GetInfoChekTrans(ExchangeOTI exchangeOTI, CurrentTransform transeMain)
        {
            OperTechInform otiMain;
            OperTechInform otiSecond;
            ObservableCollection<CurrentTransform> transCollectSecondAll = new ObservableCollection<CurrentTransform>();
            if (transeMain.DispatchCenter == exchangeOTI.DCsource)
            {
                otiMain = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIsour && x.DispatchCenter == exchangeOTI.DCsource && x.TypeOI == "ТИ");
                otiSecond = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIrec && x.DispatchCenter == exchangeOTI.DCreceiver && x.TypeOI == "ТИ");
                transCollectSecondAll = new ObservableCollection<CurrentTransform>(CurrentTransformCollect.Where(x => x.DispatchCenter == exchangeOTI.DCreceiver));
            }
            else
            {
                otiMain = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIrec && x.DispatchCenter == exchangeOTI.DCreceiver && x.TypeOI == "ТИ");
                otiSecond = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIsour && x.DispatchCenter == exchangeOTI.DCsource && x.TypeOI == "ТИ");
                transCollectSecondAll = new ObservableCollection<CurrentTransform>(CurrentTransformCollect.Where(x => x.DispatchCenter == exchangeOTI.DCsource));
            }
            string info = "";
            try
            {
                CurrentTransform transSecond = transCollectSecondAll.First(x => x.NumberIfact == otiSecond.NumberOI && x.DispatchCenter == otiSecond.DispatchCenter);
                info = "Несоответсвие объекта контроля:\n"
                    + "ТИ" + otiMain.NumberOI + " ВЛ: " + transeMain.NameTransform + "\nобъект: " + transeMain.TransformWinding + " (" + transeMain.DispatchCenter + ")"
                    + ".\n" + "ТИ" + otiSecond.NumberOI + " ВЛ: " + transeMain.NameTransform + "\nобъект: " + transSecond.TransformWinding + " (" + transSecond.DispatchCenter + ")";

            }
            catch
            {
                info = "Несоответсвие формирования Iфакт контроля:\n" +
                    "ТИ" + otiMain.NumberOI + " для контроля применяется только в " + otiMain.DispatchCenter
                + ".\n" + "ТИ" + otiSecond.NumberOI + " для контроля не применяется в " + otiSecond.DispatchCenter;
            }
            return info;
        }
        private string GetInfoChekLine(CurrentLine lineMain, CurrentLine lineSecond, ObservableCollection<ExchangeOTI> exchangeCollect)
        {
            string info = "";
            bool flag = false;
            ObservableCollection<OperTechInform> otiListError = new ObservableCollection<OperTechInform>();
            foreach (OperTechInform otiMain in lineMain.ListFormula)
            {
                foreach (OperTechInform otiSecond in lineSecond.ListFormula)
                {
                    try
                    {
                        var exchangeOTI = exchangeCollect.First(x =>
                        (x.DCreceiver == lineMain.DispatchCenter && x.DCsource == lineSecond.DispatchCenter &&
                        x.NumberOIrec == otiMain.NumberOI && x.NumberOIsour == otiSecond.NumberOI) ||
                        (x.DCreceiver == lineSecond.DispatchCenter && x.DCsource == lineMain.DispatchCenter &&
                        x.NumberOIrec == otiSecond.NumberOI && x.NumberOIsour == otiMain.NumberOI));
                        flag = true; break;
                    }
                    catch { }
                }
                if (flag != true)
                { otiListError.Add(otiMain); }
            }
            if (otiListError.Count() > 0)
            {
                info = "Несоответсвие формирования Iфакт:\n";
            }
            for (int i = 0; i < otiListError.Count(); i++)
            {
                info += "ТИ" + otiListError[i].NumberOI + " в обмене между " + lineMain.DispatchCenter + " и " + lineSecond.DispatchCenter + " не участвует. \n";
            }
            return info;
        }
        private string GetInfoChekTrans(CurrentTransform transMain, CurrentTransform transSecond, ObservableCollection<ExchangeOTI> exchangeCollect)
        {
            string info = "";
            bool flag = false;
            ObservableCollection<OperTechInform> otiListError = new ObservableCollection<OperTechInform>();
            foreach (OperTechInform otiMain in transMain.ListFormula)
            {
                foreach (OperTechInform otiSecond in transSecond.ListFormula)
                {
                    try
                    {
                        var exchangeOTI = exchangeCollect.First(x =>
                        (x.DCreceiver == transMain.DispatchCenter && x.DCsource == transSecond.DispatchCenter &&
                        x.NumberOIrec == otiMain.NumberOI && x.NumberOIsour == otiSecond.NumberOI) ||
                        (x.DCreceiver == transSecond.DispatchCenter && x.DCsource == transMain.DispatchCenter &&
                        x.NumberOIrec == otiSecond.NumberOI && x.NumberOIsour == otiMain.NumberOI));
                        flag = true; break;
                    }
                    catch { }
                }
                if (flag != true)
                { otiListError.Add(otiMain); }
            }
            if (otiListError.Count() > 0)
            {
                info = "Несоответсвие формирования Iфакт:\n";
            }
            for (int i = 0; i < otiListError.Count(); i++)
            {
                info += "ТИ" + otiListError[i].NumberOI + " в обмене между " + transMain.DispatchCenter + " и " + transSecond.DispatchCenter + " не участвует. \n";
            }
            return info;
        }
        private string GetInfoChekBreaker(ExchangeOTI exchangeOTI, CurrentBreaker breakerMain)
        {
            OperTechInform otiMain;
            OperTechInform otiSecond;
            ObservableCollection<CurrentBreaker> breakerCollectSecondAll = new ObservableCollection<CurrentBreaker>();
            if (breakerMain.DispatchCenter == exchangeOTI.DCsource)
            {
                otiMain = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIsour && x.DispatchCenter == exchangeOTI.DCsource && x.TypeOI == "ТИ");
                otiSecond = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIrec && x.DispatchCenter == exchangeOTI.DCreceiver && x.TypeOI == "ТИ");
                breakerCollectSecondAll = new ObservableCollection<CurrentBreaker>(CurrentBreakerCollect.Where(x => x.DispatchCenter == exchangeOTI.DCreceiver));
            }
            else
            {
                otiMain = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIrec && x.DispatchCenter == exchangeOTI.DCreceiver && x.TypeOI == "ТИ");
                otiSecond = OtiCollect.First(x => x.NumberOI == exchangeOTI.NumberOIsour && x.DispatchCenter == exchangeOTI.DCsource && x.TypeOI == "ТИ");
                breakerCollectSecondAll = new ObservableCollection<CurrentBreaker>(CurrentBreakerCollect.Where(x => x.DispatchCenter == exchangeOTI.DCsource));
            }
            string info = "";
            try
            {
                CurrentBreaker breakerSecond = breakerCollectSecondAll.First(x => x.NumberIfact == otiSecond.NumberOI && x.DispatchCenter == otiSecond.DispatchCenter);
                info = "Несоответсвие объекта контроля:\n"
                    + "ТИ" + otiMain.NumberOI + " Выключатель: " + breakerMain.NameBreaker + " (" + breakerMain.DispatchCenter + ")"
                    + ".\n" + "ТИ" + otiSecond.NumberOI + " Выключатель: " + breakerMain.NameBreaker + " (" + breakerSecond.DispatchCenter + ")";

            }
            catch
            {
                info = "Несоответсвие формирования Iфакт контроля:\n" +
                    "ТИ" + otiMain.NumberOI + " для контроля применяется только в " + otiMain.DispatchCenter
                + ".\n" + "ТИ" + otiSecond.NumberOI + " для контроля не применяется в " + otiSecond.DispatchCenter;
            }
            return info;
        }
        private string GetInfoChekBreaker(CurrentBreaker breakerMain, CurrentBreaker breakerSecond, ObservableCollection<ExchangeOTI> exchangeCollect)
        {
            string info = "";
            bool flag = false;
            ObservableCollection<OperTechInform> otiListError = new ObservableCollection<OperTechInform>();
            foreach (OperTechInform otiMain in breakerMain.ListFormula)
            {
                foreach (OperTechInform otiSecond in breakerSecond.ListFormula)
                {
                    try
                    {
                        var exchangeOTI = exchangeCollect.First(x =>
                        (x.DCreceiver == breakerMain.DispatchCenter && x.DCsource == breakerSecond.DispatchCenter &&
                        x.NumberOIrec == otiMain.NumberOI && x.NumberOIsour == otiSecond.NumberOI) ||
                        (x.DCreceiver == breakerSecond.DispatchCenter && x.DCsource == breakerMain.DispatchCenter &&
                        x.NumberOIrec == otiSecond.NumberOI && x.NumberOIsour == otiMain.NumberOI));
                        flag = true; break;
                    }
                    catch { }
                }
                if (flag != true)
                { otiListError.Add(otiMain); }
            }
            if (otiListError.Count() > 0)
            {
                info = "Несоответсвие формирования Iфакт:\n";
            }
            for (int i = 0; i < otiListError.Count(); i++)
            {
                info += "ТИ" + otiListError[i].NumberOI + " в обмене между " + breakerMain.DispatchCenter + " и " + breakerSecond.DispatchCenter + " не участвует. \n";
            }
            return info;
        }

        /// <summary>
        /// Команда вызова окна настроек
        /// </summary>
        public ICommand SettingsCommand { get { return new RelayCommand(UpdateSettingsCheck, CanUpdateSettingsCheck); } }
        void UpdateSettingsCheck()
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.TriggerKPOS = triggerCheckKPOS;
            settingsWindow.TriggerMUN = triggerCheckMUN;
            settingsWindow.TriggerSMTNline = triggerCheckSMTNline;
            settingsWindow.TriggerSMTNbreaker = triggerCheckSMTNbreaker;
            settingsWindow.TriggerSMTNequipment = triggerCheckSMTNequipment;
            settingsWindow.TriggerSMTNtransformer = triggerCheckSMTNtransformer;
            settingsWindow.TriggerAOPO = triggerCheckSMTNAOPO;
            settingsWindow.TriggerAIP = triggerCheckAIP;
            settingsWindow.TriggerExchange = triggerCheckExchange;
            settingsWindow.TriggerCIM = triggerCheckCIM;
            settingsWindow.ShowDialog();
            if (settingsWindow.SaveChange)
            {
                triggerCheckKPOS = settingsWindow.TriggerKPOS;
                triggerCheckMUN = settingsWindow.TriggerMUN;
                triggerCheckSMTNline = settingsWindow.TriggerSMTNline;
                triggerCheckSMTNbreaker = settingsWindow.TriggerSMTNbreaker;
                triggerCheckSMTNequipment = settingsWindow.TriggerSMTNequipment;
                triggerCheckSMTNtransformer = settingsWindow.TriggerSMTNtransformer;
                triggerCheckSMTNAOPO = settingsWindow.TriggerAOPO;
                triggerCheckAIP = settingsWindow.TriggerAIP;
                triggerCheckExchange = settingsWindow.TriggerExchange;
                triggerCheckCIM = settingsWindow.TriggerCIM;
            }

        }
        bool CanUpdateSettingsCheck() { return true; }
        /// <summary>
        /// Команда подключения к БД
        /// </summary>
        public ICommand ConnectCommand { get { return new RelayCommand(UpdateOtiCollectExecute, CanUpdateOtiCollectExecute); } }

        void UpdateOtiCollectExecute()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            WaitWindow waitWindow = new WaitWindow();
            //waitWindow.progress.IsIndeterminate = true;
            waitWindow.Show();
            if (OtiCollect != null)
                OtiCollect.Clear();
            if (VoltageCollect != null)
                VoltageCollect.Clear();
            if (SecheniyaCollect != null)
                SecheniyaCollect.Clear();
            if (CurrentAopoCollect != null)
                CurrentAopoCollect.Clear();
            if (ReseiverCollect != null)
                ReseiverCollect.Clear();
            if (SourceCollect != null)
                SourceCollect.Clear();
            if (CurrentLineCollect != null)
                CurrentLineCollect.Clear();
            if (CurrentTransformCollect != null)
                CurrentTransformCollect.Clear();
            if (CurrentBreakerCollect != null)
                CurrentBreakerCollect.Clear();
            if (CurrentEquipmentCollect != null)
                CurrentEquipmentCollect.Clear();
            if (ExchangeCollect != null)
                ExchangeCollect.Clear();
            OtiCollect = _database.GetInfo();
            SecheniyaCollect = _database.GetSech();
            CurrentAopoCollect = _database.GetAOPO();
            SaveButtonIsEnebled = true;
            SourceCollect = _database.GetSource();
            ReseiverCollect = _database.GetReception();
            var result = from exRec in ReseiverCollect
                         join exSour in SourceCollect on new { exRec.Address, exRec.DCreceiver, exRec.DCsource, Type = exRec.TypeOIrec }
                         equals new { exSour.Address, exSour.DCreceiver, exSour.DCsource, Type = exSour.TypeOIsour }
                         select (new ExchangeOTI()
                         {
                             NumberOIrec = exRec.NumberOIrec,
                             DCreceiver = exRec.DCreceiver,
                             TypeOIrec = exRec.TypeOIrec,
                             NumberOIsour = exSour.NumberOIsour,
                             DCsource = exSour.DCsource,
                             TypeOIsour = exSour.TypeOIsour,
                             Address = exSour.Address
                         });
            ExchangeCollect = new ObservableCollection<ExchangeOTI>(result);
            ObservableCollection<Voltage> voltageList = _database.GetMUN();
            for (int i = 0; i < voltageList.Count; i++)
            {
                int count = 0;
                if (voltageList[i].ListFormula != null)
                    count = voltageList[i].ListFormula.Count;
                for (int j = 0; j < count; j++)
                {
                    if (voltageList[i].ListFormula[j].TypeOI == "ТИ")
                    {
                        Voltage voltage = new Voltage();
                        voltage.DispatchCenter = voltageList[i].DispatchCenter;
                        voltage.EnergyObject = voltageList[i].EnergyObject;
                        voltage.IdVoltage = voltageList[i].IdVoltage;
                        voltage.VoltageLevel = voltageList[i].VoltageLevel;
                        voltage.NumberOIitog = voltageList[i].NumberOIitog;
                        voltage.NameOIitog = voltageList[i].NameOIitog;
                        voltage.TypeOIitog = voltageList[i].NameOIitog;
                        voltage.TypeControl = voltageList[i].TypeControl;
                        voltage.Formula = voltageList[i].Formula;
                        //voltage.NumberOI = voltageList[i].ListFormula[j].NumberOI;
                        //voltage.TypeOI = voltageList[i].ListFormula[j].TypeOI;
                        var operTechInform = OtiCollect.FirstOrDefault(x => x.DispatchCenter == voltage.DispatchCenter
                         && x.NumberOI == voltageList[i].ListFormula[j].NumberOI
                         && x.TypeOI == voltageList[i].ListFormula[j].TypeOI);
                        voltage.NumberOI = operTechInform.NumberOI;
                        voltage.TypeOI = operTechInform.TypeOI;
                        voltage.NameOI = operTechInform.NameOI;
                        voltage.FormulaOI = operTechInform.Formula;
                        voltage.ListFormula = operTechInform.ListFormula;
                        VoltageCollect.Add(voltage);
                    }
                }
            }

            CurrentLineCollect = _database.GetSmtnLine();
            CurrentTransformCollect = _database.GetSmtnTransform();
            CurrentBreakerCollect = _database.GetSmtnBreaker();
            CurrentEquipmentCollect = _database.GetSmtnEquipment();
            //waitWindow.progress.IsIndeterminate = false;
            waitWindow.Close();
            Mouse.OverrideCursor = null;
        }
        bool CanUpdateOtiCollectExecute() { return true; }
        /// <summary>
        /// Команда сохранения в БД
        /// </summary>
        public ICommand SaveCommand { get { return new RelayCommand(SaveAopoExecute, CanSaveAopoExecute); } }

        bool CanSaveAopoExecute() { return true; }
        void SaveAopoExecute()
        {
            if (CurrentAopoCollect.Count != 0)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                WaitWindow waitWindow = new WaitWindow();
                waitWindow.progress.IsIndeterminate = true;
                waitWindow.Show();
                var flag = _database.SaveAopo(CurrentAopoCollect);
                var flag1 = _database.SaveDevice(OtiCollect);
                var flag2 = _database.SaveApplication(OtiCollect);
                waitWindow.progress.IsIndeterminate = false;
                waitWindow.Close();
                Mouse.OverrideCursor = null;
            }
        }

        #endregion

        #region Переходы по ОТИ
        /// <summary>
        /// Переход на вкладку ОТИ из КПОС
        /// </summary>
        public ICommand SelectOtiFromKpos { get { return new RelayCommand(StartSelectOtiFromKpos, CanSelectOtiFromKpos); } }
        bool CanSelectOtiFromKpos() { return true; }
        void StartSelectOtiFromKpos()
        {
            if (SelectedSech != null)
            {
                DisplayXamlTab = true;
                SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedSech.DispatchCenter
                && x.NumberOI == SelectedSech.NumberOI && x.TypeOI == SelectedSech.TypeOI);
            }
        }
        /// <summary>
        /// Переход на вкладку ОТИ из МУН
        /// </summary>
        public ICommand SelectOtiFromMun { get { return new RelayCommand(StartSelectOtiFromMun, CanSelectOtiFromMun); } }
        bool CanSelectOtiFromMun() { return true; }
        void StartSelectOtiFromMun()
        {
            try
            {
                if (SelectedVoltage != null)
                {
                    switch (_index)
                    {
                        case 4:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedVoltage.DispatchCenter
                            && x.NumberOI == SelectedVoltage.NumberOI && x.TypeOI == "ТИ");
                            break;
                        default:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedVoltage.DispatchCenter
                            && x.NumberOI == SelectedVoltage.NumberOIitog && x.TypeOI == "ТИ");
                            break;
                    }
                    DisplayXamlTab = true;
                }
            }
            catch (InvalidOperationException) { return; }
        }
        /// <summary>
        /// Переход на вкладку ОТИ из СМТН_ЛЭП
        /// </summary>
        public ICommand SelectOtiFromSmtnLine { get { return new RelayCommand(StartSelectOtiFromSmtnLine, CanSelectOtiFromSmtnLine); } }
        bool CanSelectOtiFromSmtnLine() { return true; }
        void StartSelectOtiFromSmtnLine()
        {
            try
            {
                if (SelectedCurrentLine != null)
                {
                    switch (_index)
                    {
                        case 7:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentLine.DispatchCenter
                            && x.NumberOI == SelectedCurrentLine.NumberIa && x.TypeOI == "ТИ");
                            break;
                        case 9:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentLine.DispatchCenter
                            && x.NumberOI == SelectedCurrentLine.NumberIb && x.TypeOI == "ТИ");
                            break;
                        case 11:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentLine.DispatchCenter
                            && x.NumberOI == SelectedCurrentLine.NumberIc && x.TypeOI == "ТИ");
                            break;
                        case 13:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentLine.DispatchCenter
                            && x.NumberOI == SelectedCurrentLine.NumberItnv && x.TypeOI == "ТИ");
                            break;
                        case 15:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentLine.DispatchCenter
                            && x.NumberOI == SelectedCurrentLine.NumberS && x.TypeOI == "ТС");
                            break;
                        default:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentLine.DispatchCenter
                            && x.NumberOI == SelectedCurrentLine.NumberIfact && x.TypeOI == "ТИ");
                            break;
                    }
                    DisplayXamlTab = true;
                }
            }
            catch (InvalidOperationException) { return; }
        }
        /// <summary>
        /// Переход на вкладку ОТИ из СМТН_АТ(Т)
        /// </summary>
        public ICommand SelectOtiFromSmtnTransform { get { return new RelayCommand(StartSelectOtiFromSmtnTransform, CanSelectOtiFromSmtnTransform); } }
        bool CanSelectOtiFromSmtnTransform() { return true; }
        void StartSelectOtiFromSmtnTransform()
        {
            try
            {
                if (SelectedCurrentTransform != null)
                {
                    switch (_index)
                    {

                        case 8:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentTransform.DispatchCenter
                            && x.NumberOI == SelectedCurrentTransform.NumberRpn && x.TypeOI == "ТИ");
                            break;
                        case 10:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentTransform.DispatchCenter
                            && x.NumberOI == SelectedCurrentTransform.NumberItnv && x.TypeOI == "ТИ");
                            break;
                        case 12:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentTransform.DispatchCenter
                            && x.NumberOI == SelectedCurrentTransform.NumberS && x.TypeOI == "ТС");
                            break;
                        default:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentTransform.DispatchCenter
                            && x.NumberOI == SelectedCurrentTransform.NumberIfact && x.TypeOI == "ТИ");
                            break;
                    }
                    DisplayXamlTab = true;
                }
            }
            catch (InvalidOperationException) { return; }
        }
        /// <summary>
        /// Переход на вкладку ОТИ из СМТН.Выкл
        /// </summary>
        public ICommand SelectOtiFromSmtnBreaker { get { return new RelayCommand(StartSelectOtiFromSmtnBreaker, CanSelectOtiFromSmtnBreaker); } }
        bool CanSelectOtiFromSmtnBreaker() { return true; }
        void StartSelectOtiFromSmtnBreaker()
        {
            try
            {
                if (SelectedCurrentBreaker != null)
                {
                    switch (_index)
                    {

                        case 7:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentBreaker.DispatchCenter
                            && x.NumberOI == SelectedCurrentBreaker.NumberIa && x.TypeOI == "ТИ");
                            break;
                        case 9:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentBreaker.DispatchCenter
                            && x.NumberOI == SelectedCurrentBreaker.NumberIb && x.TypeOI == "ТИ");
                            break;
                        case 11:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentBreaker.DispatchCenter
                            && x.NumberOI == SelectedCurrentBreaker.NumberIc && x.TypeOI == "ТИ");
                            break;
                        case 13:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentBreaker.DispatchCenter
                            && x.NumberOI == SelectedCurrentBreaker.NumberItnv && x.TypeOI == "ТИ");
                            break;
                        case 15:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentBreaker.DispatchCenter
                            && x.NumberOI == SelectedCurrentBreaker.NumberS && x.TypeOI == "ТС");
                            break;
                        default:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentBreaker.DispatchCenter
                            && x.NumberOI == SelectedCurrentBreaker.NumberIfact && x.TypeOI == "ТИ");
                            break;
                    }
                    DisplayXamlTab = true;
                }
            }
            catch (InvalidOperationException) { return; }
        }

        /// <summary>
        /// Переход на вкладку ОТИ из СМТН.АОПО
        /// </summary>
        public ICommand SelectOtiFromSmtnAopo { get { return new RelayCommand(StartSelectOtiFromSmtnAopo, CanSelectOtiFromSmtnAopo); } }
        bool CanSelectOtiFromSmtnAopo() { return true; }
        void StartSelectOtiFromSmtnAopo()
        {
            try
            {
                if (SelectedCurrentAopo != null)
                {
                    switch (_index)
                    {
                        case 4:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentAopo.DispatchCenter
                            && x.NumberOI == SelectedCurrentAopo.NumberTIobject && x.TypeOI == "ТИ");
                            break;
                        case 6:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentAopo.DispatchCenter
                            && x.NumberOI == SelectedCurrentAopo.NumberTIstandart && x.TypeOI == "ТИ");
                            break;
                        case 10:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentAopo.DispatchCenter
                            && x.NumberOI == SelectedCurrentAopo.NumberSitog && x.TypeOI == "ТС");
                            break;
                        case 12:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentAopo.DispatchCenter
                            && x.NumberOI == SelectedCurrentAopo.NumberS && x.TypeOI == "ТС");
                            break;
                        default:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedCurrentAopo.DispatchCenter
                            && x.NumberOI == SelectedCurrentAopo.NumberTI && x.TypeOI == "ТИ");
                            break;
                    }
                    DisplayXamlTab = true;
                }
            }
            catch (InvalidOperationException) { return; }
        }
        /// <summary>
        /// Переход на вкладку ОТИ из Протокола проверки
        /// </summary>
        public ICommand SelectCheckInfo { get { return new RelayCommand(StartSelectCheckInfo, CanSelectCheckInfo); } }
        bool CanSelectCheckInfo() { return true; }
        void StartSelectCheckInfo()
        {
            try
            {
                if (SelectedItemProtocol != null)
                {
                    switch (_index)
                    {
                        case 4:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedItemProtocol.DcSource
                            && x.NumberOI == SelectedItemProtocol.NumberOIsource && x.TypeOI == SelectedItemProtocol.TypeOI);
                            break;
                        case 5:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedItemProtocol.DCReception
                            && x.NumberOI == SelectedItemProtocol.NumberOIreception && x.TypeOI == SelectedItemProtocol.TypeOI);
                            break;
                        default:
                            SelectedOti = OtiCollect.Single(x => x.DispatchCenter == SelectedItemProtocol.DispatchCenter
                            && x.NumberOI == SelectedItemProtocol.NumberOI && x.TypeOI == SelectedItemProtocol.TypeOI);
                            break;
                    }
                    DisplayXamlTab = true;
                }
            }
            catch (InvalidOperationException) { return; }
        }
        #endregion
    }


    /// <summary>
    /// Для отображения инструментов фильтрации
    /// </summary>
    public class OppositeBooleanToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
            {
                return System.Windows.Visibility.Visible;
            }
            else
            {
                return System.Windows.Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility visibility = (System.Windows.Visibility)value;

            return visibility == System.Windows.Visibility.Visible ? false : true;
        }
    }

}
