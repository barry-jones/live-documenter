﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocumentationTest
{
    /// <summary>
    /// Test for constants defined in classes
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// This is a summary for PRIVATE_CONST
        /// </summary>
        private const string PRIVATE_CONST = "Constant";

        /// <summary>
        /// This is a summary for PUBLIC_CONST
        /// </summary>
        public const int PUBLIC_CONST = 3;

        protected const int PROTECTED_CONST = 1;

        internal const int INTERNAL_CONST = 1;

        protected internal const int PROTECTED_INTERNAL_CONST = 1;

        /// <summary>
        /// A field in a class defining constants.
        /// </summary>
        public string AField;

        /// <summary>
        /// A private field in a class defining constants.
        /// </summary>
        private string anotherField;
    }
}
