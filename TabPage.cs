using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloseAbleTab {
    public class TabPage : Panel {
        public TabPage() {
            TabHeaderText = "";
            isEdited = false;
            UniqueId = -1;
            LoadedFile = "";
            PageType = TabPageType.Unregonized;
            CanClose = true;
        }
        public virtual void InitializedOnDynamicTab() {
            return;
        }

        public TabPageType PageType { get; set; }
        public bool CanClose { get; set; }

        public bool IsEdited {
            get {
                return isEdited;
            }
            set {
                TabTextHeaderChangedEventArgs e = new TabTextHeaderChangedEventArgs();
                if (value) {
                    e.TextHeader = "*" + tabHeaderText;
                } else {
                    e.TextHeader = tabHeaderText;
                }
                e.DynamicTabIndex = DynamicTabIndex;
                onTabHeadrTextChanged(e);
                isEdited = value;
            }
        }

        public string LoadedFile { get; set; }

        private string tabHeaderText;
        private bool isEdited;
        public int ForeginId { get; set; }

        protected virtual void onTabHeadrTextChanged(TabTextHeaderChangedEventArgs e) {
            EventHandler<TabTextHeaderChangedEventArgs> handler = TabHeaderTextChanged;
            if (handler != null) {
                handler(this, e);
            }
        }

        public void ForceCloseTabPage() {
            EventHandler<TabCloseTabEventArgs> handle = ForceCloseTab;
            TabCloseTabEventArgs e = new TabCloseTabEventArgs();
            e.IndexTab = DynamicTabIndex;
            e.UniqueId = UniqueId;
            if (handle != null)
                handle(this, e);
        }

        protected bool IsDuplicateFileInTab(string filePath) {
            EventHandler<TabCompareTabPageEventArgs> handle = CompareTabPage;
            TabCompareTabPageEventArgs e = new TabCompareTabPageEventArgs();
            e.TabPageIndex = this.TabIndex;
            e.FilePath = filePath;
            if (handle != null)
                handle(this, e);
            return e.TabPageIsExist;
        }

        public string TabHeaderText {
            get {
                return tabHeaderText;
            }
            set {
                TabTextHeaderChangedEventArgs e = new TabTextHeaderChangedEventArgs();
                e.TextHeader = isEdited ? "*" + value : value;
                e.DynamicTabIndex = DynamicTabIndex;
                onTabHeadrTextChanged(e);
                tabHeaderText = value;
            }
        }

        public virtual bool CloseTabAnyway() {
            DialogResult dialogResult = Msg.WarrnQ("Are you sure, you want close this tab ?");
            switch (dialogResult) {
                case DialogResult.Yes:
                    return true;
                case DialogResult.No:
                    return false;
            }
            return false;
        }

        public virtual void OnClosedTab() { }
        public virtual bool OnSaveFile() { return true; }
        public virtual void SelectControls() { }

        public event EventHandler<TabTextHeaderChangedEventArgs> TabHeaderTextChanged;
        public event EventHandler<TabCloseTabEventArgs> ForceCloseTab;
        public event EventHandler<TabCompareTabPageEventArgs> CompareTabPage;
        public int UniqueId { get; set; }
        public int DynamicTabIndex { get; set; }
    }
}
