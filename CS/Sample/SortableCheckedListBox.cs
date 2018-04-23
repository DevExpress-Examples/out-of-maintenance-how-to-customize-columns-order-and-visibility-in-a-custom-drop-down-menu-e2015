using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;

namespace Sample
{
    class CustomCheckedlistBox : CheckedListBoxControl
    {
        int dragSourceIndex, dragTargetIndex;
        bool isDraging;
        bool[] allowMove;

        Rectangle dragBeginRect;
        public CustomCheckedlistBox()
            : base()
        {
            SelectionMode = SelectionMode.None;
            TabStop = false;
            CheckOnClick = true;
            isDraging = false;            
        }
        protected virtual internal void SetAllowMoving(int index, bool value)
        {            
            allowMove[index] = value;
        }
        protected virtual internal void CreateAllowMovingArray() { allowMove = new bool[Items.Count]; }
        protected virtual bool IsDraging { get { return isDraging; } set { if (isDraging != value) isDraging = value; } }
        protected override BaseStyleControlViewInfo CreateViewInfo() { return new CustomCheckedListBoxViewInfo(this); }
        protected override BaseControlPainter CreatePainter() { return new CustomPainterCheckedListBox(); }
        protected new virtual CustomCheckedListBoxViewInfo ViewInfo { get { return base.ViewInfo as CustomCheckedListBoxViewInfo; } }
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                int index = this.IndexFromPoint(e.Location);
                if (index >= 0 && index < Items.Count && !allowMove[index]) return;
                IsDraging = false;
                dragSourceIndex = index;
                dragTargetIndex = dragSourceIndex;
                if (dragSourceIndex != -1)
                {
                    Size dragSize = SystemInformation.DragSize;
                    dragBeginRect = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                    dragBeginRect = Rectangle.Empty;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!IsDraging || dragBeginRect == Rectangle.Empty) base.OnMouseUp(e);
            if (dragBeginRect == Rectangle.Empty) return;
            if (dragSourceIndex != -1 && dragTargetIndex != dragSourceIndex)
                ChangeItemsPositionCore(dragSourceIndex, dragTargetIndex);

            dragBeginRect = Rectangle.Empty;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                if (dragBeginRect != Rectangle.Empty && !dragBeginRect.Contains(e.X, e.Y))
                {
                    IsDraging = true;
                    dragTargetIndex = this.IndexFromPoint(e.Location);
                    if (dragTargetIndex == -1)
                        if (e.Y < this.ViewInfo.GetItemRectangle(0).Bottom) dragTargetIndex = 0;
                    CheckedListBoxViewInfo.CheckedItemInfo info = ViewInfo.GetItemByIndex(dragSourceIndex);
                    if (info != null) info.PaintAppearance.ForeColor = Color.Red;
                    ViewInfo.MarkItem(dragTargetIndex, dragSourceIndex);
                }
        }
        protected virtual void ChangeItemsPositionCore(int source, int target)
        {
            CorrectAllowMove(source, target);
            if (target == -1)
            {
                Items.Add(Items[source]);
            }
            else
            {
                Items.Insert(target, Items[source]);
                if (source > target) source++;
            }
            Items.RemoveAt(source);         
        }

