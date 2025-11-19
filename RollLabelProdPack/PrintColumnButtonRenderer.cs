using BrightIdeasSoftware;
using System;
using System.Drawing;
using log4net;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Collections;
using System.Windows.Forms.VisualStyles;

namespace RollLabelProdPack
{
    /// <summary>
    /// ColumnButtonRenderer class for a print column button.
    /// </summary>
    public class PrintColumnButtonRenderer : ColumnButtonRenderer
    {
        private ILog _log;
        private Image _printButtonImage;

        /// <summary>
        /// Initializes a new instance of PrintColumnButtonRenderer
        /// with the supplied print button image.
        /// </summary>
        /// <param name="printButtonImage">Print button image to use</param>
        public PrintColumnButtonRenderer(Image printButtonImage)
        {
            _log = LogManager.GetLogger(this.GetType());
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In constructor for PrintColumnButtonRenderer");
            _printButtonImage = printButtonImage;
        }

        /// <summary>
        /// Overrides the AlignRectangle method to align an inner rectangle within an outer rectangle.
        /// </summary>
        /// <param name="outer">The outer rectangle.</param>
        /// <param name="inner">The inner rectangle.</param>
        /// <returns>The aligned rectangle.</returns>
        protected override Rectangle AlignRectangle(Rectangle outer, Rectangle inner)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In AlignRectangle");

            // Call the base class implementation to perform the alignment
            return base.AlignRectangle(outer, inner);
        }


        /// <summary>
        /// Overrides the ApplyCellPadding method to apply cell padding to a rectangle.
        /// </summary>
        /// <param name="r">The rectangle to apply cell padding to.</param>
        /// <returns>The modified rectangle with applied cell padding.</returns>
        public override Rectangle ApplyCellPadding(Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In ApplyCellPadding");

            // Call the base class implementation to apply the cell padding
            return base.ApplyCellPadding(r);
        }


        /// <summary>
        /// Overrides the CalculateAlignedRectangle method to calculate an aligned rectangle based on the graphics context and input rectangle.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="r">The input rectangle.</param>
        /// <returns>The calculated aligned rectangle.</returns>
        protected override Rectangle CalculateAlignedRectangle(Graphics g, Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateAlignedRectangle");

            // Call the base class implementation to calculate the aligned rectangle
            return base.CalculateAlignedRectangle(g, r);
        }


        /// <summary>
        /// Overrides the CalculateCheckBoxSize method to calculate the size of the checkbox based on the graphics context.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <returns>The calculated size of the checkbox.</returns>
        protected override Size CalculateCheckBoxSize(Graphics g)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateCheckBoxSize");

