using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Awe.UI.Controls
{
    internal class LoadingAutomationPeer : UserControlAutomationPeer
    {
        public LoadingAutomationPeer(UserControl owner) : base(owner)
        {
        }

        protected override string GetLocalizedControlTypeCore()
        {
            return "加载中...";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Image;
        }
    }
}
