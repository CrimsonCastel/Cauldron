﻿using Couldron.ViewModels;
using System;
using System.Reflection;

namespace Couldron.Validation
{
    /// <summary>
    /// Validates the property if value is greater than the given value or the given property
    /// </summary>
    /// <exception cref="ArgumentException">The property was not found</exception>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class GreaterThanAttribute : ValidationBaseAttribute
    {
        private string propertyName;
        private double? value;

        /// <summary>
        /// Initializes a new instance of <see cref="GreaterThanAttribute"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessageKey"></param>
        public GreaterThanAttribute(double value, string errorMessageKey) : base(errorMessageKey)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="GreaterThanAttribute"/>
        /// </summary>
        /// <param name="propertyName">The name of the property to validate against</param>
        /// <param name="errorMessageKey">The key of the localized error message string</param>
        /// <exception cref="ArgumentNullException"><paramref name="propertyName"/> is null</exception>
        public GreaterThanAttribute(string propertyName, string errorMessageKey) : base(errorMessageKey)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            this.propertyName = propertyName;
        }

        /// <summary>
        /// Invokes the validation on the specified context with the specified parameters
        /// </summary>
        /// <param name="sender">The property that invoked the validation</param>
        /// <param name="context">The Viewmodel context that has to be validated</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> of the validated property</param>
        /// <param name="value">The value of the property</param>
        /// <returns>Has to return true on validation error otherwise false</returns>
        protected override bool OnValidate(PropertyInfo sender, IValidatableViewModel context, PropertyInfo propertyInfo, object value)
        {
            if (this.value.HasValue)
                return !(value.ToDouble() > this.value.Value);

            var otherProperty = context.GetType().GetProperty(this.propertyName);

            if (otherProperty == null)
                throw new ArgumentException(string.Format("The property '{0}' was not found on '{1}'.", this.propertyName, context.GetType().FullName));

            return !(value.ToDouble() > otherProperty.GetValue(context).ToDouble());
        }
    }
}