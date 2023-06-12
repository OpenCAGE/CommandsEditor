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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommandsEditor.UserControls.Variants
{
    /// <summary>
    /// Interaction logic for WPF_Dropdown.xaml
    /// </summary>
    public partial class WPF_Dropdown : UserControl
    {
        public Action<string> OnValueChanged;
        public string Value { get { return dropdown.Text; } }
        public AssetList.Value ValueObj { get { return _values == null ? null : _values.FirstOrDefault(o => o.value == _lastValueChanged); } }
        public int Index { get { return _values == null ? -1 : _values.IndexOf(ValueObj); } }

        private List<AssetList.Value> _values = null;

        public WPF_Dropdown()
        {
            InitializeComponent();
        }

        public void SetValues(List<AssetList.Value> values, string defaultVal = "")
        {
            _values = values;
            
            for (int i = 0; i < _values.Count; i++)
                dropdown.Items.Add(_values[i].value);

            dropdown.SelectedItem = defaultVal;
        }
        public void SetValues(List<string> values, string defaultVal = "")
        {
            for (int i = 0; i < _values.Count; i++)
                dropdown.Items.Add(_values[i]);

            dropdown.SelectedItem = defaultVal;
        }

        private void dropdown_DropDownClosed(object sender, EventArgs e)
        {
            ValueChanged();
        }
        private void dropdown_KeyUp(object sender, KeyEventArgs e)
        {
            ValueChanged();
        }
        private void dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValueChanged();
        }

        private string _lastValueChanged = "";
        private void ValueChanged()
        {
            if (_lastValueChanged == dropdown.Text) return;
            _lastValueChanged = dropdown.Text;

            dropdown.ToolTip = (ValueObj != null) ? ValueObj.tooltip : "";
            OnValueChanged?.Invoke(_lastValueChanged);
        }
    }
}
