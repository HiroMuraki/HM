using System.Text.RegularExpressions;

namespace HM.CLI
{
    public class LaunchArgumentsMatcher
    {
        /// <summary>
        /// 选项名
        /// </summary>
        public string Name { get; init; } = string.Empty;
        /// <summary>
        /// 选项别名
        /// </summary>
        public string[] Aliases { get; init; } = Array.Empty<string>();
        /// <summary>
        /// 选项前缀
        /// </summary>
        public string Prefix { get; init; } = string.Empty;
        /// <summary>
        /// 是否大小写敏感
        /// </summary>
        public bool CaseSensitivity { get; init; } = true;
        /// <summary>
        /// 请求的参数数量
        /// </summary>
        public int ParameterCount { get; init; } = 1;
        /// <summary>
        /// 选项的固定位置
        /// 0表示不固定，为默认值
        /// 1表示第一个参数，2表示第二个参数，以此推类
        /// -1表示最后一个参数，-2表示倒数第二个参数，以此推类
        /// </summary>
        public int FixedPosition { get; init; } = 0;
        /// <summary>
        /// 选项是否允许设置多次
        /// </summary>
        public bool AllowMultiple { get; init; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidLaunchArgumentsOptionException"></exception>
        public string[] FindValues(string[] args)
        {
            if (ParameterCount != 1)
            {
                throw new InvalidLaunchArgumentsOptionException("ParameterCount of launchOption's value should be 1, or use " + nameof(FindValuesArray) + " instead");
            }
            return FindValuesArray(args).Select(t => t.First()).ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFindValues(string[] args, out string[]? result)
        {
            try
            {
                result = FindValues(args);
                return result.Length > 0;
            }
            catch
            {
                result = null;
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidLaunchArgumentsOptionException"></exception>
        public string[][] FindValuesArray(string[] args)
        {
            if (FixedPosition != 0)
            {
                throw new InvalidLaunchArgumentsOptionException("launch option is position-fixed");
            }
            if (ParameterCount == 0)
            {
                throw new InvalidLaunchArgumentsOptionException("ParameterCount cant't be 0");
            }

            var result = new List<string[]>();

            for (int i = 0; i < args.Length; i++)
            {
                /* 如果参数符合launchOption，则将其后的ParameterCount个元素作为参数传入（若参数数量不够则抛异常），
                 * 如果不允许多参数，直接返回，否则将索引值后移ParameterCount位后继续匹配 */
                if (IsArgMatch(args[i]))
                {
                    string[] values = new string[ParameterCount];
                    for (int offset = 0; offset < ParameterCount; offset++)
                    {
                        values[offset] = args[i + 1 + offset];
                    }
                    result.Add(values);
                    i += ParameterCount;
                    if (!AllowMultiple)
                    {
                        break;
                    }
                }
            }

            return result.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFindValueArray(string[] args, out string[][]? result)
        {
            try
            {
                result = FindValuesArray(args);
                return result.Length > 0;
            }
            catch
            {
                result = null;
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string? FindValue(string[] args)
        {
            if (FixedPosition > 0)
            {
                return args[FixedPosition - 1];
            }
            else if (FixedPosition < 0)
            {
                return args[args.Length + FixedPosition];
            }

            return FindValues(args)[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFindValue(string[] args, out string? result)
        {
            try
            {
                result = FindValue(args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Exists(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (IsArgMatch(args[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public LaunchArgumentsMatcher() { }
        public LaunchArgumentsMatcher(string name, params string[] aliases) : this(name, 1, aliases) { }
        private LaunchArgumentsMatcher(string name, int parametersCount, params string[] aliases)
        {
            Name = name;
            Aliases = aliases;
            ParameterCount = parametersCount;
        }

        private bool IsArgMatch(string arg)
        {
            if (!string.IsNullOrEmpty(Prefix))
            {
                if (arg.Length <= Prefix.Length) return false;
                if (arg[0..Prefix.Length] != Prefix) return false;
            }

            arg = arg[Prefix.Length..];
            if (CaseSensitivity)
            {
                return arg == Name || Aliases.Contains(arg);
            }
            else
            {
                return arg.ToLower() == Name.ToLower() || Aliases.Select(a => a.ToLower()).Contains(arg);
            }
        }
    }
}