namespace Core.Attributes
{
    /// <summary>
    /// Attach this attribute to the concrete class inheriting from <see cref="Abstractions.ResultItemBase"/> in order to display the property.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DetailViewColumnAttribute : Attribute
    {
        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailViewColumnAttribute"/> class.
        /// </summary>
        /// <param name="type">The type to cast to.</param>
        /// <remarks>Defaults to <see cref="string"/>.</remarks>
        public DetailViewColumnAttribute(Type? type = null)
        {
            TargetType = type ?? typeof(string);
        }
    }

    /// <inheritdoc cref="DetailViewColumnAttribute" />
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DetailViewColumnAttribute<TTarget> : DetailViewColumnAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailViewColumnAttribute"/> class.
        /// </summary>
        public DetailViewColumnAttribute()
            : base(typeof(TTarget))
        {
        }
    }
}
