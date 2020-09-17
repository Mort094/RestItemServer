using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLib
{
    public class FilterItem
    {
        private double _lowQuantity;
        private double _highQuantity;

        public double LowQuantity
        {
            get => _lowQuantity;
            set => _lowQuantity = value;
        }

        public double HighQuantity
        {
            get => _highQuantity;
            set => _highQuantity = value;
        }
    }
}
