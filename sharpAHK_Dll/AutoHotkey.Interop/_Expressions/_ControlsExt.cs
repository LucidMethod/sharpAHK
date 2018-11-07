using AHKExpressions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHKExpressions
{
    public static partial class StringExtension
    {
        //  === Controls ===


        public static void Button_Image(Button button, string ImagePath, bool ImageLeft = true)
        {
            Image image = ImagePath.ToImg(); // convert input to Image format

            //=== Button Image: text far left - icon middle ====  (works)
            button.TextImageRelation = TextImageRelation.TextBeforeImage;
            button.BackgroundImageLayout = ImageLayout.Zoom; // adjust zoom to make button icon fit (works)
            button.BackgroundImage = image;

            if (ImageLeft)  // image on left side of button
            {
                button.ImageAlign = ContentAlignment.MiddleRight;
                button.TextAlign = ContentAlignment.MiddleLeft;
            }
            if (!ImageLeft)  // image on right side of button
            {
                button.ImageAlign = ContentAlignment.MiddleLeft;
                button.TextAlign = ContentAlignment.MiddleRight;
            }

        }


        // === Draggable Controls Extension ===

        // Example Use:   AnyControl.Draggable(true);

        /// <summary>
        /// Enabling/disabling dragging for control
        /// Source: https://www.codeproject.com/Tips/178587/Draggable-WinForms-Controls
        /// </summary>
        public static void Draggable(this Control control, bool Enable = true)
        {
            if (Enable)
            {
                // enable drag feature
                if (draggables.ContainsKey(control))
                {   // return if control is already draggable
                    return;
                }
                // 'false' - initial state is 'not dragging'
                draggables.Add(control, false);

                // assign required event handlersnnn
                control.MouseDown += new MouseEventHandler(control_MouseDown);
                control.MouseUp += new MouseEventHandler(control_MouseUp);
                control.MouseMove += new MouseEventHandler(control_MouseMove);
            }
            else
            {
                // disable drag feature
                if (!draggables.ContainsKey(control))
                {  // return if control is not draggable
                    return;
                }
                // remove event handlers
                control.MouseDown -= control_MouseDown;
                control.MouseUp -= control_MouseUp;
                control.MouseMove -= control_MouseMove;
                draggables.Remove(control);
            }
        }
        static void control_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOffset = new System.Drawing.Size(e.Location);
            // turning on dragging
            draggables[(Control)sender] = true;
        }
        static void control_MouseUp(object sender, MouseEventArgs e)
        {
            // turning off dragging
            draggables[(Control)sender] = false;
        }
        static void control_MouseMove(object sender, MouseEventArgs e)
        {
            // only if dragging is turned on
            if (draggables[(Control)sender] == true)
            {
                // calculations of control's new position
                System.Drawing.Point newLocationOffset = e.Location - mouseOffset;
                ((Control)sender).Left += newLocationOffset.X;
                ((Control)sender).Top += newLocationOffset.Y;
            }
        }
        // TKey is control to drag, TValue is a flag used while dragging
        private static Dictionary<Control, bool> draggables =
                   new Dictionary<Control, bool>();
        private static System.Drawing.Size mouseOffset;

        



        
    }
}
