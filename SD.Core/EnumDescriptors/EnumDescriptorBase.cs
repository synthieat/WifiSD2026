using SD.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.EnumDescriptors
{
    public abstract class EnumDescriptorBase<T> : IEquatable<EnumDescriptorBase<T>>
      where T : Enum
    {
        public T Enum { get; }
        public string Code { get; }
        public string ResourceKey { get; }

        protected EnumDescriptorBase(T @enum, string code, string resourceKey)
        {
            Enum = @enum;
            Code = code;
            ResourceKey = resourceKey;
        }

        public bool Equals(EnumDescriptorBase<T>? other) => other is not null && Code == other.Code;

        public override bool Equals(object? obj) => obj is EnumDescriptorBase<T> other && Equals(other);

        public override int GetHashCode() => Code.GetHashCode();

        public override string ToString() => this.ResourceKey != null ? BasicRes.ResourceManager.GetString(ResourceKey) ?? Code : Code;

        public static bool operator == (EnumDescriptorBase<T>? left, EnumDescriptorBase<T>? right) => left?.Equals(right) ?? right is null;

        public static bool operator != (EnumDescriptorBase<T>? left, EnumDescriptorBase<T>? right) => !(left == right);
    }

}
