using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Grid.Handler;
using DevExpress.XtraGrid.Views.Base.Handler;
using DevExpress.XtraGrid.Dragging;
using System.ComponentModel;
using DevExpress.Utils.Serializing;
using DevExpress.Utils.Controls;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.LookAndFeel.Helpers;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using System.IO;
using DevExpress.Utils;
using System.Drawing.Imaging;
using DevExpress.Skins;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Views.Layout.Modes;
using DevExpress.XtraEditors.Controls;

namespace Sample
{
    public class CustomGridView : GridView
    {

        QuickHideEdit hideEdit;
        public CustomGridView() : base() { }
        protected internal virtual void SetGridControlAccessMetod(GridControl newControl) { SetGridControl(newControl); }
        protected override string ViewName { get { return "CustomGridView"; } }
        protected override GridColumnCollection CreateColumnCollection() { return new CustomGridColumnCollection(this); }
        protected override GridOptionsCustomization CreateOptionsCustomization() { return new CustomGridOptionsCustomization(); }
        [Description("Provides access to the View's customization options."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.DefaultValue)]
        public new CustomGridOptionsCustomization OptionsCustomization { get { return base.OptionsCustomization as CustomGridOptionsCustomization; } }
        protected virtual internal EmbeddedLookAndFeel GetLookAndFeel() { return ElementsLookAndFeel; }
        public override bool IsFocusedView
        {
            get
            {
                if (hideEdit != null)
                    if (hideEdit.Enabled == true)
                        return true;
                return base.IsFocusedView; ;
            }
        }
        protected virtual internal void ShowColumnCustomizationMenu(Point p)
        {
            if (hideEdit == null) CreateHideEdit();
            LocateHideEdit();
            PopulateHideEdit();
            hideEdit.ShowPopup();
        }
        private void LocateHideEdit()
        {
            hideEdit.MakeEnable();
            hideEdit.Location = ((CustomGridViewInfo)ViewInfo).QuickCustomisationBounds.Location;
        }
        private void CreateHideEdit()
        {
            hideEdit = new QuickHideEdit(this);
            LocateHideEdit();
            GridControl.Controls.Add(hideEdit);
            this.Focus();
        }
        protected internal virtual void AcceptQuickHide()
        {
            foreach (GridColumn col in Columns)
            {
                ColumnProperties cp = hideEdit.Properties.Columns[col.ToString()];
                if (cp == null) continue;
                col.VisibleIndex = cp.VisibleIndex;
                col.Visible = cp.Visible;
            }
        }
        protected virtual void PopulateHideEdit()
        {
            hideEdit.Properties.Columns.Clear();
            if (Columns.Count == 0) return;
            foreach (CustomGridColumn col in Columns)
            {
                if (col.Visible || col.OptionsColumn.ShowInCustomizationForm)
                    hideEdit.Properties.Columns.Add(col.ToString(), col.Visible, col.VisibleIndex, GetColumnHideState(col), GetColumnMoveState(col));
            }
            hideEdit.Properties.Columns.Sort();
        }
        protected virtual bool GetColumnHideState(CustomGridColumn column)
        {
            return column.OptionsColumn.AllowQuickHide && OptionsCustomization.AllowQuickHideColumns;
        }
        protected virtual bool GetColumnMoveState(CustomGridColumn column)
        {
            return column.OptionsColumn.AllowMove && OptionsCustomization.AllowColumnMoving;
        }
    }
    public class CustomGridControl : GridControl
    {
        public CustomGridControl() : base() { }
        protected override void RegisterAvailableViewsCore(InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new CustomGridInfoRegistrator());
        }
        protected override BaseView CreateDefaultView() { return CreateView("CustomGridView"); }
    }
    public class CustomGridInfoRegistrator : GridInfoRegistrator
    {
        public CustomGridInfoRegistrator() : base() { }
        public override string ViewName { get { return "CustomGridView"; } }
        public override BaseViewHandler CreateHandler(BaseView view) { return new CustomGridHandler(view as GridView); }
        public override BaseViewPainter CreatePainter(BaseView view) { return new CustomGridPainter(view as GridView); }
        public override BaseViewInfo CreateViewInfo(BaseView view) { return new CustomGridViewInfo(view as GridView); }
        public override BaseView CreateView(GridControl grid)
        {
            CustomGridView view = new CustomGridView();
            view.SetGridControlAccessMetod(grid);
            return view;
        }
    }
    public class CustomGridViewInfo : GridViewInfo
    {
        public QuickCustomisationIconStatus QuickCustomisationIconStatus;
        static int QuickCustomisationWidth = 10, QuickCustomisationHeight = 11, QuickCustomisationSpacing = 2;
        Rectangle quickCustumisationBounds;
        public CustomGridViewInfo(GridView gridView)
            : base(gridView)
        {
            quickCustumisationBounds = Rectangle.Empty;
            QuickCustomisationIconStatus = QuickCustomisationIconStatus.Hidden;
        }
        public virtual Rectangle QuickCustomisationBounds
        {
            get
            {
                Rectangle rec = new Rectangle();
                rec.Location = new Point(ColumnsInfo[0].Bounds.Right - QuickCustomisationWidth - QuickCustomisationSpacing,
                    ColumnsInfo[0].Bounds.Top + QuickCustomisationSpacing);
                rec.Size = new Size(QuickCustomisationWidth, QuickCustomisationHeight);
                return rec;
            }
        }
        public virtual bool IsQuickCustomisationButton(Point p) { return QuickCustomisationBounds.Contains(p); }
        public bool AllowQuickCustomisation { get { return ((CustomGridView)View).OptionsCustomization.AllowQuickCustomisation; } }

        public virtual QuickCustomizationIcon QuickCustomisationIcon { get { return ((CustomGridView)View).OptionsCustomization.QuickCustomizationIcons; } }
    }
    public class CustomGridPainter : GridPainter
    {
        public CustomGridPainter(GridView view) : base(view) { }
        protected override void DrawIndicatorCore(GridViewDrawArgs e, IndicatorObjectInfoArgs info, int rowHandle, IndicatorKind kind)
        {
            base.DrawIndicatorCore(e, info, rowHandle, kind);
            DrawQuickCustomisationIcon(e, info, kind);
        }
        protected virtual void DrawQuickCustomisationIcon(GridViewDrawArgs e, IndicatorObjectInfoArgs info, IndicatorKind kind)
        {
            if (kind == DevExpress.Utils.Drawing.IndicatorKind.Header && ((CustomGridViewInfo)e.ViewInfo).QuickCustomisationIconStatus != QuickCustomisationIconStatus.Hidden)
                DrawQuickCustomisationIconCore(e, info, ((CustomGridViewInfo)e.ViewInfo).QuickCustomisationIcon, ((CustomGridViewInfo)e.ViewInfo).QuickCustomisationBounds, ((CustomGridViewInfo)e.ViewInfo).QuickCustomisationIconStatus);
        }
        protected virtual void DrawQuickCustomisationIconCore(GridViewDrawArgs e, IndicatorObjectInfoArgs info, QuickCustomizationIcon icon, Rectangle bounds, QuickCustomisationIconStatus status)
        {
            Rectangle patchedRec = new Rectangle(bounds.X + 1, bounds.Y, bounds.Width - 1, bounds.Height);
            GridColumnInfoArgs args = new GridColumnInfoArgs(e.Cache, e.ViewInfo.ColumnsInfo[0].Column);
            args.Cache = e.Cache;
            args.Bounds = patchedRec;
            ((HeaderObjectInfoArgs)args).HeaderPosition = HeaderPositionKind.Center;
            if (status == QuickCustomisationIconStatus.Hot)
                ((HeaderObjectInfoArgs)args).State = ObjectState.Hot;
            ElementsPainter.Column.DrawObject(args);

            if (icon.Image != null)
            {
                Rectangle rec = new Rectangle();
                rec.Location = new Point(bounds.Left + 1, bounds.Top + 1);
                rec.Size = new Size(bounds.Width - 2, bounds.Height - 2);
                ImageAttributes attr = new ImageAttributes();
                attr.SetColorKey(icon.TransperentColor, icon.TransperentColor);
                e.Graphics.DrawImage(icon.Image, rec, 0, 0, icon.Image.Width, icon.Image.Height, GraphicsUnit.Pixel, attr);
            }
        }
    }
    public class CustomGridHandler : GridHandler
    {
        public CustomGridHandler(GridView gridView) : base(gridView) { }
        protected override GridDragManager CreateDragManager() { return new CustomGridDragManager(View); }
        public override void DoClickAction(DevExpress.XtraGrid.Views.Base.ViewInfo.BaseHitInfo hitInfo)
        {
            base.DoClickAction(hitInfo);
            GridHitInfo hit = hitInfo as GridHitInfo;
            if (hit.HitTest == GridHitTest.ColumnButton && ((CustomGridView)View).OptionsCustomization.AllowQuickCustomisation)
                if (((CustomGridViewInfo)ViewInfo).IsQuickCustomisationButton(hitInfo.HitPoint))
                    ((CustomGridView)View).ShowColumnCustomizationMenu(hit.HitPoint);

        }
        protected override bool OnMouseMove(MouseEventArgs ev)
        {
            DXMouseEventArgs e = DXMouseEventArgs.GetMouseArgs(ev);
            Point p = new Point(e.X, e.Y);
            UpdateQuickCustumisationIconState(p);
            return base.OnMouseMove(ev);
        }
        protected virtual void UpdateQuickCustumisationIconState(Point p)
        {
            CustomGridViewInfo vi = ViewInfo as CustomGridViewInfo;
            if (!vi.AllowQuickCustomisation) return;
            GridHitInfo hi = ViewInfo.CalcHitInfo(p);
            if (hi.HitTest == GridHitTest.ColumnButton)
            {
                if (vi.IsQuickCustomisationButton(p))
                {
                    if (vi.QuickCustomisationIconStatus != QuickCustomisationIconStatus.Hot)
                    {
                        vi.QuickCustomisationIconStatus = QuickCustomisationIconStatus.Hot;
                        ViewInfo.View.Invalidate();
                    }
                    return;
                }
                if (vi.QuickCustomisationIconStatus != QuickCustomisationIconStatus.Enabled)
                {
                    vi.QuickCustomisationIconStatus = QuickCustomisationIconStatus.Enabled;
                    ViewInfo.View.Invalidate();
                }
            }
            else
                if (vi.QuickCustomisationIconStatus != QuickCustomisationIconStatus.Hidden)
                {
                    vi.QuickCustomisationIconStatus = QuickCustomisationIconStatus.Hidden;
                    ViewInfo.View.Invalidate();
                }
        }
    }
    public class CustomGridDragManager : GridDragManager
    {
        public CustomGridDragManager(GridView view) : base(view) { }
        protected override PositionInfo CalcColumnDrag(GridHitInfo hit, GridColumn column)
        {
            PositionInfo patchedPI = new PositionInfo();
            patchedPI = base.CalcColumnDrag(hit, column);
            if (patchedPI.Index == HideElementPosition && patchedPI.Valid)
            {
                CustomGridColumn col = column as CustomGridColumn;
                if (col != null)
                    if (!col.OptionsColumn.AllowQuickHide)
                    {
                        patchedPI = new PositionInfo();
                        patchedPI.Valid = false;
                    }
            }
            return patchedPI;
        }
    }
    public class CustomGridColumn : GridColumn
    {
        public CustomGridColumn() : base() { }
        protected override OptionsColumn CreateOptionsColumn() { return new CustomOptionsColum(); }
        [Description("Provides access to the column's options."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.DefaultValue)]
        public new CustomOptionsColum OptionsColumn
        {
            get { return (CustomOptionsColum)base.OptionsColumn; }
        }
    }
    public class CustomGridColumnCollection : GridColumnCollection
    {
        public CustomGridColumnCollection(ColumnView view) : base(view) { }
        protected override GridColumn CreateColumn() { return new CustomGridColumn(); }
    }
    public class CustomOptionsColum : OptionsColumn
    {
        bool allowQuickHide;
        public CustomOptionsColum()
            : base()
        {
            allowQuickHide = true;
        }
        [Description("Gets or sets whether the column allow quick hide."), DefaultValue(true), XtraSerializableProperty()]
        public bool AllowQuickHide
        {
            set
            {
                if (allowQuickHide == value) return;
                allowQuickHide = value;
            }
            get { return allowQuickHide; }
        }
    }
    public class CustomGridOptionsCustomization : GridOptionsCustomization
    {
        bool allowQuickCustomisation;
        QuickCustomizationIcon quickCustomizationIcon;
        public CustomGridOptionsCustomization()
            : base()
        {
            this.allowQuickCustomisation = true;
            quickCustomizationIcon = new QuickCustomizationIcon();
        }
        [Description("Gets or sets a value which specifies whether end-users can use quick customisation drop dawn."), DefaultValue(true), XtraSerializableProperty()]
        public virtual bool AllowQuickCustomisation
        {
            get { return allowQuickCustomisation; }
            set
            {
                if (allowQuickCustomisation == value) return;
                bool prevValue = allowQuickCustomisation;
                allowQuickCustomisation = value;
                OnChanged(new BaseOptionChangedEventArgs("AllowQuickCustomisation", prevValue, allowQuickCustomisation));
            }
        }
        [Description("Allow chose different icon for QuickCustomizationButton."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        XtraSerializableProperty()]
        public virtual QuickCustomizationIcon QuickCustomizationIcons { get { return quickCustomizationIcon; } }
    }
    public class QuickCustomizationIcon : ViewBaseOptions
    {
        Image image;
        Color transperentColor;
        public QuickCustomizationIcon()
        {
            transperentColor = Color.Empty;
        }
        [Description("Allow to chose image to show on QuickCustomisationButton"), XtraSerializableProperty()]
        public Image Image { set { if (image != value) image = value; } get { return image; } }
        [Description("Allow to chose transperent color for QuickCustumisationImage"), XtraSerializableProperty()]
        public Color TransperentColor { get { return transperentColor; } set { if (transperentColor != value) transperentColor = value; } }
    }
    public enum QuickCustomisationIconStatus { Hidden, Enabled, Hot };
}

