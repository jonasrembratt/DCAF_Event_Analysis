
using System;
using System.Collections.Generic;
using System.Text;

namespace DCAF.Inspection._lib
{
    public static class StringHelper
    {
        public static string ToIdentifier(this string self, IdentifierCasing casing = IdentifierCasing.Camel)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            
            return process(
                new StringProcessingArgs(self)
                {
                    Casing = casing,
                    RemoveAllWhitespace = true
                },  
                trimWhitespace,
                setInitialCasing);

        }

        public static string ToUpperInitial(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;

            return process(new StringProcessingArgs(self)
                {
                    Casing = IdentifierCasing.Pascal
                },
                setInitialCasing);
        }

        public static string ToLowerInitial(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;

            return process(new StringProcessingArgs(self)
                {
                    Casing = IdentifierCasing.Lower
                },
                setInitialCasing);
        }

        static StringProcessResult trimWhitespace<T>(T args, out char use) where T : StringProcessingArgs
        {
            use = args.Array[args.Index];
            return char.IsWhiteSpace(args.Array[args.Index]) ? StringProcessResult.Skip : StringProcessResult.Continue;
        }
        
        static StringProcessResult setInitialCasing<T>(T args, out char use) where T : StringProcessingArgs
        {
            var ca = args.Array;
            var i = args.Index;
            var isInitial = args.Index == 0 || char.IsWhiteSpace(ca[i - 1]) && char.IsLetter(ca[i]);
            use = ca[i];
            if (!isInitial)
                return StringProcessResult.Continue;

            use = args.Casing switch
            {
                IdentifierCasing.None => ca[i],
                IdentifierCasing.Camel => char.ToLower(ca[i]),
                IdentifierCasing.Pascal => char.ToUpper(ca[i]),
                IdentifierCasing.Kebab => '-',
                IdentifierCasing.Snake => '_',
                _ => throw new Exception()
            };
            
            return StringProcessResult.Continue;
        }

        static string process<T>(T args, params StringProcessor<T>[] processors) where T : StringProcessingArgs
        {
            var sb = args.StringBuilder;
            var ca = args.Array;
            var i = args.Index;
            for (; i < ca.Length; i++)
            {
                args.Index = i;
                for (var p = 0; p < processors.Length; p++)
                {
                    var process = processors[p];
                    switch (process(args, out var use))
                    {
                        case StringProcessResult.None:
                            continue;
                        
                        case StringProcessResult.Continue:
                            sb.Append(use);
                            break;
                    
                        case StringProcessResult.Skip:
                            break;
                    
                        case StringProcessResult.EndExclusive:
                            return sb.ToString();

                        case StringProcessResult.EndInclusive:
                            sb.Append(use);
                            return sb.ToString();

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return sb.ToString();
        }

        enum StringProcessResult
        {
            None,
            
            Continue,
            
            Skip,
            
            EndExclusive,
            
            EndInclusive
        }

        public enum IdentifierCasing
        {
            None,
            
            Camel,
            
            Pascal,
            
            Kebab,
            
            Snake,
            
            Lower,
            
            Upper
        }

        delegate StringProcessResult StringProcessor<in T>(T args, out char use)
            where T : StringProcessingArgs;
        
        class StringProcessingArgs
        {
            Dictionary<string, object?>? _values;

            internal StringBuilder StringBuilder { get; }
            
            public char[] Array { get; }

            public int Index { get; set; }
            
            public IdentifierCasing Casing { get; set; }

            public bool RemoveAllWhitespace { get; set; }
            
            public T? GetValue<T>(string key, T useDefault = default!)
            {
                if (_values is null || !_values.TryGetValue(key, out var obj) || obj is not T tv)
                    return useDefault;

                return tv;
            }
        
            public bool TryGetValue<T>(string key, out T? value)
            {
                value = default;
                if (_values is null || !_values.TryGetValue(key, out var obj) || obj is not T tv)
                    return false;

                value = tv;
                return true;
            }

            protected void SetValue(string key, object? value, bool overwrite = false)
            {
                if (_values is null)
                {
                    _values = new Dictionary<string, object?> { { key, value } };
                    return;
                }

                if (!_values.TryGetValue(key, out _))
                {
                    _values.Add(key, value);
                    return;
                }

                if (!overwrite)
                    throw new Exception($"Cannot add tag '{key}' to args (was already added)");

                _values[key] = value;
            }

            
            public StringProcessingArgs(string s) : this(new StringBuilder(), s.ToCharArray(), 0)
            {
            }

            public StringProcessingArgs(StringBuilder stringBuilder, char[] array, int index)
            {
                StringBuilder = stringBuilder;
                Array = array;
                Index = index;
            }
        }
    }
}