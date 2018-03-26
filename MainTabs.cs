using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloseAbleTab {
    public partial class MainTabs : UserControl {
        public MainTabs() {
            InitializeComponent();
            lsbList.LostFocus += new System.EventHandler(this.lsbList_LostFocus);
            wtab = 140;
            lentab = -1;
            tabPages = new List<TabPage>();
            tabIndex = -1;
            tabTextHeaderFont = new System.Drawing.Font("Microsoft Sans Serif",
                                                               9.0F,
                                                               System.Drawing.FontStyle.Regular,
                                                               System.Drawing.GraphicsUnit.Point,
                                                               ((byte)(0)));
            panTabHeaderControls.Visible = false;
            maxListTab = 7;
            panHTabHeader.Size = new System.Drawing.Size(1, 26);
            defaultTextTabHeader = "New Tab";
        }

        private byte wtab;
        private int lentab;
        private int tabIndex;
        private byte maxListTab;
        private List<TabPage> tabPages;
        private Font tabTextHeaderFont;
        private String defaultTextTabHeader;

        public Panel PanelTabMain {
            get {
                return panMain;
            }
        }

        public int TabSelectedIndex { 
            get {
                return tabIndex;
            }
            set {
                int val = value;

                if (val >= -1 && val < DynamicTabCount) {
                    if (val == -1)
                        return;
                    selectTab(val);
                    deselectTab(tabIndex);
                    tabIndex = val;
                    TabSelectedTabEventArgs e = new TabSelectedTabEventArgs();
                    e.IndexTab = val;
                    e.TabPage = tabPages[val].PageType;
                    
                } else
                    throw new IndexOutOfRangeException("Index Out of Range !");
            }
        }

        public String DefaultTextHeaderTab {
            get {
                return defaultTextTabHeader;
            }
            set {
                defaultTextTabHeader = value;
            }
        }

        public List<TabPage> TabPages {
            get {
                return tabPages;               
            }
        }

        public int DynamicTabCount {
            get {
                return tabPages.Count;
            }
        }

        private void tabClick(object sender, EventArgs e) {
            for (int i = 0; i < panHTabHeader.Controls.Count; ++i) {
                if (panHTabHeader.Controls[i].Focused) {
                    if (i == tabIndex)
                        return;
                    TabSelectedIndex = i;
                    tabSizeChangd();
                    break;
                }
            }
        }

        private void OnSelctedTab(TabSelectedTabEventArgs e) {
            if (SelectedTab != null)
                SelectedTab(this, e);
        }

        private int getIndexFromMouseHover() {
            int i = 0;
            int x = panHTabHeader.PointToClient(Cursor.Position).X;
            for (i = 0; i < panHTabHeader.Controls.Count; ++i) {
                if (((i * wtab) + wtab) > x) {
                    return i;
                }
            }
            return i;
        }

        private void TabMouseDown(object sender, MouseEventArgs e) {
            int i = getIndexFromMouseHover();
            if (i == tabIndex)
                return;
            TabSelectedIndex = i;
            tabSizeChangd();
        }

        private void lsbAdd(string item) {
            lsbList.Items.Add(item);
            if (lsbList.Items.Count <= maxListTab) {
                int y1 = lsbList.Size.Height;
                int y2 = lsbList.Font.Height + 1;
                int x = lsbList.Size.Width;
                lsbList.Size = new System.Drawing.Size(x, y1 + y2);
            }
            
        }

        private void lsbRemoveAt(int idx) {
            lsbList.Items.RemoveAt(idx);
            if (lsbList.Size.Height >= 13) {
                if (lsbList.Items.Count < maxListTab)
                    lsbList.Size = new System.Drawing.Size(lsbList.Size.Width, lsbList.Size.Height - lsbList.Font.Height);
            }
        }

        private void tabSizeChangd() {
            int endTab = this.Size.Width - panTabHeaderControls.Size.Width;//57;
            int startTab = 0;
            int cPositionTab = wtab * tabIndex;
            int middle = this.lentab / 2;
            if (this.lentab < endTab) {
                panHTabHeader.Location = new System.Drawing.Point(startTab, panHTabHeader.Location.Y);
                return;
            }
            
            if (tabIndex == 0) {
                panHTabHeader.Location = new System.Drawing.Point(startTab, panHTabHeader.Location.Y);
                return;
            } else if (tabIndex == (panHTabHeader.Controls.Count - 1)) {
                panHTabHeader.Location = new System.Drawing.Point(endTab - lentab, panHTabHeader.Location.Y);
                return;
            }

            if (((panHTabHeader.Location.X + cPositionTab) + wtab) >= endTab)
                panHTabHeader.Location = new System.Drawing.Point(endTab - (cPositionTab + wtab), panHTabHeader.Location.Y);
            if ((panHTabHeader.Location.X + cPositionTab) < startTab) 
                panHTabHeader.Location = new System.Drawing.Point(startTab - cPositionTab, panHTabHeader.Location.Y);
        }

        private void selectTab(int idx) {
            if (panHTabHeader.Controls.Count < 1)
                return;
            lsbList.SelectedIndex = idx;
            Button bt = (Button)panHTabHeader.Controls[idx];
            bt.Location = new System.Drawing.Point(bt.Location.X, 0);
            bt.Size = new System.Drawing.Size(bt.Size.Width, 30);
            bt.UseVisualStyleBackColor = true;
            panMain.Controls.Clear();
            if (tabPages[idx].Dock == DockStyle.None)
                tabPages[idx].Size = new Size(panMain.Size.Width, panMain.Size.Height);
            panMain.Controls.Add(tabPages[idx]);
            tabPages[idx].SelectControls();
        }

        private void deselectTab(int idx) {
            if (panHTabHeader.Controls.Count <= 1)
                return;
            Button bt = (Button)panHTabHeader.Controls[idx];
            bt.Location = new System.Drawing.Point(bt.Location.X, 4);
            bt.Size = new System.Drawing.Size(bt.Size.Width, 26);
            bt.UseVisualStyleBackColor = false;
        }

        public int GetUniqueId() {
            int idx = tabPages.Count;
            int max = -1;
            for (int i = 0; i < idx; ++i) {
                if (max < tabPages[i].UniqueId) 
                    max = tabPages[i].UniqueId;
            }
            return ++max;
        }

        public void AddTabPage(TabPage tabPage) {
            int idx = panHTabHeader.Controls.Count;

            tabPage.TabIndex = idx;
            tabPage.DynamicTabIndex = idx;

            if (tabPage.Dock == DockStyle.None) {
                tabPage.Location = new Point(0, 0);
                tabPage.Size = new System.Drawing.Size(this.panMain.Size.Width, this.panMain.Size.Height);
                tabPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));
            }

            tabPage.TabHeaderTextChanged += new EventHandler<TabTextHeaderChangedEventArgs>(tabPages_TabHeaderTextChanged);
            tabPage.ForceCloseTab += new EventHandler<TabCloseTabEventArgs>(forceCloseTab);
            tabPage.CompareTabPage += new EventHandler<TabCompareTabPageEventArgs>(CompareTabPage);

            if (tabPage.UniqueId == -1)
                tabPage.UniqueId = GetUniqueId();


            Button bt = new Button();
            bt.Font = tabTextHeaderFont;
            bt.Location = new System.Drawing.Point(lentab, 4);
            bt.Name = "Tab_headr" + idx;
            bt.Size = new System.Drawing.Size(wtab, 26);
            bt.Text = tabPage.TabHeaderText;
            bt.UseVisualStyleBackColor = true;
            bt.TextAlign = ContentAlignment.MiddleLeft;
            bt.Click += new EventHandler(tabClick);
            bt.MouseDown += new System.Windows.Forms.MouseEventHandler(TabMouseDown);
            bt.MouseWheel += new MouseEventHandler(bt_MouseWheel);

            panHTabHeader.Controls.Add(bt);
            tabPages.Add(tabPage);
            lsbAdd(tabPage.TabHeaderText);
            TabSelectedTabEventArgs e = new TabSelectedTabEventArgs();
            e.IndexTab = idx;
            e.TabPage = tabPage.PageType;
            OnSelctedTab(e);
            panHTabHeader.Size = new System.Drawing.Size(panHTabHeader.Size.Width + wtab,
                                                         panHTabHeader.Size.Height);

            TabSelectedIndex = idx;

            tabIndex = idx;
            lentab += wtab;
            tabSizeChangd();

            tabPage.InitializedOnDynamicTab();
            
            if (tabPage.TabHeaderText.Equals("")) {
                tabPage.TabHeaderText = "[" + defaultTextTabHeader + " " + tabPage.UniqueId + "]";
                tabPage.IsEdited = true;
            } else
                SetTextTab(tabPage.TabHeaderText, idx);

            if (!panTabHeaderControls.Visible)
                panTabHeaderControls.Visible = true;
            tabPage.IsEdited = tabPage.IsEdited;
        }

        void bt_MouseWheel(object sender, MouseEventArgs e) {
            int res = 0;
            int delta = e.Delta / 8;
            int resLocation = 0;
            int del = 0;
            int endTab = this.Size.Width - panTabHeaderControls.Size.Width;
            int startTab = 0;
            int lenTabSpace = this.Size.Width - this.toolStrip1.Size.Width;


            if (panHTabHeader.Size.Width < endTab)
                return;

            if (e.Delta < 0) {
                res = panHTabHeader.Location.X + panHTabHeader.Size.Width;
                del = res + delta;
                resLocation = del < endTab ? panHTabHeader.Location.X : panHTabHeader.Location.X + delta;
            }

            if (e.Delta > 0) {
                del = panHTabHeader.Location.X + delta;
                resLocation = del >= startTab ? 0 : panHTabHeader.Location.X + delta;
            }
            panHTabHeader.Location = new Point(resLocation, 0);
        }

        private void tabPages_TabHeaderTextChanged(object sender, TabTextHeaderChangedEventArgs e) {
            SetTextTab(e.TextHeader, e.DynamicTabIndex);
        }

        private void LoackControlPanHeader(object sender, TabLockControl e) {
            panTabHeaderControls.Enabled = e.LockedControl;

        }

        public int isFileLoaded(string path) {
            for (int i = 0; i < tabPages.Count; ++i) {
                if (tabPages[i].LoadedFile.Equals(path))
                    return i;
            }

            return -1;
        }

        private void SetTextTab(string text, int idx) {
            if (idx < 0)
                return;

            string htext = text;

            using (Graphics tabGraphics = this.CreateGraphics()) {
                SizeF sf = new SizeF();
                int i = 0;
                int sizeF = 0;

                for (i = 0; i < text.Length; ++i) {
                    sf = tabGraphics.MeasureString(text[i].ToString(), tabTextHeaderFont);
                    sizeF += (int)Math.Ceiling(sf.Width) <= 11 ? (int)Math.Floor(sf.Width - 1.5) : (int)Math.Ceiling(sf.Width - 3.5);
                    if (sizeF >= wtab) {
                        htext = text.Remove(i) + "...";
                        break;
                    }
                }
            }

            panHTabHeader.Controls[idx].Text = htext;
            if (idx < lsbList.Items.Count)
                lsbList.Items[idx] = text;
        }

        private void btClose_Click(object sender, EventArgs e) {
            CloseTabByIndex(tabIndex);
        }

        public void CloseTabByForeginId(int id) {
            int max = DynamicTabCount;
            int cnt = 0;
            var lsClose = new List<int>();
            for (int i = 0; i < max; ++i) {
                if (id == tabPages[i].ForeginId) {
                    lsClose.Add(i);
                }
            }

            for (int i = 0; i < lsClose.Count; ++i) {
                TabSelectedIndex = lsClose[i] - cnt;
                CloseTabByIndex(lsClose[i] - cnt);
                ++cnt;
            }
        }

        private void changeIndaxTabInTabPage() {
            for (int i = 0; i < tabPages.Count; ++i) {
                tabPages[i].DynamicTabIndex = i;


                tabPages[i].TabIndex = i;
            }
        }

        private void forceCloseTab(object sender, TabCloseTabEventArgs e) {
            tabPages[e.IndexTab].OnClosedTab();
            panHTabHeader.Controls[e.IndexTab].Dispose();
            tabPages[e.IndexTab].Dispose();
            tabPages.RemoveAt(e.IndexTab);
            panMain.Controls.Clear();
            lsbRemoveAt(e.IndexTab);


            panHTabHeader.Size = new System.Drawing.Size(panHTabHeader.Size.Width - wtab,
                                                         panHTabHeader.Size.Height);

            lentab -= wtab;

            if (e.IndexTab == panHTabHeader.Controls.Count) {
                tabIndex = (e.IndexTab > 0) ? e.IndexTab - 1 : -1;
            } else {
                for (int i = tabIndex; i < panHTabHeader.Controls.Count; ++i) {
                    panHTabHeader.Controls[i].Location = new Point(panHTabHeader.Controls[i].Location.X - wtab + 1,
                                                                   panHTabHeader.Controls[i].Location.Y);
                }
            }

            changeIndaxTabInTabPage();

            selectTab(tabIndex);
            tabSizeChangd();
        }

        public void CloseAllTab() {

            for (int idx = 0; idx < tabPages.Count; ++idx) {
                tabPages[idx].OnClosedTab();
                onCloseTab(idx, tabPages[idx].UniqueId, tabPages[idx].PageType);
            }

            panMain.Controls.Clear();

            foreach (Control ctrl in panHTabHeader.Controls)
                ctrl.Dispose();
            panHTabHeader.Controls.Clear();
            foreach (TabPage page in tabPages)
                page.Dispose();
            tabPages.Clear();
            lsbList.Size = new System.Drawing.Size(lsbList.Size.Width, 0);


            panHTabHeader.Size = new System.Drawing.Size(1, 26);

            lentab = -1;
            tabIndex = -1;

            //panTabHeaderControls.Visible = false;
        }

        public void CloseTabByIndex(int idx) {
            if (idx < 0)
                return;
            if (!tabPages[idx].CanClose)
                return;
            if (tabPages[idx].IsEdited) {
                if (!tabPages[idx].CloseTabAnyway()) {
                    return;
                }
            }

            tabPages[idx].OnClosedTab();
            onCloseTab(idx, tabPages[idx].UniqueId, tabPages[idx].PageType);
            panHTabHeader.Controls[idx].Dispose();
            tabPages[idx].Dispose();
            tabPages.RemoveAt(idx);
            panMain.Controls.Clear();
            lsbRemoveAt(idx);

            panHTabHeader.Size = new System.Drawing.Size(panHTabHeader.Size.Width - wtab,
                                                         panHTabHeader.Size.Height);

            lentab -= wtab;

            if (idx == panHTabHeader.Controls.Count) 
                tabIndex = (idx > 0) ? idx - 1 : -1;
            else {
                for (int i = tabIndex; i < panHTabHeader.Controls.Count; ++i)
                    panHTabHeader.Controls[i].Location = new Point(panHTabHeader.Controls[i].Location.X - wtab + 1,
                                                                   panHTabHeader.Controls[i].Location.Y);
            }

            changeIndaxTabInTabPage();

            selectTab(tabIndex);
            tabSizeChangd();
        }

        public bool getUniqueIdByIndex(int idx, out int res) {
            res = -1;
            if (idx < DynamicTabCount) {
                res = tabPages[idx].UniqueId;
                return true;
            }
            return false;
        }

        public void CompareTabPage(object sender, TabCompareTabPageEventArgs e) {
            for (int i = 0; i < tabPages.Count; ++i) {
                if (e.TabPageIndex == i)
                    continue;
                if (tabPages[i].LoadedFile.Equals(e.FilePath)) {
                    e.TabPageIsExist = true;
                    break;
                }
            }
        }

        private void btList_Click(object sender, EventArgs e) {
            if (lsbList.Items.Count == 0)
                return;
            if (lsbList.Visible) {
                lsbList.Visible = false;
                return;
            }
            lsbList.Visible = true;
            lsbList.Select();
        }

        private void lsbList_LostFocus(object sender, EventArgs e) {
            lsbList.Visible = false;
        }

        private void DynamicTabs_SizeChanged(object sender, EventArgs e) {
            tabSizeChangd();
        }

        private void lsbList_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                TabSelectedIndex = lsbList.SelectedIndex;
                panHTabHeader.Controls[TabSelectedIndex].Select();
                tabSizeChangd();
                lsbList.Visible = false;
            }
        }

        private void onCloseTab(int idx, int uniqueId, TabPageType tPageType) {
            TabCloseTabEventArgs e = new TabCloseTabEventArgs();
            e.IndexTab = idx;
            e.UniqueId = uniqueId;
            e.TabPage = tPageType;
            EventHandler<TabCloseTabEventArgs> handler = ClosingTab;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler<TabCloseTabEventArgs> ClosingTab;
        public event EventHandler<TabSelectedTabEventArgs> SelectedTab;
    }

    public enum TabPageType {
        Unregonized,
        SqlEditor,
        TableBuilder,
        DatabaseBuilder,
        GridResult,
        ImpotExport,
        ConsoleSQL,
        UserAccounts
    }
}
