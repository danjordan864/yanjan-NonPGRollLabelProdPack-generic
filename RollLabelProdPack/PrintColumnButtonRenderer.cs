using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using log4net;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Collections;
using System.Windows.Forms.VisualStyles;

namespace RollLabelProdPack
{
    public class PrintColumnButtonRenderer : ColumnButtonRenderer
    {
        private ILog _log;
        private Image _printButtonImage;

        public PrintColumnButtonRenderer(Image printButtonImage)
        {
            _log = LogManager.GetLogger(this.GetType());
            if (_log.IsDebugEnabled)
                _log.Debug("In constructor for PrintColumnButtonRenderer");
            _printButtonImage = printButtonImage;
        }

        protected override Rectangle AlignRectangle(Rectangle outer, Rectangle inner)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In AlignRectangle");
            return base.AlignRectangle(outer, inner);
        }

        public override Rectangle ApplyCellPadding(Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In ApplyCellPadding");
            return base.ApplyCellPadding(r);
        }

        protected override Rectangle CalculateAlignedRectangle(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateAlignedRectangle");
            return base.CalculateAlignedRectangle(g, r);
        }

        protected override Size CalculateCheckBoxSize(Graphics g)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateCheckBoxSize");
            return base.CalculateCheckBoxSize(g);
        }

        protected override Size CalculateContentSize(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateContentSize");
            return base.CalculateContentSize(g, r);
        }

        protected override int CalculateImageHeight(Graphics g, object imageSelector)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateImageHeight");
            return base.CalculateImageHeight(g, imageSelector);
        }

        protected override Size CalculateImageSize(Graphics g, object imageSelector)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateImageSize");
            return base.CalculateImageSize(g, imageSelector);
        }

        protected override int CalculateImageWidth(Graphics g, object imageSelector)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateImageWidth");
            return base.CalculateImageWidth(g, imageSelector);
        }

        protected override Size CalculatePrimaryCheckBoxSize(Graphics g)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculatePrimaryCheckBoxSize");
            return base.CalculatePrimaryCheckBoxSize(g);
        }

        protected override Size CalculateTextSize(Graphics g, string txt, int width)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateTextSize");
            return base.CalculateTextSize(g, txt, width);
        }

        protected override int CalculateTextWidth(Graphics g, string txt, int width)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CalculateTextWidth");
            return base.CalculateTextWidth(g, txt, width);
        }

        public override void ConfigureItem(DrawListViewItemEventArgs e, Rectangle itemBounds, object model)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In ConfigureItem");
            base.ConfigureItem(e, itemBounds, model);
        }

        public override void ConfigureSubItem(DrawListViewSubItemEventArgs e, Rectangle cellBounds, object model)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In ConfigureSubItem");
            base.ConfigureSubItem(e, cellBounds, model);
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In CreateObjRef");
            return base.CreateObjRef(requestedType);
        }

        protected override void DrawAlignedImage(Graphics g, Rectangle r, Image image)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawAlignedImage");
            base.DrawAlignedImage(g, r, image);
        }

        protected override void DrawAlignedImageAndText(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawAlignedImageAndText");
            base.DrawAlignedImageAndText(g, r);
        }

        protected override void DrawBackground(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawBackground");
            base.DrawBackground(g, r);
        }

        protected override int DrawCheckBox(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawCheckBox");
            return base.DrawCheckBox(g, r);
        }

        protected override void DrawImageAndText(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawImageAndText");
            g.DrawImage(_printButtonImage, r);
            //base.DrawImageAndText(g, r);
        }

        protected override int DrawImages(Graphics g, Rectangle r, ICollection imageSelectors)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawImages");
            return base.DrawImages(g, r, imageSelectors);
        }

        public override void DrawText(Graphics g, Rectangle r, string txt)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawText");
            base.DrawText(g, r, txt);
        }

        protected override void DrawTextGdi(Graphics g, Rectangle r, string txt)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawTextGdi");
            base.DrawTextGdi(g, r, txt);
        }

        protected override void DrawTextGdiPlus(Graphics g, Rectangle r, string txt)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In DrawTextGdiPlus");
            base.DrawTextGdiPlus(g, r, txt);
        }

        public override Color GetBackgroundColor()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetBackgroundColor");
            return base.GetBackgroundColor();
        }

        protected override CheckBoxState GetCheckBoxState(CheckState checkState)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetCheckBoxState");
            return base.GetCheckBoxState(checkState);
        }

        public override Rectangle GetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetEditRectangle");
            return base.GetEditRectangle(g, cellBounds, item, subItemIndex, preferredSize);
        }

        public override Color GetForegroundColor()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetForegroundColor");
            return base.GetForegroundColor();
        }

        protected override Image GetImage()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetImage");
            return base.GetImage();
        }

        protected override Image GetImage(object imageSelector)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetImage (with imageSelector)");
            return base.GetImage(imageSelector);
        }

        protected override object GetImageSelector()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetImageSelector");
            return base.GetImageSelector();
        }

        public override Color GetSelectedBackgroundColor()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetSelectedBackgroundColor");
            return base.GetSelectedBackgroundColor();
        }

        public override Color GetSelectedForegroundColor()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetSelectedForegroundColor");
            return base.GetSelectedForegroundColor();
        }

        protected override object GetService(Type service)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetService");
            return base.GetService(service);
        }

        protected override string GetText()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetText");
            return base.GetText();
        }

        [Obsolete]
        protected override Color GetTextBackgroundColor()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In GetTextBackgroundColor");
            return base.GetTextBackgroundColor();
        }

        protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In HandleGetEditRectangle");
            return base.HandleGetEditRectangle(g, cellBounds, item, subItemIndex, preferredSize);
        }

        protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In HandleHitTest");
            base.HandleHitTest(g, hti, x, y);
        }

        public override void HitTest(OlvListViewHitTestInfo hti, int x, int y)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In HitTest");
            base.HitTest(hti, x, y);
        }

        public override bool OptionalRender(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In OptionalRender");
            return base.OptionalRender(g, r);
        }

        public override void Render(Graphics g, Rectangle r)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In Render");
            base.Render(g, r);
        }

        public override bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, object model)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In RenderItem");
            return base.RenderItem(e, g, itemBounds, model);
        }

        public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object model)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In RenderSubItem");
            return base.RenderSubItem(e, g, cellBounds, model);
        }

        protected override Rectangle StandardGetEditRectangle(Graphics g, Rectangle cellBounds, Size preferredSize)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In StandardGetEditRectangle");
            return base.StandardGetEditRectangle(g, cellBounds, preferredSize);
        }

        protected override void StandardHitTest(Graphics g, OlvListViewHitTestInfo hti, Rectangle bounds, int x, int y)
        {
            if (_log.IsDebugEnabled)
                _log.Debug("In StandardHitTest");
            base.StandardHitTest(g, hti, bounds, x, y);
        }

        protected override int DrawImage(Graphics g, Rectangle r, object imageSelector)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug($"About to draw image for {imageSelector}");
            }
            g.DrawImage(_printButtonImage, r);
            return _printButtonImage.Width;
        }

    }
}
