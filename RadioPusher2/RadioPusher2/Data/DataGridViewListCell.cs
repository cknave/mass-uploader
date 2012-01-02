using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace NUtils
{

    public class DataGridViewListColumn : DataGridViewImageColumn
    {
        public DataGridViewListColumn()
        {
            CellTemplate = new DataGridViewListCell();
        }
    }

    class DataGridViewListCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        List<string> contents = new List<string>();
        static DataGridViewListCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public DataGridViewListCell()
        {
            this.ValueType = typeof(int);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            List<int> contents = value as List<int>;

            string contentsstring = String.Empty;
            if (value != null) {
                foreach (int c in contents) {
                    contentsstring += c.ToString() + ",";
                }
            }
           // Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
            // Draws the cell grid
            base.Paint(g, clipBounds, cellBounds,
             rowIndex, cellState, value, formattedValue, errorText,
             cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));
            if (contentsstring.Equals("")) {
                g.DrawString("-", cellStyle.Font, foreColorBrush, cellBounds.X, cellBounds.Y + 2);            
            } else {
                g.DrawString(contentsstring, cellStyle.Font, foreColorBrush, cellBounds.X, cellBounds.Y + 2);            
            }
        }
    }
}