            // Call the base class implementation to calculate the checkbox size
            return base.CalculateCheckBoxSize(g);
        }


        /// <summary>
        /// Overrides the CalculateContentSize method to calculate the size of the content based on the graphics context and the specified rectangle.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="r">The rectangle to calculate the content size within.</param>
        /// <returns>The calculated size of the content.</returns>
        protected override Size CalculateContentSize(Graphics g, Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateContentSize");

            // Call the base class implementation to calculate the content size
            return base.CalculateContentSize(g, r);
        }


        /// <summary>
        /// Overrides the CalculateImageHeight method to calculate the height of the image based on the graphics context and the specified image selector.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="imageSelector">The image selector object.</param>
        /// <returns>The calculated height of the image.</returns>
        protected override int CalculateImageHeight(Graphics g, object imageSelector)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateImageHeight");

            // Call the base class implementation to calculate the image height
            return base.CalculateImageHeight(g, imageSelector);
        }


        /// <summary>
        /// Overrides the CalculateImageSize method to calculate the size of the image based on the graphics context and the specified image selector.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="imageSelector">The image selector object.</param>
        /// <returns>The calculated size of the image.</returns>
        protected override Size CalculateImageSize(Graphics g, object imageSelector)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateImageSize");

            // Call the base class implementation to calculate the image size
            return base.CalculateImageSize(g, imageSelector);
        }


        /// <summary>
        /// Overrides the CalculateImageWidth method to calculate the width of the image based on the graphics context and the specified image selector.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="imageSelector">The image selector object.</param>
        /// <returns>The calculated width of the image.</returns>
        protected override int CalculateImageWidth(Graphics g, object imageSelector)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateImageWidth");

            // Call the base class implementation to calculate the image width
            return base.CalculateImageWidth(g, imageSelector);
        }


        /// <summary>
        /// Overrides the CalculatePrimaryCheckBoxSize method to calculate the size of the primary check box based on the graphics context.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <returns>The calculated size of the primary check box.</returns>
        protected override Size CalculatePrimaryCheckBoxSize(Graphics g)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculatePrimaryCheckBoxSize");

            // Call the base class implementation to calculate the primary check box size
            return base.CalculatePrimaryCheckBoxSize(g);
        }


        /// <summary>
        /// Overrides the CalculateTextSize method to calculate the size of the text based on the graphics context, the text string, and the specified width.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="txt">The text string.</param>
        /// <param name="width">The specified width.</param>
        /// <returns>The calculated size of the text.</returns>
        protected override Size CalculateTextSize(Graphics g, string txt, int width)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateTextSize");

            // Call the base class implementation to calculate the text size
            return base.CalculateTextSize(g, txt, width);
        }


        /// <summary>
        /// Overrides the CalculateTextWidth method to calculate the width of the text based on the graphics context, the text string, and the specified width.
        /// </summary>
        /// <param name="g">The graphics context.</param>
        /// <param name="txt">The text string.</param>
        /// <param name="width">The specified width.</param>
        /// <returns>The calculated width of the text.</returns>
        protected override int CalculateTextWidth(Graphics g, string txt, int width)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CalculateTextWidth");

            // Call the base class implementation to calculate the text width
            return base.CalculateTextWidth(g, txt, width);
        }


        /// <summary>
        /// Overrides the ConfigureItem method to configure the appearance of a list view item.
        /// </summary>
        /// <param name="e">The DrawListViewItemEventArgs containing the event data.</param>
        /// <param name="itemBounds">The bounding rectangle of the list view item.</param>
        /// <param name="model">The model object associated with the list view item.</param>
        public override void ConfigureItem(DrawListViewItemEventArgs e, Rectangle itemBounds, object model)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In ConfigureItem");

            // Call the base class implementation to configure the appearance of the list view item
            base.ConfigureItem(e, itemBounds, model);
        }


        /// <summary>
        /// Overrides the ConfigureSubItem method to configure the appearance of a list view subitem.
        /// </summary>
        /// <param name="e">The DrawListViewSubItemEventArgs containing the event data.</param>
        /// <param name="cellBounds">The bounding rectangle of the list view subitem.</param>
        /// <param name="model">The model object associated with the list view subitem.</param>
        public override void ConfigureSubItem(DrawListViewSubItemEventArgs e, Rectangle cellBounds, object model)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In ConfigureSubItem");

            // Call the base class implementation to configure the appearance of the list view subitem
            base.ConfigureSubItem(e, cellBounds, model);
        }


        /// <summary>
        /// Overrides the CreateObjRef method to create a marshaled object reference (ObjRef) that represents the current object.
        /// </summary>
        /// <param name="requestedType">The <see cref="Type"/> of the object that the new ObjRef will reference.</param>
        /// <returns>An ObjRef that represents the current object.</returns>
        public override ObjRef CreateObjRef(Type requestedType)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In CreateObjRef");

            // Call the base class implementation to create the ObjRef
            return base.CreateObjRef(requestedType);
        }


        /// <summary>
        /// Overrides the DrawAlignedImage method to draw an aligned image within the specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the image.</param>
        /// <param name="image">The <see cref="Image"/> to be drawn.</param>
        protected override void DrawAlignedImage(Graphics g, Rectangle r, Image image)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawAlignedImage");

            // Call the base class implementation to draw the aligned image
            base.DrawAlignedImage(g, r, image);
        }


        /// <summary>
        /// Overrides the DrawAlignedImageAndText method to draw both an aligned image and text within the specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the image and text.</param>
        protected override void DrawAlignedImageAndText(Graphics g, Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawAlignedImageAndText");

            // Call the base class implementation to draw the aligned image and text
            base.DrawAlignedImageAndText(g, r);
        }


        /// <summary>
        /// Overrides the DrawBackground method to draw the background of the object.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the background.</param>
        protected override void DrawBackground(Graphics g, Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawBackground");

            // Call the base class implementation to draw the background
            base.DrawBackground(g, r);
        }


        /// <summary>
        /// Overrides the DrawCheckBox method to draw the checkbox element of the object.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the checkbox.</param>
        /// <returns>The width of the checkbox element.</returns>
        protected override int DrawCheckBox(Graphics g, Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawCheckBox");

            // Call the base class implementation to draw the checkbox
            return base.DrawCheckBox(g, r);
        }


        /// <summary>
        /// Overrides the DrawImageAndText method to draw both the image and text of the object.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the image and text.</param>
        protected override void DrawImageAndText(Graphics g, Rectangle r)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawImageAndText");

            // Draw the image using the specified Graphics object and Rectangle
            g.DrawImage(_printButtonImage, r);

            // Commented out the base class implementation to only draw the image
            //base.DrawImageAndText(g, r);
        }


        /// <summary>
        /// Overrides the DrawImages method to draw multiple images within a specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the images.</param>
        /// <param name="imageSelectors">The collection of image selectors.</param>
        /// <returns>The total width of the drawn images.</returns>
        protected override int DrawImages(Graphics g, Rectangle r, ICollection imageSelectors)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawImages");

            // Call the base class implementation to draw the images
            return base.DrawImages(g, r, imageSelectors);
        }


        /// <summary>
        /// Overrides the DrawText method to draw text within a specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the text.</param>
        /// <param name="txt">The text to be drawn.</param>
        public override void DrawText(Graphics g, Rectangle r, string txt)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawText");

            // Call the base class implementation to draw the text
            base.DrawText(g, r, txt);
        }


        /// <summary>
        /// Overrides the DrawTextGdi method to draw text using GDI within a specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the text.</param>
        /// <param name="txt">The text to be drawn.</param>
        protected override void DrawTextGdi(Graphics g, Rectangle r, string txt)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawTextGdi");

            // Call the base class implementation to draw the text using GDI
            base.DrawTextGdi(g, r, txt);
        }


        /// <summary>
        /// Overrides the DrawTextGdiPlus method to draw text using GDI+ within a specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="r">The <see cref="Rectangle"/> in which to draw the text.</param>
        /// <param name="txt">The text to be drawn.</param>
        protected override void DrawTextGdiPlus(Graphics g, Rectangle r, string txt)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In DrawTextGdiPlus");

            // Call the base class implementation to draw the text using GDI+
            base.DrawTextGdiPlus(g, r, txt);
        }


        /// <summary>
        /// Overrides the GetBackgroundColor method to determine the background color of the object.
        /// </summary>
        /// <returns>The background color of the object.</returns>
        public override Color GetBackgroundColor()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetBackgroundColor");

            // Call the base class implementation to retrieve the background color
            return base.GetBackgroundColor();
        }


        /// <summary>
        /// Overrides the GetCheckBoxState method to determine the visual state of a checkbox.
        /// </summary>
        /// <param name="checkState">The check state of the checkbox.</param>
        /// <returns>The visual state of the checkbox.</returns>
        protected override CheckBoxState GetCheckBoxState(CheckState checkState)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetCheckBoxState");

            // Call the base class implementation to determine the visual state of the checkbox
            return base.GetCheckBoxState(checkState);
        }


        /// <summary>
        /// Overrides the GetEditRectangle method to determine the editing rectangle for a subitem.
        /// </summary>
        /// <param name="g">The Graphics object used for drawing.</param>
        /// <param name="cellBounds">The bounding rectangle of the cell.</param>
        /// <param name="item">The OLVListItem that contains the subitem being edited.</param>
        /// <param name="subItemIndex">The index of the subitem being edited.</param>
        /// <param name="preferredSize">The preferred size of the editor control.</param>
        /// <returns>The editing rectangle for the subitem.</returns>
        public override Rectangle GetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetEditRectangle");

            // Call the base class implementation to get the default editing rectangle
            return base.GetEditRectangle(g, cellBounds, item, subItemIndex, preferredSize);
        }


        /// <summary>
        /// Overrides the GetForegroundColor method to determine the foreground color of the item.
        /// </summary>
        /// <returns>The foreground color of the item.</returns>
        public override Color GetForegroundColor()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetForegroundColor");

            // Call the base class implementation to get the default foreground color
            return base.GetForegroundColor();
        }


        /// <summary>
        /// Overrides the GetImage method to retrieve the image associated with the item.
        /// </summary>
        /// <returns>The image associated with the item.</returns>
        protected override Image GetImage()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetImage");

            // Call the base class implementation to get the default image
            return base.GetImage();
        }


        /// <summary>
        /// Overrides the GetImage method to retrieve the image associated with a specific image selector.
        /// </summary>
        /// <param name="imageSelector">The image selector for which to retrieve the image.</param>
        /// <returns>The image associated with the specified image selector.</returns>
        protected override Image GetImage(object imageSelector)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetImage (with imageSelector)");

            // Call the base class implementation to get the image based on the image selector
            return base.GetImage(imageSelector);
        }


        /// <summary>
        /// Overrides the GetImageSelector method to retrieve the image selector associated with the item being drawn.
        /// </summary>
        /// <returns>The image selector associated with the item.</returns>
        protected override object GetImageSelector()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetImageSelector");

            // Call the base class implementation to get the image selector
            return base.GetImageSelector();
        }


        /// <summary>
        /// Overrides the GetSelectedBackgroundColor method to retrieve the background color for a selected item.
        /// </summary>
        /// <returns>The background color for a selected item.</returns>
        public override Color GetSelectedBackgroundColor()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetSelectedBackgroundColor");

            // Call the base class implementation to get the selected background color
            return base.GetSelectedBackgroundColor();
        }


        /// <summary>
        /// Overrides the GetSelectedForegroundColor method to retrieve the foreground color for a selected item.
        /// </summary>
        /// <returns>The foreground color for a selected item.</returns>
        public override Color GetSelectedForegroundColor()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetSelectedForegroundColor");

            // Call the base class implementation to get the selected foreground color
            return base.GetSelectedForegroundColor();
        }


        /// <summary>
        /// Overrides the GetService method to retrieve a service object of the specified type.
        /// </summary>
        /// <param name="service">The type of the requested service object.</param>
        /// <returns>A service object of the specified type, or null if the service is not available.</returns>
        protected override object GetService(Type service)
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetService");

            // Call the base class implementation to retrieve the service object
            return base.GetService(service);
        }


        /// <summary>
        /// Overrides the GetText method to retrieve the text to be displayed in the cell.
        /// </summary>
        /// <returns>The text to be displayed in the cell.</returns>
        protected override string GetText()
        {
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetText");

            // Call the base class implementation to retrieve the text
            return base.GetText();
        }


        /// <summary>
        /// Gets the background color of the text.
        /// </summary>
        /// <returns>The background color of the text.</returns>
        [Obsolete]
        protected override Color GetTextBackgroundColor()
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In GetTextBackgroundColor");

            // Call the base implementation to get the text background color
            return base.GetTextBackgroundColor();
        }


        /// <summary>
        /// Handles the calculation of the edit rectangle for a cell.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="cellBounds">The bounds of the cell.</param>
        /// <param name="item">The list view item.</param>
        /// <param name="subItemIndex">The index of the subitem within the item.</param>
        /// <param name="preferredSize">The preferred size of the edit control.</param>
        /// <returns>The edit rectangle for the cell.</returns>
        protected override Rectangle HandleGetEditRectangle(Graphics g, Rectangle cellBounds, OLVListItem item, int subItemIndex, Size preferredSize)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In HandleGetEditRectangle");

            // Call the base implementation to calculate the edit rectangle
            return base.HandleGetEditRectangle(g, cellBounds, item, subItemIndex, preferredSize);
        }

        /// <summary>
        /// Handles the hit test operation for the ObjectListView control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="hti">The hit test information.</param>
        /// <param name="x">The x-coordinate of the hit test.</param>
        /// <param name="y">The y-coordinate of the hit test.</param>
        protected override void HandleHitTest(Graphics g, OlvListViewHitTestInfo hti, int x, int y)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In HandleHitTest");

            // Call the base implementation to perform the hit test operation
            base.HandleHitTest(g, hti, x, y);
        }


        /// <summary>
        /// Performs a hit test operation at the specified coordinates.
        /// </summary>
        /// <param name="hti">The hit test information.</param>
        /// <param name="x">The x-coordinate of the hit test.</param>
        /// <param name="y">The y-coordinate of the hit test.</param>
        public override void HitTest(OlvListViewHitTestInfo hti, int x, int y)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In HitTest");

            // Call the base implementation to perform the hit test operation
            base.HitTest(hti, x, y);
        }


        /// <summary>
        /// Performs an optional render operation within the specified bounds.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="r">The bounding rectangle for the render operation.</param>
        /// <returns><c>true</c> if the optional render operation is performed successfully; otherwise, <c>false</c>.</returns>
        public override bool OptionalRender(Graphics g, Rectangle r)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In OptionalRender");

            // Call the base implementation to perform the optional render operation
            return base.OptionalRender(g, r);
        }


        /// <summary>
        /// Renders the control within the specified bounds.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="r">The bounding rectangle for the render operation.</param>
        public override void Render(Graphics g, Rectangle r)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In Render");

            // Call the base implementation to perform the rendering
            base.Render(g, r);
        }


        /// <summary>
        /// Renders the individual list view item within the specified bounds.
        /// </summary>
        /// <param name="e">The <see cref="DrawListViewItemEventArgs"/> containing the event data.</param>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="itemBounds">The bounding rectangle for the item.</param>
        /// <param name="model">The model object associated with the item.</param>
        /// <returns><c>true</c> if the item was rendered successfully; otherwise, <c>false</c>.</returns>
        public override bool RenderItem(DrawListViewItemEventArgs e, Graphics g, Rectangle itemBounds, object model)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In RenderItem");

            // Call the base implementation to perform the rendering
            return base.RenderItem(e, g, itemBounds, model);
        }


        /// <summary>
        /// Renders the individual subitem within the specified bounds.
        /// </summary>
        /// <param name="e">The <see cref="DrawListViewSubItemEventArgs"/> containing the event data.</param>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="cellBounds">The bounding rectangle for the subitem.</param>
        /// <param name="model">The model object associated with the subitem.</param>
        /// <returns><c>true</c> if the subitem was rendered successfully; otherwise, <c>false</c>.</returns>
        public override bool RenderSubItem(DrawListViewSubItemEventArgs e, Graphics g, Rectangle cellBounds, object model)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In RenderSubItem");

            // Call the base implementation to perform the rendering
            return base.RenderSubItem(e, g, cellBounds, model);
        }


        /// <summary>
        /// Gets the standard edit rectangle for the cell based on the cell bounds and preferred size.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="cellBounds">The bounding rectangle for the cell.</param>
        /// <param name="preferredSize">The preferred size of the edit control.</param>
        /// <returns>The edit rectangle for the cell.</returns>
        protected override Rectangle StandardGetEditRectangle(Graphics g, Rectangle cellBounds, Size preferredSize)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In StandardGetEditRectangle");

            // Call the base implementation to get the standard edit rectangle
            return base.StandardGetEditRectangle(g, cellBounds, preferredSize);
        }


        /// <summary>
        /// Performs the standard hit testing for the cell based on the specified coordinates.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="hti">The <see cref="OlvListViewHitTestInfo"/> object to populate with hit test results.</param>
        /// <param name="bounds">The bounding rectangle of the cell.</param>
        /// <param name="x">The x-coordinate of the hit test point.</param>
        /// <param name="y">The y-coordinate of the hit test point.</param>
        protected override void StandardHitTest(Graphics g, OlvListViewHitTestInfo hti, Rectangle bounds, int x, int y)
        {
            // Log debug information if enabled
            //if (_log.IsDebugEnabled)
            //    _log.Debug("In StandardHitTest");

            // Call the base implementation to perform the standard hit testing
            base.StandardHitTest(g, hti, bounds, x, y);
        }


        /// <summary>
        /// Draws the specified image within the specified rectangle.
        /// </summary>
        /// <param name="g">The <see cref="Graphics"/> object used for rendering.</param>
        /// <param name="r">The bounding rectangle in which to draw the image.</param>
        /// <param name="imageSelector">The image selector object used to identify the image to be drawn.</param>
        /// <returns>The width of the drawn image.</returns>
        protected override int DrawImage(Graphics g, Rectangle r, object imageSelector)
        {
            // Log debug information if enabled
            if (_log.IsDebugEnabled)
            {
                _log.Debug($"About to draw image for {imageSelector}");
            }

            // Draw the image
            g.DrawImage(_printButtonImage, r);

            // Return the width of the drawn image
            return _printButtonImage.Width;
        }

    }
}
