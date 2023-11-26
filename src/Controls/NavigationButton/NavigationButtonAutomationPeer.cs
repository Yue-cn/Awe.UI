using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Awe.UI.Controls
{
    public class NavigationButtonAutomationPeer : ButtonBaseAutomationPeer
    {
        public NavigationButtonAutomationPeer(NavigationButton owner) : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface is PatternInterface.Invoke)
            {
                return patternInterface;
            }

            return base.GetPattern(patternInterface);
        }
    }
}
