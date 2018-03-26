using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloseAbleTab {
    public class TabTextHeaderChangedEventArgs : EventArgs {
        public string TextHeader { get; set; }
        public int DynamicTabIndex { get; set; }
    }

    public class TabCloseTabEventArgs : EventArgs {
        public int IndexTab { get; set; }
        public int UniqueId { get; set; }
        public TabPageType TabPage { get; set; }
    }

    public class TabLockControl : EventArgs {
        public bool LockedControl { get; set; }
    }

    public class TabCompareTabPageEventArgs : EventArgs {
        public string FilePath { get; set; }
        public int TabPageIndex { get; set; }
        public bool TabPageIsExist { get; set; }
    }

    public class TabSelectedTabEventArgs : EventArgs {
        public TabPageType TabPage { get; set; }
        public int IndexTab { get; set; }
    }
}
