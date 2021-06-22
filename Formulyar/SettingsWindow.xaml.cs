using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Formulyar.ViewModel;

namespace Formulyar
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Members
        private bool _triggerSMTNline;
        private bool _triggerSMTNtransformer;
        private bool _triggerSMTNbreaker;
        private bool _triggerSMTNequipment;
        private bool _triggerMUN;
        private bool _triggerKPOS;
        private bool _triggerAOPO;
        private bool _triggerAIP;
        private bool _triggerExchange;
        private bool _triggerCim;
        #endregion

        #region Properites
        public bool SaveChange=false;
        public bool TriggerSMTNline
        {
            get { return _triggerSMTNline; }
            set { _triggerSMTNline = value; checkBoxSmtnLine.IsChecked = value; }
        }
        public bool TriggerSMTNtransformer
        {
            get { return _triggerSMTNtransformer; }
            set { _triggerSMTNtransformer = value; checkBoxSmtnTransform.IsChecked = value; }
        }
        public bool TriggerSMTNbreaker
        {
            get { return _triggerSMTNbreaker; }
            set { _triggerSMTNbreaker = value; checkBoxSmtnBreaker.IsChecked = value; }
        }
        public bool TriggerSMTNequipment
        {
            get { return _triggerSMTNequipment; }
            set { _triggerSMTNequipment = value; checkBoxSmtnDop.IsChecked = value; }
        }
        public bool TriggerMUN
        {
            get { return _triggerMUN; }
            set { _triggerMUN = value; checkBoxMun.IsChecked = value; }
        }
        public bool TriggerKPOS
        {
            get { return _triggerKPOS; }
            set { _triggerKPOS = value; checkBoxKpos.IsChecked = value; }
        }
        public bool TriggerAOPO
        {
            get { return _triggerAOPO; }
            set { _triggerAOPO = value; checkBoxSmtnAOPO.IsChecked = value; }
        }
        public bool TriggerAIP
        {
            get { return _triggerAIP; }
            set { _triggerAIP = value; checkBoxAIP.IsChecked = value; }
        }
        public bool TriggerExchange
        {
            get { return _triggerExchange; }
            set { _triggerExchange = value; checkBoxExchange.IsChecked = value; }
        }
        public bool TriggerCIM
        {
            get { return _triggerCim; }
            set { _triggerCim = value; checkBoxCIM.IsChecked = value; }
        }
        #endregion

        #region Checked\UnChecked
        private void checkBoxKpos_Checked(object sender, RoutedEventArgs e)
        { TriggerKPOS = true; }
        private void checkBoxKpos_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerKPOS = false;        }
        private void checkBoxMun_Checked(object sender, RoutedEventArgs e)
        {            TriggerMUN = true;        }
        private void checkBoxMun_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerMUN = false;        }
        private void checkBoxSmtnLine_Checked(object sender, RoutedEventArgs e)
        {            TriggerSMTNline = true;        }
        private void checkBoxSmtnLine_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerSMTNline = false;        }
        private void checkBoxSmtnTransform_Checked(object sender, RoutedEventArgs e)
        {            TriggerSMTNtransformer = true;        }
        private void checkBoxSmtnTransform_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerSMTNtransformer = false;        }
        private void checkBoxSmtnBreaker_Checked(object sender, RoutedEventArgs e)
        {            TriggerSMTNbreaker = true;        }
        private void checkBoxSmtnBreaker_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerSMTNbreaker = false;        }
        private void checkBoxSmtnDop_Checked(object sender, RoutedEventArgs e)
        {            TriggerSMTNequipment = true;        }
        private void checkBoxSmtnDop_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerSMTNequipment = false;        }
        private void checkBoxSmtnAOPO_Checked(object sender, RoutedEventArgs e)
        {            TriggerAOPO = true;        }
        private void checkBoxSmtnAOPO_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerAOPO = false;        }
        private void checkBoxAIP_Checked(object sender, RoutedEventArgs e)
        {            TriggerAIP = true;        }
        private void checkBoxAIP_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerAIP = false;        }
        private void checkBoxExchange_Checked(object sender, RoutedEventArgs e)
        {            TriggerExchange = true;        }
        private void checkBoxExchange_Unchecked(object sender, RoutedEventArgs e)
        {            TriggerExchange = false;        }
        private void checkBoxCIM_Checked(object sender, RoutedEventArgs e)
        { TriggerCIM = true; }
        private void checkBoxCIM_Unchecked(object sender, RoutedEventArgs e)
        { TriggerCIM = false; }
        #endregion
        public SettingsWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TriggerKPOS = true;
            TriggerMUN = true;
            TriggerSMTNline = true;
            TriggerSMTNtransformer = true;
            TriggerSMTNbreaker = true;
            TriggerSMTNequipment = true;
            TriggerAOPO = true;
            TriggerAIP = false;
            TriggerExchange = false;
            TriggerCIM = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveChange = false;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveChange = true;
            this.Close();
        }
        
    }
}
