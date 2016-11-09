
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    internal class SequenceDiagramPanel : Panel
    {
        private LiveDocumenter.Model.Diagram.SequenceDiagram.SequenceDiagram data;
        // private Dictionary<Model.Diagram.SequenceDiagram.Object, UIElement> controlReference = new Dictionary<Object, UIElement>();
        private Dictionary<object, UIElement> controlmap = new Dictionary<object, UIElement>();

        /// <summary>
        /// Static constructor initialises the default styles associated with this control
        /// </summary>
        static SequenceDiagramPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SequenceDiagramPanel),
                new FrameworkPropertyMetadata(typeof(SequenceDiagramPanel)));
        }

        public LiveDocumenter.Model.Diagram.SequenceDiagram.SequenceDiagram Data
        {
            set
            {
                this.controlmap.Clear();
                this.data = value;

                // Clear the child controls and recreate them

                // First add the object instances in the order they are called, this is the
                // horizontal portion of the layout
                foreach(Model.Diagram.SequenceDiagram.Object o in this.data.GetObjects())
                {
                    Object oCtrl = new Object(o);
                    controlmap.Add(o, oCtrl);
                    this.Children.Add(oCtrl);
                    foreach(Model.Diagram.SequenceDiagram.Activation a in o.Activations)
                    {
                        Activation aCtrl = new Activation(a);
                        this.controlmap.Add(a, aCtrl);
                        this.Children.Add(aCtrl);
                        foreach(Model.Diagram.SequenceDiagram.Call c in a.Calls)
                        {
                            Call cCtrl = new Call(c);
                            this.controlmap.Add(c, cCtrl);
                            this.Children.Add(cCtrl);
                        }
                    }
                }

                // this.Children.Add(new TextBlock(new Run("Test")));

                this.InvalidateVisual();
            }
        }

        #region Drawing and Positioning
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {
            if(this.Children == null || this.Children.Count == 0)
                return finalSize;

            Dictionary<object, Arrangement> arranged = new Dictionary<object, Arrangement>();

            double objectLineX = 0;

            // Get the first item and
            // Now measure the activations and calls, start at Start!
            Activation start = (Activation)this.controlmap[this.data.Start.RecievingActivation];
            Object firstReciever = (Object)this.controlmap[this.data.Start.Reciever];
            // Place the starting object
            firstReciever.Arrange(new Rect(objectLineX, 0, firstReciever.DesiredSize.Width,
                firstReciever.DesiredSize.Height));
            arranged.Add(firstReciever, new Arrangement(objectLineX, 0, firstReciever.DesiredSize.Width, firstReciever.DesiredSize.Height));
            objectLineX += firstReciever.DesiredSize.Width; // Move to the ext available space
                                                            // Start to calculate the size and placement of the first activation
            double containingActivationX = firstReciever.DesiredSize.Width / 2, containingActivationY = firstReciever.DesiredSize.Height;
            double containingActivationHeight = 20;
            double callX = containingActivationX + start.DesiredSize.Width / 2, callY = containingActivationY;
            foreach(Model.Diagram.SequenceDiagram.Call currentCall in start.ActualActivation.Calls)
            {
                // Get the call control
                Call callControl = (Call)this.controlmap[currentCall];
                callControl.Arrange(new Rect(callX, callY, callControl.DesiredSize.Width, callControl.DesiredSize.Height));

                // Check if we are calling ourselves
                if(currentCall.RecievingActivation.Recieved.Reciever != currentCall.Caller.Recieved.Reciever)
                {
                    // Arrange the calls called object
                    Object reciever = (Object)this.controlmap[callControl.ActualCall.Reciever];
                    if(!arranged.ContainsKey(reciever))
                    {
                        reciever.Arrange(new Rect(objectLineX, 0, reciever.DesiredSize.Width, reciever.DesiredSize.Height));
                        arranged.Add(reciever, new Arrangement(objectLineX, 0, reciever.DesiredSize.Width, reciever.DesiredSize.Height));
                        objectLineX += reciever.DesiredSize.Width;
                    }

                    // Arrange the calls called activation
                    Activation calledActivation = (Activation)this.controlmap[currentCall.RecievingActivation];
                    if(!arranged.ContainsKey(calledActivation))
                    {
                        Arrangement recieverDetails = arranged[reciever];

                        calledActivation.Arrange(new Rect(recieverDetails.x + (recieverDetails.width / 2), callY, calledActivation.DesiredSize.Width, calledActivation.DesiredSize.Height));
                    }
                }

                // Update for the next call
                callY += callControl.DesiredSize.Height;
            }
            start.Arrange(new Rect(containingActivationX, containingActivationY, start.DesiredSize.Width, callY));


            //// Arrange the object items in the order in which they were added, this should be the
            //// order in which they are called.
            //double curX = 0, curY = 0;
            //IEnumerable<Object> objects = this.Children.OfType<Object>();
            //foreach (Object child in objects) {
            //    child.Arrange(new Rect(curX, curY, child.DesiredSize.Width,
            //        child.DesiredSize.Height));

            //    // Add its activations
            //    if(child.ActualObject.Activations != null) {
            //        foreach (Model.Diagram.SequenceDiagram.Activation current in child.ActualObject.Activations) {
            //            if (this.controlmap.ContainsKey(current)) {
            //                Activation a = (Activation)this.controlmap[current];
            //                    a.Arrange(new Rect(
            //                        curX + child.DesiredSize.Width / 2, 
            //                        curY + child.DesiredSize.Height + 10,
            //                        10, 20));
            //            }
            //        }
            //    }

            //    curX += child.DesiredSize.Width;
            //}

            // Now measure the activations and calls, start at Start!
            //Activation start = (Activation)this.controlmap[this.data.Start.RecievingActivation];
            //Object firstReciever = (Object)this.controlmap[this.data.Start.Reciever];

            // start.Arrange(new Rect(firstReciever.POs

            // Now arrange the activations with the associated objects.

            return finalSize;
        }

        private class Arrangement
        {
            public Arrangement(double x, double y, double w, double h)
            {
                this.x = x;
                this.y = y;
                this.width = w;
                this.height = h;
            }
            public double x;
            public double y;
            public double width;
            public double height;
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            Size infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
            double curX = 0;

            // Measure all of object elements
            // IEnumerable<Object> objects = this.Children.OfType<Object>();
            foreach(UIElement child in this.Children)
            {
                child.Measure(infiniteSize);
                curX += child.DesiredSize.Width;
            }


            Size resultSize = new Size();
            resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ?
                curX : availableSize.Width;
            resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ?
                resultSize.Height : availableSize.Height;
            return resultSize;
        }
        #endregion
    }
}