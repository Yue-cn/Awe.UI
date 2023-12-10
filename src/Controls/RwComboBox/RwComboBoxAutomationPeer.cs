using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Awe.UI.Controls
{
    public class RwComboBoxAutomationPeer : ComboBoxAutomationPeer,IExpandCollapseProvider
    {
        public RwComboBoxAutomationPeer(RwComboBox owner) : base(owner)
        {
           
        }

        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
        {
            get
            {
                RwComboBox comboBox = (RwComboBox)Owner;
                if (!comboBox.IsOpened)
                {
                    return ExpandCollapseState.Collapsed;
                }

                return ExpandCollapseState.Expanded;
            }
        }


    }
}