        protected virtual void CorrectAllowMove(int source, int target)
        {
            bool b = allowMove[source];
            if (target == -1) target = Items.Count-1;
            if (target > source)
                for (int i = source; i < target; i++) allowMove[i] = allowMove[i+1];
            else
                for (int i = source; i > target; i--) allowMove[i] = allowMove[i-1];
            allowMove[target] = b;
        }
    }
    public class CustomCheckedListBoxViewInfo : CheckedListBoxViewInfo
    {
        Color dragDropLineColor;
        public CustomCheckedListBoxViewInfo(CheckedListBoxControl listBox) : base(listBox) { }
        protected override ItemInfo CreateItemInfo(Rectangle bounds, object item, string text, int index)
        {
            CheckedItemInfo info = base.CreateItemInfo(bounds, item, text, index) as CheckedItemInfo;
            CustomCheckedItemInfo patchedInfo = new CustomCheckedItemInfo(info);
            return patchedInfo;
        }
        public virtual Color DragDropLineColor { get { return dragDropLineColor; } set { if (dragDropLineColor != value) dragDropLineColor = value; } }
        protected internal virtual void UnderlineItem(int index)
        {
            CustomCheckedItemInfo info = base.GetItemByIndex(index) as CustomCheckedItemInfo;
            if (info != null)
                info.IsUnderLine = true;
        }
        protected internal virtual void OverlineItem(int index)
        {
            CustomCheckedItemInfo info = base.GetItemByIndex(index) as CustomCheckedItemInfo;
            if (info != null)
                info.IsOverLine = true;
        }
        protected virtual internal int ItemCountAccessMethod() { return ItemCount; }
        protected internal virtual void DropLine()
        {
            foreach (CustomCheckedItemInfo info in VisibleItems)                
                info.DropLine();
        }
        protected internal virtual void MarkItem(int targetIndex, int sourceIndex)
        {
            DropLine();
            dragDropLineColor = Color.Red;
            if ((targetIndex == sourceIndex) || (targetIndex == sourceIndex + 1) ||
                (sourceIndex == ItemCount  - 1 && targetIndex == -1)) dragDropLineColor = Color.LightGray;
            if (targetIndex == -1)
                UnderlineItem( ItemCount - 1);
            else
                OverlineItem(targetIndex);
            if (targetIndex > 0) 
                UnderlineItem(targetIndex - 1);
            OwnerControl.Invalidate();
        }

        public class CustomCheckedItemInfo : CheckedItemInfo
        {
            bool isUnderLine, isOverLine;
            public CustomCheckedItemInfo(Rectangle rect, object item, string text, int index, CheckState checkState, bool enabled)
                : base(rect, item, text, index, checkState, enabled)
            {
                DropLine();
            }
            public CustomCheckedItemInfo(CheckedItemInfo info)
                : this(info.Bounds, info.Item, info.Text, info.Index, info.CheckArgs.CheckState, info.Enabled)
            {
                this.CheckArgs.Assign(info.CheckArgs);
                this.TextRect = info.TextRect;
            }
            protected virtual internal bool IsUnderLine { get { return isUnderLine; } set { isUnderLine = value; } }
            protected virtual internal bool IsOverLine { get { return isOverLine; } set { isOverLine = value; } }            
            protected virtual internal void DropLine()
            {
                isUnderLine = false;
                isOverLine = false;
            }
        }
    }
    public class CustomPainterCheckedListBox : PainterCheckedListBox
    {
        const int lineWidth = 1;
        protected override void DrawItemCore(ControlGraphicsInfoArgs info, BaseListBoxViewInfo.ItemInfo itemInfo, ListBoxDrawItemEventArgs e)
        {
            base.DrawItemCore(info, itemInfo, e);
            Sample.CustomCheckedListBoxViewInfo.CustomCheckedItemInfo customInfo =
                itemInfo as Sample.CustomCheckedListBoxViewInfo.CustomCheckedItemInfo;
            if (customInfo == null) return;
            Rectangle rec = new Rectangle(itemInfo.Bounds.Location, new Size(itemInfo.Bounds.Width, lineWidth));
            Color lineColor = ((CustomCheckedListBoxViewInfo)info.ViewInfo).DragDropLineColor;
            if (customInfo.IsOverLine)
            {
                if (customInfo.Index == 0) rec.Height++;
                info.Graphics.FillRectangle(info.Cache.GetSolidBrush(lineColor), rec);
            }
            if (customInfo.IsUnderLine)
            {
                rec.Offset(0, itemInfo.Bounds.Height - lineWidth);
                if (customInfo.Index == ((CustomCheckedListBoxViewInfo)info.ViewInfo).ItemCountAccessMethod() - 1) rec.Height++;
                info.Graphics.FillRectangle(info.Cache.GetSolidBrush(lineColor), rec);
            }
        }
    }
}
