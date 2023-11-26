using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;

namespace Awe.UI.Controls
{
    public class ToggleSwitchAutomationPeer : ToggleButtonAutomationPeer
    {
        public ToggleSwitchAutomationPeer(ToggleSwitch owner) : base(owner)
        {
        }

        protected override string GetLocalizedControlTypeCore()
        {
            return "开关";
        }
    }
}
