using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TypeFinder
{
    public interface IRule
    {
        bool Complies(Type type);
    }

    internal sealed class CombinedRule : IRule
    {
        private readonly IRule? RuleA;
        private readonly IRule RuleB;

        public CombinedRule(IRule? ruleA, IRule ruleB)
        {
            this.RuleA = ruleA;
            this.RuleB = ruleB ?? throw new ArgumentNullException(nameof(ruleB));
        }

        public bool Complies(Type type) => (RuleA?.Complies(type) ?? true) && RuleB.Complies(type);
    }

    internal sealed class ExcludeTypesRule : IRule
    {
        private readonly IList<Type> Excluded;

        public ExcludeTypesRule(IList<Type> excluded)
        {
            this.Excluded = excluded ?? throw new ArgumentNullException(nameof(excluded));
        }

        public bool Complies(Type type) => !Excluded.Contains(type);
    }

    internal sealed class InheritanceRule : IRule
    {
        private readonly Type BaseType;

        public InheritanceRule(Type baseType)
        {
            this.BaseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
        }

        public bool Complies(Type type) => BaseType.IsAssignableFrom(type) && type != BaseType;
    }

    internal sealed class NameRegexRule : IRule
    {
        private readonly Regex Regex;
        private readonly bool MatchFullName;

        public NameRegexRule(string regex, bool matchFullName)
        {
            this.Regex = new Regex(regex);
            this.MatchFullName = matchFullName;
        }

        public bool Complies(Type type) => Regex.IsMatch(MatchFullName ? type.FullName : type.Name);
    }

    internal sealed class InNamespaceRule : IRule
    {
        private readonly string Namespace;

        public InNamespaceRule(string @namespace)
        {
            this.Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
        }

        public bool Complies(Type type) => type.Namespace == Namespace;
    }
}
